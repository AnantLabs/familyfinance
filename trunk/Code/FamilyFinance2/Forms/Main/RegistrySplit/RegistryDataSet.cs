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
        //   Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void myCalcBalance()
        {
            decimal bal = 0.0m;
            bool debitAccount = this.Account.FindByid(this.currentAccountID).creditDebit == LineCD.DEBIT;
            bool showingEnvelopes = currentEnvelopeID != SpclEnvelope.NULL;

            if (debitAccount || showingEnvelopes)
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
        }


        ///////////////////////////////////////////
        // Used by the Registry dataset
        public void myDeleteTransaction()
        {
            // delete the envelope lines in the transaction
            while (this.tDataSet.EnvelopeLine.Rows.Count > 0)
                this.tDataSet.EnvelopeLine[0].Delete();

            // delete the lines in the transaction
            while (this.tDataSet.LineItem.Rows.Count > 0)
                this.tDataSet.LineItem[0].Delete();
        }

        private void myForwardLineEdits(LineItemRow rLine)
        {
            TransactionDataSet.LineItemRow tLine = this.tDataSet.LineItem.FindByid(rLine.id);

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


            // Update the error flags
            this.tDataSet.myCheckTransaction();
            rLine.transactionError = this.tDataSet.TransactionError;
            rLine.lineError = tLine.lineError;
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
                this.tDataSet.EnvelopeLine.myEnvelopeLineCount(this.LineItem[0].id, out envCount, out eLineID);
                if (envCount == 1)
                    this.tDataSet.EnvelopeLine.FindByid(eLineID).amount = newAmount;

                this.tDataSet.EnvelopeLine.myEnvelopeLineCount(this.LineItem[1].id, out envCount, out eLineID);
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
            this.currentAccountID = accountID;
            this.currentEnvelopeID = envelopeID;

            if (accountID == SpclAccount.NULL && envelopeID == SpclEnvelope.NULL)
                this.EnvelopeLineView.Clear();

            else if (accountID == SpclAccount.NULL)
                this.envelopeLineViewTA.FillByEnvelope(this.EnvelopeLineView, envelopeID);

            else
                this.envelopeLineViewTA.FillByAccountAndEnvelope(this.EnvelopeLineView, accountID, envelopeID);

            this.myCalcBalance();
        }

        public void myDeleteTransaction(int transID)
        {
            // Fill up the tDataset and delete the transaction;
            this.tDataSet.myFillLineItemAndSubLine(transID);
            this.myDeleteTransaction();
            this.tDataSet.mySaveChanges();

            foreach (LineItemRow line in this.LineItem)
                if (line.transactionID == transID)
                    line.Delete();

            this.myCalcBalance();

            // Save the changes
            this.LineItem.AcceptChanges();
        }

        public void mySaveSingleLineEdits(int lineID)
        {
            LineItemRow line = this.LineItem.FindByid(lineID);
            this.tDataSet.myFillLineItemAndSubLine(line.transactionID);
            this.myForwardLineEdits(line);
            this.tDataSet.mySaveChanges();

            this.LineItem.AcceptChanges();
        }

        public List<AEPair> myGetChanges()
        {
            return this.tDataSet.myGetChanges();
        }

    }
}

