using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

using FamilyFinance.Buisness;
using FamilyFinance.Data;

namespace FamilyFinance.Presentation.EditAccount
{
    /// <summary>
    /// The View Model used in editing account details.
    /// </summary>
    class EditAccountsVM : ViewModel
    {
        ///////////////////////////////////////////////////////////
        // Properties
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
                //this.RaisePropertyChanged("Accounts");
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
                //this.RaisePropertyChanged("Accounts");
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
                //this.RaisePropertyChanged("Accounts");
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
                //this.RaisePropertyChanged("Accounts");
            }
        }

        /// <summary>
        /// Gets or sets the collection of accounts.
        /// </summary>
        //public ObservableCollection<AccountBankInfoDRM> Accounts 
        //{
        //    get
        //    {
        //        ObservableCollection<AccountBankInfoDRM> accounts = new ObservableCollection<AccountBankInfoDRM>();

        //        List<byte> cats = new List<byte>();

        //        if (this._IncludeIncomes)
        //            cats.Add(CatagoryCON.INCOME.ID);

        //        if (this._IncludeAccounts)
        //            cats.Add(CatagoryCON.ACCOUNT.ID);

        //        if (this._IncludeExpences)
        //            cats.Add(CatagoryCON.EXPENCE.ID);

        //        foreach (FFDataSet.AccountRow aRow in MyData.getInstance().Account)
        //        {
        //            bool validID = aRow.id > 0;
        //            bool validCat = cats.Contains(aRow.catagory);
        //            bool inSearch = aRow.name.ToLower().Contains(this._SearchText.ToLower());
        //            bool doShow = this._ShowClosed || !aRow.closed;

        //            if (validID && validCat && inSearch && doShow)
        //                accounts.Add(new AccountBankInfoDRM(aRow));
        //        }

        //        return accounts;
        //    }
        //}

        private ICollectionView _Accounts;
        public ICollectionView Accounts
        {
            get 
            {
                return _Accounts;
            }
        }

        public ICollectionView AccountTypes
        {
            get
            {
                ICollectionView temp = CollectionViewSource.GetDefaultView(new AccountTypeTM().AllAccountTypes);

                temp.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

                return temp;
            }
        }

        public List<CatagoryCON> Catagories
        {
            get
            {
                return new AccountCatagoryTM().Catagories;
            }
        }

        ///////////////////////////////////////////////////////////
        // Public functions

        /// <summary>
        /// Creates the view model for editing accounts
        /// </summary>
        public EditAccountsVM()
        {
            this._IncludeAccounts = true;
            this._IncludeExpences = false;
            this._IncludeIncomes = false;
            this._SearchText = "";
            this._ShowClosed = false;

            this._Accounts = CollectionViewSource.GetDefaultView(new AccountTM().EditableAccounts);
            this._Accounts.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
    
    }
}
