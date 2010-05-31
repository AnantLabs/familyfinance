using FamilyFinance2.Forms.Main.RegistrySplit.Register.RegistryDataSetTableAdapters;
using FamilyFinance2.Forms.Transaction;
using FamilyFinance2.SharedElements;


namespace FamilyFinance2.Forms.Main.RegistrySplit.Register 
{
    public partial class RegistryDataSet 
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private TransactionDataSet tDataSet;

        private AccountTableAdapter accountTA;
        private EnvelopeTableAdapter envelopeTA;
        private LineItemTableAdapter lineTA;
        private LineTypeTableAdapter lineTypeTA;
        private EnvelopeLineViewTableAdapter envelopeLineViewTA;

        private int CurrentLineID;


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
            if (accountID == SpclAccount.NULL)
                this.LineItem.Clear();
            else
                this.lineTA.FillByAccount(this.LineItem, accountID);

            decimal bal = 0.0m;

            if (this.Account.FindByid(accountID).creditDebit == LineCD.DEBIT)
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

        public void myFillLines(int accountID, int envelopeID)
        {
            if (accountID == SpclAccount.NULL && envelopeID == SpclEnvelope.NULL)
                this.EnvelopeLineView.Clear();

            else if (accountID == SpclAccount.NULL)

                this.envelopeLineViewTA.FillByEnvelope(this.EnvelopeLineView, envelopeID);

            else
                this.envelopeLineViewTA.FillByAccountAndEnvelope(this.EnvelopeLineView, accountID, envelopeID);


            decimal bal = 0.0m;

            foreach (EnvelopeLineViewRow row in this.EnvelopeLineView)
            {
                if (row.creditDebit == LineCD.CREDIT)
                {
                    row.creditAmount = row.amount;
                    bal -= row.amount;
                    row.balanceAmount = bal;
                }
                else
                {
                    row.debitAmount = row.amount;
                    bal += row.amount;
                    row.balanceAmount = bal;

                    string temp = row.sourceAccount;
                    row.sourceAccount = row.destinationAccount;
                    row.destinationAccount = temp;
                }
            }


        }


        public void myDeleteLine(int lineID)
        {
            int tID = this.LineItem.FindByid(lineID).transactionID;

            this.tDataSet.myFillLineItemAndSubLine(tID);
            this.tDataSet.myDeleteLine(lineID);
            this.tDataSet.myCheckTransaction();
            this.tDataSet.mySaveChanges();

            foreach (LineItemRow row in this.LineItem)
            {
                if (row.transactionID == tID)
                {
                    row.Delete();
                    row.AcceptChanges();
                }
            }
        }

        public void myEditLine(int lineID)
        {
            if (lineID != this.CurrentLineID)
            {
                this.CurrentLineID = lineID;
                this.tDataSet.myFillLineItemAndSubLine(this.LineItem.FindByid(lineID).transactionID);
            }
        }

        public void myFinishEdit()
        {
            this.tDataSet.myForwardLineEdits(this.LineItem.FindByid(this.CurrentLineID));
        }

    }
}

