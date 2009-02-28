//#define RUN_TESTS

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlServerCe;
using FamilyFinance2.Forms;

namespace FamilyFinance2
{
    static class Program
    {
        [STAThread] // The main entry point for the application.
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if (DEBUG)
    #if (RUN_TESTS)
                    testCode();
                    return;
    #else
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetDirectoryName(Application.ExecutablePath));
            FFDBDataSet.myCreateDBFile();
            runProgram();
            return;

    #endif

#else
                if (isSQLceInstaled())
                    if (findPath())
                        runProgram();

#endif

        }

        private static bool isSQLceInstaled()
        {
            // See if SQLCE is installed
            try { sqlCommand(); }

            catch
            {
                string message = "Please note that this program does not yet run on 64bit versions of Windows.\n Also, please make sure SQLServerCE3-5-1.msi has heen installed first.\n";
                string caption = "Error";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        private static void sqlCommand()
        {
            SqlCeCommand test = new SqlCeCommand();
            test.Dispose();
        }

        private static void runProgram()
        {
            //FamilyFinanceDBDataSet globalDataSet = new FamilyFinanceDBDataSet();
            //globalDataSet.myInit();

#if (DEBUG)

            //globalDataSet.Test_myResetDataBase();
            //globalDataSet.myCheckPassword(1, "123");
            //Application.Run(new MainForm(ref globalDataSet));

            //Application.Run(new LoginForm(ref globalDataSet));
            Application.Run(new MainForm());

#else

            Application.Run(new MainForm());

#endif

        }

        private static bool findPath()
        {
            bool result;
            string dbFilePath;
            string dbDir;

            dbDir = Properties.Settings.Default.DataDirectory;
            dbFilePath = dbDir + "\\" + Properties.Settings.Default.DBFileName;

            if (File.Exists(dbFilePath) == false)
            {
                FindDBForm findDB = new FindDBForm();
                findDB.ShowDialog();
                dbDir = findDB.FileDir;

                if (findDB.Result == FindDBForm.OpenResult.Cancel)
                    return false;

                else
                {
                    Properties.Settings.Default.DataDirectory = dbDir;
                    Properties.Settings.Default.Save();
                }
            }

            AppDomain.CurrentDomain.SetData("DataDirectory", Properties.Settings.Default.DataDirectory);

            try { result = FFDBDataSet.myGoodPath(); }

            catch
            {
                MessageBox.Show("Failed to find the database file.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (result == false)
            {
                try { result = FFDBDataSet.myCreateDBFile(); }

                catch { result = false; }

                if (result == false)
                {
                    MessageBox.Show("Failed to create the database file.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            return true;
        }


        // Run UnitTesting
#if (DEBUG)
        private static void testCode()
        {
            //FamilyFinanceDBDataSet testDS = new FamilyFinanceDBDataSet();
            //testDS.Test_myRunAllTests();
            //testDS.Dispose();
        }
#endif

    }// END Class Program
}// END namespace FamilyFinance