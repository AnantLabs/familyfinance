using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class SubLineViewDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private FFDBDataSetTableAdapters.SubLineViewTableAdapter thisTableAdapter;


            ///////////////////////////////////////////////////////////////////////
            //   Properties
            ///////////////////////////////////////////////////////////////////////



            ///////////////////////////////////////////////////////////////////////
            //   Function Overrides
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.thisTableAdapter = new FFDBDataSetTableAdapters.SubLineViewTableAdapter();
                this.thisTableAdapter.ClearBeforeFill = true;

            }


            ///////////////////////////////////////////////////////////////////////
            //   Function Private
            ///////////////////////////////////////////////////////////////////////
            private void myFillCDandOther()
            {
                decimal balance = 0.0m;

                foreach (SubLineViewRow row in this)
                {
                    if (row.creditDebit == LineCD.CREDIT)
                    {
                        balance -= row.amount;
                        row.balanceAmount = balance;
                        row.creditAmount = row.amount;
                        row.SetdebitAmountNull();

                    }
                    else
                    {
                        balance += row.amount;
                        row.balanceAmount = balance;
                        row.debitAmount = row.amount;
                        row.SetcreditAmountNull();

                        string swap = row.sourceAccount;
                        row.sourceAccount = row.destinationAccount;
                        row.destinationAccount = swap;
                    }
                }

                this.AcceptChanges();
            }
            

            ///////////////////////////////////////////////////////////////////////
            //   Function Public
            ///////////////////////////////////////////////////////////////////////
            public void myFillTAByEnvelopeAndAccount(short envelopeID, short accountID)
            {
                this.thisTableAdapter.FillByEnvelopeAndAccount(this, envelopeID, accountID);
                this.myFillCDandOther();
            }

            public void myFillTAByEnvelope(short envelopeID)
            {
                this.thisTableAdapter.FillByEnvelope(this, envelopeID);
                this.myFillCDandOther();
            }
            

            // Do not add functions that pull information from this table because
            // this table is not garenteed to be current.
            // Pull information from LineItem and SubLineItem instead.

        }// END endpartial class SubLineViewDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance
