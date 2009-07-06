using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class EnvelopeDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private short newID;


            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Functions Override
            ////////////////////////////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(EnvelopeDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(EnvelopeDataTable_ColumnChanged);

                this.newID = 1;
            }


            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Internal Events
            ////////////////////////////////////////////////////////////////////////////////////////////
            private void EnvelopeDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                EnvelopeRow envelopeRow = e.Row as EnvelopeRow;
                envelopeRow.BeginEdit();

                envelopeRow.id = this.newID++;
                envelopeRow.name = "";
                envelopeRow.fullName = "";
                envelopeRow.parentEnvelope = SpclEnvelope.NULL;
                envelopeRow.closed = false;
                envelopeRow.endingBalance = 0.0m;
                envelopeRow.currentBalance = 0.0m;

                envelopeRow.EndEdit();
            }

            private void EnvelopeDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                string tmp;
                int maxLen;
                EnvelopeRow row = e.Row as EnvelopeRow;
                row.BeginEdit();

                switch (e.Column.ColumnName)
                {
                    case "name":
                        tmp = e.ProposedValue as string;
                        maxLen = this.nameColumn.MaxLength;

                        if (tmp.Length > maxLen)
                            row.name = tmp.Substring(0, maxLen);

                        mySetFullName(ref row);
                        break;

                    case "parentEnvelope":
                        mySetFullName(ref row);
                        break;
                }

                row.EndEdit();
            }



            ////////////////////////////////////////////////////////////////////////////////////////////
            //   External Events
            ////////////////////////////////////////////////////////////////////////////////////////////



            ////////////////////////////////////////////////////////////////////////////////////////////
            //   Functions Private
            ////////////////////////////////////////////////////////////////////////////////////////////
            private void mySetFullName(ref EnvelopeRow thisEnvelope)
            {
                string fullName;
                List<short> childIDList;
                int maxLen = this.fullNameColumn.MaxLength;

                if (thisEnvelope == null)
                    return;

                // Get this envelope Full Name
                fullName = this.myGetFullName(ref thisEnvelope);

                // Truncate if too long
                if (fullName.Length > maxLen)
                    fullName = fullName.Substring(0, maxLen);

                // Set the Full name
                thisEnvelope.fullName = fullName;

                // Find all the child envelopes and update their full names too.
                childIDList = this.myGetChildEnvelopeIDList(thisEnvelope.id);

                foreach (short id in childIDList)
                {
                    EnvelopeRow row = this.FindByid(id);
                    mySetFullName(ref row);
                }
            }

            private string myGetFullName(ref EnvelopeRow thisEnvelope)
            {
                if (thisEnvelope.parentEnvelope == SpclEnvelope.NULL)
                    return thisEnvelope.name;

                else
                {
                    EnvelopeRow parent = this.FindByid(thisEnvelope.parentEnvelope);
                    return this.myGetFullName(ref parent) + ":" + thisEnvelope.name;
                }
            }

            private void mySaveAddedRow(ref SqlCeCommand command, ref EnvelopeRow row)
            {
                string query;

                // INSERT INTO table_name (column1, column2, column3,...)
                // VALUES (value1, value2, value3,...)

                query = "INSERT INTO Envelope VALUES (";
                query += row.id.ToString() + ", ";
                query += "'" + row.name.Replace("'", "''") + "', ";
                query += "'" + row.fullName.Replace("'", "''") + "', ";
                query += row.parentEnvelope.ToString() + ", ";
                query += Convert.ToInt16(row.closed).ToString() + ", ";
                query += row.currentBalance.ToString() + ", ";
                query += row.endingBalance.ToString() + ");";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }

            private void myRemoveDeletedRow(ref SqlCeCommand command, ref EnvelopeRow row)
            {
                //string query;

                //query = "DELETE FROM Envelope WHERE id = " + row.id.ToString() + ";";

                //command.CommandText = query;
                //command.ExecuteNonQuery();

                throw new Exception("Deleting is not handled yet.");
            }

            private void mySaveModifiedRow(ref SqlCeCommand command, ref EnvelopeRow row)
            {
                string query;

                // UPDATE table_name
                // SET column1=value, column2=value2,...
                // WHERE some_column=some_value

                query = "UPDATE Envelope SET ";
                query += "name = '" + row.name.Replace("'", "''") + "', ";
                query += "fullName = '" + row.fullName.Replace("'", "''") + "', ";
                query += "parentEnvelope = " + row.parentEnvelope.ToString() + ", ";
                query += "closed = " + Convert.ToInt16(row.closed).ToString() + ", ";
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
                string query = "SELECT * FROM Envelope;";
                object[] newRow = new object[7];

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
                this.newID = (short)FFDBDataSet.myDBGetNewID("id", "Envelope");
            }

            public void mySaveChanges()
            {
                SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
                SqlCeCommand command = new SqlCeCommand("", connection);
                connection.Open();

                for (int index = 0; index < this.Rows.Count; index++)
                {
                    EnvelopeRow row = this.Rows[index] as EnvelopeRow;

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



            public void myUpdateEnvelopeEBUndo(short oldEnvelopeID, bool oldCD, decimal oldAmount)
            {
                EnvelopeRow row = this.FindByid(oldEnvelopeID);

                // Undo the old Amount
                if (oldCD == LineCD.CREDIT)
                    row.endingBalance += oldAmount;

                else
                    row.endingBalance -= oldAmount;

            }

            public void myUpdateEnvelopeEBDo(short newEnvelopeID, bool newCD, decimal newAmount)
            {
                EnvelopeRow row = FindByid(newEnvelopeID);

                //  Update to the new amount
                if (newCD == LineCD.CREDIT)
                    row.endingBalance -= newAmount;

                else
                    row.endingBalance += newAmount;

            }

            public void myUpdateEnvelopeEBUndoDo(short oldEnvelopeID, bool oldCD, decimal oldAmount, short newEnvelopeID, bool newCD, decimal newAmount)
            {
                EnvelopeRow oldEnvelopeRow = FindByid(oldEnvelopeID);
                EnvelopeRow newEnvelopeRow = FindByid(newEnvelopeID);

                // Undo the old Amount
                if (oldCD == LineCD.CREDIT)
                    oldEnvelopeRow.endingBalance += oldAmount;

                else
                    oldEnvelopeRow.endingBalance -= oldAmount;


                //  Update to the new amount
                if (newCD == LineCD.CREDIT)
                    newEnvelopeRow.endingBalance -= newAmount;

                else
                    newEnvelopeRow.endingBalance += newAmount;


            }



            public List<short> myGetChildEnvelopeIDList(short envelopeID)
            {
                List<short> idList = new List<short>();

                foreach (EnvelopeRow row in this)
                {
                    if (row.id > 0 && row.parentEnvelope == envelopeID)
                        idList.Add(row.id);
                }

                idList.Sort();
                return idList;
            }

            public List<short> myGetAllChildEnvelopeIDList(short envelopeID)
            {
                List<short> idList = myGetChildEnvelopeIDList(envelopeID);
                List<short> temp;

                for (int i = 0; i < idList.Count; i++ )
                {
                    temp = myGetChildEnvelopeIDList(idList[i]);

                    foreach (short id in temp)
                        idList.Add(id);
                }

                return idList;
            }


        }// END Envelope partial class
    }// END partial class FamilyFinanceDBDataSet
} //END namespace FamilyFinance
