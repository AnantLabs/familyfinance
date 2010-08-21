using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlServerCe;
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
        public List<AccountNormal> AccountNormalList;


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

            this.AccountNormalList = new List<AccountNormal>();
            this.AccountNormalList.Add(new AccountNormal(LineCD.CREDIT, "Credit"));
            this.AccountNormalList.Add(new AccountNormal(LineCD.DEBIT, "Debit"));
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
            public byte Id { get; set; }
            public string Name { get; set; }

            public AccountCatagory(byte id, string name)
            {
                this.Id = id;
                this.Name = name;
            }
        }


        ///////////////////////////////////////////////////////////////////////
        //   Account Normal Data
        ///////////////////////////////////////////////////////////////////////
        public class AccountNormal
        {
            public bool Id { get; set; }
            public string Name { get; set; }

            public AccountNormal(bool id, string name)
            {
                this.Id = id;
                this.Name = name;
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


            //////////////////////////
            //   Overriden Functions 
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(AccountDataTable_TableNewRow);
                this.ColumnChanging += new DataColumnChangeEventHandler(AccountDataTable_ColumnChanging);

                this.newID = 1;
            }

            
            /////////////////////////
            //   Internal Events
            private void AccountDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                AccountRow accountRow = e.Row as AccountRow;

                accountRow.id = this.newID++;
                accountRow.name = "New Account";
                accountRow.typeID = SpclAccountType.NULL;
                accountRow.catagory = SpclAccountCat.ACCOUNT;
                accountRow.closed = false;
                accountRow.creditDebit = LineCD.DEBIT;
                accountRow.envelopes = false;
            }

            private void AccountDataTable_ColumnChanging(object sender, DataColumnChangeEventArgs e)
            {
                if (e.Column.ColumnName == "name")
                {
                    string tmp = e.ProposedValue as string;
                    int maxLen = this.nameColumn.MaxLength;

                    if (tmp.Length > maxLen)
                        e.ProposedValue = tmp.Substring(0, maxLen);
                }
            }

            /////////////////////////
            //   Functions Public 
            public void myResetID()
            {
                this.newID = DBquery.getNewID("id", "Account");
            }
        }

    }
}
