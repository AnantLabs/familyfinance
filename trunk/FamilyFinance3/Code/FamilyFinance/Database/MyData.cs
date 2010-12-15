using System;
using System.Collections.Generic;
using System.Linq;

using FamilyFinance.Database.FFDataSetTableAdapters;
using System.Data;

namespace FamilyFinance.Database
{
    /// <summary>
    /// A singelten instance of the local copy of the data tables from the database.
    /// </summary>
    class MyData
    {
        // The dataset
        private FFDataSet ffDataSet;

        // The table adapters
        private EnvelopeGroupTableAdapter envelopeGroupTA = new EnvelopeGroupTableAdapter();
        private EnvelopeTableAdapter envelopeTA = new EnvelopeTableAdapter();
        private EnvelopeLineTableAdapter envelopeLineTA = new EnvelopeLineTableAdapter();
        private LineTypeTableAdapter lineTypeTA = new LineTypeTableAdapter();
        private TransactionTableAdapter transactionTA = new TransactionTableAdapter();
        private LineItemTableAdapter lineItemTA = new LineItemTableAdapter();
        private FitLineTableAdapter fitLineTA = new FitLineTableAdapter();
        private AccountTableAdapter accountTA = new AccountTableAdapter();
        private SettingsTableAdapter settingsTA = new SettingsTableAdapter();
        private AccountTypeTableAdapter accountTypeTA = new AccountTypeTableAdapter();
        private BillsTableAdapter billsTA = new BillsTableAdapter();
        private BankTableAdapter bankTA = new BankTableAdapter();
        private BankInfoTableAdapter bankInfoTA = new BankInfoTableAdapter();
        private OFXFilesTableAdapter ofxFilesTA = new OFXFilesTableAdapter();


        /// <summary>
        /// Static instance of this class.
        /// </summary>
        private static MyData Instance;

        /// <summary>
        /// Gets the singleton instance of the MyData class.
        /// </summary>
        /// <returns>Singelton instance of the MyData class</returns>
        public static MyData getInstance()
        {
            if(MyData.Instance == null)
                Instance = new MyData();

            return MyData.Instance;
        }

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.EnvelopeGroupDataTable EnvelopeGroup;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.EnvelopeDataTable Envelope;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.EnvelopeLineDataTable EnvelopeLine;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.LineTypeDataTable LineType;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.TransactionDataTable Transaction;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.LineItemDataTable LineItem;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.FitLineDataTable FitLine;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.AccountDataTable Account;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.AccountTypeDataTable AccountType;
        
        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.BillsDataTable Bills;
            
        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.SettingsDataTable Settings;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.BankDataTable Bank;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.BankInfoDataTable BankInfo;

        /// <summary>
        /// Local referance of the table.
        /// </summary>
        public FFDataSet.OFXFilesDataTable OFXFiles;


        /// <summary>
        /// Commits the changes in the given row to the database.
        /// </summary>
        /// <param name="ofxRow">Any tables row.</param>
        public void saveRow(DataRow row)
        {

            if (row.Table == this.Account)
                accountTA.Update(row);

            else if (row.Table == this.AccountType)
                accountTypeTA.Update(row);

            else if (row.Table == this.Bank)
                bankTA.Update(row);

            else if (row.Table == this.BankInfo)
                bankInfoTA.Update(row);

            else if (row.Table == this.Bills)
                billsTA.Update(row);

            else if (row.Table == this.Envelope)
                envelopeTA.Update(row);

            else if (row.Table == this.EnvelopeGroup)
                envelopeGroupTA.Update(row);

            else if (row.Table == this.EnvelopeLine)
                envelopeLineTA.Update(row);

            else if (row.Table == this.FitLine)
                fitLineTA.Update(row);

            else if (row.Table == this.LineItem)
                lineItemTA.Update(row);

            else if (row.Table == this.LineType)
                lineTypeTA.Update(row);

            else if (row.Table == this.OFXFiles)
                ofxFilesTA.Update(row);

            else if (row.Table == this.Settings)
                settingsTA.Update(row);

            else if (row.Table == this.Transaction)
                transactionTA.Update(row);

            else
                throw new Exception("Unknown Table");
        }






        /// <summary>
        /// Prevents instantiation of this class. Instantiates the singleton instance of this
        /// class.
        /// </summary>
        private MyData()
        {
            // Initialize the dataset
            this.ffDataSet = new FFDataSet();

            // Initialize the tables
            this.EnvelopeGroup = this.ffDataSet.EnvelopeGroup;
            this.Envelope = this.ffDataSet.Envelope;
            this.EnvelopeLine = this.ffDataSet.EnvelopeLine;
            this.LineType = this.ffDataSet.LineType;
            this.Transaction = this.ffDataSet.Transaction;
            this.LineItem = this.ffDataSet.LineItem;
            this.FitLine = this.ffDataSet.FitLine;
            this.Account = this.ffDataSet.Account;
            this.AccountType = this.ffDataSet.AccountType;
            this.Bills = this.ffDataSet.Bills;
            this.Settings = this.ffDataSet.Settings;
            this.Bank = this.ffDataSet.Bank;
            this.BankInfo = this.ffDataSet.BankInfo;
            this.OFXFiles = this.ffDataSet.OFXFiles;

                      


            // Set the clear before fill
            this.envelopeGroupTA.ClearBeforeFill = true;
            this.envelopeTA.ClearBeforeFill = true;
            this.envelopeLineTA.ClearBeforeFill = true;
            this.lineTypeTA.ClearBeforeFill = true;
            this.transactionTA.ClearBeforeFill = true;
            this.lineItemTA.ClearBeforeFill = true;
            this.fitLineTA.ClearBeforeFill = true;
            this.accountTypeTA.ClearBeforeFill = true;
            this.accountTA.ClearBeforeFill = true;
            this.billsTA.ClearBeforeFill = true;
            this.settingsTA.ClearBeforeFill = true;
            this.bankTA.ClearBeforeFill = true;
            this.bankInfoTA.ClearBeforeFill = true;
            this.ofxFilesTA.ClearBeforeFill = true;

            // Fill all the tables
            this.envelopeGroupTA.Fill(EnvelopeGroup);
            this.envelopeTA.Fill(Envelope);
            this.lineTypeTA.Fill(LineType);
            this.transactionTA.Fill(Transaction);
            this.fitLineTA.Fill(FitLine);
            this.accountTypeTA.Fill(AccountType);
            this.accountTA.Fill(Account);
            this.lineItemTA.Fill(LineItem);
            this.envelopeLineTA.Fill(EnvelopeLine);
            this.billsTA.Fill(Bills);
            this.settingsTA.Fill(Settings);
            this.bankTA.Fill(Bank);
            this.bankInfoTA.Fill(BankInfo);
            this.ofxFilesTA.Fill(OFXFiles);

            // Subscribe to the new lines
            this.Account.TableNewRow += new System.Data.DataTableNewRowEventHandler(Account_TableNewRow);
            this.AccountType.TableNewRow += new System.Data.DataTableNewRowEventHandler(AccountType_TableNewRow);
            this.Bank.TableNewRow += new System.Data.DataTableNewRowEventHandler(Bank_TableNewRow);
            this.BankInfo.TableNewRow += new System.Data.DataTableNewRowEventHandler(BankInfo_TableNewRow);
            this.Bills.TableNewRow += new System.Data.DataTableNewRowEventHandler(Bills_TableNewRow);
            this.Envelope.TableNewRow += new System.Data.DataTableNewRowEventHandler(Envelope_TableNewRow);
            this.EnvelopeGroup.TableNewRow += new System.Data.DataTableNewRowEventHandler(EnvelopeGroup_TableNewRow);
            this.EnvelopeLine.TableNewRow += new System.Data.DataTableNewRowEventHandler(EnvelopeLine_TableNewRow);
            this.FitLine.TableNewRow += new System.Data.DataTableNewRowEventHandler(FitLine_TableNewRow);
            this.LineItem.TableNewRow += new System.Data.DataTableNewRowEventHandler(LineItem_TableNewRow);
            this.LineType.TableNewRow += new System.Data.DataTableNewRowEventHandler(LineType_TableNewRow);
            this.OFXFiles.TableNewRow += new System.Data.DataTableNewRowEventHandler(OFXFiles_TableNewRow);
            // this.Settings
            this.Transaction.TableNewRow += new System.Data.DataTableNewRowEventHandler(Transaction_TableNewRow);


            // Subscribe to column Changing
            this.Account.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            this.AccountType.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            this.Bank.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            this.BankInfo.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            this.Envelope.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            this.EnvelopeGroup.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            this.FitLine.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            this.LineType.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);

            // Not Bills this.Bills.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            // Not Settings this.Settings.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            // Not EnvelopeLine this.EnvelopeLine.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            // Not LineItem this.LineItem.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            // Not OFXFiles this.OFXFiles.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            // Not Transaction this.Transaction.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);
            // not Goal

        }

        /// <summary>
        /// Shared function subscribing to select tables for trunnckating an text colmn to the max
        /// allowed for that column.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColumnChanging(object sender, System.Data.DataColumnChangeEventArgs e)
        {
            if (e.Column.DataType.Name.Equals("String") && (e.ProposedValue as string).Length > e.Column.MaxLength)
                e.ProposedValue = (e.ProposedValue as string).Substring(0, e.Column.MaxLength);
        }

        private void Account_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            FFDataSet.AccountRow row = e.Row as FFDataSet.AccountRow;

            int max =
               (from Account in this.Account
                select Account.id).Max();

            row.id = max + 1;
            row.name = "";
            row.typeID = SpclAccountType.NULL;
            row.catagory = SpclAccountCat.EXPENSE;
            row.closed = false;
            row.envelopes = false;
        }

        private void AccountType_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            FFDataSet.AccountTypeRow row = e.Row as FFDataSet.AccountTypeRow;

            int max =
               (from AccountType in this.AccountType
                select AccountType.id).Max();

            row.id = max + 1;
            row.name = "";
        }

        private void Bank_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            FFDataSet.BankRow row = e.Row as FFDataSet.BankRow;

            int max =
               (from Bank in this.Bank
                select Bank.id).Max();

            row.id = max + 1;
            row.name = "";
            row.routingNumber = "";
        }

        private void BankInfo_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            FFDataSet.BankInfoRow row = e.Row as FFDataSet.BankInfoRow;

            // account id needs to be set by the funcion getting the new line.  row.accountID = SpclAccount.NULL;
            row.bankID = SpclBank.NULL;
            row.accountNumber = "";
            row.creditDebit = LineCD.DEBIT;
        }

        private void Bills_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
        }

        private void Envelope_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            FFDataSet.EnvelopeRow row = e.Row as FFDataSet.EnvelopeRow;

            int max =
               (from Envelope in this.Envelope
                select Envelope.id).Max();

            int maxP =
               (from Envelope in this.Envelope
                select Envelope.priorityOrder).Max();


            row.id = max + 1;
            row.name = "";
            row.groupID = SpclEnvelope.NULL;
            row.closed = false;
            row.accountID = SpclAccount.NULL;

            row.priorityOrder = maxP + 1;
            row.notes = "";
            row.step = 0.0m;
            row.cap = 0.0m;
            row.nextDate = DateTime.Today;
            row.intervalDate = DateTime.MinValue;
        }

        private void EnvelopeGroup_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            FFDataSet.EnvelopeGroupRow row = e.Row as FFDataSet.EnvelopeGroupRow;

            int max =
               (from EnvelopeGroup in this.EnvelopeGroup
                select EnvelopeGroup.id).Max();

            row.id = max + 1;
            row.name = "";
        }

        private void EnvelopeLine_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
        }

        private void FitLine_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
        }

        private void LineItem_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            FFDataSet.LineItemRow row = e.Row as FFDataSet.LineItemRow;

            int max =
               (from LineItem in this.LineItem
                select LineItem.id).Max();

            row.id = max + 1;
            // TransactionID needs to be set by calling function.
            row.accountID = SpclAccount.NULL;
            row.amount = 0.0m;
            row.creditDebit = LineCD.CREDIT;
        }

        private void LineType_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            FFDataSet.LineTypeRow row = e.Row as FFDataSet.LineTypeRow;

            int max =
               (from LineType in this.LineType
                select LineType.id).Max();

            row.id = max + 1;
            row.name = "";
        }

        private void OFXFiles_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
        }

        private void Transaction_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            FFDataSet.TransactionRow row = e.Row as FFDataSet.TransactionRow;

            int max =
               (from Transaction in this.Transaction
                select Transaction.id).Max();

            row.id = max + 1;
            row.date = DateTime.Today;
            row.typeID = SpclAccountType.NULL;
            row.description = "";
            row.confirmationNumber = "";
            row.complete = LineComplete.PENDING;
        }


    }
}
