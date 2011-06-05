using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System;

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

        private bool _ShowIncomes;
        public bool ShowIncomes 
        {
            get
            {
                return _ShowIncomes;
            }
            set
            {
                this._ShowIncomes = value;
                this.refreshViewFilter(this._AccountsView);
            }
        }

        private bool _ShowAccounts;
        public bool ShowAccounts
        {
            get
            {
                return _ShowAccounts;
            }
            set
            {
                this._ShowAccounts = value;
                this.refreshViewFilter(this._AccountsView);
            }
        }

        private bool _ShowExpenses;
        public bool ShowExpenses
        {
            get
            {
                return _ShowExpenses;
            }
            set
            {
                this._ShowExpenses = value;
                this.refreshViewFilter(this._AccountsView);
            }
        }

        private bool _ShowClosed;
        public bool ShowClosed
        {
            get
            {
                return _ShowClosed;
            }
            set
            {
                this._ShowClosed = value;
                this.refreshViewFilter(this._AccountsView);
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
                this.refreshViewFilter(this._AccountsView);
            }
        }

        private ListCollectionView _AccountsView;
        public ListCollectionView AccountsView
        {
            get 
            {
                return _AccountsView;
            }
        }

        public ListCollectionView AccountTypesView
        {
            get
            {
                ListCollectionView temp;
                    
                temp = (ListCollectionView)CollectionViewSource.GetDefaultView(new AccountTypeTM().AllAccountTypes);
                temp.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

                return temp;
            }
        }

        public ListCollectionView BanksView
        {
            get
            {
                ListCollectionView temp;

                temp = (ListCollectionView)CollectionViewSource.GetDefaultView(new BankTM().AllBanks);
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

        public List<CreditDebitCON> Normals
        {
            get
            {
                return new CreditDebitTM().List;
            }
        }


        ///////////////////////////////////////////////////////////
        // Private functions
        private bool Filter(object item)
        {
            AccountDRM accRow = (AccountDRM)item;
            bool keepItem = true; // Assume the item will be shown in the list

            // Remove the item if we don't want to see incomes, accounts, expenses, closed, or not in the search.
            if (!this._ShowIncomes && accRow.CatagoryID == CatagoryCON.INCOME.ID)
                keepItem = false;

            else if (!this._ShowAccounts && accRow.CatagoryID == CatagoryCON.ACCOUNT.ID)
                keepItem = false;

            else if (!this._ShowExpenses && accRow.CatagoryID == CatagoryCON.EXPENSE.ID)
                keepItem = false;

            else if (!this._ShowClosed && accRow.Closed)
                keepItem = false;

            else if (!String.IsNullOrEmpty(this._SearchText) && !accRow.Name.ToLower().Contains(this.SearchText.ToLower()))
                keepItem = false;

            return keepItem;
        }



        ///////////////////////////////////////////////////////////
        // Public functions
        public EditAccountsVM()
        {
            this._ShowIncomes = false;
            this._ShowAccounts = true;
            this._ShowExpenses = false;
            this._ShowClosed = false;
            this._SearchText = "";

            this._AccountsView = (ListCollectionView)CollectionViewSource.GetDefaultView(new AccountTM().EditableAccounts);
            this._AccountsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            this._AccountsView.Filter = new Predicate<Object>(Filter);
        }

    }
}
