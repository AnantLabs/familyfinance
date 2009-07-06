using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class LineTypeDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private short newID;


            ///////////////////////////////////////////////////////////////////////
            //   Function Overrides
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(LineTypeDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(LineTypeDataTable_ColumnChanged);

                this.newID = 1;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void LineTypeDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                LineTypeRow lineTypeRow = e.Row as LineTypeRow;
                lineTypeRow.BeginEdit();
                
                lineTypeRow.id = newID++;
                lineTypeRow.name = "";
                
                lineTypeRow.EndEdit();
            }

            private void LineTypeDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                LineTypeRow row;
                string tmp;
                int maxLen;

                row = e.Row as LineTypeRow;
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
            //   Function Private
            ///////////////////////////////////////////////////////////////////////
            private void mySaveAddedRow(ref SqlCeCommand command, ref LineTypeRow row)
            {
                string query;

                // INSERT INTO table_name (column1, column2, column3,...)
                // VALUES (value1, value2, value3,...)

                query = "INSERT INTO LineType VALUES (";
                query += row.id.ToString() + ", ";
                query += "'" + row.name.Replace("'", "''") + "');";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }

            private void myRemoveDeletedRow(ref SqlCeCommand command, ref LineTypeRow row)
            {
                //string query;

                //query = "DELETE FROM Account WHERE id = " + row.id.ToString() + ";";

                //command.CommandText = query;
                //command.ExecuteNonQuery();

                throw new Exception("Deleting is not handled yet.");
            }

            private void mySaveModifiedRow(ref SqlCeCommand command, ref LineTypeRow row)
            {
                string query;

                // UPDATE table_name
                // SET column1=value, column2=value2,...
                // WHERE some_column=some_value

                query = "UPDATE LineType SET ";
                query += "name = '" + row.name.Replace("'", "''") + "', ";
                query += "WHERE id = " + row.id.ToString() + ";";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }


            ///////////////////////////////////////////////////////////////////////
            //   Functions Public 
            ///////////////////////////////////////////////////////////////////////
            public void myFill()
            {
                string query = "SELECT * FROM LineType;";
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
                this.newID = (short)FFDBDataSet.myDBGetNewID("id", "LineType");
            }

            public void mySaveChanges()
            {
                SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
                SqlCeCommand command = new SqlCeCommand("", connection);
                connection.Open();

                for (int index = 0; index < this.Rows.Count; index++)
                {
                    LineTypeRow row = this.Rows[index] as LineTypeRow;

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

            //public int myAddType(string name)
            //{
            //    foreach (LineTypeRow row in this)
            //        if (row.name == name)
            //            return row.id;

            //    LineTypeRow newType = this.NewLineTypeRow();
            //    newType.name = name;

            //    this.Rows.Add(newType);
            //    this.thisTableAdapter.Update(newType);

            //    return newType.id;
            //}



        }// END partial class LineTypeDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance
