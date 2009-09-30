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
                stayOut = true;
                LineItemRow newRow = e.Row as LineItemRow;
                
                newRow.id = newID++;
                newRow.transactionID = FFDBDataSet.myDBGetNewID("transactionID", "LineItem");
                newRow.date = DateTime.Now.Date;
                newRow.lineTypeID = SpclLineType.NULL;
                newRow.accountID = SpclAccount.NULL;
                newRow.oppAccountID = SpclAccount.NULL;
                newRow.description = "";
                newRow.confirmationNumber = "";
                newRow.envelopeID = SpclEnvelope.NULL;
                newRow.complete = LineState.PENDING;
                newRow.amount = 0.0m;
                newRow.creditDebit = LineCD.CREDIT;
                newRow.transactionError = false;
                newRow.lineError = false;

                stayOut = false;
            }

            private void LineItemDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                if (stayOut)
                    return;

                stayOut = true;
                LineItemRow row = e.Row as LineItemRow;

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
                        string tmp = e.ProposedValue as string;
                        if (tmp.Length > 1)
                            row.complete = tmp.Substring(0, 1);
                        break;
                }

                stayOut = false;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Function Private
            ///////////////////////////////////////////////////////////////////////
            private void myValidateAmount(ref LineItemRow row, bool newCD, decimal newAmount)
            {
                if (newAmount < 0)
                {
                    newAmount = newAmount * -1;
                    newCD = !newCD;
                }

                // Keep only to the penny.
                newAmount = Convert.ToInt32(newAmount * 100) / 100.0m;

                if (newAmount == 0)
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
            }

            private void mySaveAddedRow(ref SqlCeCommand command, ref LineItemRow row)
            {
                string query;

                // INSERT INTO table_name (column1, column2, column3,...)
                // VALUES (value1, value2, value3,...)

                query = "INSERT INTO LineItem VALUES (";
                query += row.id.ToString() + ", ";
                query += row.transactionID.ToString() + ", ";
                query += "'" + row.date.ToString("d") + "', ";
                query += row.lineTypeID.ToString() + ", ";
                query += row.accountID.ToString() + ", ";
                query += row.oppAccountID.ToString() + ", ";

                if(row.IsdescriptionNull())
                    query += "null, ";
                else
                    query += "'" + row.description.Replace("'", "''") + "', ";

                if (row.IsconfirmationNumberNull())
                    query += "null, ";
                else
                    query += "'" + row.confirmationNumber.Replace("'", "''") + "', ";

                query += row.envelopeID.ToString() + ", ";
                query += "'" + row.complete + "', ";
                query += row.amount.ToString() + ", ";
                query += Convert.ToInt16(row.creditDebit).ToString() + ", ";
                query += Convert.ToInt16(row.transactionError).ToString() + ", ";
                query += Convert.ToInt16(row.lineError).ToString() + ");";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }

            private void myRemoveDeletedRow(ref SqlCeCommand command, ref LineItemRow row)
            {
                //string queryquery = "DELETE FROM LineItem WHERE id = " + row.id.ToString() + ";";

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
                query += "date = '" + row.date.ToString("d") + "', ";
                query += "lineTypeID = " + row.lineTypeID.ToString() + ", ";
                query += "accountID = " + row.accountID.ToString() + ", ";
                query += "oppAccountID = " + row.oppAccountID.ToString() + ", ";

                if (row.IsdescriptionNull())
                    query += "description = null, ";
                else
                    query += "description = '" + row.description.Replace("'", "''") + "', ";

                if (row.IsconfirmationNumberNull())
                    query += "confirmationNumber = null, ";
                else
                    query += "confirmationNumber = '" + row.confirmationNumber.Replace("'", "''") + "', ";

                query += "envelopeID = " + row.envelopeID.ToString() + ", ";
                query += "complete = '" + row.complete + "', ";
                query += "amount = " + row.amount.ToString() + ", ";
                query += "creditDebit = " + Convert.ToInt16(row.creditDebit).ToString() + ", ";
                query += "transactionError = " + Convert.ToInt16(row.transactionError).ToString() + ", ";
                query += "lineError = " + Convert.ToInt16(row.lineError).ToString() + " ";
                query += "WHERE id = " + row.id.ToString() + ";";

                command.CommandText = query;
                command.ExecuteNonQuery();
            }


            ///////////////////////////////////////////////////////////////////////
            //   Check, Ripple and Save the transactions
            ///////////////////////////////////////////////////////////////////////
            public bool myIsMinorSave;
            public bool myIsMajorSave;

            private int myC_TransactionID;
            private int myC_LineID;

            private int myC_creditCount;
            private int myC_debitCount; 
            private short myC_creditOppAccountID;
            private short myC_debitOppAccountID;
            private decimal myC_creditSum;
            private decimal myC_debitSum;

            private decimal myC_subSum;
            private int myC_subCount;
            private short myC_envelopeID;
            
            private void myGetOtherLinesInfoInDB(int transID, int lineID)
            {
                // Initialize values
                myC_TransactionID = transID;
                myC_LineID = lineID;
                myC_creditSum = 0.0m;
                myC_debitSum = 0.0m;
                myC_creditOppAccountID = SpclAccount.NULL;
                myC_debitOppAccountID = SpclAccount.NULL;
                myC_creditCount = 0;
                myC_debitCount = 0;

                // DB queries
                List<OtherLineDetails> otherLines = FFDBDataSet.myGetOtherLinesInTrans(lineID, transID);
                myC_subSum = FFDBDataSet.myDBGetSubSum(lineID, out myC_subCount, out myC_envelopeID);

                // Gather information from the other lines
                foreach (OtherLineDetails otherLine in otherLines)
                {
                    if (otherLine.creditDebit == LineCD.CREDIT)
                    {
                        myC_creditCount++;
                        myC_creditSum += otherLine.amount;
                        myC_debitOppAccountID = otherLine.accountID; // This is usefull when there is only one credit
                    }
                    else
                    {
                        myC_debitCount++;
                        myC_debitSum += otherLine.amount;
                        myC_creditOppAccountID = otherLine.accountID; // This is usefull when there is only one debit
                    }
                }
            }

            public void mySaveLine(int lineID)
            {
                LineItemRow thisLine = this.FindByid(lineID);
                bool usesEnvelopes = thisLine.AccountRowByFK_Line_accountID.envelopes;
                bool lineError;
                bool transError;

                if (thisLine == null)
                    return;

                // Gather information from this line
                if (thisLine.creditDebit == LineCD.CREDIT)
                {
                    myC_creditCount++;
                    myC_creditSum += thisLine.amount;
                    myC_debitOppAccountID = thisLine.accountID; // This is usefull when there is only one credit
                }
                else
                {
                    myC_debitCount++;
                    myC_debitSum += thisLine.amount;
                    myC_creditOppAccountID = thisLine.accountID; // This is usefull when there is only one debit
                }

                // Determine transaction and line errors.
                transError = (myC_creditSum != myC_debitSum);

                if ((usesEnvelopes && thisLine.amount == myC_subSum) || (!usesEnvelopes && myC_subSum == 0.0m))
                    lineError = false;
                else
                    lineError = true;

                // Set the errors in this line
                if (thisLine.transactionError != transError)
                    thisLine.transactionError = transError;

                if (thisLine.lineError != lineError)
                    thisLine.lineError = lineError;


                // TODO: Ripple oppAccount changes.

                // Determine the oppAccount values for complex transactions ELSE oppAccount has the correct accountID
                if (myC_creditCount > 1)
                    myC_debitOppAccountID = SpclAccount.MULTIPLE;

                if (myC_debitCount > 1)
                    myC_creditOppAccountID = SpclAccount.MULTIPLE;


                // Set the OppAcountID if needed
                if (thisLine.creditDebit == LineCD.CREDIT)
                {
                    if (thisLine.oppAccountID != myC_creditOppAccountID)
                        thisLine.oppAccountID = myC_creditOppAccountID;
                }
                else
                {
                    if (thisLine.oppAccountID != myC_debitOppAccountID)
                        thisLine.oppAccountID = myC_debitOppAccountID;
                }

                // Set the EnvelopeID if needed
                if (thisLine.envelopeID != envelopeID)
                    thisLine.envelopeID = envelopeID;
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
                            row.debitAmount = row.amount;
                            row.SetcreditAmountNull();
                            row.balanceAmount = balance += row.amount;
                        }
                        else
                        {
                            row.creditAmount = row.amount;
                            row.SetdebitAmountNull();
                            row.balanceAmount = balance -= row.amount;
                        }
                }
                else
                {
                    foreach (LineItemRow row in this)
                        if (row.creditDebit == LineCD.DEBIT)
                        {
                            row.debitAmount = row.amount;
                            row.SetcreditAmountNull();
                            row.balanceAmount = balance -= row.amount;
                        }
                        else
                        {
                            row.creditAmount = row.amount;
                            row.SetdebitAmountNull();
                            row.balanceAmount = balance += row.amount;
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

                stayOut = true;

                // Set the balances and get the accounts CD.
                balance = 0.0m;
                accountCD = (this.Rows[0] as LineItemRow).AccountRowByFK_Line_accountID.creditDebit;

                // Set balances By going down the list
                if (accountCD == LineCD.DEBIT)
                {
                    foreach (LineItemRow row in this)
                        if (row.creditDebit == LineCD.DEBIT)
                            row.balanceAmount = balance += row.amount;

                        else
                            row.balanceAmount = balance -= row.amount;
                }
                else
                {
                    foreach (LineItemRow row in this)
                        if (row.creditDebit == LineCD.DEBIT)
                            row.balanceAmount = balance -= row.amount;
                        else
                            row.balanceAmount = balance += row.amount;
                }

                this.AcceptChanges();
                stayOut = false;
            }

            public void myGetTransCDSum(int transID, out decimal creditSum, out decimal debitSum)
            {
                creditSum = 0.00m;
                debitSum = 0.00m;

                foreach (LineItemRow line in this)
                {
                    if(line.RowState != DataRowState.Deleted && line.transactionID == transID)
                    {
                        if(line.creditDebit == LineCD.CREDIT)
                            creditSum += line.amount;
                        else
                            debitSum += line.amount;
                    }
                }
            }



        }//END partial class LineItemDataTable
    }// END FamilyFinanceDBDataSet Partial Class
}// END nameSpace FamilyFinance
