using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2.SharedElements
{
    ///////////////////////////////////////////////////////////////////////
    //   Data Structures 
    ///////////////////////////////////////////////////////////////////////
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

    public class AccountError
    {
        public byte Catagory;
        public int TypeID;
        public int AccountID;

        public AccountError(byte catagory, int typeID, int accountID)
        {
            this.Catagory = catagory;
            this.TypeID = typeID;
            this.AccountID = accountID;
        }
    }


    ///////////////////////////////////////////////////////////////////////
    //   DBquery Class
    ///////////////////////////////////////////////////////////////////////
    public class DBquery
    {
        ///////////////////////////////////////////////////////////////////////
        //   Private STATIC Queries
        ///////////////////////////////////////////////////////////////////////
        static private bool executeFile(string fileAsString)
        {
            bool result = false;
            string[] commands = fileAsString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand sqlCmd = new SqlCeCommand();

            connection.Open();
            sqlCmd.Connection = connection;

            try
            {
                foreach (string line in commands)
                {
                    string temp = line.Trim();
                    if (temp != "")
                    {
                        temp += ";";
                        sqlCmd.CommandText = temp;
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                connection.Close();
                sqlCmd.Dispose();
            }

            return result;
        }

        static private object queryValue(string query)
        {
            object result;
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand sqlCmd = new SqlCeCommand(query, connection);

            connection.Open();

            result = sqlCmd.ExecuteScalar();

            connection.Close();
            sqlCmd.Dispose();

            return result;
        }

        static private decimal queryBalance(string query)
        {
            decimal result = 0.0m;
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand sqlCmd = new SqlCeCommand(query, connection);

            connection.Open();

            result = Convert.ToDecimal(sqlCmd.ExecuteScalar());

            connection.Close();
            sqlCmd.Dispose();

            return result;
        }

        static private List<int> queryIds(string query)
        {
            List<int> queryResults = new List<int>();
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            connection.Open();

            SqlCeDataReader reader = command.ExecuteReader();

            // Iterate through the results
            while (reader.Read())
                queryResults.Add(reader.GetInt32(0));

            // Always call Close the reader and connection when done reading
            reader.Close();
            command.Dispose();
            connection.Close();

            return queryResults;
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
            command.Dispose();
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
            command.Dispose();
            connection.Close();

            return queryResults;
        }

        static private List<AccountError> queryAccountErrors(string query)
        {
            List<AccountError> queryResults = new List<AccountError>();
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand(query, connection);
            connection.Open();

            SqlCeDataReader reader = command.ExecuteReader();


            // Iterate through the results
            while (reader.Read())
                queryResults.Add(new AccountError(reader.GetByte(0), reader.GetInt32(1), reader.GetInt32(2)));

            // Always call Close the reader and connection when done reading
            reader.Close();
            command.Dispose();
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
            command.Dispose();
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
            command.Dispose();
            connection.Close();

            return queryResults;
        }



        ///////////////////////////////////////////////////////////////////////
        //   Public STATIC Functions
        ///////////////////////////////////////////////////////////////////////
        static public bool createDBFile()
        {
            bool result = false;
            SqlCeEngine engine = new SqlCeEngine();
            engine.LocalConnectionString = Properties.Settings.Default.FFDBConnectionString;

            string filePath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            filePath += "\\" + Properties.Settings.Default.DBFileName;

            if (File.Exists(filePath))
                result = false;
            else
            {
                engine.CreateDatabase();
                engine.Dispose();

                result = buildTables();
            }

            return result;
        }

        static public bool goodPath()
        {
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            bool result;

            try
            {
                connection.Open();

                if (connection.State == ConnectionState.Open)
                    result = true;
                else
                    result = false;
            }
            catch (Exception)
            {
                result = false;
            }
                
            connection.Close();
            return result;
        }
        
        static public bool dropTables()
        {
            return executeFile(Properties.Resources.DropTables);
        }
        
        static public bool buildTables()
        {
            return executeFile(Properties.Resources.BuildTables);
        }

        static public bool deleteOrphanELines()
        {
            return executeFile(Properties.Resources.DeleteOrphanELines);
        }



        static public int getNewID(string col, string table)
        {
            string query;
            object result;
            int num;

            query = "SELECT MAX(" + col + ") FROM " + table + " ;";

            result = queryValue(query);

            if (result == DBNull.Value)
                return 1;

            num = Convert.ToInt32(result);

            if (num <= 0)
                return 1;

            return num + 1;
        }

        public static int getELineCount(int accountID)
        {
            int count;
            string query = Properties.Resources.CountELines.Replace("@@", accountID.ToString());

            count = Convert.ToInt32(queryValue(query));

            return count;
        }

        public static int getAccountErrorCount(int accountID)
        {
            int count;
            string query = Properties.Resources.CountAccountErrors.Replace("@@", accountID.ToString());

            count = Convert.ToInt32(queryValue(query));

            return count;
        }




        static public decimal getAccBalance(int accountID)
        {
            string query = Properties.Resources.AccountSingleBalance.Replace("@@", accountID.ToString());

            return queryBalance(query);
        }

        static public decimal getEnvBalance(int envelopeID)
        {
            string query = Properties.Resources.EnvelopeSingleBalance.Replace("@@", envelopeID.ToString());

            return queryBalance(query);
        }

        static public decimal getAEBalance(int accountID, int envelopeID)
        {
            string query = Properties.Resources.AESingleBalance.Replace("@eID", envelopeID.ToString());
            query = query.Replace("@aID", accountID.ToString());

            return queryBalance(query);
        }


        static public List<int> getTransactionErrors(int accountID)
        {
            string query = Properties.Resources.LineTransactionErrors.Replace("@@", accountID.ToString());

            return queryIds(query);
        }

        static public List<int> getLineErrors(int accountID)
        {
            string query = Properties.Resources.LineEnvelopeLineErrors.Replace("@@", accountID.ToString());

            return queryIds(query);
        }

        static public List<IdName> getAccountTypes(byte catagory)
        {
            string query = Properties.Resources.AccountTypes.Replace("@@", catagory.ToString());

            return queryIdNames(query);
        }

        static public List<IdName> getEnvelopeGroups()
        {
            return queryIdNames(Properties.Resources.EnvelopeGroups);
        }

        static public List<IdName> getAllEnvelopeNames()
        {
            string query = Properties.Resources.EnvelopeNames.Replace("@@", "");
            return queryIdNames(query);
        }

        static public List<IdName> getEnvelopeNamesByGroup(int groupID)
        {
            string query = Properties.Resources.EnvelopeNames;
            query = query.Replace("@@", "AND groupID = " + groupID.ToString());

            return queryIdNames(query);
        }



        static public List<IdBalance> getAccountBalancesByCatagory(byte catagory)
        {
            string query = Properties.Resources.AccountBalances.Replace("@@", catagory.ToString());

            return queryIdBalance(query);
        }

        static public List<IdBalance> getAccountBalancesByType(byte catagory, int typeID)
        {
            string query = Properties.Resources.AccountBalances;
            query = query.Replace("@@", catagory.ToString() + " AND typeID = " + typeID.ToString());

            return queryIdBalance(query);
        }

        static public List<IdBalance> getAllEnvelopeBalances()
        {
            string query = Properties.Resources.EnvelopeBalances.Replace("@@", "");

            return queryIdBalance(query);
        }

        static public List<IdBalance> getEnvelopeBalancesByGroup(int groupID)
        {
            string query = Properties.Resources.EnvelopeBalances;
            query = query.Replace("@@", "AND groupID = " + groupID.ToString());

            return queryIdBalance(query);
        }



        static public List<AccountError> getAccountErrors()
        {
            return queryAccountErrors(Properties.Resources.AccountErrors);
        }



        static public List<AccountDetails> getAccountNamesByCatagory(byte catagory)
        {
            string query = Properties.Resources.AccountDetails.Replace("@@", catagory.ToString());

            return queryAccountDetails(query);
        }

        static public List<AccountDetails> getAccountNamesByCatagoryAndType(byte catagory, int typeID)
        {
            string query = Properties.Resources.AccountDetails;
            query = query.Replace("@@", catagory.ToString() + " AND typeID = " + typeID.ToString());

            return queryAccountDetails(query);
        }




        static public List<SubBalanceDetails> getSubAccountBalances(int accountID)
        {
            string query = Properties.Resources.SubAccountBalances.Replace("@@", accountID.ToString());

            return querySubBalanceDetails(query);
        }

        static public List<SubBalanceDetails> getSubEnvelopeBalances(int envelopeID)
        {
            string query = Properties.Resources.SubEnvelopeBalances.Replace("@@", envelopeID.ToString());

            return querySubBalanceDetails(query);
        }




    }

} // END namespace FamilyFinance



