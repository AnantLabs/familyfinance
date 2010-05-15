using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main.RegistrySplit.TreeView
{
    public class IdName
    {
        public int ID;
        public string Name;

        public IdName(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }

    public class IdBalance
    {
        public int ID;
        public decimal Balance;

        public IdBalance(int id, decimal balance)
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

    public class AccountErrors
    {
        public byte Catagory;
        public int TypeID;
        public int AccountID;

        public AccountErrors(byte catagory, int typeID, int accountID)
        {
            this.Catagory = catagory;
            this.TypeID = typeID;
            this.AccountID = accountID;
        }
    }

    public class TreeQuery
    {
        static private decimal queryBalance(string query)
        {
            decimal result = 0.0m;
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);

            connection.Open();
            result = Convert.ToDecimal(command.ExecuteScalar());

            return result;
        }

        static private List<IdName> queryIdNames(string query)
        {
            List<IdName> queryResults = new List<IdName>();
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            // Iterate through the results
            while (reader.Read())
                queryResults.Add(new IdName(reader.GetInt32(0), reader.GetString(1)));

            // Always call Close the reader and connection when done reading
            reader.Close();
            connection.Close();

            return queryResults;
        }

        static private List<IdBalance> queryIdBalance(string query)
        {
            List<IdBalance> queryResults = new List<IdBalance>();
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            // Iterate through the results
            while (reader.Read())
                queryResults.Add(new IdBalance(reader.GetInt32(0), reader.GetDecimal(1)));
            
            // Always call Close the reader and connection when done reading
            reader.Close();
            connection.Close();

            return queryResults;
        }

        static private List<AccountErrors> queryAccountErrors(string query)
        {
            List<AccountErrors> queryResults = new List<AccountErrors>();
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);

            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            // Iterate through the results
            while (reader.Read())
                queryResults.Add(new AccountErrors(reader.GetByte(0), reader.GetInt32(1), reader.GetInt32(2)));

            // Always call Close the reader and connection when done reading
            reader.Close();
            connection.Close();

            return queryResults;
        }

        static private List<AccountDetails> queryAccountDetails(string query)
        {
            List<AccountDetails> queryResults = new List<AccountDetails>();
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            // Iterate through the results
            while (reader.Read())
                queryResults.Add(new AccountDetails(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2)));
            
            // Always call Close the reader and connection when done reading
            reader.Close();
            connection.Close();

            return queryResults;
        }

        static private List<SubBalanceDetails> querySubBalanceDetails(string query)
        {
            List<SubBalanceDetails> queryResults = new List<SubBalanceDetails>();
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);

            connection.Open();
            SqlCeDataReader reader = command.ExecuteReader();

            // Iterate through the results
            while (reader.Read())
                queryResults.Add(new SubBalanceDetails(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(2)));

            // Always call Close the reader and connection when done reading
            reader.Close();
            connection.Close();

            return queryResults;
        }


        static public List<AccountErrors> getAccountErrors()
        {
            return TreeQuery.queryAccountErrors(Properties.Resources.ErrorAccounts);
        }


        static public decimal getAccBalance(int accountID)
        {
            string query = Properties.Resources.SingleAccBalance.Replace("@@", accountID.ToString());
            return queryBalance(query);
        }

        static public decimal getEnvBalance(int envelopeID)
        {
            string query = Properties.Resources.SingleEnvBalance.Replace("@@", envelopeID.ToString());
            return queryBalance(query);
        }

        static public decimal getAEBalance(int accountID, int envelopeID)
        {
            string query = Properties.Resources.SingleAEBalance.Replace("@eID", envelopeID.ToString());
            query = query.Replace("@aID", accountID.ToString());
            return queryBalance(query);
        }


        static public List<IdName> getAccountTypes(byte catagory)
        {
            string query = Properties.Resources.AccountTypes.Replace("@@", catagory.ToString());
            return TreeQuery.queryIdNames(query);
        }

        static public List<IdName> getEnvelopeGroups()
        {
            return TreeQuery.queryIdNames(Properties.Resources.EnvelopeGroups);
        }

        static public List<IdName> getAllEnvelopeNames()
        {
            string query = Properties.Resources.EnvelopeNames.Replace("@@", "");
            return TreeQuery.queryIdNames(query);
        }

        static public List<IdName> getEnvelopeNamesByGroup(int groupID)
        {
            string query = Properties.Resources.EnvelopeNames;
            query = query.Replace("@@", "AND groupID = " + groupID.ToString());

            return TreeQuery.queryIdNames(query);
        }




        static public List<AccountDetails> getAccountNamesByCatagory(byte catagory)
        {
            string query = Properties.Resources.AccountDetails.Replace("@@", catagory.ToString());
            return TreeQuery.queryAccountDetails(query);
        }

        static public List<AccountDetails> getAccountNamesByCatagoryAndType(byte catagory, int typeID)
        {
            string query = Properties.Resources.AccountDetails;
            query = query.Replace("@@", catagory.ToString() + " AND typeID = " + typeID.ToString());

            return TreeQuery.queryAccountDetails(query);
        }



        static public List<IdBalance> getAccountBalancesByCatagory(byte catagory)
        {
            string query = Properties.Resources.AccountBalances.Replace("@@", catagory.ToString());
            return TreeQuery.queryIdBalance(query);
        }

        static public List<IdBalance> getAccountBalancesByType(byte catagory, int typeID)
        {
            string query = Properties.Resources.AccountBalances;
            query = query.Replace("@@", catagory.ToString() + " AND typeID = " + typeID.ToString());

            return TreeQuery.queryIdBalance(query);
        }

        static public List<IdBalance> getAllEnvelopeBalances()
        {
            string query = Properties.Resources.EnvelopeBalances.Replace("@@", "");
            return TreeQuery.queryIdBalance(query);
        }

        static public List<IdBalance> getEnvelopeBalancesByGroup(int groupID)
        {
            string query = Properties.Resources.EnvelopeBalances;
            query = query.Replace("@@", "AND groupID = " + groupID.ToString());

            return TreeQuery.queryIdBalance(query);
        }




        static public List<SubBalanceDetails> getSubAccountBalances(int accountID)
        {
            string query = Properties.Resources.SubAccountBalances.Replace("@@", accountID.ToString());
            return TreeQuery.querySubBalanceDetails(query);
        }

        static public List<SubBalanceDetails> getSubEnvelopeBalances(int envelopeID)
        {
            string query = Properties.Resources.SubEnvelopeBalances.Replace("@@", envelopeID.ToString());
            return TreeQuery.querySubBalanceDetails(query);
        }




    }

}
