using FamilyFinance2.Forms.Main.RegistrySplit.RegistryDataSetTableAdapters;
using FamilyFinance2.Forms.Transaction;
using FamilyFinance2.SharedElements;
using System.ComponentModel;
using System.Collections.Generic;
using System;

namespace FamilyFinance2.Forms.Main.RegistrySplit
{
    public partial class RegistryDataSet
    {        
        ///////////////////////////////////////////////////////////////////////
        //   Error Finder
        ///////////////////////////////////////////////////////////////////////
        private BackgroundWorker e_Finder;
        private List<int> e_TransactionErrors;
        private List<int> e_LineErrors;


        private void findErrors(int accountID)
        {
            if (!e_Finder.IsBusy)
                e_Finder.RunWorkerAsync(accountID);
        }

        private void e_Finder_DoWork(object sender, DoWorkEventArgs e)
        {
            int accountID = (int) e.Argument;
            e_TransactionErrors = DBquery.getTransactionErrors(accountID);
            e_LineErrors = DBquery.getLineErrors(accountID);
        }

        private void e_Finder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                throw new Exception("Finding Transaction error exception", e.Error);

            if (stayOut)
                return;
            else
                stayOut = true;

            foreach (int lineID in e_LineErrors)
            {
                LineItemRow line = this.LineItem.FindByid(lineID);
                if (line.RowState == System.Data.DataRowState.Unchanged)
                {
                    line.lineError = true;
                    line.AcceptChanges();
                }
                else
                    line.lineError = true;
            }

            foreach (int lineID in e_TransactionErrors)
            {
                LineItemRow line = this.LineItem.FindByid(lineID);
                if (line.RowState == System.Data.DataRowState.Unchanged)
                {
                    line.transactionError = true;
                    line.AcceptChanges();
                }
                else
                    line.transactionError = true;
            }

            this.OnErrorsFound(new EventArgs());
            stayOut = false;
        }


        ///////////////////////////////////////////////////////////////////////
        //   External Events
        ///////////////////////////////////////////////////////////////////////   
        public event EventHandler ErrorsFound;
        private void OnErrorsFound(EventArgs e)
        {
            if (ErrorsFound != null)
                ErrorsFound(this, e);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private TransactionDataSet tDataSet;
        private int currentAccountID = SpclAccount.NULL;
        private int currentEnvelopeID = SpclEnvelope.NULL;

        private LineItemTableAdapter lineTA;
        private EnvelopeLineViewTableAdapter envelopeLineViewTA;

        // local referances to the tables in the transactionDataSet
        public TransactionDataSet.AccountDataTable Account;
        public TransactionDataSet.EnvelopeDataTable Envelope;
        public TransactionDataSet.LineTypeDataTable LineType;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Private table events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private bool stayOut;

        private void LineItem_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            stayOut = true;
            LineItemRow newLine = e.Row as LineItemRow;

            newLine.id = DBquery.getNewID("id", "LineItem");
            newLine.transactionID = DBquery.getNewID("transactionID", "LineItem");
            newLine.date = DateTime.Today;
            newLine.typeID = SpclLineType.NULL;
            newLine.accountID = currentAccountID;
            newLine.oppAccountID = SpclAccount.NULL;
            newLine.description = "";
            newLine.confirmationNumber = "";
            newLine.envelopeID = SpclEnvelope.NULL;
            newLine.complete = LineState.PENDING;
            newLine.amount = 0.00m;
            newLine.creditDebit = LineCD.CREDIT;
            newLine.lineError = false;
            newLine.transactionError = false;

            stayOut = false;
        }

        private void LineItem_ColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
        {
            if (stayOut)
                return;
            else
                stayOut = true;
            
            LineItemRow row = e.Row as LineItemRow;

            if (e.Column == this.LineItem.creditAmountColumn)
            {
                decimal newAmount = Convert.ToDecimal(e.ProposedValue);
                newBalance(row, newAmount, LineCD.CREDIT);
            }
            else if (e.Column == this.LineItem.debitAmountColumn)
            {
                decimal newAmount = Convert.ToDecimal(e.ProposedValue);
                newBalance(row, newAmount, LineCD.DEBIT);
            }

            stayOut = false;
        }

        private void newBalance(LineItemRow row, decimal newAmount, bool newCD)
        {
            if (newAmount < 0.0m)
            {
                newAmount = Decimal.Negate(newAmount);
                newCD = !newCD;
            }

            newAmount = Decimal.Round(newAmount, 2);

            if (row.amount != newAmount)
                row.amount = newAmount;

            if (row.creditDebit != newCD)
                row.creditDebit = newCD;

            if (newCD == LineCD.CREDIT)
            {
                row.creditAmount = newAmount;
                row.SetdebitAmountNull();
            }
            else
            {
                row.debitAmount = newAmount;
                row.SetcreditAmountNull();
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Private Forwarding line edits
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void myForwardLineEdits(LineItemRow rLine)
        {
            TransactionDataSet.LineItemRow tLine = this.tDataSet.LineItem.FindByid(rLine.id);

            if (tLine == null)
                tLine = this.myNewTransaction(rLine);

            else
            {
                // Copy the simple items.
                if (tLine.date != rLine.date)
                    tLine.date = rLine.date;

                if (tLine.typeID != rLine.typeID)
                    tLine.typeID = rLine.typeID;

                if (tLine.description != rLine.description)
                    tLine.description = rLine.description;

                if (tLine.confirmationNumber != rLine.confirmationNumber)
                    tLine.confirmationNumber = rLine.confirmationNumber;

                if (tLine.complete != rLine.complete)
                    tLine.complete = rLine.complete;

                // The following affect other lines / envelopeLines 
                if (tLine.oppAccountID != rLine.oppAccountID)
                    this.myChangeOppAccountID(ref tLine, rLine.oppAccountID);

                if (tLine.creditDebit != rLine.creditDebit)
                    this.myChangeCreditDebit(ref tLine);

                if (tLine.amount != rLine.amount)
                    this.myChangeAmount(ref tLine, rLine.amount);

                if (tLine.accountID != rLine.accountID)
                    this.myChangeAccountID(ref tLine, rLine.accountID);

                if (tLine.envelopeID != rLine.envelopeID)
                    this.myChangeEnvelopeID(ref tLine, rLine.envelopeID);
            }

            // Update the error flags
            this.tDataSet.myCheckTransaction();

            this.stayOut = true;

            rLine.transactionError = this.tDataSet.TransactionError;
            rLine.lineError = tLine.lineError;

            this.stayOut = false;
        }

        private TransactionDataSet.LineItemRow myNewTransaction(LineItemRow rLine)
        {
            stayOut = true;

            TransactionDataSet.LineItemRow tLine = this.tDataSet.LineItem.NewLineItemRow();

            // Initially assume this is a single line transaction.
            tLine.id = rLine.id;
            tLine.transactionID = rLine.transactionID;
            tLine.date = rLine.date;
            tLine.typeID = rLine.typeID;
            tLine.description = rLine.description;
            tLine.confirmationNumber = rLine.confirmationNumber;
            tLine.complete = rLine.complete;
            tLine.amount = rLine.amount;
            tLine.creditDebit = rLine.creditDebit;
            tLine.accountID = rLine.accountID;
            tLine.oppAccountID = rLine.oppAccountID;

            if (tLine.envelopeID != rLine.envelopeID)
                this.myChangeEnvelopeID(ref tLine, rLine.envelopeID);

            this.tDataSet.LineItem.AddLineItemRow(tLine);

            // if not a sigle line make the other half.
            if (rLine.accountID != rLine.oppAccountID)
            {
                TransactionDataSet.LineItemRow tOppLine = this.tDataSet.LineItem.NewLineItemRow();

                tOppLine.id = rLine.id + 1;
                tOppLine.transactionID = rLine.transactionID;
                tOppLine.date = rLine.date;
                tOppLine.typeID = rLine.typeID;
                tOppLine.description = rLine.description;
                tOppLine.confirmationNumber = rLine.confirmationNumber;
                tOppLine.complete = rLine.complete;
                tOppLine.amount = rLine.amount;
                tOppLine.creditDebit = !rLine.creditDebit;
                tOppLine.oppAccountID = rLine.accountID;
                tOppLine.accountID = rLine.oppAccountID;

                if (tLine.envelopeID != rLine.envelopeID)
                    this.myChangeEnvelopeID(ref tOppLine, rLine.envelopeID);

                this.tDataSet.LineItem.AddLineItemRow(tOppLine);
            }

            stayOut = false;
            return tLine;
        }
        
        private void myChangeCreditDebit(ref TransactionDataSet.LineItemRow lineBeingChange)
        {
            int thisSideCount = 0;
            int oppLineCount = 0;

            // Count how many lines have the same creditDebit as the line and how many lines
            // are opposite this line.
            foreach (TransactionDataSet.LineItemRow oppLine in this.tDataSet.LineItem)
            {
                if (oppLine.creditDebit == lineBeingChange.creditDebit)
                    thisSideCount++;
                else
                    oppLineCount++;
            }

            // If this is a simple transaction update both creditDebits
            if (thisSideCount == 1 && oppLineCount == 1)
            {
                this.tDataSet.LineItem[0].creditDebit = !this.LineItem[0].creditDebit;
                this.tDataSet.LineItem[1].creditDebit = !this.LineItem[1].creditDebit;
            }
            else // else just update the lineBeingchanged
                lineBeingChange.creditDebit = !lineBeingChange.creditDebit;
        }

        private void myChangeAmount(ref TransactionDataSet.LineItemRow lineBeingChange, decimal newAmount)
        {
            int thisSideCount = 0;
            int oppLineCount = 0;
            int envCount;
            int eLineID;

            // Count how many lines have the sam creditDebit as the line and how many lines
            // are opposite this line.
            foreach (TransactionDataSet.LineItemRow oppLine in this.tDataSet.LineItem)
            {
                if (oppLine.creditDebit == lineBeingChange.creditDebit)
                    thisSideCount++;
                else
                    oppLineCount++;
            }

            // If this is a simple transaction update both amounts
            if (thisSideCount == 1 && oppLineCount == 1)
            {
                this.tDataSet.LineItem[0].amount = newAmount;
                this.tDataSet.LineItem[1].amount = newAmount;

                // Now update the envelopeLines with the new amount;
                this.tDataSet.EnvelopeLine.myEnvelopeLineCount(this.tDataSet.LineItem[0].id, out envCount, out eLineID);
                if (envCount == 1)
                    this.tDataSet.EnvelopeLine.FindByid(eLineID).amount = newAmount;

                this.tDataSet.EnvelopeLine.myEnvelopeLineCount(this.tDataSet.LineItem[1].id, out envCount, out eLineID);
                if (envCount == 1)
                    this.tDataSet.EnvelopeLine.FindByid(eLineID).amount = newAmount;
            }

            // else just update the lineBeingchanged
            else
            {
                lineBeingChange.amount = newAmount;

                // Now update the envelopeLines with the new amount;
                this.tDataSet.EnvelopeLine.myEnvelopeLineCount(lineBeingChange.id, out envCount, out eLineID);
                if (envCount == 1)
                    this.tDataSet.EnvelopeLine.FindByid(eLineID).amount = newAmount;
            }
        }

        private void myChangeAccountID(ref TransactionDataSet.LineItemRow lineBeingChange, int newAccountID)
        {
            int thisSideCount = 0;

            // Count how many lines have the same creditDebit as the line.
            foreach (TransactionDataSet.LineItemRow oppLine in this.tDataSet.LineItem)
            {
                if (oppLine.creditDebit == lineBeingChange.creditDebit)
                    thisSideCount++;
            }

            // If thisSide has only one line (the given line) then update the oppLines.oppAccountID with the new
            // accountID
            if (thisSideCount == 1)
            {
                foreach (TransactionDataSet.LineItemRow oppLine in this.tDataSet.LineItem)
                    if (oppLine.creditDebit != lineBeingChange.creditDebit)
                        oppLine.oppAccountID = newAccountID;
            }

            // Make the change to the one being changed.
            lineBeingChange.accountID = newAccountID;
        }

        private void myChangeOppAccountID(ref TransactionDataSet.LineItemRow lineBeingChange, int newOppAccountID)
        {
            int oppLineCount = 0;

            // Count how many lines are opposite this line.
            foreach (TransactionDataSet.LineItemRow oppLine in this.tDataSet.LineItem)
            {
                if (oppLine.creditDebit != lineBeingChange.creditDebit)
                    oppLineCount++;
            }

            // If oppLine is only one then update all the lines. The lines on the same saide as the one
            // getting the update get there oppAccount ids updated. Then the line opposite the one being
            // changed will get its accountID changed.
            if (oppLineCount == 1)
            {
                foreach (TransactionDataSet.LineItemRow sameSideLine in this.tDataSet.LineItem)
                    if (sameSideLine.creditDebit == lineBeingChange.creditDebit)
                        sameSideLine.oppAccountID = newOppAccountID;
                    else
                        sameSideLine.accountID = newOppAccountID;
            }
        }

        private void myChangeEnvelopeID(ref TransactionDataSet.LineItemRow lineBeingChange, int newEnvelopeID)
        {
            int envCount;
            int eLineID;

            lineBeingChange.envelopeID = newEnvelopeID;

            // Now update the envelopeLine If there is only one with the new envelopeID;
            this.tDataSet.EnvelopeLine.myEnvelopeLineCount(lineBeingChange.id, out envCount, out eLineID);

            if (envCount == 1 && newEnvelopeID > SpclEnvelope.NULL)
                this.tDataSet.EnvelopeLine.FindByid(eLineID).envelopeID = newEnvelopeID;

            else if (envCount == 1 && newEnvelopeID == SpclEnvelope.NULL)
                this.tDataSet.EnvelopeLine.FindByid(eLineID).Delete();

            else if (envCount == 0 && newEnvelopeID > SpclEnvelope.NULL)
            {
                TransactionDataSet.EnvelopeLineRow eLine = this.tDataSet.EnvelopeLine.NewEnvelopeLineRow();

                eLine.lineItemID = lineBeingChange.id;
                eLine.envelopeID = newEnvelopeID;
                eLine.description = lineBeingChange.description;
                eLine.amount = lineBeingChange.amount;

                this.tDataSet.EnvelopeLine.AddEnvelopeLineRow(eLine);
            }
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Private Duplicating a transaction
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void myDuplicateTransaction(DateTime newDate)
        {
            LineItemRow temp = this.LineItem.NewLineItemRow();

            int newTransID = temp.transactionID;
            int transSize = this.tDataSet.LineItem.Count;
            int envCount = this.tDataSet.EnvelopeLine.Count;

            // Copy the lines from this transaction.
            for (int tIndex = 0; tIndex < transSize; tIndex++)
            {
                TransactionDataSet.LineItemRow origTLine = this.tDataSet.LineItem[tIndex];
                TransactionDataSet.LineItemRow newTLine = this.tDataSet.LineItem.NewLineItemRow();

                newTLine.transactionID = newTransID;
                newTLine.date = newDate;
                newTLine.typeID = origTLine.typeID;
                newTLine.accountID = origTLine.accountID;
                newTLine.oppAccountID = origTLine.oppAccountID;
                newTLine.description = origTLine.description;
                newTLine.confirmationNumber = origTLine.confirmationNumber;
                newTLine.envelopeID = origTLine.envelopeID;
                newTLine.complete = LineState.PENDING;
                newTLine.amount = origTLine.amount;
                newTLine.creditDebit = origTLine.creditDebit;
                newTLine.lineError = origTLine.lineError;

                this.tDataSet.LineItem.AddLineItemRow(newTLine);

                // Copy the envelopelines for this line.
                for (int envIndex = 0; envIndex < envCount; envIndex++)
                {
                    TransactionDataSet.EnvelopeLineRow origEnvRow = this.tDataSet.EnvelopeLine[envIndex];

                    if (origEnvRow.lineItemID == origTLine.id)
                    {
                        TransactionDataSet.EnvelopeLineRow newEnvRow = this.tDataSet.EnvelopeLine.NewEnvelopeLineRow();

                        newEnvRow.lineItemID = newTLine.id;
                        newEnvRow.envelopeID = origEnvRow.envelopeID;
                        newEnvRow.description = origEnvRow.description;
                        newEnvRow.amount = origEnvRow.amount;

                        this.tDataSet.EnvelopeLine.AddEnvelopeLineRow(newEnvRow);
                    }
                }
            }

            // now copy back to the registry dataset
            for (int tIndex = transSize; tIndex < this.tDataSet.LineItem.Rows.Count; tIndex++)
            {
                TransactionDataSet.LineItemRow tLine = this.tDataSet.LineItem[tIndex];

                if (tLine.accountID == currentAccountID)
                {
                    LineItemRow rLine = this.LineItem.NewLineItemRow();

                    rLine.id = tLine.id;
                    rLine.transactionID = tLine.transactionID;
                    rLine.date = tLine.date;
                    rLine.typeID = tLine.typeID;
                    rLine.accountID = tLine.accountID;
                    rLine.oppAccountID = tLine.oppAccountID;
                    rLine.description = tLine.description;
                    rLine.confirmationNumber = tLine.confirmationNumber;
                    rLine.envelopeID = tLine.envelopeID;
                    rLine.complete = tLine.complete;
                    rLine.amount = tLine.amount;
                    rLine.creditDebit = tLine.creditDebit;
                    rLine.lineError = tLine.lineError;

                    this.LineItem.AddLineItemRow(rLine);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Private internal calculations
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void myCalcBalance()
        {
            if (this.stayOut)
                return;
            else
                this.stayOut = true;

            decimal bal = 0.0m;
            bool debitAccount = this.Account.FindByid(this.currentAccountID).creditDebit == LineCD.DEBIT;

            if (debitAccount)
            {
                foreach (LineItemRow row in this.LineItem)
                {
                    if (row.creditDebit == LineCD.CREDIT)
                        row.balanceAmount = bal -= row.creditAmount = row.amount;
                    else
                        row.balanceAmount = bal += row.debitAmount = row.amount;
                }
            }
            else
            {
                foreach (LineItemRow row in this.LineItem)
                {
                    if (row.creditDebit == LineCD.CREDIT)
                        row.balanceAmount = bal += row.creditAmount = row.amount;
                    else
                        row.balanceAmount = bal -= row.debitAmount = row.amount;
                }
            }

            this.stayOut = false;
        }

        private void myGetLineEdits(int transID)
        {
            stayOut = true;

            for (int index = 0; index < this.LineItem.Rows.Count; index++)
            {
                LineItemRow rLine = this.LineItem[index];
                bool found = false;

                if (rLine.transactionID != transID)
                    continue;

                foreach (TransactionDataSet.LineItemRow tLine in this.tDataSet.LineItem)
                {
                    if (tLine.id == rLine.id)
                    {
                        found = true;

                        rLine.date = tLine.date;
                        rLine.typeID = tLine.typeID;
                        rLine.accountID = tLine.accountID;
                        rLine.oppAccountID = tLine.oppAccountID;
                        rLine.description = tLine.description;
                        rLine.confirmationNumber = tLine.confirmationNumber;
                        rLine.envelopeID = tLine.envelopeID;
                        rLine.complete = tLine.complete;
                        rLine.amount = tLine.amount;
                        rLine.creditDebit = tLine.creditDebit;
                        rLine.lineError = tLine.lineError;
                        rLine.transactionError = this.tDataSet.TransactionError;
                    }
                }

                if (!found)
                {
                    rLine.Delete();
                    index--;
                }
            }

            stayOut = false;
        }




        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public void myInit()
        {
            // Setup the TransactionDataset
            this.tDataSet = new TransactionDataSet();
            this.tDataSet.myInit();

            // Setup this dataset
            this.lineTA = new LineItemTableAdapter();
            this.lineTA.ClearBeforeFill = true;

            this.envelopeLineViewTA = new EnvelopeLineViewTableAdapter();
            this.envelopeLineViewTA.ClearBeforeFill = true;

            this.LineItem.ColumnChanged += new System.Data.DataColumnChangeEventHandler(LineItem_ColumnChanged);
            this.LineItem.TableNewRow += new System.Data.DataTableNewRowEventHandler(LineItem_TableNewRow);
            this.stayOut = false;

            // Reference the tables in the transactionDataSet
            this.Account = this.tDataSet.Account;
            this.Envelope = this.tDataSet.Envelope;
            this.LineType = this.tDataSet.LineType;

            // Setup the error finder.
            this.e_Finder = new BackgroundWorker();
            this.e_Finder.RunWorkerCompleted += new RunWorkerCompletedEventHandler(e_Finder_RunWorkerCompleted);
            this.e_Finder.DoWork += new DoWorkEventHandler(e_Finder_DoWork);
        }

        public void myFillAccountTable()
        {
            this.tDataSet.myFillAccountTable();
        }

        public void myFillEnvelopeTable()
        {
            this.tDataSet.myFillEnvelopeTable();
        }

        public void myFillLineTypeTable()
        {
            this.tDataSet.myFillLineTypeTable();
        }

        public void myFillLines(int accountID)
        {
            this.currentAccountID = accountID;
            this.currentEnvelopeID = SpclEnvelope.NULL;

            if (accountID == SpclAccount.NULL)
                this.LineItem.Clear();
            
            else
            {
                this.lineTA.FillByAccount(this.LineItem, accountID);
                this.findErrors(accountID);
            }

            this.myCalcBalance();
        }

        public void myFillLines(int accountID, int envelopeID)
        {
            decimal bal = 0.0m;
            this.currentAccountID = accountID;
            this.currentEnvelopeID = envelopeID;

            if (accountID == SpclAccount.NULL && envelopeID == SpclEnvelope.NULL)
                this.EnvelopeLineView.Clear();

            else if (accountID == SpclAccount.NULL)
                this.envelopeLineViewTA.FillByEnvelope(this.EnvelopeLineView, envelopeID);

            else
                this.envelopeLineViewTA.FillByAccountAndEnvelope(this.EnvelopeLineView, accountID, envelopeID);


            foreach (EnvelopeLineViewRow  row in this.EnvelopeLineView)
            {
                // Change the polarity of negative Envelop Line amounts
                if (row.amount < 0.0m)
                {
                    row.creditDebit = !row.creditDebit;
                    row.amount = decimal.Negate(row.amount);
                }

                if (row.creditDebit == LineCD.CREDIT)
                {
                    row.balanceAmount = bal -= row.creditAmount = row.amount;
                }
                else
                {
                    string temp = row.sourceAccount;
                    row.sourceAccount = row.destinationAccount;
                    row.destinationAccount = temp;

                    row.balanceAmount = bal += row.debitAmount = row.amount;
                }
            }
        }

        public void myDeleteTransaction(int transID)
        {

            // Fill up the tDataset and delete the transaction;
            this.tDataSet.myFillLineItemAndSubLine(transID);

            // Delete the envelope lines in the transaction
            foreach (TransactionDataSet.EnvelopeLineRow eRow in this.tDataSet.EnvelopeLine)
                eRow.Delete();

            // Delete the lines in the transaction
            foreach (TransactionDataSet.LineItemRow eRow in this.tDataSet.LineItem)
                eRow.Delete();

            // Delete the transaction in the registries copy.
            foreach (LineItemRow line in this.LineItem)
                if (line.transactionID == transID)
                    line.Delete();

            // Save the changes
            this.tDataSet.mySaveChanges();
            this.LineItem.AcceptChanges(); // save so calculation can happen
            this.myCalcBalance();
            this.LineItem.AcceptChanges(); // save to accept the running total
 
        }

        public void myDuplicateTransaction(int transID, DateTime newDate)
        {
            this.tDataSet.myFillLineItemAndSubLine(transID);

            this.myDuplicateTransaction(newDate);

            this.tDataSet.mySaveChanges();
            this.myCalcBalance();
            this.LineItem.AcceptChanges();
        }

        public void mySaveSingleLineEdits(int lineID)
        {
            LineItemRow line = this.LineItem.FindByid(lineID);
            this.tDataSet.myFillLineItemAndSubLine(line.transactionID);

            this.myForwardLineEdits(line);

            this.tDataSet.mySaveChanges();
            this.myCalcBalance();
            this.LineItem.AcceptChanges();
        }

        public void myGetTransactionEdits(int transID)
        {
            this.tDataSet.myFillLineItemAndSubLine(transID);
            this.tDataSet.myCheckTransaction();

            this.myGetLineEdits(transID);
            
            this.myCalcBalance();
            this.LineItem.AcceptChanges();
        }

        public List<AEPair> myGetChanges()
        {
            return this.tDataSet.myGetChanges();
        }

    }
}


