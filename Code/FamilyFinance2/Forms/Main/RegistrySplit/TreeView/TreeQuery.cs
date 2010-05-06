using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main.RegistrySplit.TreeView
{
    public class AccountBalanceDetails
    {
        public int accountID;
        public string accountName;
        public decimal balance;
        public bool envelopes;
    }

    public class EnvelopeBalanceDetails
    {
        public int envelopeID;
        public string envelopeName;
        public decimal balance;
    }

    public class SubBalanceDetails
    {
        public int id;
        public string name;
        public decimal subBalance;
    }

    public class TreeQuery
    {
        static public Dictionary<int, string> getAccountTypes(byte catagory)
        {
            Dictionary<int, string> queryResults = new Dictionary<int, string>();
            
            string query = Properties.Resources.AccountTypes.Replace("@@", catagory.ToString());
            
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            { // Iterate through the results
                while (reader.Read())
                    queryResults.Add(reader.GetInt32(0), reader.GetString(1));
            }
            finally
            { // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public Dictionary<int, string> getGroups()
        {
            Dictionary<int, string> queryResults = new Dictionary<int, string>();
            string query = Properties.Resources.Groups;
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            { // Iterate through the results
                while (reader.Read())
                    queryResults.Add(reader.GetInt32(0), reader.GetString(1));
            }
            finally
            { // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }
        
        static public Dictionary<int, string> getAccountNamesByCatAndType(byte catagory, int typeID)
        {
            Dictionary<int, string> queryResults = new Dictionary<int, string>();
            string query = Properties.Resources.IncomeOrExpense;

            if (typeID != SpclAccountType.NULL)
                query = query.Replace("@@", catagory.ToString() + " AND typeID = " + typeID.ToString());
            else
                query = query.Replace("@@", catagory.ToString());

            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            { // Iterate through the results
                while (reader.Read())
                    queryResults.Add(reader.GetInt32(0), reader.GetString(1));
            }
            finally
            { // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public List<AccountBalanceDetails> getRealAccountsDetails(int typeID)
        {
            List<AccountBalanceDetails> queryResults = new List<AccountBalanceDetails>();
            string query = Properties.Resources.RealAccountDetails;

            if (typeID != SpclAccountType.NULL)
                query = query.Replace("@@", " AND typeID = " + typeID.ToString());
            else
                query = query.Replace("@@", "");

            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            { // Iterate through the results
                while (reader.Read())
                {
                    AccountBalanceDetails acd = new AccountBalanceDetails();
                    acd.accountID = reader.GetInt32(0);
                    acd.accountName = reader.GetString(1);
                    bool creditDebit = reader.GetBoolean(2);
                    acd.envelopes = reader.GetBoolean(3);
                    decimal credit = reader.GetDecimal(4);
                    decimal debit = reader.GetDecimal(5);

                    if (creditDebit == LineCD.DEBIT)
                        acd.balance = debit - credit;
                    else
                        acd.balance = credit - debit;

                    queryResults.Add(acd);
                }
            }
            finally
            { // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public List<EnvelopeBalanceDetails> getEnvelopeDetails(int groupID)
        {
            List<EnvelopeBalanceDetails> queryResults = new List<EnvelopeBalanceDetails>();
            string query = Properties.Resources.EnvelopeDetails;

            if (groupID != SpclEnvelopeGroup.NULL)
                query = query.Replace("@@", " AND groupID = " + groupID.ToString());
            else
                query = query.Replace("@@", "");

            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);

            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            {
                // Iterate through the results
                while (reader.Read())
                {
                    EnvelopeBalanceDetails acd = new EnvelopeBalanceDetails();
                    acd.envelopeID = reader.GetInt32(0);
                    acd.envelopeName = reader.GetString(1);
                    acd.balance = reader.GetDecimal(2);

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

        static public List<SubBalanceDetails> getSubAccountDetails(int accountID)
        {
            List<SubBalanceDetails> queryResults = new List<SubBalanceDetails>();
            string query = Properties.Resources.SubAccountDetails;
            query = query.Replace("@@", accountID.ToString());

            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);

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

        static public List<SubBalanceDetails> getSubEnvelopeDetails(int envelopeID)
        {
            List<SubBalanceDetails> queryResults = new List<SubBalanceDetails>();
            string query = Properties.Resources.SubEnvelopeDetails;
            query = query.Replace("@@", envelopeID.ToString());

            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);

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

    }
}
