using System.Data;
using System.Collections.Generic;
using FamilyFinance2.Forms.EditAccounts.EADataSetTableAdapters;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.EditAccounts 
{
    public partial class EADataSet 
    {
        //////////////////////////
        //   Local Variables
        private AccountTableAdapter AccountTA;
        private AccountTypeTableAdapter AccountTypeTA;

        public List<AccountCatagory> AccountCatagoryList;


        /////////////////////////
        //   Functions Public 
        public void myInit()
        {
            this.AccountTA = new AccountTableAdapter();
            this.AccountTA.ClearBeforeFill = true;

            this.AccountTypeTA = new AccountTypeTableAdapter();
            this.AccountTypeTA.ClearBeforeFill = true;

            this.AccountCatagoryList = new List<AccountCatagory>();
            this.AccountCatagoryList.Add(new AccountCatagory(SpclAccountCat.INCOME, "Income"));
            this.AccountCatagoryList.Add(new AccountCatagory(SpclAccountCat.ACCOUNT, "Account"));
            this.AccountCatagoryList.Add(new AccountCatagory(SpclAccountCat.EXPENSE, "Expense"));
        }

        public void myUpdateAccountDB()
        {
            this.AccountTA.Update(this.Account);
        }

        public void myFillAccountTable()
        {
            this.AccountTA.Fill(this.Account);
            this.Account.myResetID();
        }

        public void myFillAccountTypeTable()
        {
            this.AccountTypeTA.Fill(this.AccountType);
        }


        ///////////////////////////////////////////////////////////////////////
        //   Account Catagory Data
        ///////////////////////////////////////////////////////////////////////
        public class AccountCatagory
        {
            public byte ID { get; set; }
            public string Name { get; set; }

            public AccountCatagory(byte id, string name)
            {
                ID = id;
                Name = name;
            }
        }


        ///////////////////////////////////////////////////////////////////////
        //   Account Data Table 
        ///////////////////////////////////////////////////////////////////////
        public partial class AccountDataTable
        {
            //////////////////////////
            //   Local Variables
            private int newID;
            private bool stayOut;


            //////////////////////////
            //   Overriden Functions 
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(AccountDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(AccountDataTable_ColumnChanged);

                this.newID = 1;
                this.stayOut = false;
            }


            /////////////////////////
            //   Internal Events
            private void AccountDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                stayOut = true;
                AccountRow accountRow = e.Row as AccountRow;

                accountRow.id = this.newID++;
                accountRow.name = "New Account";
                accountRow.accountTypeID = SpclAccountType.NULL;
                accountRow.catagoryID = SpclAccountCat.ACCOUNT;
                accountRow.closed = false;
                accountRow.creditDebit = LineCD.DEBIT;
                accountRow.envelopes = false;
                accountRow.endingBalance = 0.0m;

                stayOut = false;
            }

            private void AccountDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                if (stayOut)
                    return;

                stayOut = true;
                AccountRow row = e.Row as AccountRow;
                string tmp;
                int maxLen;

                if (e.Column.ColumnName == "name")
                {
                    tmp = e.ProposedValue as string;
                    maxLen = this.nameColumn.MaxLength;

                    if (tmp.Length > maxLen)
                        row.name = tmp.Substring(0, maxLen);
                }

                stayOut = false;
            }


            /////////////////////////
            //   Functions Public 
            public void myResetID()
            {
                this.newID = FFDataBase.myDBGetNewID("id", "Account");
            }

        }


        ///////////////////////////////////////////////////////////////////////
        //   Account Type Data Table 
        ///////////////////////////////////////////////////////////////////////
        //public partial class AccountTypeDataTable
        //{
        //    // Nothing special needed
        //}

    }
}
