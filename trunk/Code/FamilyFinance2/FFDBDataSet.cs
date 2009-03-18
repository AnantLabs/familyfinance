using System;
using System.Data.SqlServerCe;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using FamilyFinance2.FFDBDataSetTableAdapters;

namespace FamilyFinance2 
{
    public partial class FFDBDataSet 
    {

        ///////////////////////////////////////////////////////////////////////
        //   Functions STATIC 
        ///////////////////////////////////////////////////////////////////////
        static private void myExecuteFile(string fileAsString, bool catchExceptions)
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

        static private object myNewID(string table, string column)
        {
            SqlCeConnection connection;
            SqlCeCommand command;
            object result;
            string select;

            connection = new SqlCeConnection(Properties.Settings.Default.FFDBConnectionString);
            connection.Open();

            select = "SELECT MAX( " + column + " ) FROM " + table;
            command = new SqlCeCommand(select, connection);

            try
            {
                result = command.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw new Exception("Caught a bad SQL Line <" + select + ">", e);
            }
            finally
            {
                connection.Close();
            }

            if (result == DBNull.Value)
                result = 1;

            return result;
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
            AccountDataTable account = new AccountDataTable();
            AccountTableAdapter accTA = new AccountTableAdapter();
            AccountSumsViewDataTable balances;
            AccountSumsViewTableAdapter balTA = new AccountSumsViewTableAdapter();

            // Bring up the Account table with the ending and current balances Zeroed out.
            accTA.FillByZero(account);

            // Get the endingBalance Sums
            balances = balTA.GetData();

            // Reset the endingBalances
            foreach (AccountSumsViewRow balRow in balances)
            {
                if (account.FindByid(balRow.accountID).creditDebit == LineCD.DEBIT)
                {
                    // If this in a Debit account (Checking, Savings) then subtract the Credits and add the debits
                    if (balRow.creditDebit == LineCD.CREDIT)
                        account.FindByid(balRow.accountID).endingBalance -= balRow.sum;
                    else
                        account.FindByid(balRow.accountID).endingBalance += balRow.sum;
                }
                else
                {
                    // Else this is a Credit account (Loan, Credit) then add the Credits and subtract the debits
                    if (balRow.creditDebit == LineCD.CREDIT)
                        account.FindByid(balRow.accountID).endingBalance += balRow.sum;
                    else
                        account.FindByid(balRow.accountID).endingBalance -= balRow.sum;
                }
            }

            // Get tomorrows Date and the currentBalance Sums
            DateTime tomorrow = DateTime.Today;
            tomorrow.AddDays(1.0);
            balances = balTA.GetDataByDate(tomorrow);

            // Reset the currentBalances
            foreach (AccountSumsViewRow balRow in balances)
            {
                if (account.FindByid(balRow.accountID).creditDebit == LineCD.DEBIT)
                {
                    // If this in a Debit account (Checking, Savings) then subtract the Credits and add the Debits
                    if (balRow.creditDebit == LineCD.CREDIT)
                        account.FindByid(balRow.accountID).currentBalance -= balRow.sum;
                    else
                        account.FindByid(balRow.accountID).currentBalance += balRow.sum;
                }
                else
                {
                    // Else this is a Credit account (Loan, Credit) then add the Credits and subtract the Debits
                    if (balRow.creditDebit == LineCD.CREDIT)
                        account.FindByid(balRow.accountID).currentBalance += balRow.sum;
                    else
                        account.FindByid(balRow.accountID).currentBalance -= balRow.sum;
                }
            }

            // Save back to the database and dispose of the temperary tables and adapters.
            accTA.Update(account);
            account.Dispose();
            balances.Dispose();
            balTA.Dispose();
        }

        static public void myResetEnvelopeBalances()
        {
            EnvelopeDataTable envelope;
            EnvelopeSumsViewDataTable balances;
            EnvelopeTableAdapter envTA = new EnvelopeTableAdapter();
            EnvelopeSumsViewTableAdapter balTA = new EnvelopeSumsViewTableAdapter();

            // Bring up the Envelope table with the ending and current balances Zeroed out.
            envelope = envTA.GetDataByZero();

            // Get the endingBalance Sums
            balances = balTA.GetData();

            // Reset the endingBalances
            foreach (EnvelopeSumsViewRow balRow in balances)
            {
                // For Envelopes subtract the Credits and add the Debits
                if (balRow.creditDebit == LineCD.CREDIT)
                    envelope.FindByid(balRow.envelopeID).endingBalance -= balRow.sum;
                else
                    envelope.FindByid(balRow.envelopeID).endingBalance += balRow.sum;
            }

            // Get tomorrows Date and the currentBalance Sums
            DateTime tomorrow = DateTime.Today;
            tomorrow.AddDays(1.0);
            balances = balTA.GetDataByDate(tomorrow);

            // Reset the currentBalances
            foreach (EnvelopeSumsViewRow balRow in balances)
            {
                // Subtract the Credits and add the Debits
                if (balRow.creditDebit == LineCD.CREDIT)
                    envelope.FindByid(balRow.envelopeID).currentBalance -= balRow.sum;
                else
                    envelope.FindByid(balRow.envelopeID).currentBalance += balRow.sum;
            }

            // Save back to the database and dispose of the temperary tabls and adapters.
            envTA.Update(envelope);
            envelope.Dispose();
            balances.Dispose();
            balTA.Dispose();
        }

        static public void myResetAEBalance()
        {
            AEBalanceDataTable aeBalance;
            AEBalanceTableAdapter aeTA = new AEBalanceTableAdapter();
            AEBalanceSumsViewDataTable balances;
            AEBalanceSumsViewTableAdapter balTA = new AEBalanceSumsViewTableAdapter();

            // Delete the AEBalance Table
            aeTA.DeleteQuery();
            aeBalance = aeTA.GetData();

            // Get the endingBalance Sums
            balances = balTA.GetData();

            // Reset the endingBalances
            foreach (AEBalanceSumsViewRow balRow in balances)
            {
                // For AEBalances subtract the Credits and add the Debits
                if (balRow.creditDebit == LineCD.CREDIT)
                    aeBalance.myGetRow(balRow.accountID, balRow.envelopeID).endingBalance -= balRow.sum;
                else
                    aeBalance.myGetRow(balRow.accountID, balRow.envelopeID).endingBalance += balRow.sum;
            }

            // Get tomorrows Date and the currentBalance Sums
            DateTime tomorrow = DateTime.Today;
            tomorrow.AddDays(1.0);
            balances = balTA.GetDataByDate(tomorrow);

            // Reset the currentBalances
            foreach (AEBalanceSumsViewRow balRow in balances)
            {
                // Subtract the Credits and add the Debits
                if (balRow.creditDebit == LineCD.CREDIT)
                    aeBalance.myGetRow(balRow.accountID, balRow.envelopeID).currentBalance -= balRow.sum;
                else
                    aeBalance.myGetRow(balRow.accountID, balRow.envelopeID).currentBalance += balRow.sum;
            }

            // Save back to the database and dispose of the temperary tabls and adapters.
            aeTA.Update(aeBalance);
            aeBalance.Dispose();
            balances.Dispose();
            balTA.Dispose();
        }



        ///////////////////////////////////////////////////////////////////////
        //   Functions Public 
        ///////////////////////////////////////////////////////////////////////


    }
}
