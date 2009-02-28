using System;
using System.Data.SqlServerCe;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace FamilyFinance2 
{
    public partial class FFDBDataSet 
    {

        ///////////////////////////////////////////////////////////////////////
        //   Functions STATIC 
        ///////////////////////////////////////////////////////////////////////
        static private SqlCeConnection myConnection;

        static private void myConnect()
        {
            if (
                myConnection == null || 
                myConnection.State == ConnectionState.Closed || 
                myConnection.State == ConnectionState.Broken
                )
                myConnection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
        }

        static public bool myCreateDBFile()
        {
            SqlCeEngine engine = new SqlCeEngine();
            SqlCeConnection connection;
            string connectionString = Properties.Settings.Default.FFDBConnectionString;
            string filePath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            filePath += "\\" + Properties.Settings.Default.DBFileName;

            if (File.Exists(filePath))
                return false;

            engine.LocalConnectionString = connectionString;

            try
            {
                engine.CreateDatabase();
            }

            catch (Exception e)
            {
                throw new Exception("Failed to make the DBFile", e);
            }

            engine.Dispose();

            connection = new SqlCeConnection(connectionString);

            connection.Open();
            myExecuteFile(connection, Properties.Resources.Build_Tables, true);
            connection.Close();
            connection.Dispose();

            return true;
        }

        static public bool myGoodPath()
        {
            SqlCeConnection connection;
            string connectionString;
            bool result;

            connectionString = Properties.Settings.Default.FFDBConnectionString;
            connection = new SqlCeConnection(connectionString);

            try
            {
                connection.Open();

                if (connection.State == ConnectionState.Open)
                    result = true;
                else
                    result = false;
            }
            catch (Exception e)
            {
                string temp = e.Message;
                result = false;
            }

            connection.Close();
            connection.Dispose();

            return result;
        }

        static private void myExecuteFile(SqlCeConnection connection, string fileAsString, bool catchExceptions)
        {
            SqlCeCommand sqlCmd;
            string command;
            int start = 0;
            int end = 0;


            // Remove all comments
            while (true)
            {
                start = fileAsString.IndexOf("--", 0);
                if (start == -1)
                    break;

                end = fileAsString.IndexOf("\n", start) + 1;
                fileAsString = fileAsString.Remove(start, end - start);
            }


            // Replace all the white space characters
            fileAsString = fileAsString.Replace("\n", "");
            fileAsString = fileAsString.Replace("\r", "");
            fileAsString = fileAsString.Replace('\t', ' ');


            while (true)
            {
                // Find the next statment end
                end = fileAsString.IndexOf(";", 0) + 1;
                if (end == 0)
                    break;

                command = fileAsString.Substring(0, end);
                fileAsString = fileAsString.Remove(0, end);

                sqlCmd = new SqlCeCommand(command, connection);

                try { sqlCmd.ExecuteNonQuery(); }

                catch (Exception e)
                {
                    throw new Exception("Caught a bad SQL Line <" + command + ">", e);
                }
            }
        }

        static private object myNewID(string table, string column)
        {
            object result;
            string select;
            SqlCeCommand command;

            myConnect();

            select = "SELECT MAX( " + column + " ) FROM " + table;
            command = new SqlCeCommand(select, myConnection);

            result = command.ExecuteScalar();

            if (result == DBNull.Value)
                result = 1;

            return result;
        }

    }
}
