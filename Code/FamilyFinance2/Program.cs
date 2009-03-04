//#define RUN_TESTS

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlServerCe;
using System.Linq;
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
            Application.Run(new MainForm());
            return;

    #endif

#else

            if (canRun())
                if (findPath())
                    runProgram();

#endif

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
            try { sqlCommand(); }
            catch
            {
                message = "It appears this computer does not yet have SQLServerCE3-5-1.msi installed.\n Please get it installed before running Family Finance.";
                caption = "Error";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            // See if 64 bit machine
            if (IntPtr.Size == 8)
            {
                message = "Family Finance is not yet supported in 64-bit versions of Windows.\n";
                caption = "Error";
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