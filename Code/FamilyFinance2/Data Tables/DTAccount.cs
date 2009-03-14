using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class AccountDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   LocalVariables
            ///////////////////////////////////////////////////////////////////////
            private bool autoChange;


            ///////////////////////////////////////////////////////////////////////
            //   Properties
            ///////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////
            //   Overriden Functions 
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new System.Data.DataTableNewRowEventHandler(AccountDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(AccountDataTable_ColumnChanged);

                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void AccountDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                AccountRow accountRow = e.Row as AccountRow;
                short newID = -1;

                if (this.Count > 0)
                    newID = Convert.ToInt16(this[this.Count - 1].id + 1);
                
                if (newID > 0)
                    accountRow.id = newID;
                else
                    accountRow.id = 1;

                accountRow.name = "";
                accountRow.accountTypeID = SpclAccountType.NULL;
                accountRow.catagoryID = SpclAccountCat.ACCOUNT;
                accountRow.closed = false;
                accountRow.creditDebit = LineCD.DEBIT;
                accountRow.envelopes = false;

            }

            private void AccountDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                AccountRow row;
                string tmp;
                int maxLen;

                if (autoChange == false)
                    return;

                autoChange = false;          

                row = e.Row as AccountRow;

                switch (e.Column.ColumnName)
                {
                    case "name":
                        {
                            tmp = e.ProposedValue as string;
                            maxLen = this.nameColumn.MaxLength;

                            if (tmp.Length > maxLen)
                                row.name = tmp.Substring(0, maxLen);

                            break;
                        }
                }

                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Functions Private 
            ///////////////////////////////////////////////////////////////////////
            

            ///////////////////////////////////////////////////////////////////////
            //   Functions Public 
            ///////////////////////////////////////////////////////////////////////


        }//END partial class AccountDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance



