using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class SubLineViewDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////
            //   Function Overrides
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();
            }


            ///////////////////////////////////////////////////////////////////////
            //   Function Private
            ///////////////////////////////////////////////////////////////////////
            private void myFillCDandOther()
            {
                decimal balance = 0.0m;

                foreach (SubLineViewRow row in this)
                {
                    if (row.creditDebit == LineCD.CREDIT)
                    {
                        balance -= row.amount;
                        row.balanceAmount = balance;
                        row.creditAmount = row.amount;
                        row.SetdebitAmountNull();
                    }
                    else
                    {
                        balance += row.amount;
                        row.balanceAmount = balance;
                        row.debitAmount = row.amount;
                        row.SetcreditAmountNull();

                        string swap = row.sourceAccount;
                        row.sourceAccount = row.destinationAccount;
                        row.destinationAccount = swap;
                    }
                }
            }
            

            ///////////////////////////////////////////////////////////////////////
            //   Function Public
            ///////////////////////////////////////////////////////////////////////
            public void myFillByEnvelopeAndAccount(short envelopeID, short accountID)
            {
                string query;
                object[] newRow = new object[this.Columns.Count];

                this.Rows.Clear();

                query =  "SELECT s.id AS subLineItemID, l.transactionID, l.date, lt.name AS lineType, a.name AS sourceAccount, l.transactionError | l.lineError AS lineError, a1.name AS destinationAccount, s.description, l.creditDebit, s.amount, l.complete ";//, 0.0 AS creditAmount, 0.0 AS debitAmount, 0.0 AS balanceAmount ";

                query += "FROM        LineItem    AS l  ";
                query += " INNER JOIN LineType    AS lt ON l.lineTypeID = lt.id ";
                query += " INNER JOIN Account     AS a  ON l.accountID = a.id ";
                query += " INNER JOIN Account     AS a1 ON l.oppAccountID = a1.id ";
                query += " INNER JOIN SubLineItem AS s  ON l.id = s.lineItemID ";

                query += "WHERE (s.envelopeID = " + envelopeID.ToString() + ") AND (l.accountID = " + accountID.ToString() + ") ";
                query += "ORDER BY l.date, l.creditDebit DESC, subLineItemID ;";

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
                this.myFillCDandOther();
                this.AcceptChanges();
            }

            public void myFillByEnvelope(short envelopeID)
            {
                string query;
                object[] newRow = new object[this.Columns.Count];

                this.Rows.Clear();

                query = "SELECT s.id AS subLineItemID, l.transactionID, l.date, lt.name AS lineType, a.name AS sourceAccount, l.transactionError | l.lineError AS lineError, a1.name AS destinationAccount, s.description, l.creditDebit, s.amount, l.complete ";//, 0.0 AS creditAmount, 0.0 AS debitAmount, 0.0 AS balanceAmount ";

                query += "FROM        LineItem    AS l  ";
                query += " INNER JOIN LineType    AS lt ON l.lineTypeID = lt.id ";
                query += " INNER JOIN Account     AS a  ON l.accountID = a.id ";
                query += " INNER JOIN Account     AS a1 ON l.oppAccountID = a1.id ";
                query += " INNER JOIN SubLineItem AS s  ON l.id = s.lineItemID ";

                query += "WHERE s.envelopeID = " + envelopeID.ToString();
                query += " ORDER BY l.date, l.creditDebit DESC, subLineItemID;";

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
                this.myFillCDandOther();
                this.AcceptChanges();
            }
            

            // Do not add functions that pull information from this table because
            // this table is not garenteed to be current.
            // Pull information from LineItem and SubLineItem instead.

        }// END endpartial class SubLineViewDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance
