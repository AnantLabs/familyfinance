using FamilyFinance2.Forms.Main.RegistrySplit.Register.SubLineDataSetTableAdapters;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main.RegistrySplit.Register 
{
    public partial class SubLineDataSet 
    {
        private SubLineViewTableAdapter subLineViewTA;


        public void myInit()
        {
            this.subLineViewTA = new SubLineViewTableAdapter();
            this.subLineViewTA.ClearBeforeFill = true;
        }

        public void myFill(int accountID, int envelopeID)
        {
            if (accountID == SpclAccount.NULL && envelopeID == SpclEnvelope.NULL)
                this.SubLineView.Clear();

            else if (accountID == SpclAccount.NULL)

                this.subLineViewTA.FillByEnvelope(this.SubLineView, envelopeID);

            else
                this.subLineViewTA.FillByAccountAndEnvelope(this.SubLineView, accountID, envelopeID);


            decimal bal = 0.0m;

            foreach (SubLineViewRow row in this.SubLineView)
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
    }
}

