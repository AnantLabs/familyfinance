//#define RUN_TESTS

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlServerCe;

using FamilyFinance2.Forms.FindDB;
using FamilyFinance2.Forms.Main;
using FamilyFinance2.SharedElements;
using FamilyFinance2.Testing;

namespace FamilyFinance2
{
    static class Program
    {
        [STAThread] // The main entry point for the application.
        static void Main(string [] args)
        {
            if (!canRun())
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 1 && setPath(args[0]))
                runProgram();

            else if (args.Length == 1)
            {
                string dbDir = args[0];

                if (createDataBase(dbDir))
                {
                    Properties.Settings.Default.DataDirectory = dbDir;
                    runProgram();
                }
                else
                    MessageBox.Show("Failed to find the database file.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else if (args.Length == 0 && setPath(Properties.Settings.Default.DataDirectory))
                runProgram();

            else if (args.Length == 0)
            {
                string dbDir = findUserPath();

                if (dbDir != null && (setPath(dbDir) || createDataBase(dbDir)))
                {
                    Properties.Settings.Default.DataDirectory = dbDir;
                    Properties.Settings.Default.Save();
                    runProgram();
                }
                else
                    MessageBox.Show("Failed to find the database file.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private static void runProgram()
        {
            //Application.Run(new FamilyFinance2.Forms.EditAccounts.EditAccountsForm());
            //Application.Run(new FamilyFinance2.Forms.EditEnvelopes.EditEnvelopesForm());

            //Testing.Testing test = new FamilyFinance2.Testing.Testing();
            //test.stressFillDataBase();

            //Application.Run(new FamilyFinance2.Forms.Transaction.TransactionForm(3));
            Application.Run(new MainForm());
        }

        private static bool createDataBase(string path)
        {
            bool result = false;
                            
            try
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", path);
                result = DBquery.createDBFile();
            } 
            catch  
            { 
                MessageBox.Show("Failed to create the database file.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                result = false; 
            }

            return result;
        }

        private static bool canRun()
        {
            string message;
            string caption;

            // see if framework 3.5 is installed
            if (!Directory.Exists("C:\\Windows\\Microsoft.NET\\Framework\\v3.5"))
            {
                message = "It appears this computer does not yet have the .NET Framework v3.5 installed. \n Please get it installed before running Family Finance.";
                caption = "Error";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            // See if SQLCE is installed
            try { sqlCommandTest(); }
            catch
            {
                message = "It appears this computer does not yet have SQLServerCE3-5-1.msi installed.\n Please get it installed before running Family Finance.";
                caption = "Error";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private static void sqlCommandTest()
        {
            SqlCeCommand test = new SqlCeCommand();
            test.Dispose();
        }

        private static bool setPath(string dbDir)
        {
            bool result = false;
            string dbFilePath = dbDir + "\\" + Properties.Settings.Default.DBFileName;

            if (File.Exists(dbFilePath))
            {
                try
                {
                    AppDomain.CurrentDomain.SetData("DataDirectory", dbDir);
                    result = DBquery.goodPath(); 
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        private static string findUserPath()
        {
            string dbDir;
            FindDBForm findDB = new FindDBForm();
            findDB.ShowDialog();

            if (findDB.Result == FindDBForm.OpenResult.Cancel)
                dbDir = null;
            else
                dbDir = findDB.FileDir;

            return dbDir;
        }



    }// END Class Program
}// END namespace FamilyFinance