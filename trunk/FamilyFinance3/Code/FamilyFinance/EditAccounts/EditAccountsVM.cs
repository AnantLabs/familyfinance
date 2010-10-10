using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Model;

namespace FamilyFinance.EditAccounts
{
    /// <summary>
    /// The View Model used in editing account details.
    /// </summary>
    class EditAccountsVM : ModelBase
    {
        private bool _IncludeIncomes;
        public bool IncludeIncomes
        {
            get
            {
                return this._IncludeIncomes;
            }
            set
            {
                this._IncludeIncomes = value;
                loadAccounts();
            } 
        }

        private bool _IncludeAccounts;
        public bool IncludeAccounts
        {
            get
            {
                return this._IncludeAccounts;
            }
            set
            {
                this._IncludeAccounts = value;
                loadAccounts();
            }
        }

        private bool _IncludeExpences;
        public bool IncludeExpences
        {
            get
            {
                return this._IncludeExpences;
            }
            set
            {
                this._IncludeExpences = value;
                loadAccounts();
            }
        }

        private string _SearchText;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                this._SearchText = value;
                loadAccounts();
            }
        }


        private bool _ShowClosed;
        public bool ShowClosed
        {
            get
            {
                return this._ShowClosed;
            }
            set
            {
                this._ShowClosed = value;
                loadAccounts();
            }
        }

        /// <summary>
        /// Gets or sets the collection of accounts.
        /// </summary>
        public ObservableCollection<AccountBankModel> Accounts { get; set; }

        /// <summary>
        /// Gets or sets the collection of account types.
        /// </summary>
        public List<IdName> AccountTypes { get; set; }

        /// <summary>
        /// Gets or sets the collection of account types.
        /// </summary>
        public List<IdName> Banks { get; set; }

        /// <summary>
        /// Gets or sets the list of catagories that the accounts can be apart of.
        /// </summary>
        public CatagoryModel[] Catagories { get; set; }

        public CreditDebitModel[] CreditDebits { get; set; }

        /// <summary>
        /// Loads the Account collection with data.
        /// </summary>
        private void loadAccounts()
        {
            if (this.Accounts != null)
                this.Accounts.Clear();



            this.Accounts = CollectionBuilder.getAccountsEditable(
                this._IncludeIncomes,
                this._IncludeAccounts,
                this._IncludeExpences,
                this._ShowClosed,
                this._SearchText);

            this.RaisePropertyChanged("Accounts");
        }

        public void reloadBanks()
        {
            this.Banks = CollectionBuilder.getBanksAll();
            this.RaisePropertyChanged("Banks");
        }

        public void reloadAccountTypes()
        {
            this.AccountTypes = CollectionBuilder.getAccountTypesAll();
            this.RaisePropertyChanged("AccountTypes");
        }

        /// <summary>
        /// Creats the view model for editing accounts
        /// </summary>
        public EditAccountsVM()
        {
            this._SearchText = "";

            this.loadAccounts();
            this.reloadBanks();
            this.reloadAccountTypes();
            this.Catagories = CollectionBuilder.getCatagoryArray();
            this.CreditDebits = CollectionBuilder.getCreditDebitArray();
        }
    
    }
}
