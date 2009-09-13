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
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private short newID;
            private bool stayOut;


            ///////////////////////////////////////////////////////////////////////
            //   Overriden Functions 
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(AccountTypeDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(AccountTypeDataTable_ColumnChanged);

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
            private void mySaveAddedRow(ref SqlCeCommand command, ref AccountTypeRow row)
            {
                string query;

                // INSERT INTO table_name (column1, column2, column3,...)
                // VALUES (value1, value2, value3,...)

                query = "INSERT INTO AccountType VALUES (";
                query += row.id.ToString() + ", ";
                query += "'" + row.name.Replace("'", "''") + "');";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }

            private void myRemoveDeletedRow(ref SqlCeCommand command, ref AccountTypeRow row)
            {
                //string query;

                //query = "DELETE FROM AccountType WHERE id = " + row.id.ToString() + ";";

                //command.CommandText = query;
                //command.ExecuteNonQuery();

                throw new Exception("Deleting an account type is not handled yet.");
            }

            private void mySaveModifiedRow(ref SqlCeCommand command, ref AccountTypeRow row)
            {
                string query;

                // UPDATE table_name
                // SET column1=value, column2=value2,...
                // WHERE some_column=some_value

                query = "UPDATE AccountType SET ";
                query += "name = '" + row.name.Replace("'", "''") + "' ";
                query += "WHERE id = " + row.id.ToString() + ";";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }


            ///////////////////////////////////////////////////////////////////////
            //   Functions Public 
            ///////////////////////////////////////////////////////////////////////
            public void myFill()
            {
                string query = "SELECT * FROM AccountType;";
                object[] newRow = new object[2];

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
                this.newID = (short)FFDBDataSet.myDBGetNewID("id", "AccountType");
            }

            public void mySaveChanges()
            {
                SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
                SqlCeCommand command = new SqlCeCommand("", connection);
                connection.Open();

                for (int index = 0; index < this.Rows.Count; index++)
                {
                    AccountTypeRow row = this.Rows[index] as AccountTypeRow;

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
