using FamilyFinance2.Forms.Main.RegistrySplit.Register.RegistryDataSetTableAdapters;
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

        private AccountTableAdapter accountTA;
        private EnvelopeTableAdapter envelopeTA;
        private LineItemTableAdapter lineTA;
        private LineTypeTableAdapter lineTypeTA;
        private EnvelopeLineViewTableAdapter envelopeLineViewTA;


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


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public void myInit()
        {
            // Setup this dataset
            this.accountTA = new AccountTableAdapter();
            this.accountTA.ClearBeforeFill = true;

            this.envelopeTA = new EnvelopeTableAdapter();
            this.envelopeTA.ClearBeforeFill = true;

            this.lineTA = new LineItemTableAdapter();
            this.lineTA.ClearBeforeFill = true;

            this.lineTypeTA = new LineTypeTableAdapter();
            this.lineTypeTA.ClearBeforeFill = true;

            this.envelopeLineViewTA = new EnvelopeLineViewTableAdapter();
            this.envelopeLineViewTA.ClearBeforeFill = true;

            // Setup the TransactionDataset
            this.tDataSet = new TransactionDataSet();
            this.tDataSet.myInit();

            this.e_Finder = new BackgroundWorker();
            this.e_Finder.RunWorkerCompleted += new RunWorkerCompletedEventHandler(e_Finder_RunWorkerCompleted);
            this.e_Finder.DoWork += new DoWorkEventHandler(e_Finder_DoWork);

            //this.CurrentTransID = -1;
        }

        public void myFillAccountTable()
        {
            this.accountTA.Fill(this.Account);
        }

        public void myFillEnvelopeTable()
        {
            this.envelopeTA.Fill(this.Envelope);
        }

        public void myFillLineTypeTable()
        {
            this.lineTypeTA.Fill(this.LineType);
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
            this.tDataSet.myDeleteTransaction();
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
            this.tDataSet.myForwardLineEdits(line);
            this.tDataSet.mySaveChanges();

            this.LineItem.AcceptChanges();
        }

        public List<AEPair> myGetChanges()
        {
            return this.tDataSet.myGetChanges();
        }

    }
}

