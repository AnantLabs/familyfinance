using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class AccountTypeDataTable
        {
            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Local Variables
            ////////////////////////////////////////////////////////////////////////////////////////////
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

                this.TableNewRow += new DataTableNewRowEventHandler(AccountTypeDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(AccountTypeDataTable_ColumnChanged);

                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void AccountTypeDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                AccountTypeRow accountTypeRow = e.Row as AccountTypeRow;
                int newID = -1;

                if (this.Count > 0)
                    newID = this[this.Count - 1].id + 1;

                if (newID > 0)
                    accountTypeRow.id = (short)newID;
                else
                    accountTypeRow.id = 1;

                accountTypeRow.name = "";
            }

            private void AccountTypeDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {

                AccountTypeRow row;
                string tmp;
                int maxLen;

                if (autoChange == false)
                    return;

                autoChange = false;

                row = e.Row as AccountTypeRow;

                if (e.Column.ColumnName == "name")
                {
                    tmp = e.ProposedValue as string;
                    maxLen = this.nameColumn.MaxLength;

                    if (tmp.Length > maxLen)
                        row.name = tmp.Substring(0, maxLen);
                }

                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Functions Public
            ///////////////////////////////////////////////////////////////////////
            public int myAddType(string newTypeName)
            {
                if (newTypeName == null)
                    return SpclAccountType.NULL;

                foreach (AccountTypeRow row in this)
                {
                    if (row.name == newTypeName)
                        return row.id;
                }

                AccountTypeRow temp = this.NewAccountTypeRow();
                temp.name = newTypeName;
                this.Rows.Add(temp);

                return temp.id;
            }

            public void myAddDefaultTypes(int viewID)
            {
                AccountTypeRow newType;

                //Account Types
                newType = this.NewAccountTypeRow();
                newType.name = "Checking";
                this.Rows.Add(newType);

                newType = this.NewAccountTypeRow();
                newType.name = "Savings";
                this.Rows.Add(newType);

                newType = this.NewAccountTypeRow();
                newType.name = "Loan";
                this.Rows.Add(newType);

                newType = this.NewAccountTypeRow();
                newType.name = "Credit Card";
                this.Rows.Add(newType);

                newType = this.NewAccountTypeRow();
                newType.name = "Cash";
                this.Rows.Add(newType);

                //Income Types
                newType = this.NewAccountTypeRow();
                newType.name = "Job";
                this.Rows.Add(newType);

                newType = this.NewAccountTypeRow();
                newType.name = "Other";
                this.Rows.Add(newType);

                //Expense Types
                newType = this.NewAccountTypeRow();
                newType.name = "Person";
                this.Rows.Add(newType);

                newType = this.NewAccountTypeRow();
                newType.name = "Store";
                this.Rows.Add(newType);

                newType = this.NewAccountTypeRow();
                newType.name = "Utility";
                this.Rows.Add(newType);

                newType = this.NewAccountTypeRow();
                newType.name = "Restraunt";
                this.Rows.Add(newType);

                newType = this.NewAccountTypeRow();
                newType.name = "Insurance";
                this.Rows.Add(newType);

                newType = this.NewAccountTypeRow();
                newType.name = "Grocery Store";
                this.Rows.Add(newType);

            }

        }// END partial class AccountTypeDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance
