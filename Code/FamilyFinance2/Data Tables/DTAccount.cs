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
            private short newID;


            ///////////////////////////////////////////////////////////////////////
            //   Overriden Functions 
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(AccountDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(AccountDataTable_ColumnChanged);

                this.newID = 1;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void AccountDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                AccountRow accountRow = e.Row as AccountRow;
                accountRow.BeginEdit();

                accountRow.id = this.newID++;
                accountRow.name = "";
                accountRow.accountTypeID = SpclAccountType.NULL;
                accountRow.catagoryID = SpclAccountCat.ACCOUNT;
                accountRow.closed = false;
                accountRow.creditDebit = LineCD.DEBIT;
                accountRow.envelopes = false;
                accountRow.endingBalance = 0.0m;
                accountRow.currentBalance = 0.0m;

                accountRow.EndEdit(); 
            }

            private void AccountDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                string tmp;
                int maxLen;
                AccountRow row = e.Row as AccountRow;
                row.BeginEdit();

                if (e.Column.ColumnName == "name")
                {
                    tmp = e.ProposedValue as string;
                    maxLen = this.nameColumn.MaxLength;

                    if (tmp.Length > maxLen)
                        row.name = tmp.Substring(0, maxLen);
                }

                row.EndEdit();
            }


            ///////////////////////////////////////////////////////////////////////
            //   Functions Private 
            ///////////////////////////////////////////////////////////////////////
            private void mySaveAddedRow(ref SqlCeCommand command, ref AccountRow row)
            {
                string query;

                // INSERT INTO table_name (column1, column2, column3,...)
                // VALUES (value1, value2, value3,...)

                query = "INSERT INTO Account VALUES (";
                query += row.id.ToString() + ", ";
                query += "'" + row.name.Replace("'", "''") + "', ";
                query += row.accountTypeID.ToString() + ", ";
                query += row.catagoryID.ToString() + ", ";
                query += Convert.ToInt16(row.closed).ToString() + ", ";
                query += Convert.ToInt16(row.creditDebit).ToString() + ", ";
                query += Convert.ToInt16(row.envelopes).ToString() + ", ";
                query += row.currentBalance.ToString() + ", ";
                query += row.endingBalance.ToString() + ");";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }

            private void myRemoveDeletedRow(ref SqlCeCommand command, ref AccountRow row)
            {
                //string query;

                //query = "DELETE FROM Account WHERE id = " + row.id.ToString() + ";";

                //command.CommandText = query;
                //command.ExecuteNonQuery();

                throw new Exception("Deleting is not handled yet.");
            }

            private void mySaveModifiedRow(ref SqlCeCommand command, ref AccountRow row)
            {
                string query;

                // UPDATE table_name
                // SET column1=value, column2=value2,...
                // WHERE some_column=some_value

                query =  "UPDATE Account SET ";
                query += "name = '" + row.name.Replace("'", "''") + "', ";
                query += "accountTypeID = " + row.accountTypeID.ToString() + ", ";
                query += "catagoryID = " + row.catagoryID.ToString() + ", ";
                query += "closed = " + Convert.ToInt16(row.closed).ToString() + ", ";
                query += "creditDebit = " + Convert.ToInt16(row.creditDebit).ToString() + ", ";
                query += "envelopes = " + Convert.ToInt16(row.envelopes).ToString() + ", ";
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
                string query = "SELECT * FROM Account;";
                object[] newRow = new object[9];

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
                this.newID = (short)FFDBDataSet.myDBGetNewID("id", "Account");
            }

            public void mySaveChanges()
            {
                SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
                SqlCeCommand command = new SqlCeCommand("", connection);
                connection.Open();

                for (int index = 0; index < this.Rows.Count; index++)
                {
                    AccountRow row = this.Rows[index] as AccountRow;

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
                        case DataRowState.Unchanged:                          //
                            this.mySaveModifiedRow(ref command, ref row);     // TODO: find out why EditAccountForm doesn't set modified row states to modified 
                            break;                                            //
                        default:
                            break;
                    }
                }

                this.AcceptChanges();
                connection.Close();
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

                //this.thisTableAdapter.Update(row);
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

                //this.thisTableAdapter.Update(row);
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
                    //this.thisTableAdapter.Update(newRow);
                }
                else
                {
                    //this.thisTableAdapter.Update(newRow);
                    //this.thisTableAdapter.Update(oldRow);
                }

            }




        }//END partial class AccountDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance



