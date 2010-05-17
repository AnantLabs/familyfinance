using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2.SharedElements
{
    public partial class FFDataBase
    {
        ///////////////////////////////////////////////////////////////////////
        //   Functions STATIC 
        ///////////////////////////////////////////////////////////////////////
        static public void myExecuteFile(string fileAsString, bool catchExceptions)
        {
            SqlCeConnection connection;
            SqlCeCommand command;
            string scriptLine = "";
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
            fileAsString = fileAsString.Replace("\t", " ");


            connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            connection.Open();



            try
            {
                while (true)
                {
                    // Find the next statments end
                    end = fileAsString.IndexOf(";", 0) + 1;
                    if (end == 0)
                        break;

                    scriptLine = fileAsString.Substring(0, end);
                    fileAsString = fileAsString.Remove(0, end);

                    // Execute the statement
                    command = new SqlCeCommand(scriptLine, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Caught a bad SQL Line <" + scriptLine + ">", e);
            }
            finally
            {
                connection.Close();
            }

        }

        static public bool myCreateDBFile()
        {
            SqlCeEngine engine = new SqlCeEngine();
            string connection = Properties.Settings.Default.FFDBConnectionString;
            string filePath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            filePath += "\\" + Properties.Settings.Default.DBFileName;

            if (File.Exists(filePath))
                return false;

            engine.LocalConnectionString = connection;

            try
            {
                engine.CreateDatabase();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to make the DBFile", e);
            }
            finally
            {
                engine.Dispose();
            }

            myExecuteFile(Properties.Resources.BuildTables, true);

            return true;
        }

        static public bool myGoodPath()
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
            catch (Exception e)
            {
                string temp = e.Message;
                result = false;
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        static public List<int> myDBGetIDList(string col, string table, string filter, string sort)
        {
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            List<int> idList = new List<int>();
            SqlCeCommand selectCmd;
            SqlCeDataReader reader;
            string command;

            connection.Open();

            command = "SELECT DISTINCT " + col;
            command += " FROM " + table;

            if (filter != "")
                command += " WHERE " + filter;

            if (sort != "")
                command += " ORDER BY " + sort;

            command += " ;";

            selectCmd = new SqlCeCommand(command, connection);
            reader = selectCmd.ExecuteReader();

            while (reader.Read())
                idList.Add(reader.GetInt32(0));

            reader.Close();
            connection.Close();

            return idList;
        }

        static public int myDBGetNewID(string col, string table)
        {
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand selectCmd = new SqlCeCommand();
            object result;
            int num;

            connection.Open();

            selectCmd.Connection = connection;
            selectCmd.CommandText = "SELECT MAX(" + col + ") FROM " + table + " ;";
            result = selectCmd.ExecuteScalar();

            connection.Close();

            if (result == DBNull.Value)
                return 1;

            num = Convert.ToInt32(result);

            if (num <= 0)
                return 1;

            return num + 1;
        }

        static public decimal myDBGetSubSum(int lineID, out int subCount, out short envelopeID)
        {
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand selectCmd = new SqlCeCommand();
            SqlCeDataReader reader;
            decimal sum;

            sum = 0.0m;
            subCount = 0;
            envelopeID = SpclEnvelope.NULL;

            connection.Open();

            selectCmd.Connection = connection;
            selectCmd.CommandText = "SELECT amount, envelopeID FROM SubLineItem WHERE lineItemID = " + lineID.ToString() + ";";
            reader = selectCmd.ExecuteReader();

            while (reader.Read())
            {
                sum += reader.GetDecimal(0);
                subCount++;
                envelopeID = reader.GetInt16(1);
            }

            if (subCount > 1)
                envelopeID = SpclEnvelope.SPLIT;

            reader.Close();
            connection.Close();

            return sum;
        }
    }


} // END namespace FamilyFinance



