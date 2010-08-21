using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data.SqlServerCe;

using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Import.Qif
{
    class ImportQIF
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private FFDataSet ffDataSet; // Dataset holding all converted data tobe saved to the database
        private BackgroundWorker importWorker;

        // Structure holding all the QIF data Accounts hold Transactions wich hold Splits
        private List<QifAccount> qifAccounts;

        // The transactions that are a transfer because they have an [account]
        private List<QifTransaction> qifTransfers; 
        
        private readonly string fileName;

        ///////////////////////////////////////////////////////////////////////
        //   External Events
        ///////////////////////////////////////////////////////////////////////   
        public event ProgressChangedEventHandler ProgressChanged;
        private void OnProgressChanged(ProgressChangedEventArgs e)
        {
            // Raises the event ProgressChanged
            if (ProgressChanged != null)
                ProgressChanged(this, e);
        }

        public event RunWorkerCompletedEventHandler WorkCompleted;
        private void OnWorkCompleted(RunWorkerCompletedEventArgs e)
        {
            // Raises the event ProgressChanged
            if (WorkCompleted != null)
                WorkCompleted(this, e);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //    Import worker events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void importWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnProgressChanged(e);
        }

        private void importWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnWorkCompleted(e);
        }

        private void importWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // PHASE 1 - Read in the QIF file and 
            this.readHeaders();

            // PHASE 2 - Import the data.
            this.importQIFData();
            this.importSpecialCaseTransactions();
            this.importTheRest();
            

            // PHAZE 3 - Commit the changes to the database. Determine wich accounts should have Envelopes
            this.ffDataSet.mySaveData();
            this.setAccountEnvelopes();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Phase 1 - Read the QIF file and store the data in the more usable qifAccounts list.
        //   We are using the information we get from the list of accounts in the QIF file and the
        //   transactions assosiated with each account. From this we are also getting all the 
        //   catagory names that were being used.
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void readHeaders()
        {
            int complete = 0;
            int count = 0;
            TextReader inFile;
            QifAccount lastAccount = null;

            // Read in the file and split into strings by headers that start with a "\n!"
            inFile = new StreamReader(fileName);
            
            // Skip the first char '!' so it looks like the rest of the headers that will be missing the '!'
            inFile.Read();  
            string [] sections = inFile.ReadToEnd().Split(new  string[] { "\n!" }, StringSplitOptions.RemoveEmptyEntries);
            inFile.Close();
            count = sections.Length;

            foreach (string section in sections)
            {
                string firstLine;
                string data = "";

                // Divide the section into the first line and the rest of the data
                int charIndex = section.IndexOfAny(new char[] {'\r', '\n'});

                if (charIndex > 0)
                {
                    firstLine = section.Substring(0, charIndex);
                    data = section.Substring(charIndex).Replace("\r", "").Trim();
                }
                else
                {
                    firstLine = section;
                    data = "";
                }

                // decide what to do according to the first line of the section
                switch (firstLine)
                {
                    case "Account":
                        lastAccount = getAccountData(data);
                        break;

                    case "Type:Bank ":
                        getTransactionData(lastAccount, data);
                        break;

                    case "Type:Cash":
                        getTransactionData(lastAccount, data);
                        break;

                    case "Type:CCard":
                        getTransactionData(lastAccount, data);
                        break;

                    case "Type:Oth A":
                        getTransactionData(lastAccount, data);
                        break;

                    case "Type:Oth L":
                        getTransactionData(lastAccount, data);
                        break;

                    default:
                        break;

                } // end switch

                this.importWorker.ReportProgress((complete += 100) / count, "Reading Qif File.");
            }// END foreach section
        }

        private QifAccount getAccountData(string data)
        {
            QifAccount lastAccount = null;
            string[] accArray = data.Split(new string[] { "\n^" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string accInfo in accArray)
            {
                string[] properties = accInfo.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                QifAccount newAccount = new QifAccount();

                foreach (string property in properties)
                {
                    switch (property[0])
                    {
                        case 'N':    //N	Name
                            newAccount.Name = property.Substring(1);
                            break;

                        case 'T':    //T 	Type of account
                            newAccount.Type = property.Substring(1);
                            break;

                        case 'D':    //D 	Description
                            newAccount.Description = property.Substring(1);
                            break;

                        case '$':    //$ 	Statement balance
                            newAccount.StatementBalance = Convert.ToDecimal(property.Substring(1));
                            break;

                        case 'L':    //L 	Credit limit (only for credit card account)
                            newAccount.CreditLimit = Convert.ToDecimal(property.Substring(1));
                            break;

                        case '/':     /// 	Statement balance date
                            newAccount.StatementBalanceDate = myStringToDate(property.Substring(1));
                            break;

                        default:
                            throw new Exception("Not handled " + property);
                    }// End switch
                }// End foreach property

                lastAccount = this.addQifAccount(newAccount);
            }// End foreach accInfo

            return lastAccount;
        }

        private QifAccount addQifAccount(QifAccount newAccount)
        {
            foreach (QifAccount acc in this.qifAccounts)
            {
                if (acc.Name == newAccount.Name)
                {
                    acc.Type = longerString(acc.Type, newAccount.Type);
                    acc.Description = longerString(acc.Description, newAccount.Description);
                    acc.StatementBalance = beterDecimal(acc.StatementBalance, newAccount.StatementBalance);
                    acc.CreditLimit = beterDecimal(acc.CreditLimit, newAccount.CreditLimit);
                    acc.StatementBalanceDate = beterDate(acc.StatementBalanceDate, newAccount.StatementBalanceDate);
                    return acc;
                }
            }

            this.qifAccounts.Add(newAccount);
            return newAccount;
        }

        private string longerString(string a, string b)
        {
            if (a.Length > b.Length)
                return a;
            else
                return b;
        }

        private decimal beterDecimal(decimal a, decimal b)
        {
            if (a == QifAccount.NULL_DECIMAL)
                return b;
            else
                return a;
        }

        private DateTime beterDate(DateTime a, DateTime b)
        {
            if (a.CompareTo(b) > 0)
                return a;
            else
                return b;
        }

        private DateTime myStringToDate(string text)
        {
            DateTime date;
            int start;
            int length;
            int year;
            int month;
            int day;

            start = 0;
            length = text.IndexOf('/') - start;
            month = Convert.ToInt32(text.Substring(0, length));

            start += length + 1;
            length = text.IndexOf('\'') - start;
            day = Convert.ToInt32(text.Substring(start, length));

            start += length + 1;
            year = Convert.ToInt32(text.Substring(start));

            date = new DateTime(year, month, day);

            if (date < new DateTime(100, 1, 1))
                date = date.AddYears(2000);

            return date;
        }

        private void getTransactionData(QifAccount account, string data)
        {
            string[] transArray = data.Split(new string[] { "\n^" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string transInfo in transArray)
            {
                string[] properties = transInfo.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                QifTransaction newTransaction = new QifTransaction();
                QifSplit newSplit = null;

                foreach (string property in properties)
                {
                    switch (property[0])
                    {
                        case 'D':    //D  	Date
                            newTransaction.Date = myStringToDate(property.Substring(1));
                            continue;

                        case 'T':    //T 	Amount
                            newTransaction.Amount = Convert.ToDecimal(property.Substring(1));
                            continue;

                        case 'C':    //C 	Cleared status
                            newTransaction.ClearedStatus = property.Substring(1);
                            continue;

                        case 'N':    //N 	Num (check or reference number)
                            newTransaction.Num = property.Substring(1);
                            continue;

                        case 'M':    //M 	Memo
                            newTransaction.Memo = property.Substring(1);
                            continue;

                        case 'P':    //P 	Payee
                            newTransaction.Payee = property.Substring(1);
                            continue;

                        case 'A':    //A 	Address (up to five lines; the sixth line is an optional message)
                            newTransaction.Address += property.Substring(1);
                            continue;

                        case 'L':    //L 	Category (Category/Subcategory/Transfer/Class)
                            newTransaction.Catagory = property.Substring(1);
                            continue;

                        case 'S':    //S 	Category in split (Category/Transfer/Class)
                            newSplit = new QifSplit();
                            newSplit.Catagory = property.Substring(1);
                            continue;

                        case 'E':     //E 	Memo in split
                            newSplit.Memo = property.Substring(1);
                            continue;

                        case '$':    //$ 	Dollar amount of split
                            newSplit.Amount = Convert.ToDecimal(property.Substring(1));
                            if(newSplit.Catagory.StartsWith("["))
                                newTransaction.OppLines.Add(newSplit);
                            else
                                newTransaction.EnvLines.Add(newSplit);
                            continue;

                        case 'U':    //Don't know. Always seams to be the same as T (The amount).
                            continue;

                        default:
                            throw new Exception("Not handled " + property);
                    }// End switch
                }// End foreach property

                account.Transactions.Add(newTransaction);
            }// End foreach accInfo

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Phase 2 - Convert the QIF data to FamilyFinance data and store it in the tables.
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void importQIFData()
        {
            int count = this.qifAccounts.Count;
            int progress = 0;

            // Add all the accounts so the are in the accountMap
            foreach (QifAccount qAcc in this.qifAccounts)
            {
                this.ffDataSet.myImportAccount(qAcc.Name, qAcc.Type, SpclAccountCat.ACCOUNT);
            }

            // Double nested foreach loop to process each account and then each transaction 
            // Report the progress too.
            foreach (QifAccount qAcc in this.qifAccounts)
            {
                int accountID = this.ffDataSet.myImportAccount(qAcc.Name, qAcc.Type, SpclAccountCat.ACCOUNT);

                foreach (QifTransaction trans in qAcc.Transactions)
                {
                    trans.AccountID = accountID;

                    if (trans.Catagory.StartsWith("[") || trans.OppLines.Count > 0)
                        this.importQIFTransfer(trans);
                    else
                        this.importQIFSimpleTransaction(trans);
                }

                progress += 100;
                this.importWorker.ReportProgress(progress / count, "Importing Transactions");
            }
        }

        private void importQIFSimpleTransaction(QifTransaction qTrans)
        {
            // Easy stuff (date, type, account, description, confNum, complete, amount, creditdebit)
            FFDataSet.LineItemRow newLine = this.getEasyLineData(qTrans);

            // Set oppAccount and envelopeID, and import envelope Lines
            byte cat = (qTrans.Amount > 0.0m) ? SpclAccountCat.INCOME : SpclAccountCat.EXPENSE;
            newLine.oppAccountID = this.ffDataSet.myImportAccount(qTrans.Payee, "", cat);
            newLine.envelopeID = this.importEnvelopeLines(qTrans, newLine.id);

            if (newLine.accountID == newLine.oppAccountID)
                newLine.oppAccountID = this.ffDataSet.myImportAccount(qTrans.Payee + "-Payee", "", cat);

            // Build the oppLine 
            FFDataSet.LineItemRow payeeLine = this.ffDataSet.LineItem.NewLineItemRow();
            payeeLine.transactionID = newLine.transactionID;
            payeeLine.date = newLine.date;
            payeeLine.typeID = newLine.typeID;
            payeeLine.accountID = newLine.oppAccountID;
            payeeLine.oppAccountID = newLine.accountID;
            payeeLine.description = newLine.description;
            payeeLine.confirmationNumber = newLine.confirmationNumber;
            payeeLine.envelopeID = SpclEnvelope.NULL;
            payeeLine.complete = newLine.complete;
            payeeLine.amount = newLine.amount;
            payeeLine.creditDebit = !newLine.creditDebit;
            
            // Add the lines to the table
            this.ffDataSet.LineItem.AddLineItemRow(newLine);
            this.ffDataSet.LineItem.AddLineItemRow(payeeLine);
        }

        private FFDataSet.LineItemRow getEasyLineData(QifTransaction qTrans)
        {
            FFDataSet.LineItemRow newLine = this.ffDataSet.LineItem.NewLineItemRow();

            newLine.date = qTrans.Date;
            newLine.accountID = qTrans.AccountID;
            newLine.description = qTrans.Memo + " " + qTrans.Address;

            // Set the amount and creditDebit
            if (qTrans.Amount >= 0)
            {
                newLine.amount = qTrans.Amount;
                newLine.creditDebit = LineCD.DEBIT;
            }
            else
            {
                newLine.amount = Decimal.Negate(qTrans.Amount);
                newLine.creditDebit = LineCD.CREDIT;
            }

            // Set the complete
            switch (qTrans.ClearedStatus)
            {
                case "S":
                    newLine.complete = LineState.RECONSILED;
                    break;

                case "X":
                    newLine.complete = LineState.RECONSILED;
                    break;

                case "*":
                    newLine.complete = LineState.CLEARED;
                    break;

                case "":
                    newLine.complete = LineState.PENDING;
                    break;

                default:
                    newLine.complete = LineState.PENDING;
                    break;
            }

            // Set the Num/Type and Confermation number if needed.
            try
            {
                Convert.ToInt32(qTrans.Num);
                newLine.typeID = this.ffDataSet.myImportLineType("Check");
                newLine.confirmationNumber = "Check Number: " + qTrans.Num;
            }
            catch (FormatException)
            {
                newLine.typeID = this.ffDataSet.myImportLineType(qTrans.Num);
            }

            return newLine;
        }

        private int importEnvelopeLines(QifTransaction qTrans, int lineID)
        {
            FFDataSet.EnvelopeLineRow eLine;
            int envelopeID = SpclEnvelope.NULL;
            int countELines = 0;

            if (qTrans.EnvLines.Count > 0)
            {
                // Build the envelope lines from the splits.
                foreach (QifSplit split in qTrans.EnvLines)
                {
                    if (qTrans.Amount < 0.0m)
                    {
                        eLine = this.ffDataSet.EnvelopeLine.NewEnvelopeLineRow();

                        eLine.lineItemID = lineID;
                        eLine.envelopeID = envelopeID = this.ffDataSet.myImportEnvelope(split.Catagory);
                        eLine.description = split.Memo;

                        if (qTrans.Amount < 0.0m)
                            eLine.amount = Decimal.Negate(split.Amount);
                        else
                            eLine.amount = split.Amount;

                        this.ffDataSet.EnvelopeLine.AddEnvelopeLineRow(eLine);
                        countELines++;
                    }
                }
            }
            else if (qTrans.Catagory.StartsWith("["))
            {
                // Filter out the Transfers
                countELines = 0;
            }
            else if (qTrans.Amount < 0.0m)
            {
                // Else there are no splits so build an envelopeLine from the transaction.
                eLine = this.ffDataSet.EnvelopeLine.NewEnvelopeLineRow();
                
                eLine.lineItemID = lineID;
                eLine.envelopeID = envelopeID = this.ffDataSet.myImportEnvelope(qTrans.Catagory);
                eLine.description = qTrans.Memo;
                
                if(qTrans.Amount < 0.0m)
                    eLine.amount = Decimal.Negate(qTrans.Amount);
                else
                    eLine.amount = qTrans.Amount;

                this.ffDataSet.EnvelopeLine.AddEnvelopeLineRow(eLine);
                countELines++;
            }

            if (countELines == 0)
                envelopeID = SpclEnvelope.NULL;
            else if (countELines > 1)
                envelopeID = SpclEnvelope.SPLIT;

            return envelopeID;
        }

        private void importQIFTransfer(QifTransaction qTrans)
        {
            FFDataSet.LineItemRow newLine;
            FFDataSet.LineItemRow oppLine;
            QifTransaction oppTrans = null;
            int oppAccountID;

            // Determine oppSum that will be used if this is a complex transaction.
            // Mostly a complex transaction in Quicken is for a paycheck where a --Form-- is used.
            // Also a complex transaction can be where a purchase and transfer are combined into one action.
            // For example, a purchase at a store where you get cash back and the cash is then deposited 
            // into another account. If this is the case the payee should be an oppAccount also. 
            // the money that goes to the store = the purchase total minus the amount transfered.
            decimal oppSum = 0.0m;

            foreach (QifSplit oppLn in qTrans.OppLines)
                oppSum += oppLn.Amount;

            // If a complex transaction. Payee is also an oppAccount
            if (qTrans.EnvLines.Count >= 1 && qTrans.EnvLines.Count >= 1)
                qTrans.OppLines.Add(new QifSplit(qTrans.Payee, qTrans.Memo, qTrans.Amount - oppSum));

            // Determine the oppAccountID of the given transaction
            if (qTrans.OppLines.Count == 0)
            {   // Simple Transfer
                if (!this.ffDataSet.myGetAccount(qTrans.Catagory, out oppAccountID))
                    throw new InvalidDataException("The transactions Catagory is not an account in the map.");
            }
            else if (qTrans.OppLines.Count == 1)
            {   // Don't think this will ever happen, but in case it does...
                if (!this.ffDataSet.myGetAccount(qTrans.OppLines[0].Catagory, out oppAccountID))
                    throw new InvalidDataException("The transactions OppLine.Catagory is not an account in the map.");
            }
            else // Another type of complex transaction.
                oppAccountID = SpclAccount.MULTIPLE;

            qTrans.OppAccountID = oppAccountID;

            // Look for a match to this transfer. This might be the second half of a simple transfer.
            // Once found assign in to oppTrans and remove it from the qifTransfer list
            foreach (QifTransaction trans in this.qifTransfers)
            {
                if(trans.isOpposite(qTrans))
                {
                    oppTrans = trans;
                    this.qifTransfers.Remove(trans);
                    break;
                }
            }

            // If we didn't find an opposite transaction in the transfer list this transfer might be
            // a one sided transaction like an opening balance. If not add it to the transfer list.
            if (oppTrans == null)
            {
                if (qTrans.AccountID == qTrans.OppAccountID)
                {
                    // This is a one sided transaction (Opening Balance.)
                    newLine = this.getEasyLineData(qTrans);
                    newLine.oppAccountID = newLine.accountID;
                    newLine.envelopeID = this.importEnvelopeLines(qTrans, newLine.id);

                    this.ffDataSet.LineItem.AddLineItemRow(newLine);
                }
                else
                {
                    // Not one sided, add it to the list to be dealt with later.
                    this.qifTransfers.Add(qTrans);
                }

                return;
            }
         

            // If we get here this is a simple Transfer.
            newLine = this.getEasyLineData(qTrans);
            oppLine = this.getEasyLineData(oppTrans);

            // Finish newLine
            newLine.oppAccountID = oppLine.accountID;
            newLine.envelopeID = this.importEnvelopeLines(qTrans, newLine.id);

            // Finish the oppLine 
            oppLine.transactionID = newLine.transactionID;
            oppLine.oppAccountID = newLine.accountID;
            oppLine.envelopeID = this.importEnvelopeLines(oppTrans, oppLine.id);

            // Add the lines to the table
            this.ffDataSet.LineItem.AddLineItemRow(newLine);
            this.ffDataSet.LineItem.AddLineItemRow(oppLine);
        }

        private void importSpecialCaseTransactions()
        {
            List<QifTransaction> majorTransList = new List<QifTransaction>();

            // Gather in all the Major Halfs of the transactions. Those that have multiple oppLines
            foreach (QifTransaction majorTrans in this.qifTransfers)
            {   
                if (majorTrans.OppAccountID == SpclAccount.MULTIPLE)
                {
                    majorTransList.Add(majorTrans);

                    foreach (QifSplit split in majorTrans.OppLines)
                    {
                        int aID;
                        if (split.Catagory.StartsWith("["))
                        {
                            if (this.ffDataSet.myGetAccount(split.Catagory, out aID))
                                split.OppAccountID = aID;
                            else
                                throw new InvalidDataException("Invalid Account");
                        }
                        else
                        {
                            byte cat = (split.Amount > 0.0m) ? SpclAccountCat.INCOME : SpclAccountCat.EXPENSE;
                            split.OppAccountID = this.ffDataSet.myImportAccount(split.Catagory, "", cat);
                        }
                    }
                }
            }

            foreach (QifTransaction majorTrans in majorTransList)
            {
                // Condense duplicate [Accounts]
                for (int x = 0; x < majorTrans.OppLines.Count - 1; x++)
                {
                    for (int y = x + 1; y < majorTrans.OppLines.Count; y++)
                    {
                        if (majorTrans.OppLines[x].OppAccountID == majorTrans.OppLines[y].OppAccountID)
                        {
                            string newMemo; 
                            newMemo  = majorTrans.OppLines[x].Memo + " ";
                            newMemo += majorTrans.OppLines[x].Amount.ToString("C2") + ", ";
                            newMemo += majorTrans.OppLines[y].Memo + " ";
                            newMemo += majorTrans.OppLines[y].Amount.ToString("C2") + " ";

                            majorTrans.OppLines[x].Memo = newMemo;
                            majorTrans.OppLines[x].Amount += majorTrans.OppLines[y].Amount;

                            majorTrans.OppLines.RemoveAt(y);
                            y--;
                        }
                    }
                }

                // Pull out the payee if it exists
                QifSplit payee = null;
                foreach (QifSplit split in majorTrans.OppLines)
                    if (!split.Catagory.StartsWith("["))
                    {
                        payee = split;
                        break;
                    }

                this.importQIFTransfers(majorTrans, payee);
            }
        }

        private void importQIFTransfers(QifTransaction majorTrans, QifSplit payee)
        {
            List<FFDataSet.LineItemRow> transLines = new List<FFDataSet.LineItemRow>();
            List<QifTransaction> oppTransList = new List<QifTransaction>();

            // Match up
            foreach (QifSplit split in majorTrans.OppLines)
            {
                // Skip the payee oppline
                if (split.Catagory.StartsWith("["))
                {
                    foreach (QifTransaction minorTrans in this.qifTransfers)
                    {
                        if (majorTrans.Date.Equals(minorTrans.Date)
                            && majorTrans.AccountID.Equals(minorTrans.OppAccountID)
                            && split.OppAccountID.Equals(minorTrans.AccountID)
                            && split.Amount.Equals(Decimal.Negate(minorTrans.Amount)))
                            oppTransList.Add(minorTrans);
                    }
                }
            }

            // Complex transfer between more than two accounts.
            if (payee == null && majorTrans.OppLines.Count == oppTransList.Count)
            {
                // Finish the MajorLine
                FFDataSet.LineItemRow majorLine = this.getEasyLineData(majorTrans);

                majorLine.envelopeID = this.importEnvelopeLines(majorTrans, majorLine.id);

                if(oppTransList.Count == 1)
                    majorLine.oppAccountID = oppTransList[0].AccountID;
                else
                    majorLine.oppAccountID = SpclAccount.MULTIPLE;

                this.ffDataSet.LineItem.AddLineItemRow(majorLine);
                transLines.Add(majorLine);

                this.qifTransfers.Remove(majorTrans);

                // Finish the MinorLines 
                foreach (QifTransaction minorTrans in oppTransList)
                {
                    FFDataSet.LineItemRow minorLine = this.getEasyLineData(minorTrans);

                    minorLine.transactionID = majorLine.transactionID;
                    minorLine.oppAccountID = majorLine.accountID;
                    minorLine.envelopeID = this.importEnvelopeLines(minorTrans, minorLine.id);

                    this.ffDataSet.LineItem.AddLineItemRow(minorLine);
                    transLines.Add(minorLine);

                    this.qifTransfers.Remove(minorTrans);
                }
            }

            // Complex transfer between atleast two accounts and a payee
            if (payee != null && majorTrans.OppLines.Count == oppTransList.Count + 1)
            {
                // Finish the MajorLine
                FFDataSet.LineItemRow majorLine = this.getEasyLineData(majorTrans);

                majorLine.oppAccountID = SpclAccount.MULTIPLE;
                majorLine.envelopeID = this.importEnvelopeLines(majorTrans, majorLine.id);

                this.ffDataSet.LineItem.AddLineItemRow(majorLine);
                transLines.Add(majorLine);

                this.qifTransfers.Remove(majorTrans);

                // Make the Payee Line
                FFDataSet.LineItemRow payeeLine = this.getEasyLineData(majorTrans);
                payeeLine.transactionID = majorLine.transactionID;
                payeeLine.date = majorLine.date;
                payeeLine.typeID = majorLine.typeID;
                payeeLine.oppAccountID = majorLine.accountID;
                payeeLine.confirmationNumber = majorLine.confirmationNumber;
                payeeLine.complete = majorLine.complete;
                payeeLine.envelopeID = SpclEnvelope.NULL;
                payeeLine.creditDebit = !majorLine.creditDebit;

                payeeLine.accountID = payee.OppAccountID;
                payeeLine.description = payee.Memo;

                if (payee.Amount > 0.0m)
                {
                    payeeLine.amount = payee.Amount;
                }
                else
                {
                    payeeLine.amount = Decimal.Negate(payee.Amount);
                }

                this.ffDataSet.LineItem.AddLineItemRow(payeeLine);
                transLines.Add(payeeLine);

                // Finish the MinorLines 
                foreach (QifTransaction minorTrans in oppTransList)
                {
                    FFDataSet.LineItemRow minorLine = this.getEasyLineData(minorTrans);

                    minorLine.transactionID = majorLine.transactionID;
                    minorLine.oppAccountID = majorLine.accountID;
                    minorLine.envelopeID = this.importEnvelopeLines(minorTrans, minorLine.id);

                    this.ffDataSet.LineItem.AddLineItemRow(minorLine);
                    transLines.Add(minorLine);

                    this.qifTransfers.Remove(minorTrans);
                }

                // Make sure the oppAccount was set correctly
                int debitCount = 0;
                int creditCount = 0;
                int debitAccountID = SpclAccount.NULL;
                int creditAccountID = SpclAccount.NULL;

                // Count things up
                foreach (FFDataSet.LineItemRow line in transLines)
                {
                    if (line.creditDebit == LineCD.CREDIT)
                    {
                        creditCount++;
                        creditAccountID = line.accountID;
                    }
                    else
                    {
                        debitCount++;
                        debitAccountID = line.accountID;
                    }
                }

                // Set correct values
                foreach (FFDataSet.LineItemRow line in transLines)
                {
                    if (line.creditDebit == LineCD.CREDIT && debitCount == 1)
                        line.oppAccountID = debitAccountID;

                    else if (line.creditDebit == LineCD.CREDIT && debitCount > 1)
                        line.oppAccountID = SpclAccount.MULTIPLE;

                    else if (line.creditDebit == LineCD.DEBIT && creditCount == 1)
                        line.oppAccountID = creditAccountID;

                    else if (line.creditDebit == LineCD.DEBIT && creditCount > 1)
                        line.oppAccountID = SpclAccount.MULTIPLE;

                }


            }
        }

        private void importTheRest()
        {
            if (this.qifTransfers.Count > 0)
                throw new Exception("Unable to match some transactions.");
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Phase 3 - Save the data to the Database.
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void setAccountEnvelopes()
        {
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(Properties.Resources.SetAccountEnvelopes, connection);
            connection.Open();

            command.ExecuteNonQuery();

            // Always call Close the reader and connection when done reading
            connection.Close();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Public Functions
        ////////////////////////////////////////////////////////////////////////////////////////////
        public ImportQIF(string filePath)
        {
            this.fileName = filePath;

            this.ffDataSet = new FFDataSet();
            this.ffDataSet.myInit();

            this.qifAccounts = new List<QifAccount>();
            this.qifTransfers = new List<QifTransaction>();

            // Setup the worker
            this.importWorker = new BackgroundWorker();
            this.importWorker.WorkerReportsProgress = true;
            this.importWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(importWorker_RunWorkerCompleted);
            this.importWorker.ProgressChanged += new ProgressChangedEventHandler(importWorker_ProgressChanged);
            this.importWorker.DoWork += new DoWorkEventHandler(importWorker_DoWork);
        }

        public void RunAsync()
        {
            this.importWorker.RunWorkerAsync();
        }


    }
}
