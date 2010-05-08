using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main.RegistrySplit.TreeView
{
    public class Name
    {
        public int ID;
        public string Name;

        public Name(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }

    public class Balance
    {
        public int ID;
        public decimal Balance;

        public Balance(int id, decimal balance)
        {
            this.ID = id;
            this.Balance = balance;
        }
    }

    public class AccountDetails
    {
        public int ID;
        public string Name;
        public bool Envelopes;

        public AccountDetails(int id, string name, bool envelopes)
        {
            this.ID = id;
            this.Name = name;
            this.Envelopes = envelopes;
        }
    }

    public class SubBalanceDetails
    {
        public int ID;
        public string Name;
        public decimal SubBalance;

        public SubBalanceDetails(int id, string name, decimal balance)
        {
            this.ID = id;
            this.Name = name;
            this.SubBalance = balance;
        }
    }

    public class TreeQuery
    {
        static public List<Name> getAccountTypes(byte catagory)
        {
            List<Name> queryResults = new List<Name>();
            
            string query = Properties.Resources.AccountTypes.Replace("@@", catagory.ToString());
            
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            { // Iterate through the results
                while (reader.Read())
                    queryResults.Add(new Name(reader.GetInt32(0), reader.GetString(1)));
            }
            finally
            { // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public List<Name> getEnvelopeGroups()
        {
            List<Name> queryResults = new List<Name>();
            string query = Properties.Resources.EnvelopeGroups;
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            { // Iterate through the results
                while (reader.Read())
                    queryResults.Add(new Name(reader.GetInt32(0), reader.GetString(1)));
            }
            finally
            { // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public List<Name> getEnvelopeNames(int groupID)
        {
            List<Name> queryResults = new List<Name>();
            string query = Properties.Resources.AccountBalances;

            if (groupID == SpclEnvelopeGroup.NULL)
                query = query.Replace("@@", "");
            else
                query = query.Replace("@@", "AND groupID = " + groupID.ToString());

            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);

            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            { // Iterate through the results
                while (reader.Read())
                {
                    queryResults.Add(new Name(reader.GetInt32(0), reader.GetString(1)));
                }
            }
            finally
            { // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public List<AccountDetails> getAccountNames(byte catagory, int typeID)
        {
            List<AccountDetails> queryResults = new List<AccountDetails>();
            string query = Properties.Resources.AccountNames;

            if (typeID == SpclAccountType.NULL)
                query = query.Replace("@@", catagory.ToString());
            else
                query = query.Replace("@@", catagory.ToString() + " AND typeID = " + typeID.ToString());

            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            try
            { // Iterate through the results
                while (reader.Read())
                    queryResults.Add(new AccountDetails(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2)));
            }
            finally
            { // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public List<Balance> getAccountBalances(int typeID)
        {
            Dictionary<int, decimal> queryResults = new Dictionary<int, decimal>();
            string query = Properties.Resources.AccountBalances;

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
                    queryResults.Add(reader.GetInt32(0), reader.GetDecimal(1));
                }
            }
            finally
            { // Always call Close the reader and connection when done reading
                reader.Close();
                connection.Close();
            }

            return queryResults;
        }

        static public List<Balance> getEnvelopeBalances(int groupID)
        {
            List<EnvelopeBalanceDetails> queryResults = new List<EnvelopeBalanceDetails>();
            string query = Properties.Resources.EnvelopeBalances;

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

        static public List<SubBalanceDetails> getSubAccountBalances(int accountID)
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

                    ad.ID = reader.GetInt32(0);
                    ad.Name = reader.GetString(1);
                    ad.SubBalance = reader.GetDecimal(2);

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

        static public List<SubBalanceDetails> getSubEnvelopeBalanses(int envelopeID)
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
                    ad.ID = reader.GetInt32(0);
                    ad.Name = reader.GetString(1);
                    ad.SubBalance = reader.GetDecimal(2);

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
