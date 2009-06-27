﻿using System;
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
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private FFDBDataSetTableAdapters.AccountTypeTableAdapter thisTableAdapter;
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

                this.thisTableAdapter = new FFDBDataSetTableAdapters.AccountTypeTableAdapter();
                this.thisTableAdapter.ClearBeforeFill = true;

                this.TableNewRow += new DataTableNewRowEventHandler(AccountTypeDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(AccountTypeDataTable_ColumnChanged);

                newID = 1;
                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void AccountTypeDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                AccountTypeRow accountTypeRow = e.Row as AccountTypeRow;

                accountTypeRow.id = newID++;
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
            public void myFillTA()
            {
                this.thisTableAdapter.Fill(this);
                this.newID = Convert.ToInt16(FFDBDataSet.myDBGetNewID("id", "AccountType"));
            }

            public void mySaveChanges()
            {
                this.thisTableAdapter.Update(this);
            }

            //public int myAddType(string newTypeName)
            //{
            //    if (newTypeName == null)
            //        return SpclAccountType.NULL;

            //    foreach (AccountTypeRow row in this)
            //    {
            //        if (row.name == newTypeName)
            //            return row.id;
            //    }

            //    AccountTypeRow temp = this.NewAccountTypeRow();
            //    temp.name = newTypeName;
            //    this.Rows.Add(temp);
            //    this.thisTableAdapter.Update(temp);

            //    return temp.id;
            //}

        }// END partial class AccountTypeDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance