using System.Collections.Generic;
using System.Collections.ObjectModel;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{

    /// <summary>
    /// A singleton class that gives list/collection access to the data. As the name implies
    /// this is a model of a dataset.
    /// </summary>
    public class DataSetModel
    {

        ///////////////////////////////////////////////////////////////////////
        // Singleton stuff
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This is a singleton class, this is the static instance of this class.
        /// </summary>
        private static DataSetModel _Instance;

        /// <summary>
        /// Gets the singleton instance of the DataSetModel class.
        /// </summary>
        /// <returns>Singelton instance of the DataSetModel class</returns>
        public static DataSetModel getInstance()
        {
            if (DataSetModel._Instance == null)
                _Instance = new DataSetModel();

            return DataSetModel._Instance;
        }



        ///////////////////////////////////////////////////////////////////////
        // Lists that stay the same.
        ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// List of Account catagories (Account, Expense, Income) The NULL catagory is excluded.
        /// </summary>
        public List<CatagoryCON> AccountCatagories
        {
            get
            {
                List<CatagoryCON> temp = new List<CatagoryCON>();

                //temp.Add(CatagoryCON.NULL);
                temp.Add(CatagoryCON.ACCOUNT);
                temp.Add(CatagoryCON.EXPENSE);
                temp.Add(CatagoryCON.INCOME);

                return temp;
            }
        }

        /// <summary>
        /// A list containing the Credit and Debits. The credits and debits are used in describing 
        /// the polarity of a Line Item and an account that has bank information.
        /// </summary>
        public List<CreditDebitCON> CreditDebits
        {
            get
            {
                List<CreditDebitCON> temp = new List<CreditDebitCON>();

                temp.Add(CreditDebitCON.CREDIT);
                temp.Add(CreditDebitCON.DEBIT);

                return temp;
            }
        }


        ///////////////////////////////////////////////////////////////////////
        // Observable Collections that the user can modify.
        ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The perminant single referance to the collection of all accounts.
        /// </summary>
        private ObservableCollection<AccountDRM> _Accounts;

        /// <summary>
        /// A reference to the collection of all Accounts.
        /// </summary>
        public ObservableCollection<AccountDRM> Accounts
        {
            get
            {
                if (this._Accounts == null)
                {
                    _Accounts = new ObservableCollection<AccountDRM>();

                    foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
                        _Accounts.Add(new AccountDRM(row));
                }

                return _Accounts;
            }
        }

        /// <summary>
        /// The perminant single referance to the collection of all account types.
        /// </summary>
        private ObservableCollection<AccountTypeDRM> _AccountTypes;

        /// <summary>
        /// A reference to the collection of all Account Types.
        /// </summary>
        public ObservableCollection<AccountTypeDRM> AccountTypes
        {
            get
            {
                if (this._AccountTypes == null)
                {
                    _AccountTypes = new ObservableCollection<AccountTypeDRM>();

                    foreach (FFDataSet.AccountTypeRow row in MyData.getInstance().AccountType)
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

                    foreach (FFDataSet.BankRow row in MyData.getInstance().Bank)
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

                foreach (FFDataSet.EnvelopeGroupRow row in MyData.getInstance().EnvelopeGroup)
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

                foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
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

                    foreach (FFDataSet.TransactionTypeRow row in MyData.getInstance().TransactionType)
                        _TransactionType.Add(new TransactionTypeDRM(row));
                }

                return _TransactionType;
            }
        }
    }
}
