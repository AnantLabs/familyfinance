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
        TransactionDataSet tDataSet;

        AccountTableAdapter accountTA;
        EnvelopeTableAdapter envelopeTA;
        LineItemTableAdapter lineTA;
        LineTypeTableAdapter lineTypeTA;
        SubLineViewTableAdapter subLineTA;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Private
        ////////////////////////////////////////////////////////////////////////////////////////////



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public void myInit()
        {
            this.tDataSet = new TransactionDataSet();

            this.accountTA = new AccountTableAdapter();
            this.accountTA.ClearBeforeFill = true;

            this.envelopeTA = new EnvelopeTableAdapter();
            this.envelopeTA.ClearBeforeFill = true;

            this.lineTA = new LineItemTableAdapter();
            this.lineTA.ClearBeforeFill = true;

            this.lineTypeTA = new LineTypeTableAdapter();
            this.lineTypeTA.ClearBeforeFill = true;

        }

        public void myFillAccountTable()
        {
            this.accountTA.Fill(this.Account);
        }

        public void myFillEnvelopeTable()
        {
            this.envelopeTA.Fill(this.Envelope);
        }

        public void myFillLineItemTablebyAccount(int accountID)
        {
            if (accountID == SpclAccount.NULL)
                this.LineItem.Clear();
            else
                this.lineTA.FillByAccount(this.LineItem, accountID);

            decimal bal = 0.0m;

            foreach (LineItemRow row in this.LineItem)
            {
                if (row.creditDebit == LineCD.CREDIT)
                {
                    bal -= row.amount;
                    row.creditAmount = row.amount;
                    row.balanceAmount = bal;
                }
                else
                {
                    bal += row.amount;
                    row.debitAmount = row.amount;
                    row.balanceAmount = bal;
                }
            }

        }

        public void myFillLineTypeTable()
        {
            this.lineTypeTA.Fill(this.LineType);
        }

    }
}

