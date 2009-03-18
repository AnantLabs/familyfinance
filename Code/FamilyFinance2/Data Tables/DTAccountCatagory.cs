using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class AccountCatagoryDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private FFDBDataSetTableAdapters.AccountCatagoryTableAdapter thisTableAdapter;


            ///////////////////////////////////////////////////////////////////////
            //   Properties
            ///////////////////////////////////////////////////////////////////////



            ///////////////////////////////////////////////////////////////////////
            //   Overriden Functions 
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.thisTableAdapter = new FFDBDataSetTableAdapters.AccountCatagoryTableAdapter();
                this.thisTableAdapter.ClearBeforeFill = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////
            //   Functions Public
            ///////////////////////////////////////////////////////////////////////
            public void myFillTA()
            { this.thisTableAdapter.Fill(this); }


        }// END partial class AccountTypeDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance
