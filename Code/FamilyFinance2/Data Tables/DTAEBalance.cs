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
            private int newID;

            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Overriden Functions 
            ////////////////////////////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(AEBalanceDataTable_TableNewRow);

                this.newID = 1;
            }


            ////////////////////////////////////////////////////////////////////////////////////////////
            //   External Events
            ////////////////////////////////////////////////////////////////////////////////////////////
 

            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Internal Events
            ////////////////////////////////////////////////////////////////////////////////////////////
            private void AEBalanceDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                AEBalanceRow aEBalanceRow = e.Row as AEBalanceRow;
                aEBalanceRow.BeginEdit();

                aEBalanceRow.id = this.newID++;
                aEBalanceRow.accountID = SpclAccount.NULL;
                aEBalanceRow.envelopeID = SpclEnvelope.NULL;
                aEBalanceRow.endingBalance = 0.0m;
                aEBalanceRow.currentBalance = 0.0m;

                aEBalanceRow.EndEdit();
            }


            ///////////////////////////////////////////////////////////////////////
            //   Functions Private 
            ///////////////////////////////////////////////////////////////////////
            private void mySaveAddedRow(ref SqlCeCommand command, ref AEBalanceRow row)
            {
                string query;

                // INSERT INTO table_name (column1, column2, column3,...)
                // VALUES (value1, value2, value3,...)

                query = "INSERT INTO AEBalance VALUES (";
                query += row.id.ToString() + ", ";
                query += row.accountID.ToString() + "', ";
                query += row.envelopeID.ToString() + ", ";
                query += row.currentBalance.ToString() + ", ";
                query += row.endingBalance.ToString() + ");";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }

            private void myRemoveDeletedRow(ref SqlCeCommand command, ref AEBalanceRow row)
            {
                //string query;

                //query = "DELETE FROM AEBalance WHERE id = " + row.id.ToString() + ";";

                //command.CommandText = query;
                //command.ExecuteNonQuery();

                throw new Exception("Deleting is not handled yet.");
            }

            private void mySaveModifiedRow(ref SqlCeCommand command, ref AEBalanceRow row)
            {
                string query;

                // UPDATE table_name
                // SET column1=value, column2=value2,...
                // WHERE some_column=some_value

                query = "UPDATE AEBalance SET ";
                query += "accountID = " + row.accountID.ToString() + ", ";
                query += "envelopeID = " + row.envelopeID.ToString() + ", ";
                query += "currentBalance = " + row.currentBalance.ToString() + ", ";
                query += "endingBalance = " + row.endingBalance.ToString() + " ";
                query += "WHERE id = " + row.id.ToString() + ";";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }


            ///////////////////////////////////////////////////////////////////////
            //   Functions Public 
            ///////////////////////////////////////////////////////////////////////
            public void myFill()
            {
                string query = "SELECT * FROM AEBalance;";
                object[] newRow = new object[5];

                this.Rows.Clear();

                SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
                connection.Open();
                SqlCeCommand command = new SqlCeCommand(query, connection);
                SqlCeDataReader reader = command.ExecuteReader();

                // Iterate through the results
                while (reader.Read())
                {
                    reader.GetValues(newRow);
                    this.Rows.Add(newRow);
                }

                // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
                this.AcceptChanges();
                this.newID = FFDBDataSet.myDBGetNewID("id", "AEBalance");
            }

            public void mySaveChanges()
            {
                SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
                SqlCeCommand command = new SqlCeCommand("", connection);
                connection.Open();

                for (int index = 0; index < this.Rows.Count; index++)
                {
                    AEBalanceRow row = this.Rows[index] as AEBalanceRow;

                    switch (row.RowState)
                    {
                        case DataRowState.Added:
                            this.mySaveAddedRow(ref command, ref row);
                            break;
                        case DataRowState.Deleted:
                            this.myRemoveDeletedRow(ref command, ref row);
                            break;
                        case DataRowState.Modified:
                            this.mySaveModifiedRow(ref command, ref row);
                            break;
                        default:
                            break;
                    }
                }

                this.AcceptChanges();
                connection.Close();
            }


            public AEBalanceRow myGetRow(short accountID, short envelopeID)
            {
                foreach (AEBalanceRow row in this)
                    if (row.accountID == accountID && row.envelopeID == envelopeID)
                        return row;

                AEBalanceRow newRow = this.NewAEBalanceRow();
                newRow.BeginEdit();

                newRow.accountID = accountID;
                newRow.envelopeID = envelopeID;

                this.Rows.Add(newRow);
                newRow.EndEdit();

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

                //this.thisTableAdapter.Update(row);
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

                //this.thisTableAdapter.Update(row);
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


                //if (oldRow.id == newRow.id)
                //{
                //    this.thisTableAdapter.Update(newRow);
                //}
                //else
                //{
                //    this.thisTableAdapter.Update(newRow);
                //    this.thisTableAdapter.Update(oldRow);
                //}
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
