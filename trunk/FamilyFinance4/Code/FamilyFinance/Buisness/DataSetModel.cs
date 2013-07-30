using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class DataSetModel
    {
        private FFDataSet ffDataSet;
        private string xmlFileName = FamilyFinance.Properties.Settings.Default.FFDataFileName;

        ///////////////////////////////////////////////////////////////////////
        // Singleton stuff
        ///////////////////////////////////////////////////////////////////////
        private static DataSetModel _Instance;
        public static DataSetModel Instance
        {
            get
            {
                if (DataSetModel._Instance == null)
                    _Instance = new DataSetModel();

                return DataSetModel._Instance;
            }
        }

        private DataSetModel()
        {
            ffDataSet = new FFDataSet();
        }

        public void loadData()
        {
            string errorMessage = "The following data file does not exist. \n" + xmlFileName + "\nMaking a new file.";

            if (File.Exists(xmlFileName))
            {
                readDataFromTheFileIntoTheDataSet();
            }
            else
            {
                showErrorMessage(errorMessage);
                populateNewStartingDataSet();
            }
        }

        private void readDataFromTheFileIntoTheDataSet()
        {
            try
            {
                ffDataSet.ReadXml(xmlFileName);
            }
            catch (System.Security.SecurityException e)
            {
                string message = "There was an error reading the data file. \n" + xmlFileName + '\n' + e.ToString();

                showErrorMessage(message);
                populateNewStartingDataSet();
            }
        }

        private void populateNewStartingDataSet()
        {
            ffDataSet.Clear();
            addRequiredTableRows();
            addGoodDefaultRows();
        }

        private void showErrorMessage(string message)
        {
            System.Windows.MessageBox.Show(message, "Error", System.Windows.MessageBoxButton.OK);
        }

        private void addRequiredTableRows()
        {
            ////////////////////////////
            // Required Account type Rows
            FFDataSet.AccountTypeRow accountTypeNull = this.ffDataSet.AccountType.FindByid(AccountTypeCON.NULL.ID);

            if (accountTypeNull == null)
                accountTypeNull = this.ffDataSet.AccountType.AddAccountTypeRow(AccountTypeCON.NULL.ID, AccountTypeCON.NULL.Name);


            ////////////////////////////
            // Required Envelope Group Rows
            FFDataSet.EnvelopeGroupRow envelopeGroupNull = this.ffDataSet.EnvelopeGroup.FindByid(EnvelopeGroupCON.NULL.ID);

            if (envelopeGroupNull == null)
                envelopeGroupNull = this.ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(EnvelopeGroupCON.NULL.ID, AccountTypeCON.NULL.Name, 0.0m, 0.0m);


            ////////////////////////////
            // Required Transaction Type Rows
            FFDataSet.TransactionTypeRow transactionTypeNull = this.ffDataSet.TransactionType.FindByid(TransactionTypeCON.NULL.ID);

            if (transactionTypeNull == null)
                transactionTypeNull = this.ffDataSet.TransactionType.AddTransactionTypeRow(TransactionTypeCON.NULL.ID, TransactionTypeCON.NULL.Name);


            ////////////////////////////
            // Required Bank Rows
            FFDataSet.BankRow bankNull = this.ffDataSet.Bank.FindByid(BankCON.NULL.ID);

            if (bankNull == null)
                bankNull = this.ffDataSet.Bank.AddBankRow(BankCON.NULL.ID, BankCON.NULL.Name, " ");


            ////////////////////////////
            // Required Account Rows
            FFDataSet.AccountRow accountNull = this.ffDataSet.Account.FindByid(AccountCON.NULL.ID);

            if (accountNull == null)
                accountNull = this.ffDataSet.Account.AddAccountRow(AccountCON.NULL.ID, AccountCON.NULL.Name, accountTypeNull, CatagoryCON.NULL.ID, false, false);


            ////////////////////////////
            // Required Envelope Rows
            FFDataSet.EnvelopeRow envelopeNull = this.ffDataSet.Envelope.FindByid(EnvelopeCON.NULL.ID);
            FFDataSet.EnvelopeRow envelopeNoEnvelope = this.ffDataSet.Envelope.FindByid(EnvelopeCON.NO_ENVELOPE.ID);

            if (envelopeNull == null)
                envelopeNull = this.ffDataSet.Envelope.AddEnvelopeRow(EnvelopeCON.NULL.ID, EnvelopeCON.NULL.Name, envelopeGroupNull, false, accountNull, 0, " ", "N");

            if (envelopeNoEnvelope == null)
                envelopeNoEnvelope = this.ffDataSet.Envelope.AddEnvelopeRow(EnvelopeCON.NO_ENVELOPE.ID, EnvelopeCON.NO_ENVELOPE.Name, envelopeGroupNull, false, accountNull, 0, " ", "N");
            

            ////////////////////////////
            // Required Transaction Row
            FFDataSet.TransactionRow transactionNull = this.ffDataSet.Transaction.FindByid(TransactionCON.NULL.ID);

            if (transactionNull == null)
                transactionNull = this.ffDataSet.Transaction.AddTransactionRow(TransactionCON.NULL.ID, DateTime.MinValue, transactionTypeNull, "");


            ////////////////////////////
            // Required LineItem Row
            FFDataSet.LineItemRow lineItemNull = this.ffDataSet.LineItem.FindByid(LineItemCON.NULL.ID);

            if (lineItemNull == null)
                lineItemNull = this.ffDataSet.LineItem.AddLineItemRow(LineItemCON.NULL.ID, transactionNull, accountNull, "", 0, TransactionStateCON.PENDING.Value, false);



        }

        private void addGoodDefaultRows()
        {
            // If there is only the NULL Acount Type add the other good defaults.
            if (this.ffDataSet.AccountType.Rows.Count <= 1)
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
            if (this.ffDataSet.TransactionType.Rows.Count <= 1)
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

        public void saveData()
        {
            try
            {
                ffDataSet.WriteXml(xmlFileName);
            }
            catch (System.Security.SecurityException e)
            {
                string message = "There was an error saving to the data file. \n" + xmlFileName + '\n' + e.ToString();

                showErrorMessage(message);
            }
        }


        ///////////////////////////////////////////////////////////////////////
        // New Row stuff
        ///////////////////////////////////////////////////////////////////////
        private int getNextIDFromTableNamed(string tableName)
        {
            int id = 0;

            DataTable tableToGetIDFrom = this.ffDataSet.Tables[tableName];


            DataRowCollection rows = this.ffDataSet.Tables[tableName].Rows;

            foreach (DataRow row in rows)
            {
                int temp = Convert.ToInt32(row["id"]);

                if (temp > id)
                    id = temp;
            }

            return id + 1;
        }

        public FFDataSet.AccountRow NewAccountRow()
        {
            FFDataSet.AccountRow newRow = ffDataSet.Account.NewAccountRow();

            newRow.id = this.getNextIDFromTableNamed("Account");
            newRow.name = "";
            newRow.typeID = AccountTypeCON.NULL.ID;
            newRow.catagory = CatagoryCON.NULL.ID;
            newRow.closed = false;
            newRow.envelopes = false;

            ffDataSet.Account.AddAccountRow(newRow);

            return newRow;
        }

        public FFDataSet.AccountTypeRow NewAccountTypeRow()
        {
            FFDataSet.AccountTypeRow newRow = ffDataSet.AccountType.NewAccountTypeRow();

            newRow.id = this.getNextIDFromTableNamed("AccountType");
            newRow.name = "";

            ffDataSet.AccountType.AddAccountTypeRow(newRow);

            return newRow;
        }

        public FFDataSet.BankRow NewBankRow()
        {
            FFDataSet.BankRow newRow = ffDataSet.Bank.NewBankRow();

            newRow.id = getNextIDFromTableNamed("Bank");
            newRow.name = "";
            newRow.routingNumber = "";

            ffDataSet.Bank.AddBankRow(newRow);

            return newRow;
        }

        public FFDataSet.BankInfoRow NewBankInfoRow(FFDataSet.AccountRow account)
        {
            FFDataSet.BankInfoRow newRow = ffDataSet.BankInfo.NewBankInfoRow();

            newRow.accountID = account.id;
            newRow.bankID = BankCON.NULL.ID;
            newRow.accountNumber = "";
            newRow.polarity = PolarityCON.DEBIT.Value;

            ffDataSet.BankInfo.AddBankInfoRow(newRow);

            return newRow;
        }

        public FFDataSet.EnvelopeRow NewEnvelopeRow()
        {
            FFDataSet.EnvelopeRow newRow = ffDataSet.Envelope.NewEnvelopeRow();

            newRow.id = this.getNextIDFromTableNamed("Envelope");
            newRow.name = "";
            newRow.groupID = EnvelopeGroupCON.NULL.ID;
            newRow.favoriteAccountID = AccountCON.NULL.ID;
            newRow.closed = false;
            newRow.priority = newRow.id;
            newRow.notes = "";
            newRow.goal = "";

            ffDataSet.Envelope.AddEnvelopeRow(newRow);

            return newRow;
        }

        public FFDataSet.EnvelopeGroupRow NewEnvelopeGroupRow()
        {
            FFDataSet.EnvelopeGroupRow newRow = ffDataSet.EnvelopeGroup.NewEnvelopeGroupRow();

            newRow.id = this.getNextIDFromTableNamed("EnvelopeGroup");
            newRow.name = "";
            newRow.minPercent = 0m;
            newRow.maxPercent = 0m;

            ffDataSet.EnvelopeGroup.AddEnvelopeGroupRow(newRow);

            return newRow;
        }

        public FFDataSet.EnvelopeLineRow NewEnvelopeLineRow(LineItemDRM lineItem)
        {
            FFDataSet.EnvelopeLineRow newRow = this.NewEnvelopeLineRow();
            
            newRow.lineItemID = lineItem.LineID;

            return newRow;
        }

        public FFDataSet.EnvelopeLineRow NewEnvelopeLineRow()
        {
            FFDataSet.EnvelopeLineRow newRow = ffDataSet.EnvelopeLine.NewEnvelopeLineRow();

            newRow.id = this.getNextIDFromTableNamed("EnvelopeLine");
            newRow.lineItemID = LineItemCON.NULL.ID;
            newRow.envelopeID = EnvelopeCON.NULL.ID;
            newRow.description = "";
            newRow.amount = 0;

            ffDataSet.EnvelopeLine.AddEnvelopeLineRow(newRow);

            return newRow;
        }

        public FFDataSet.LineItemRow NewLineItemRow(TransactionDRM transaction)
        {
            FFDataSet.LineItemRow newRow = this.NewLineItemRow();

            newRow.transactionID = transaction.TransactionID;

            return newRow;
        }

        public FFDataSet.LineItemRow NewLineItemRow()
        {
            FFDataSet.LineItemRow newRow = ffDataSet.LineItem.NewLineItemRow();

            newRow.id = this.getNextIDFromTableNamed("LineItem");
            newRow.transactionID = TransactionCON.NULL.ID;
            newRow.accountID = AccountCON.NULL.ID;
            newRow.confirmationNumber = "";
            newRow.amount = 0m;
            newRow.polarity = PolarityCON.CREDIT.Value;
            newRow.state = TransactionStateCON.PENDING.Value;

            ffDataSet.LineItem.AddLineItemRow(newRow);

            return newRow;
        }

        public FFDataSet.TransactionRow NewTransactionRow()
        {
            FFDataSet.TransactionRow newRow = ffDataSet.Transaction.NewTransactionRow();

            newRow.id = this.getNextIDFromTableNamed("Transaction");
            newRow.date = DateTime.Today;
            newRow.typeID = TransactionTypeCON.NULL.ID;
            newRow.description = "";

            ffDataSet.Transaction.AddTransactionRow(newRow);

            return newRow;
        }

        public FFDataSet.TransactionTypeRow NewTransactionTypeRow()
        {
            FFDataSet.TransactionTypeRow newRow = ffDataSet.TransactionType.NewTransactionTypeRow();

            newRow.id = this.getNextIDFromTableNamed("TransactionType");
            newRow.name = "";

            ffDataSet.TransactionType.AddTransactionTypeRow(newRow);

            return newRow;
        }



        ///////////////////////////////////////////////////////////////////////
        // Lists that stay the same.
        ///////////////////////////////////////////////////////////////////////
        public List<CatagoryCON> AccountCatagories
        {
            get
            {
                List<CatagoryCON> temp = new List<CatagoryCON>();

                temp.Add(CatagoryCON.ACCOUNT);
                temp.Add(CatagoryCON.EXPENSE);
                temp.Add(CatagoryCON.INCOME);

                return temp;
            }
        }

        public List<PolarityCON> Polarities
        {
            get
            {
                List<PolarityCON> temp = new List<PolarityCON>();

                temp.Add(PolarityCON.CREDIT);
                temp.Add(PolarityCON.DEBIT);

                return temp;
            }
        }


        ///////////////////////////////////////////////////////////////////////
        // Observable Collections that the user can modify.
        ///////////////////////////////////////////////////////////////////////
        private ObservableCollection<AccountDRM> _Accounts;
        public ObservableCollection<AccountDRM> Accounts
        {
            get
            {
                if (this._Accounts == null)
                {
                    _Accounts = new ObservableCollection<AccountDRM>();

                    foreach (FFDataSet.AccountRow row in this.ffDataSet.Account)
                        _Accounts.Add(new AccountDRM(row, ffDataSet.BankInfo.FindByaccountID(row.id)));
                }

                return _Accounts;
            }
        }

        private ObservableCollection<AccountTypeDRM> _AccountTypes;
        public ObservableCollection<AccountTypeDRM> AccountTypes
        {
            get
            {
                if (this._AccountTypes == null)
                {
                    _AccountTypes = new ObservableCollection<AccountTypeDRM>();

                    foreach (FFDataSet.AccountTypeRow row in ffDataSet.AccountType)
                        _AccountTypes.Add(new AccountTypeDRM(row));
                }

                return _AccountTypes;
            }
        }

        private ObservableCollection<BankDRM> _Banks;
        public ObservableCollection<BankDRM> Banks
        {
            get
            {
                if (this._Banks == null)
                {
                    _Banks = new ObservableCollection<BankDRM>();

                    foreach (FFDataSet.BankRow row in ffDataSet.Bank)
                        _Banks.Add(new BankDRM(row));
                }

                return _Banks;
            }
        }

        private ObservableCollection<EnvelopeGroupDRM> _EnvelopeGroups;
        public ObservableCollection<EnvelopeGroupDRM> EnvelopeGroups
        {
            get
            {
                _EnvelopeGroups = new ObservableCollection<EnvelopeGroupDRM>();

                foreach (FFDataSet.EnvelopeGroupRow row in ffDataSet.EnvelopeGroup)
                    _EnvelopeGroups.Add(new EnvelopeGroupDRM(row));

                return _EnvelopeGroups;
            }
        }

        private ObservableCollection<EnvelopeDRM> _Envelopes;
        public ObservableCollection<EnvelopeDRM> Envelopes
        {
            get
            {
                _Envelopes = new ObservableCollection<EnvelopeDRM>();

                foreach (FFDataSet.EnvelopeRow row in ffDataSet.Envelope)
                    _Envelopes.Add(new EnvelopeDRM(row));

                return _Envelopes;
            }
        }

        private ObservableCollection<TransactionTypeDRM> _TransactionType;
        public ObservableCollection<TransactionTypeDRM> TransactionTypes
        {
            get
            {
                if (_TransactionType == null)
                {
                    _TransactionType = new ObservableCollection<TransactionTypeDRM>();

                    foreach (FFDataSet.TransactionTypeRow row in ffDataSet.TransactionType)
                        _TransactionType.Add(new TransactionTypeDRM(row));
                }

                return _TransactionType;
            }
        }



        ///////////////////////////////////////////////////////////////////////
        // Event stuff
        ///////////////////////////////////////////////////////////////////////


   

        ///////////////////////////////////////////////////////////////////////
        // 
        ///////////////////////////////////////////////////////////////////////
        public FFDataSet.TransactionRow getTransactionRowWithID(int transID)
        {
            FFDataSet.TransactionRow row = ffDataSet.Transaction.FindByid(transID);

            if (row == null)
                showErrorMessage("Invalid transaction row ID.");

            return row;
        }

        public bool doesAccountUseEnvelopes(int accountID)
        {
            FFDataSet.AccountRow account = this.ffDataSet.Account.FindByid(accountID);

            if (account != null)
                return account.envelopes;
            else
                return false;

        }

        public void updateLineItemDependants(LineItemDRM line)
        {
            if (line == null)
                return;

            foreach (AccountDRM account in this.Accounts)
            {
                if (account.ID == line.AccountID)
                {
                    account.dependentLineItemChanged();
                    return;
                }
            }

        }



    }
}
