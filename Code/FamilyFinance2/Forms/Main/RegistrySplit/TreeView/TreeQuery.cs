using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2.Forms.Main.RegistrySplit.TreeView
{
    public class AccountBalanceDetails
    {
        public int accountID;
        public string accountName;
        public decimal balance;
        public bool envelopes;
    }

    public class SubBalanceDetails
    {
        public int id;
        public string name;
        public decimal subBalance;
    }

    //public class IncomeExpenseDetails
    //{
    //    public string typeName;
    //    public int typeID;
    //    public int accountID;
    //    public string accountName;
    //}

    public class TreeQuery
    {
        static public Dictionary<int, string> getTypesForCatagory(byte catagory)
        {
            Dictionary<int, string> queryResults = new Dictionary<int, string>();
            string query;

            query = "  SELECT DISTINCT at.id, at.name";
            query += " FROM AccountType AS at INNER JOIN Account AS a ON at.id = a.typeID";
            query += " WHERE a.closed = 0 AND a.catagory = " + catagory.ToString();
            query += " ORDER BY at.name;";

            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            {
                // Iterate through the results
                while (reader.Read())
                {
                    queryResults.Add(reader.GetInt32(0), reader.GetString(1));
                }
            }
            finally
            {
                // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public Dictionary<int, string> getInExForCatagory(byte catagory)
        {
            Dictionary<int, string> queryResults = new Dictionary<int, string>();
            string query;

            query = "  SELECT id, name";
            query += " FROM Account";
            query += " WHERE closed = 0 AND catagory = " + catagory.ToString();
            query += " ORDER BY name;";

            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            {
                // Iterate through the results
                while (reader.Read())
                {
                    queryResults.Add(reader.GetInt32(0), reader.GetString(1));
                }
            }
            finally
            {
                // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public List<AccountBalanceDetails> getAccountsForCatagoryDetails()
        {
            List<AccountBalanceDetails> queryResults = new List<AccountBalanceDetails>();
            SqlCeConnection connection;
            SqlCeCommand command;
            string query;

            query = "  SELECT a.id, a.name, a.creditDebit, a.envelopes, lSum.credit, lSum.debit";
            query += " FROM Account AS a INNER JOIN ";
            query += " 	 ( SELECT accountID AS aID, SUM(CASE WHEN l.creditDebit = 0 THEN l.amount ELSE 0 END) AS credit, SUM(CASE WHEN l.creditDebit = 1 THEN l.amount ELSE 0 END) AS debit";
            query += " 	   FROM LineItem AS l";
            query += " 	   WHERE accountID IN (SELECT id FROM Account WHERE catagory = 2 AND closed = 0)";
            query += " 	   GROUP BY accountID";
            query += "   ) AS lSum ON a.id = lSum.aID";
            query += " ORDER BY a.name;";

            connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            {
                // Iterate through the results
                while (reader.Read())
                {
                    AccountBalanceDetails acd = new AccountBalanceDetails();
                    acd.accountID = reader.GetInt32(0);
                    acd.accountName = reader.GetString(1);
                    acd.envelopes = reader.GetBoolean(3);
                    decimal credit = reader.GetDecimal(4);
                    decimal debit = reader.GetDecimal(5);

                    if (reader.GetBoolean(2))
                        acd.balance = debit - credit;
                    else
                        acd.balance = credit - debit;

                    queryResults.Add(acd);
                }
            }
            finally
            {
                // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public List<AccountBalanceDetails> getAccountsForTypeDetails(int typeID)
        {
            List<AccountBalanceDetails> queryResults = new List<AccountBalanceDetails>();
            SqlCeConnection connection;
            SqlCeCommand command;
            string query;

            query = "  SELECT at.id, at.name, a.id, a.name, a.creditDebit, a.envelopes, lSum.credit, lSum.debit";
            query += " FROM AccountType AS at ";
            query += "   INNER JOIN Account AS a ON at.id = a.typeID";
            query += "   INNER JOIN ";
            query += " 	 ( SELECT accountID AS aID, SUM(CASE WHEN l.creditDebit = 0 THEN l.amount ELSE 0 END) AS credit, SUM(CASE WHEN l.creditDebit = 1 THEN l.amount ELSE 0 END) AS debit";
            query += " 	   FROM LineItem AS l";
            query += " 	   WHERE accountID IN (SELECT id FROM Account WHERE catagory = 2 AND closed = 0 AND typeID = " + typeID.ToString() + ")";
            query += " 	   GROUP BY accountID";
            query += "   ) AS lSum ON a.id = lSum.aID";
            query += " ORDER BY at.name, a.name;";

            connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            {
                // Iterate through the results
                while (reader.Read())
                {
                    AccountBalanceDetails acd = new AccountBalanceDetails();
                    acd.accountID = reader.GetInt32(0);
                    acd.accountName = reader.GetString(1);
                    acd.envelopes = reader.GetBoolean(3);
                    decimal credit = reader.GetDecimal(4);
                    decimal debit = reader.GetDecimal(5);

                    if (reader.GetBoolean(2))
                        acd.balance = debit - credit;
                    else
                        acd.balance = credit - debit;

                    queryResults.Add(acd);
                }
            }
            finally
            {
                // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public List<SubBalanceDetails> myGetSubAccountBalanceDetails(int accountID)
        {
            List<SubBalanceDetails> queryResults = new List<SubBalanceDetails>();
            SqlCeConnection connection;
            SqlCeCommand command;
            string query;

            query = "  SELECT e.id, e.name, eBal.debit - eBal.credit AS balance";
            query += " FROM Envelope AS e INNER JOIN ";
            query += " 	(SELECT el.envelopeID AS eID, SUM(CASE WHEN li.creditDebit = 0 THEN el.amount ELSE 0 END) AS credit, SUM(CASE WHEN li.creditDebit = 1 THEN el.amount ELSE 0 END) AS debit";
            query += " 	 FROM LineItem AS li INNER JOIN EnvelopeLine AS el ON li.id = el.lineItemID";
            query += " 	 WHERE li.accountID = " + accountID.ToString();
            query += " 	 GROUP BY el.envelopeID) AS eBal ON e.id = eBal.eID";
            query += " WHERE e.closed = 0 AND eBal.credit <> eBal.debit;";

            connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            {
                // Iterate through the results
                while (reader.Read())
                {
                    SubBalanceDetails ad = new SubBalanceDetails();

                    ad.id = reader.GetInt32(0);
                    ad.name = reader.GetString(1);
                    ad.subBalance = reader.GetDecimal(2);

                    queryResults.Add(ad);
                }
            }
            finally
            {
                // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public Dictionary<int, string> myGetIncomeExpenseForCatagory(byte catagory)
        {
            Dictionary<int, string> queryResults = new Dictionary<int, string>();
            SqlCeConnection connection;
            SqlCeCommand command;
            string query;

            query = "  SELECT at.id, at.name, a.id, a.name";
            query += " FROM AccountType AS at INNER JOIN Account AS a ON at.id = a.typeID";
            query += " WHERE a.catagory = " + catagory.ToString() + " AND a.closed = 0";
            query += " ORDER BY at.name, a.name";

            connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            {
                // Iterate through the results
                while (reader.Read())
                {
                    //IncomeExpenseDetails ied = new IncomeExpenseDetails();

                    //ied.typeID = reader.GetInt32(0);
                    //ied.typeName = reader.GetString(1);
                    //ied.accountID = reader.GetInt32(2);
                    //ied.accountName = reader.GetString(3);

                    //queryResults.Add(ied);
                }
            }
            finally
            {
                // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public List<SubBalanceDetails> myGetSubEnvelopeBalanceDetails(int envelopeID)
        {
            List<SubBalanceDetails> queryResults = new List<SubBalanceDetails>();
            SqlCeConnection connection;
            SqlCeCommand command;
            string query;

            query = "  SELECT AEBalance.accountID, Account.name, AEBalance.currentBalance ";
            query += " FROM AEBalance INNER JOIN Account ON AEBalance.accountID = Account.id ";
            query += " WHERE AEBalance.currentBalance <> 0.0 AND AEBalance.envelopeID = " + envelopeID.ToString();
            query += " ORDER BY Account.name ";

            connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            {
                // Iterate through the results
                while (reader.Read())
                {
                    SubBalanceDetails ad = new SubBalanceDetails();
                    ad.id = reader.GetInt16(0);
                    ad.name = reader.GetString(1);
                    ad.subBalance = reader.GetDecimal(2);

                    queryResults.Add(ad);
                }
            }
            finally
            {
                // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

    }
}
