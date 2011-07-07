using System;
using System.Collections.Generic;
using System.Data;
using System.IO;


namespace FamilyFinance.Data
{
    public class MyData
    {
        /// <summary>
        /// The data set used by the application.
        /// </summary>
        private FFDataSet ffDataSet;

        /// <summary>
        /// This is a singleton class, this is the static instance of this class.
        /// </summary>
        private static MyData Instance;

        /// <summary>
        /// Gets the singleton instance of the MyData class.
        /// </summary>
        /// <returns>Singelton instance of the MyData class</returns>
        public static MyData getInstance()
        {
            if (MyData.Instance == null)
                Instance = new MyData();

            return MyData.Instance;
        }

        /// <summary>
        /// Gets the next ID for the given table name.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public int getNextID(string tableName)
        {
            int id = 0;

            DataTable tableToGetIDFrom = this.ffDataSet.Tables[tableName];

            FamilyFinance.Buisness.InputValidator.CheckNotNull(tableToGetIDFrom, "DataTable");

            DataRowCollection rows = this.ffDataSet.Tables[tableName].Rows;

            foreach (DataRow row in rows)
            {
                int temp = Convert.ToInt32(row["id"]);

                if (temp > id)
                    id = temp;
            }

            return id + 1;
        }

        /// <summary>
        /// Local referance of the Account table.
        /// </summary>
        public FFDataSet.AccountDataTable Account;

        /// <summary>
        /// Local referance of the Account Type table.
        /// </summary>
        public FFDataSet.AccountTypeDataTable AccountType;

        /// <summary>
        /// Local referance of the Bank table.
        /// </summary>
        public FFDataSet.BankDataTable Bank;

        /// <summary>
        /// Local referance of the BankInfotable.
        /// </summary>
        public FFDataSet.BankInfoDataTable BankInfo;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.EnvelopeDataTable Envelope;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.EnvelopeGroupDataTable EnvelopeGroup;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.EnvelopeLineDataTable EnvelopeLine;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.LineItemDataTable LineItem;

        /// <summary>
        /// Local referance of the IsTransactionError table.
        /// </summary>
        public FFDataSet.TransactionDataTable Transaction;

        /// <summary>
        /// Local referance of the IsTransactionError Type table.
        /// </summary>
        public FFDataSet.TransactionTypeDataTable TransactionType;




        /// <summary>
        /// Prevents instantiation of this class. Instantiates the singleton instance of this
        /// class.
        /// </summary>
        private MyData()
        {
            // Initialize the dataset
            this.ffDataSet = new FFDataSet();

            // Initialize the tables
            this.Account = this.ffDataSet.Account;
            this.AccountType = this.ffDataSet.AccountType;
            this.Bank = this.ffDataSet.Bank;
            this.BankInfo = this.ffDataSet.BankInfo;
            this.EnvelopeGroup = this.ffDataSet.EnvelopeGroup;
            this.Envelope = this.ffDataSet.Envelope;
            this.EnvelopeLine = this.ffDataSet.EnvelopeLine;
            this.Transaction = this.ffDataSet.Transaction;
            this.TransactionType = this.ffDataSet.TransactionType;
            this.LineItem = this.ffDataSet.LineItem;

        }

        public void readData()
        {
            string xmlFile = FamilyFinance.Properties.Settings.Default.FFDataFileName;

            if (File.Exists(xmlFile))
            {
                // Read the data from the xml file
                try
                {
                    this.ffDataSet.ReadXml(xmlFile);
                }
                catch (System.Security.SecurityException e)
                {
                    string temp = e.ToString();
                    System.Windows.MessageBox.Show(temp, "Error", System.Windows.MessageBoxButton.OK);

                }

            }
            else
            {
                this.addRequiredTableRows();
                this.addGoodDefaultRows();
            }
        }

        public void saveData()
        {
            try
            {
                this.ffDataSet.WriteXml(FamilyFinance.Properties.Settings.Default.FFDataFileName);
            }
            catch (System.Security.SecurityException e)
            {
                string temp = e.ToString();
            }
        }

        private void addRequiredTableRows()
        {
            ////////////////////////////
            // Required Account type Rows
            FFDataSet.AccountTypeRow atNull = this.ffDataSet.AccountType.FindByid(AccountTypeCON.NULL.ID);

            if(atNull == null)
                atNull = this.ffDataSet.AccountType.AddAccountTypeRow(AccountTypeCON.NULL.ID, AccountTypeCON.NULL.Name);


            ////////////////////////////
            // Required Envelope Group Rows
            FFDataSet.EnvelopeGroupRow egNull = this.ffDataSet.EnvelopeGroup.FindByid(EnvelopeGroupCON.NULL.ID);

            if(egNull == null)
                egNull = this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(EnvelopeGroupCON.NULL.ID, AccountTypeCON.NULL.Name, 0.0m, 0.0m);


            ////////////////////////////
            // Required IsTransactionError Type Rows
            FFDataSet.TransactionTypeRow ttNull = this.ffDataSet.TransactionType.FindByid(TransactionTypeCON.NULL.ID);

            if(ttNull == null)
                this.ffDataSet.TransactionType.AddTransactionTypeRow(TransactionTypeCON.NULL.ID, TransactionTypeCON.NULL.Name);


            ////////////////////////////
            // Required Bank Rows
            FFDataSet.BankRow bNull = this.ffDataSet.Bank.FindByid(BankCON.NULL.ID);

            if(bNull == null)
                this.ffDataSet.Bank.AddBankRow(BankCON.NULL.ID, BankCON.NULL.Name, " ");


            ////////////////////////////
            // Required Account Rows
            FFDataSet.AccountRow aMul = this.ffDataSet.Account.FindByid(AccountCON.MULTIPLE.ID);
            FFDataSet.AccountRow aNull = this.ffDataSet.Account.FindByid(AccountCON.NULL.ID);

            if(aMul == null)
                this.ffDataSet.Account.AddAccountRow(AccountCON.MULTIPLE.ID, AccountCON.MULTIPLE.Name, atNull, CatagoryCON.NULL.ID, false, false);

            if(aNull == null)
                aNull = this.ffDataSet.Account.AddAccountRow(AccountCON.NULL.ID, AccountCON.NULL.Name, atNull, CatagoryCON.NULL.ID, false, false);


            ////////////////////////////
            // Required Envelope Rows
            FFDataSet.EnvelopeRow eSplit = this.ffDataSet.Envelope.FindByid(EnvelopeCON.SPLIT.ID);
            FFDataSet.EnvelopeRow eNull = this.ffDataSet.Envelope.FindByid(EnvelopeCON.NULL.ID);
            FFDataSet.EnvelopeRow eNoE = this.ffDataSet.Envelope.FindByid(EnvelopeCON.NO_ENVELOPE.ID);

            if(eSplit == null)
                this.ffDataSet.Envelope.AddEnvelopeRow(EnvelopeCON.SPLIT.ID, EnvelopeCON.SPLIT.Name, egNull, false, aNull, 0, " ", "N");

            if (eNull == null)
                this.ffDataSet.Envelope.AddEnvelopeRow(EnvelopeCON.NULL.ID, EnvelopeCON.NULL.Name, egNull, false, aNull, 0, " ", "N");

            if (eNoE == null)
                this.ffDataSet.Envelope.AddEnvelopeRow(EnvelopeCON.NO_ENVELOPE.ID, EnvelopeCON.NO_ENVELOPE.Name, egNull, false, aNull, 0, " ", "N");

        }

        private void addGoodDefaultRows()
        {
            // If there is only the NULL Acount Type add the other good defaults.
            if(this.ffDataSet.AccountType.Rows.Count <= 1)
            {
                this.ffDataSet.AccountType.AddAccountTypeRow(1, "Checking");
                this.ffDataSet.AccountType.AddAccountTypeRow(2, "Savings");
                this.ffDataSet.AccountType.AddAccountTypeRow(3, "Loan");
                this.ffDataSet.AccountType.AddAccountTypeRow(4, "Credit Card");
                this.ffDataSet.AccountType.AddAccountTypeRow(5, "Cash");
                this.ffDataSet.AccountType.AddAccountTypeRow(6, "Job");
                this.ffDataSet.AccountType.AddAccountTypeRow(7, "Other");
                this.ffDataSet.AccountType.AddAccountTypeRow(8, "Person");
                this.ffDataSet.AccountType.AddAccountTypeRow(9, "Store");
                this.ffDataSet.AccountType.AddAccountTypeRow(10, "Utility");
                this.ffDataSet.AccountType.AddAccountTypeRow(11, "Restraunt");
                this.ffDataSet.AccountType.AddAccountTypeRow(12, "Grocery Store");
                this.ffDataSet.AccountType.AddAccountTypeRow(13, "Online");
            }

            // If there is only the NULL Line Type add the other good defaults.
            if(this.ffDataSet.TransactionType.Rows.Count <= 1)
            {
                this.ffDataSet.TransactionType.AddTransactionTypeRow(1, "Deposit");
                this.ffDataSet.TransactionType.AddTransactionTypeRow(2, "Debit");
                this.ffDataSet.TransactionType.AddTransactionTypeRow(3, "Check");
                this.ffDataSet.TransactionType.AddTransactionTypeRow(4, "Transfer");
                this.ffDataSet.TransactionType.AddTransactionTypeRow(5, "Cash");
                this.ffDataSet.TransactionType.AddTransactionTypeRow(6, "Bill Pay");
                this.ffDataSet.TransactionType.AddTransactionTypeRow(7, "Withdrawl");
                this.ffDataSet.TransactionType.AddTransactionTypeRow(8, "Refund");

            }

            // If there is only the NULL Envelope Group add the other good defaults.
            if (this.ffDataSet.EnvelopeGroup.Rows.Count <= 1)
            {
                this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(1, "Charity", 10m, 15m);
                this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(2, "Saving", 5m, 10m);
                this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(3, "Housing", 25m, 35m);
                this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(4, "Utilities", 5m, 10m);
                this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(5, "Food", 5m, 15m);
                this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(6, "Transportation", 10m, 15m);
                this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(7, "Clothing", 2m, 7m);
                this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(8, "Medical/Health", 5m, 10m);
                this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(9, "Personal", 5m, 10m);
                this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(10, "Recreation", 5m, 10m);
                this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(11, "Debts", 5m, 10m);
            }
        }




    }
}
