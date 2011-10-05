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
                ViewModel.refreshViewFilter(this._AccountsView);
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
                ViewModel.refreshViewFilter(this._AccountsView);
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
                ViewModel.refreshViewFilter(this._AccountsView);
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
                ViewModel.refreshViewFilter(this._AccountsView);
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
                ViewModel.refreshViewFilter(this._AccountsView);
            }
        }

        private ListCollectionView _AccountsView;
        public ListCollectionView AccountsView
        {
            get 
            {
                if (this._AccountsView == null)
                {
                    this._AccountsView = new ListCollectionView(DataSetModel.Instance.Accounts);
                    this._AccountsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    this._AccountsView.Filter = new Predicate<Object>(EditableAccountsFilter);
                }

                return _AccountsView;
            }
        }

        public ListCollectionView AccountTypesView
        {
            get
            {
                ListCollectionView atView;

                atView = new ListCollectionView(DataSetModel.Instance.AccountTypes);
                atView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

                return atView;
            }
        }

        public ListCollectionView BanksView
        {
            get
            {
                ListCollectionView bView;

                bView = new ListCollectionView(DataSetModel.Instance.Banks);
                bView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

                return bView;
            }
        }

        public List<CatagoryCON> Catagories
        {
            get
            {
                // We don't need to worry about filtering or sorting just pass back the list.
                // The view will be automatically/annonymously generated.
                return DataSetModel.Instance.AccountCatagories;
            }
        }

        public List<PolarityCON> Normals
        {
            get
            {
                // We don't need to worry about filtering or sorting just pass back the list.
                // The view will be automatically/annonymously generated.
                return DataSetModel.Instance.Polarities;
            }
        }


        ///////////////////////////////////////////////////////////
        // Private functions
        private bool EditableAccountsFilter(object item)
        {
            AccountDRM accRow = (AccountDRM)item;
            bool keepItem = true; // Assume the item will be shown in the list

            // Remove the item if we don't want to see incomes, accounts, expenses, closed, or not in the search.
            if (accRow.Catagory == CatagoryCON.NULL)
                keepItem = false;
            
            else if (!this._ShowIncomes && accRow.Catagory == CatagoryCON.INCOME)
                keepItem = false;

            else if (!this._ShowAccounts && accRow.Catagory == CatagoryCON.ACCOUNT)
                keepItem = false;

            else if (!this._ShowExpenses && accRow.Catagory == CatagoryCON.EXPENSE)
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
        }

    }
}
