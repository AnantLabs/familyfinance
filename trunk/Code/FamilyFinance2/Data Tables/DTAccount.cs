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
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private FFDBDataSetTableAdapters.AccountTableAdapter thisTableAdapter;
            private short newID;
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

                this.thisTableAdapter = new FFDBDataSetTableAdapters.AccountTableAdapter();
                this.thisTableAdapter.ClearBeforeFill = true;

                this.TableNewRow += new DataTableNewRowEventHandler(AccountDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(AccountDataTable_ColumnChanged);

                newID = 1;
                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void AccountDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                AccountRow accountRow = e.Row as AccountRow;
                short id = Convert.ToInt16(FFDBDataSet.myDBGetNewID("id", "Account"));

                if (id > newID)
                {
                    accountRow.id = id++;
                    newID = id;
                }
                else
                    accountRow.id = newID++;

                accountRow.name = "";
                accountRow.accountTypeID = SpclAccountType.NULL;
                accountRow.catagoryID = SpclAccountCat.ACCOUNT;
                accountRow.closed = false;
                accountRow.creditDebit = LineCD.DEBIT;
                accountRow.envelopes = false;
                accountRow.endingBalance = 0.0m;
                accountRow.currentBalance = 0.0m;

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
            public void myFillTA()
            { this.thisTableAdapter.Fill(this); }

            public void myUpdateTA()
            {
                this.thisTableAdapter.Update(this);
            }



            public void myUpdateAccountEBUndo(short oldAccountID, bool oldCD, decimal oldAmount)
            {
                AccountRow row = FindByid(oldAccountID);

                // Undo the old Amount
                if (row.creditDebit == LineCD.DEBIT)
                {
                    if (oldCD == LineCD.CREDIT)
                        row.endingBalance += oldAmount;
                    else
                        row.endingBalance -= oldAmount;
                }
                else
                {
                    if (oldCD == LineCD.CREDIT)
                        row.endingBalance -= oldAmount;
                    else
                        row.endingBalance += oldAmount;
                }

                this.thisTableAdapter.Update(row);
            }

            public void myUpdateAccountEBDo(short newAccountID, bool newCD, decimal newAmount)
            {
                AccountRow row = FindByid(newAccountID);

                //  Update to the new amount
                if (row.creditDebit == LineCD.DEBIT)
                {
                    if (newCD == LineCD.CREDIT)
                        row.endingBalance -= newAmount;
                    else
                        row.endingBalance += newAmount;
                }
                else
                {
                    if (newCD == LineCD.CREDIT)
                        row.endingBalance += newAmount;
                    else
                        row.endingBalance -= newAmount;
                }

                this.thisTableAdapter.Update(row);
            }

            public void myUpdateAccountEBUndoDo(short oldAccountID, bool oldCD, decimal oldAmount, short newAccountID, bool newCD, decimal newAmount)
            {
                AccountRow oldRow = FindByid(oldAccountID);
                AccountRow newRow = FindByid(newAccountID);

                // Undo the old Amount
                if (oldRow.creditDebit == LineCD.DEBIT)
                {
                    if (oldCD == LineCD.CREDIT)
                        oldRow.endingBalance += oldAmount;
                    else
                        oldRow.endingBalance -= oldAmount;
                }
                else
                {
                    if (oldCD == LineCD.CREDIT)
                        oldRow.endingBalance -= oldAmount;
                    else
                        oldRow.endingBalance += oldAmount;
                }

                //  Update to the new amount
                if (newRow.creditDebit == LineCD.DEBIT)
                {
                    if (newCD == LineCD.CREDIT)
                        newRow.endingBalance -= newAmount;
                    else
                        newRow.endingBalance += newAmount;
                }
                else
                {
                    if (newCD == LineCD.CREDIT)
                        newRow.endingBalance += newAmount;
                    else
                        newRow.endingBalance -= newAmount;
                }

                if (oldAccountID == newAccountID)
                {
                    this.thisTableAdapter.Update(newRow);
                }
                else
                {
                    this.thisTableAdapter.Update(newRow);
                    this.thisTableAdapter.Update(oldRow);
                }

            }




        }//END partial class AccountDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance



