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
        partial class LineItemDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private int newID;
            private bool stayOut;


            ///////////////////////////////////////////////////////////////////////
            //   Function Overrides
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(LineItemDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(LineItemDataTable_ColumnChanged);

                this.newID = 1;
                this.stayOut = false;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void LineItemDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                LineItemRow lineItemRow = e.Row as LineItemRow;
                lineItemRow.BeginEdit();
                
                lineItemRow.id = newID++;
                lineItemRow.transactionID = FFDBDataSet.myDBGetNewID("transactionID", "LineItem");
                lineItemRow.date = DateTime.Now.Date;
                lineItemRow.lineTypeID = SpclLineType.NULL;
                lineItemRow.accountID = SpclAccount.NULL;
                lineItemRow.oppAccountID = SpclAccount.NULL;
                lineItemRow.description = "";
                lineItemRow.confirmationNumber = "";
                lineItemRow.envelopeID = SpclEnvelope.NULL;
                lineItemRow.complete = LineState.PENDING;
                lineItemRow.creditAmount = 0.0m;
                lineItemRow.transactionError = false;
                lineItemRow.lineError = false;

                lineItemRow.EndEdit();
            }

            private void LineItemDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                LineItemRow row;
                string tmp;

                if (stayOut)
                    return;

                stayOut = true;

                row = e.Row as LineItemRow;
                row.BeginEdit();

                switch (e.Column.ColumnName)
                {
                    case "amount":
                        myValidateAmount(ref row, row.creditDebit, row.amount);
                        break;

                    case "creditDebit":
                        myValidateAmount(ref row, row.creditDebit, row.amount);
                        break;
 
                    case "debitAmount":
                        myValidateAmount(ref row, LineCD.DEBIT, row.debitAmount);
                        break;

                    case "creditAmount":
                        myValidateAmount(ref row, LineCD.CREDIT, row.creditAmount);
                        break;

                    case "complete":
                        tmp = e.ProposedValue as string;
                        if (tmp.Length > 1)
                        {
                            row.complete = tmp.Substring(0, 1);
                        }
                        break;
                }

                row.EndEdit();
                stayOut = false;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Function Private
            ///////////////////////////////////////////////////////////////////////
            private void myValidateAmount(ref LineItemRow row, bool newCD, decimal newAmount)
            {
                int temp;

                if (newAmount < 0)
                {
                    newAmount = newAmount * -1;
                    newCD = !newCD;
                }

                // Keep only to the Penny.
                temp = Convert.ToInt32(newAmount * 100);
                newAmount = temp / 100.0m;

                row.BeginEdit();

                if( newAmount == 0)
                {
                    row.amount = newAmount;
                    row.creditDebit = newCD;
                    row.SetdebitAmountNull();
                    row.SetcreditAmountNull();
                }
                else if (newCD == LineCD.DEBIT)
                {
                    row.amount = newAmount;
                    row.creditDebit = LineCD.DEBIT;
                    row.debitAmount = newAmount;
                    row.SetcreditAmountNull();
                }
                else
                {
                    row.amount = newAmount;
                    row.creditDebit = LineCD.CREDIT;
                    row.creditAmount = newAmount;
                    row.SetdebitAmountNull();
                }

                row.EndEdit();
            }

            private void mySaveAddedRow(ref SqlCeCommand command, ref LineItemRow row)
            {
                string query;

                // INSERT INTO table_name (column1, column2, column3,...)
                // VALUES (value1, value2, value3,...)

                query = "INSERT INTO LineItem VALUES (";
                query += row.id.ToString() + ", ";
                query += row.transactionID.ToString() + ", ";
                query += row.date.ToString() + ", ";
                query += row.lineTypeID.ToString() + ", ";
                query += row.accountID.ToString() + ", ";
                query += row.oppAccountID.ToString() + ", ";
                query += "'" + row.description.Replace("'", "''") + "', ";
                query += "'" + row.confirmationNumber.Replace("'", "''") + "', ";
                query += row.envelopeID.ToString() + ", ";
                query += Convert.ToInt16(row.complete).ToString() + ", ";
                query += row.amount.ToString() + ", ";
                query += Convert.ToInt16(row.creditDebit).ToString() + ", ";
                query += Convert.ToInt16(row.transactionError).ToString() + ", ";
                query += Convert.ToInt16(row.lineError).ToString() + ");";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }

            private void myRemoveDeletedRow(ref SqlCeCommand command, ref LineItemRow row)
            {
                //string query;

                //query = "DELETE FROM Account WHERE id = " + row.id.ToString() + ";";

                //command.CommandText = query;
                //command.ExecuteNonQuery();

                throw new Exception("Deleting is not handled yet.");
            }

            private void mySaveModifiedRow(ref SqlCeCommand command, ref LineItemRow row)
            {
                string query;

                // UPDATE table_name
                // SET column1=value, column2=value2,...
                // WHERE some_column=some_value

                query = "UPDATE LineItem SET ";
                query += "transactionID = " + row.transactionID.ToString() + ", ";
                query += "date = " + row.date.ToString() + ", ";
                query += "lineTypeID = " + row.lineTypeID.ToString() + ", ";
                query += "accountID = " + row.accountID.ToString() + ", ";
                query += "oppAccountID = " + row.oppAccountID.ToString() + ", ";
                query += "description = '" + row.description.Replace("'", "''") + "', ";
                query += "confermationNumber = '" + row.confirmationNumber.Replace("'", "''") + "', ";
                query += "envelopeID = " + row.envelopeID.ToString() + ", ";
                query += "complete = " + row.complete + ", ";
                query += "amount = " + row.amount.ToString() + ", ";
                query += "creditDebit = " + Convert.ToInt16(row.creditDebit).ToString() + ", ";
                query += "transactionError = " + Convert.ToInt16(row.transactionError).ToString() + ", ";
                query += "lineError = " + Convert.ToInt16(row.lineError).ToString() + " ";
                query += "WHERE id = " + row.id.ToString() + ";";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }


            ///////////////////////////////////////////////////////////////////////
            //   Functions Public 
            ///////////////////////////////////////////////////////////////////////
            public void myFillByAccount(short accountID)
            {
                SqlCeConnection connection;
                SqlCeCommand command;
                SqlCeDataReader reader;
                bool accountCD;
                decimal balance;
                object[] newRow = new object[14];
                string query;

                // Clear out this table
                this.Rows.Clear();

                // Build the query
                query = "SELECT * FROM LineItem WHERE accountID = " + accountID.ToString();
                query += " ORDER BY date, creditDebit DESC, id;";

                connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
                connection.Open();
                command = new SqlCeCommand(query, connection);
                reader = command.ExecuteReader();

                // Iterate through the results
                while (reader.Read())
                {
                    reader.GetValues(newRow);
                    this.Rows.Add(newRow);
                }

                // Close the reader
                reader.Close();

                // If this is empty there is nothing to do.
                if (this.Rows.Count <= 0)
                {
                    connection.Close();
                    this.newID = FFDBDataSet.myDBGetNewID("id", "LineItem"); 
                    return;
                }

                // Set the balances and get the accounts CD.
                balance = 0.0m;

                query = "SELECT creditDebit FROM Account WHERE id = " + accountID.ToString();
                command.CommandText = query;
                accountCD = Convert.ToBoolean(command.ExecuteScalar());

                connection.Close();

                stayOut = true;

                // Set balances By going down the list
                if (accountCD == LineCD.DEBIT)
                {
                    foreach (LineItemRow row in this)
                        if (row.creditDebit == LineCD.DEBIT)
                        {
                            row.BeginEdit();
                            row.debitAmount = row.amount;
                            row.SetcreditAmountNull();
                            row.balanceAmount = balance += row.amount;
                            row.EndEdit();
                        }
                        else
                        {
                            row.BeginEdit();
                            row.creditAmount = row.amount;
                            row.SetdebitAmountNull();
                            row.balanceAmount = balance -= row.amount;
                            row.EndEdit();
                        }
                }
                else
                {
                    foreach (LineItemRow row in this)
                        if (row.creditDebit == LineCD.DEBIT)
                        {
                            row.BeginEdit();
                            row.debitAmount = row.amount;
                            row.SetcreditAmountNull();
                            row.balanceAmount = balance -= row.amount;
                            row.EndEdit();
                        }
                        else
                        {
                            row.BeginEdit();
                            row.creditAmount = row.amount;
                            row.SetdebitAmountNull();
                            row.balanceAmount = balance += row.amount;
                            row.EndEdit();
                        }
                }


                this.AcceptChanges();
                this.newID = FFDBDataSet.myDBGetNewID("id", "LineItem");
                stayOut = false;
            }

            public void myFillByTransaction(int transactionID)
            {
                string query = "SELECT * FROM LineItem WHERE transactionID = " + transactionID.ToString() + ";";
                object[] newRow = new object[14];

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
                this.newID = FFDBDataSet.myDBGetNewID("id", "LineItem");
            }

            public void mySaveChanges()
            {
                SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
                SqlCeCommand command = new SqlCeCommand("", connection);
                connection.Open();

                for (int index = 0; index < this.Rows.Count; index++)
                {
                    LineItemRow row = this.Rows[index] as LineItemRow;

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

            public void mySaveNewLines()
            {
                SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
                SqlCeCommand command = new SqlCeCommand("", connection);
                connection.Open();

                for (int index = 0; index < this.Rows.Count; index++)
                {
                    LineItemRow row = this.Rows[index] as LineItemRow;

                    if (row.RowState == DataRowState.Added)
                    {
                        this.mySaveAddedRow(ref command, ref row);
                        row.AcceptChanges();
                    }
                }

                connection.Close();
            }

            public void myFillBalance()
            {
                bool accountCD;
                decimal balance;

                // If this is empty there is nothing to do.
                if (this.Rows.Count <= 0)
                    return;

                // Set the balances and get the accounts CD.
                balance = 0.0m;
                accountCD = (this.Rows[0] as LineItemRow).AccountRowByFK_Line_accountID.creditDebit;

                // Set balances By going down the list
                if (accountCD == LineCD.DEBIT)
                {
                    foreach (LineItemRow row in this)
                    {
                        row.BeginEdit();

                        if (row.creditDebit == LineCD.DEBIT)
                            row.balanceAmount = balance += row.amount;

                        else
                            row.balanceAmount = balance -= row.amount;

                        row.EndEdit();
                    }
                }
                else
                {
                    foreach (LineItemRow row in this)
                    {
                        row.BeginEdit();

                        if (row.creditDebit == LineCD.DEBIT)
                            row.balanceAmount = balance -= row.amount;

                        else
                            row.balanceAmount = balance += row.amount;

                        row.EndEdit();
                    }
                }

                this.AcceptChanges();
            }

            public decimal myGetTransCDSum(int transID, bool creditDebit)
            {
                decimal sum = 0.0m;

                var amounts = from line in this
                          where line.transactionID == transID && line.creditDebit == creditDebit
                          select line.amount;

                foreach (var amount in amounts)
                    sum += amount;

                return sum;
            }



        }//END partial class LineItemDataTable
    }// END FamilyFinanceDBDataSet Partial Class
}// END nameSpace FamilyFinance
