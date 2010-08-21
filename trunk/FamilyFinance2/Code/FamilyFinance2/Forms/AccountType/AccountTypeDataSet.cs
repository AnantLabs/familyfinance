using System.Data;
using FamilyFinance2.Forms.AccountType.AccountTypeDataSetTableAdapters;
using FamilyFinance2.SharedElements;


namespace FamilyFinance2.Forms.AccountType 
{
    public partial class AccountTypeDataSet 
    {
        partial class AccountTypeDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private AccountTypeTableAdapter ACTableAdapter;
            private int newID;
            private bool stayOut;


            ///////////////////////////////////////////////////////////////////////
            //   Overriden Functions 
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();
                
                this.TableNewRow += new DataTableNewRowEventHandler(AccountTypeDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(AccountTypeDataTable_ColumnChanged);

                this.ACTableAdapter = new AccountTypeTableAdapter();
                this.ACTableAdapter.ClearBeforeFill = true;
            
                this.newID = 1;
                this.stayOut = false;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void AccountTypeDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                stayOut = true;
                AccountTypeRow newRow = e.Row as AccountTypeRow;

                newRow.id = this.newID++;
                newRow.name = "";

                stayOut = false;
            }

            private void AccountTypeDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                if (stayOut)
                    return;

                stayOut = true;

                AccountTypeRow row;
                string tmp;
                int maxLen;

                row = e.Row as AccountTypeRow;

                if (e.Column.ColumnName == "name")
                {
                    tmp = e.ProposedValue as string;
                    maxLen = this.nameColumn.MaxLength;

                    if (tmp.Length > maxLen)
                        row.name = tmp.Substring(0, maxLen);
                }

                stayOut = false;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Functions Private 
            ///////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////
            //   Functions Public 
            ///////////////////////////////////////////////////////////////////////
            public void myFillTable()
            {
                this.ACTableAdapter.Fill(this);
                this.newID = DBquery.getNewID("id", "AccountType");
            }

            public void myUpdateDB()
            {
                this.ACTableAdapter.Update(this);
            }

        }// END partial class AccountTypeDataTable
    }
}
