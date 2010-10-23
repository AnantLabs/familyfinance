﻿using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Model;
using FamilyFinance.Database;

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
            ObservableCollection<AccountBankModel> accounts = new ObservableCollection<AccountBankModel>();

            List<byte> cats = new List<byte>();

            if (this._IncludeIncomes)
                cats.Add(SpclAccountCat.INCOME);

            if (this._IncludeAccounts)
                cats.Add(SpclAccountCat.ACCOUNT);

            if (this._IncludeExpences)
                cats.Add(SpclAccountCat.EXPENSE);

            foreach (FFDataSet.AccountRow aRow in MyData.getInstance().Account)
            {
                bool validID = aRow.id > 0;
                bool validCat = cats.Contains(aRow.catagory);
                bool inSearch = aRow.name.ToLower().Contains(this._SearchText.ToLower());
                bool doShow = this._ShowClosed || !aRow.closed;

                if (validID && validCat && inSearch && doShow)
                    accounts.Add(new AccountBankModel(aRow));
            }

            this.Accounts = accounts;

            this.RaisePropertyChanged("Accounts");
        }

        public void reloadBanks()
        {
            List<IdName> banks = new List<IdName>();

            foreach (FFDataSet.BankRow bRow in MyData.getInstance().Bank)
            {
                banks.Add(new IdName(bRow.id, bRow.name));
            }

            banks.Sort(new IdNameComparer());

            this.Banks = banks;
            this.RaisePropertyChanged("Banks");
        }

        public void reloadAccountTypes()
        {
            List<IdName> types = new List<IdName>();

            foreach (FFDataSet.AccountTypeRow row in MyData.getInstance().AccountType)
                types.Add(new IdName(row.id, row.name));

            types.Sort(new IdNameComparer());

            this.AccountTypes = types;
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

            this.Catagories = new CatagoryModel[] { CatagoryModel.INCOME, CatagoryModel.ACCOUNT, CatagoryModel.EXPENCE };
            this.CreditDebits = new CreditDebitModel[] { CreditDebitModel.CREDIT, CreditDebitModel.DEBIT };

        }
    
    }
}
