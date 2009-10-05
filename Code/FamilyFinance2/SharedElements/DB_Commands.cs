using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
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

            myExecuteFile(Properties.Resources.Build_Tables, true);

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

        static public void myResetAccountBalances()
        {
            //AccountDataTable account = new AccountDataTable();
            //AccountTableAdapter accTA = new AccountTableAdapter();
            //List<AccountSums> sums = new List<AccountSums>();

            //// Bring up the Account table with the ending and current balances Zeroed out.
            //accTA.FillByZero(account);

            //// Get the endingBalance Sums
            //sums = FFDataBase.myGetAccountSums();

            //// Reset the endingBalances
            //foreach (AccountSums sumRow in sums)
            //{
            //    if (account.FindByid(sumRow.accountID).creditDebit == LineCD.DEBIT)
            //    {
            //        // If this in a Debit account (Checking, Savings) then subtract the Credits and add the debits
            //        if (sumRow.creditDebit == LineCD.CREDIT)
            //            account.FindByid(sumRow.accountID).endingBalance -= sumRow.balance;
            //        else
            //            account.FindByid(sumRow.accountID).endingBalance += sumRow.balance;
            //    }
            //    else
            //    {
            //        // Else this is a Credit account (Loan, Credit) then add the Credits and subtract the debits
            //        if (sumRow.creditDebit == LineCD.CREDIT)
            //            account.FindByid(sumRow.accountID).endingBalance += sumRow.balance;
            //        else
            //            account.FindByid(sumRow.accountID).endingBalance -= sumRow.balance;
            //    }
            //}

            //// Get tomorrows Date and the currentBalance Sums
            //DateTime tomorrow = DateTime.Today;
            //tomorrow.AddDays(1.0);
            //sums.Clear();
            //sums = FFDataBase.myGetAccountSumsBeforeDate(tomorrow);

            //// Reset the currentBalances
            //foreach (AccountSums sumRow in sums)
            //{
            //    if (account.FindByid(sumRow.accountID).creditDebit == LineCD.DEBIT)
            //    {
            //        // If this in a Debit account (Checking, Savings) then subtract the Credits and add the Debits
            //        if (sumRow.creditDebit == LineCD.CREDIT)
            //            account.FindByid(sumRow.accountID).currentBalance -= sumRow.balance;
            //        else
            //            account.FindByid(sumRow.accountID).currentBalance += sumRow.balance;
            //    }
            //    else
            //    {
            //        // Else this is a Credit account (Loan, Credit) then add the Credits and subtract the Debits
            //        if (sumRow.creditDebit == LineCD.CREDIT)
            //            account.FindByid(sumRow.accountID).currentBalance += sumRow.balance;
            //        else
            //            account.FindByid(sumRow.accountID).currentBalance -= sumRow.balance;
            //    }
            //}

            //// Save back to the database and dispose of the temperary tables and adapters.
            //accTA.Update(account);
            //account.Dispose();
            //sums.Clear();
        }

        static public void myResetEnvelopeBalances()
        {
            //EnvelopeDataTable envelope;
            //List<EnvelopeSums> sums;
            //EnvelopeTableAdapter envTA = new EnvelopeTableAdapter();

            //// Bring up the Envelope table with the ending and current balances Zeroed out.
            //envelope = envTA.GetDataByZero();

            //// Get the endingBalance Sums
            //sums = FFDBDataSet.myGetEnvelopeSums();

            //// Reset the endingBalances
            //foreach (EnvelopeSums balRow in sums)
            //{
            //    // For Envelopes subtract the Credits and add the Debits
            //    if (balRow.creditDebit == LineCD.CREDIT)
            //        envelope.FindByid(balRow.envelopeID).endingBalance -= balRow.balance;
            //    else
            //        envelope.FindByid(balRow.envelopeID).endingBalance += balRow.balance;
            //}

            //// Get tomorrows Date and the currentBalance Sums
            //DateTime tomorrow = DateTime.Today;
            //tomorrow.AddDays(1.0);
            //sums.Clear();
            //sums = FFDBDataSet.myGetEnvelopeSumsBeforeDate(tomorrow);

            //// Reset the currentBalances
            //foreach (EnvelopeSums balRow in sums)
            //{
            //    // Subtract the Credits and add the Debits
            //    if (balRow.creditDebit == LineCD.CREDIT)
            //        envelope.FindByid(balRow.envelopeID).currentBalance -= balRow.balance;
            //    else
            //        envelope.FindByid(balRow.envelopeID).currentBalance += balRow.balance;
            //}

            //// Save back to the database and dispose of the temperary tabls and adapters.
            //envTA.Update(envelope);
            //envelope.Dispose();
            //sums.Clear();
        }

        static public void myResetAEBalance()
        {
            //AEBalanceDataTable aeBalance;
            //AEBalanceTableAdapter aeTA = new AEBalanceTableAdapter();
            //List<AEBalanceSums> balances;

            //// Delete the AEBalance Table
            //aeTA.DeleteQuery();
            //aeBalance = aeTA.GetData();

            //// Get the endingBalance Sums
            //balances = FFDBDataSet.myGetAEBalanceSums();

            //// Reset the endingBalances
            //foreach (AEBalanceSums balRow in balances)
            //{
            //    // For AEBalances subtract the Credits and add the Debits
            //    if (balRow.creditDebit == LineCD.CREDIT)
            //        aeBalance.myGetRow(balRow.accountID, balRow.envelopeID).endingBalance -= balRow.balance;
            //    else
            //        aeBalance.myGetRow(balRow.accountID, balRow.envelopeID).endingBalance += balRow.balance;
            //}

            //// Get tomorrows Date and the currentBalance Sums
            //DateTime tomorrow = DateTime.Today;
            //tomorrow.AddDays(1.0);
            //balances.Clear();
            //balances = FFDBDataSet.myGetAEBalanceSumsBeforeDate(tomorrow);

            //// Reset the currentBalances
            //foreach (AEBalanceSums balRow in balances)
            //{
            //    // Subtract the Credits and add the Debits
            //    if (balRow.creditDebit == LineCD.CREDIT)
            //        aeBalance.myGetRow(balRow.accountID, balRow.envelopeID).currentBalance -= balRow.balance;
            //    else
            //        aeBalance.myGetRow(balRow.accountID, balRow.envelopeID).currentBalance += balRow.balance;
            //}

            //// Save back to the database and dispose of the temperary tabls and adapters.
            //aeTA.Update(aeBalance);
            //aeBalance.Dispose();
            //balances.Clear();
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

    #region My Find All Transaction and Line Errors

    partial class FFDataBase
    {
        static public void myFindAllErrors()
        {
            SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            SqlCeCommand command = new SqlCeCommand();
            string query;
            int result;

            connection.Open();
            command.Connection = connection;

            // Remove all the errors
            query = "UPDATE LineItem SET transactionError = 0, lineError = 0";
            command.CommandText = query;
            result = command.ExecuteNonQuery();

            // Find all the transaction errors
            query = " UPDATE LineItem SET transactionError = 1 WHERE transactionID IN";
            query += "   (SELECT t1.transactionID FROM ";
            query += "      (SELECT SUM(amount) AS [Sum], transactionID FROM LineItem WHERE creditDebit = 0 GROUP BY transactionID) AS t1";
            query += "    INNER JOIN ";
            query += "      (SELECT SUM(amount) AS [Sum], transactionID FROM LineItem WHERE creditDebit = 1 GROUP BY transactionID) AS t2 ";
            query += "    ON t1.transactionID = t2.transactionID ";
            query += "    WHERE t1.Sum <> t2.sum)";
            command.CommandText = query;
            result = command.ExecuteNonQuery();

            // Find all the line errors
            query = " UPDATE LineItem SET lineError = 1 WHERE id IN ";
            query += "   (SELECT Line.id FROM ";
            query += "      (SELECT id, amount FROM LineItem) AS Line ";
            query += "    INNER JOIN ";
            query += "      (SELECT lineItemID, SUM(amount) AS [Sum] FROM SubLineItem GROUP BY lineItemID) AS SubLine ";
            query += "    ON Line.id = SubLine.lineItemID ";
            query += "    WHERE Line.amount <> SubLine.Sum) ";
            command.CommandText = query;
            result = command.ExecuteNonQuery();


            connection.Close();
        }
    }

    #endregion My Find All Errors


} // END namespace FamilyFinance



