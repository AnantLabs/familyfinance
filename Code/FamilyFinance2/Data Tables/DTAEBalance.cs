using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class AEBalanceDataTable
        {
            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Local Variables
            ////////////////////////////////////////////////////////////////////////////////////////////
            private FFDBDataSetTableAdapters.AEBalanceTableAdapter thisTableAdapter;
            private int newID;

            
            ///////////////////////////////////////////////////////////////////////
            //   Properties
            ///////////////////////////////////////////////////////////////////////



            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Overriden Functions 
            ////////////////////////////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.thisTableAdapter = new FFDBDataSetTableAdapters.AEBalanceTableAdapter();
                this.thisTableAdapter.ClearBeforeFill = true;

                this.TableNewRow += new DataTableNewRowEventHandler(AEBalanceDataTable_TableNewRow);
                newID = 1;
            }


            ///////////////////////////////////////////////////////////////////////
            //   External Events
            ///////////////////////////////////////////////////////////////////////
 

            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Internal Events
            ////////////////////////////////////////////////////////////////////////////////////////////
            private void AEBalanceDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                AEBalanceRow aEBalanceRow = e.Row as AEBalanceRow;

                aEBalanceRow.id = newID++;
                aEBalanceRow.accountID = SpclAccount.NULL;
                aEBalanceRow.envelopeID = SpclEnvelope.NULL;
                aEBalanceRow.endingBalance = 0.0m;
                aEBalanceRow.currentBalance = 0.0m;
            }


            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Functions Private
            ////////////////////////////////////////////////////////////////////////////////////////////



            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Functions Public
            ////////////////////////////////////////////////////////////////////////////////////////////
            public void myFillTA()
            {
                this.thisTableAdapter.Fill(this);
                this.newID = FFDBDataSet.myDBGetNewID("id", "AEBalance");
            }

            public void mySaveChanges()
            { this.thisTableAdapter.Update(this); }

            public AEBalanceRow myGetRow(short accountID, short envelopeID)
            {
                foreach (AEBalanceRow row in this)
                    if (row.accountID == accountID && row.envelopeID == envelopeID)
                        return row;

                AEBalanceRow newRow = this.NewAEBalanceRow();
                newRow.accountID = accountID;
                newRow.envelopeID = envelopeID;

                this.Rows.Add(newRow);

                return newRow;
            }

            public void myUpdateAEBalanceUndo(short oldAccountID, short oldEnvelopeID, bool oldCD, decimal oldAmount)
            {
                AEBalanceRow row = this.myGetRow(oldAccountID, oldEnvelopeID);

                if (row == null)
                    return;

                // Undo the old Amount
                if (oldCD == LineCD.CREDIT)
                    row.endingBalance += oldAmount;

                else
                    row.endingBalance -= oldAmount;

                this.thisTableAdapter.Update(row);
            }

            public void myUpdateAEBalanceDo(short newAccountID, short newEnvelopeID, bool newCD, decimal newAmount)
            {
                AEBalanceRow row = this.myGetRow(newAccountID, newEnvelopeID);

                if (row == null)
                    return;

                // Do the new Amount
                if (newCD == LineCD.CREDIT)
                    row.endingBalance -= newAmount;

                else
                    row.endingBalance += newAmount;

                this.thisTableAdapter.Update(row);
            }

            public void myUpdateAEBalanceUndoDo(short oldAccountID, short oldEnvelopeID, bool oldCD, decimal oldAmount, short newAccountID, short newEnvelopeID, bool newCD, decimal newAmount)
            {
                AEBalanceRow oldRow = this.myGetRow(oldAccountID, oldEnvelopeID);
                AEBalanceRow newRow = this.myGetRow(newAccountID, newEnvelopeID);

                if (oldRow == null || newRow == null)
                    return;

                // Undo the old Amount
                if (oldCD == LineCD.CREDIT)
                    oldRow.endingBalance += oldAmount;

                else
                    oldRow.endingBalance -= oldAmount;


                // Do the new Amount
                if (newCD == LineCD.CREDIT)
                    newRow.endingBalance -= newAmount;

                else
                    newRow.endingBalance += newAmount;


                if (oldRow.id == newRow.id)
                {
                    this.thisTableAdapter.Update(newRow);
                }
                else
                {
                    this.thisTableAdapter.Update(newRow);
                    this.thisTableAdapter.Update(oldRow);
                }
            }

            
            //public void mySetEndingBalance(int accountID, int envelopeID, decimal newBalance)
            //{
            //    AEBalanceRow row = this.myFindByID(accountID, envelopeID);

            //    if (row != null)
            //    {
            //        row.endingBalance = newBalance;
            //        this.thisTableAdapter.Update(row);
            //    }

            //}

            //public decimal myGetEndingBalance(int accountID, int envelopeID)
            //{
            //    foreach (AEBalanceRow row in this)
            //    {
            //        if (row.accountID == accountID && row.envelopeID == envelopeID)
            //            return row.endingBalance;
            //    }

            //    return 0.0m;
            //}
            
            //public AEBalanceRow myFindByID(int accountID, int envelopeID)
            //{
            //    foreach (AEBalanceRow row in this)
            //        if (row.accountID == accountID && row.envelopeID == envelopeID)
            //            return row;

            //    return null;
            //}

            //public int myFindID(int accountID, int envelopeID)
            //{
            //    foreach (AEBalanceRow row in this)
            //        if (row.accountID == accountID && row.envelopeID == envelopeID)
            //            return row.id;

            //    return -1;
            //}

            //public void myUpdateAEBalanceEntries(List<AEBalanceEntry> list)
            //{
            //    foreach (AEBalanceEntry entry in list)
            //        this.myGetRow(entry.AccountID, entry.EnvelopeID);

            //    this.thisTableAdapter.Update(this);
            //}

            //public List<AccEnvDetails> myGetAccountBalancesInfo(string filter, string sort)
            //{
            //    DataView view = new DataView(this, filter, sort, DataViewRowState.CurrentRows);
            //    List<AccEnvDetails> list = new List<AccEnvDetails>();
            //    AEBalanceRow row;

            //    for (int i = 0; i < view.Count; i++)
            //    {
            //        AccEnvDetails details = new AccEnvDetails();

            //        row = this.FindByid(Convert.ToInt32(view[i]["id"]));

            //        details.accountID = row.accountID;
            //        details.envelopeID = row.envelopeID;
            //        details.error = false;
            //        details.name = row.AccountRow.name;
            //        details.balance = row.endingBalance;

            //        list.Add(details);
            //    }

            //    view.Dispose();
            //    return list;
            //}

            //public List<AccEnvDetails> myGetEnvelopeBalancesInfo(string filter, string sort)
            //{
            //    DataView view = new DataView(this, filter, sort, DataViewRowState.CurrentRows);
            //    List<AccEnvDetails> list = new List<AccEnvDetails>();
            //    AEBalanceRow row;

            //    for (int i = 0; i < view.Count; i++)
            //    {
            //        AccEnvDetails details = new AccEnvDetails();

            //        row = this.FindByid(Convert.ToInt32(view[i]["id"]));

            //        details.accountID = row.accountID;
            //        details.envelopeID = row.envelopeID;
            //        details.error = false;
            //        details.name = row.EnvelopeRow.fullName;
            //        details.balance = row.endingBalance;

            //        list.Add(details);
            //    }

            //    view.Dispose();
            //    return list;
            //}

        }// END partial class DownloadInfoDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance
