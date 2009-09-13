using System;
using System.Text;
using System.Data;
using System.Linq;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class SubLineItemDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private int newID;
            private bool stayOut;


            ///////////////////////////////////////////////////////////////////////
            //   Function Overridden
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(SubLineItemDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(SubLineItemDataTable_ColumnChanged);

                this.newID = 1;
                this.stayOut = false;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void SubLineItemDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                stayOut = true;
                SubLineItemRow newRow = e.Row as SubLineItemRow;

                newRow.id = this.newID++;
                newRow.envelopeID = SpclEnvelope.NULL;
                newRow.description = "";
                newRow.amount = 0.0m;

                stayOut = false;
            }

            private void SubLineItemDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                if (stayOut)
                    return;

                stayOut = true;
                SubLineItemRow row = e.Row as SubLineItemRow;

                if (e.Column.ColumnName == "amount")
                {
                    decimal newValue;
                    int tempValue;

                    newValue = Convert.ToDecimal(e.ProposedValue);
                    tempValue = Convert.ToInt32(newValue * 100);
                    newValue = Convert.ToDecimal(tempValue) / 100;

                    if (newValue < 0)
                        newValue = newValue * -1;

                    row.amount = newValue;
                }

                stayOut = false;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Function Private
            ///////////////////////////////////////////////////////////////////////
            private void mySaveAddedRow(ref SqlCeCommand command, ref SubLineItemRow row)
            {
                string query;

                // INSERT INTO table_name (column1, column2, column3,...)
                // VALUES (value1, value2, value3,...)

                query = "INSERT INTO SubLineItem VALUES (";
                query += row.id.ToString() + ", ";
                query += row.lineItemID.ToString() + ", ";
                query += row.envelopeID.ToString() + ", ";

                if (row.IsdescriptionNull())
                    query += "null, ";
                else
                    query += "'" + row.description.Replace("'", "''") + "', ";

                query += row.amount.ToString() + ");";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }

            private void myRemoveDeletedRow(ref SqlCeCommand command, ref SubLineItemRow row)
            {
                //string query;

                //query = "DELETE FROM Account WHERE id = " + row.id.ToString() + ";";

                //command.CommandText = query;
                //command.ExecuteNonQuery();

                throw new Exception("Deleting is not handled yet.");
            }

            private void mySaveModifiedRow(ref SqlCeCommand command, ref SubLineItemRow row)
            {
                string query;

                // UPDATE table_name
                // SET column1=value, column2=value2,...
                // WHERE some_column=some_value

                query = "UPDATE SubLineItem SET ";
                query += "lineItemID = " + row.lineItemID.ToString() + ", ";
                query += "envelopeID = " + row.envelopeID.ToString() + ", ";

                if (row.IsdescriptionNull())
                    query += "description = null, ";
                else
                    query += "description = '" + row.description.Replace("'", "''") + "', ";

                query += "amount = " + row.amount.ToString() + ", ";
                query += "WHERE id = " + row.id.ToString() + ";";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }


            ///////////////////////////////////////////////////////////////////////
            //   Function Public
            ///////////////////////////////////////////////////////////////////////
            public void myFillByTransaction(int transactionID)
            {
                string query;
                object[] values = new object[5];

                this.Rows.Clear();

                query = "SELECT s.id, s.lineItemID, s.envelopeID, s.description, s.amount ";
                query += " FROM SubLineItem AS s INNER JOIN LineItem AS l ON l.id = s.lineItemID ";
                query += " WHERE transactionID = " + transactionID.ToString() + ";";

                SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
                connection.Open();
                SqlCeCommand command = new SqlCeCommand(query, connection);
                SqlCeDataReader reader = command.ExecuteReader();

                // Iterate through the results
                while (reader.Read())
                {
                    reader.GetValues(values);
                    this.Rows.Add(values);
                }

                // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
                this.AcceptChanges();
                this.newID = FFDBDataSet.myDBGetNewID("id", "SubLineItem");
            }

            public void mySaveChanges()
            {
                SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
                connection.Open();
                SqlCeCommand command = new SqlCeCommand("", connection);

                for (int index = 0; index < this.Rows.Count; index++)
                {
                    SubLineItemRow row = this.Rows[index] as SubLineItemRow;

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

                connection.Close();
                this.AcceptChanges();
            }

            public decimal mySubLineSum(int lineID)
            {
                decimal sum = 0.0m;

                var amounts = from line in this
                              where line.lineItemID == lineID
                              select line.amount;

                foreach (var amount in amounts)
                    sum += amount;

                return sum;
            }

            public decimal mySubLineSum(int lineID, out int count, out short envelopeID)
            {
                decimal sum = 0.0m;

                var results = from line in this
                              where line.lineItemID == lineID
                              select new { line.amount, line.envelopeID };

                foreach (var row in results)
                    sum += row.amount;

                count = results.Count();

                if (count <= 0)
                    envelopeID = SpclEnvelope.NULL;
                else if (count == 1)
                    envelopeID = results.ElementAt(0).envelopeID;
                else 
                    envelopeID = SpclEnvelope.SPLIT;

                return sum;
            }


        }// END endpartial class SubLineItemDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance
