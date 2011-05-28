using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

using FamilyFinance.Buisness;
using FamilyFinance.Data;
using System.Windows;
using System.Windows.Controls;

namespace FamilyFinance.Presentation.EditAccount
{
    /// <summary>
    /// The View Model used in editing account details.
    /// </summary>
    class EditAccountsVM : ViewModel
    {
        ///////////////////////////////////////////////////////////
        // Properties
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

        public List<CreditDebitCON> Normals
        {
            get
            {
                return new CreditDebitTM().List;
            }
        }

        public ICollectionView Banks
        {
            get
            {
                ICollectionView temp = CollectionViewSource.GetDefaultView(new BankTM().AllBanks);
                temp.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

                return temp;
            }
        }


        ///////////////////////////////////////////////////////////
        // Public functions

        /// <summary>
        /// Creates the view model for editing accounts
        /// </summary>
        public EditAccountsVM()
        {
            //this._IncludeAccounts = true;
            //this._IncludeExpenses = false;
            //this._IncludeIncomes = false;
            //this._SearchText = "";
            //this._ShowClosed = false;

            this._Accounts = CollectionViewSource.GetDefaultView(new AccountTM().EditableAccounts);
            this._Accounts.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }



        public void IncludeIncomes(object sender, RoutedEventArgs e)
        {
            //_Accounts.Filter -= new FilterEventHandler(IncomeFilter);
        }

        public void FilterIncomes(object sender, RoutedEventArgs e)
        {
            //_Accounts.Filter += new FilterEventHandler(IncomeFilter);
        }

        private void IncomeFilter(object sender, FilterEventArgs e)
        {
            AccountBankInfoDRM row = e.Item as AccountBankInfoDRM;
         
            if ((row == null) || row.CatagoryID == CatagoryCON.INCOME.ID)
                e.Accepted = false;
        }



        public void IncludeAccounts(object sender, RoutedEventArgs e)
        {
            //_Accounts.Filter -= new FilterEventHandler(AccountFilter);
        }

        public void FilterAccounts(object sender, RoutedEventArgs e)
        {
            //_Accounts.Filter += new FilterEventHandler(AccountFilter);
        }

        private void AccountFilter(object sender, FilterEventArgs e)
        {
            AccountBankInfoDRM row = e.Item as AccountBankInfoDRM;

            if ((row == null) || row.CatagoryID == CatagoryCON.ACCOUNT.ID)
                e.Accepted = false;
        }



        public void IncludeExpenses(object sender, RoutedEventArgs e)
        {
            //_Accounts.Filter -= new FilterEventHandler(ExpenseFilter);
        }

        public void FilterExpenses(object sender, RoutedEventArgs e)
        {
            //_Accounts.Filter += new FilterEventHandler(ExpenseFilter);
        }

        private void ExpenseFilter(object sender, FilterEventArgs e)
        {
            AccountBankInfoDRM row = e.Item as AccountBankInfoDRM;

            if ((row == null) || row.CatagoryID == CatagoryCON.EXPENSE.ID)
                e.Accepted = false;
        }




        public void IncludeClosed(object sender, RoutedEventArgs e)
        {
            //_Accounts.Filter -= new FilterEventHandler(ClosedFilter);
        }

        public void FilterClosed(object sender, RoutedEventArgs e)
        {
            //_Accounts.Filter += new FilterEventHandler(ClosedFilter);
        }

        private void ClosedFilter(object sender, FilterEventArgs e)
        {
            AccountBankInfoDRM row = e.Item as AccountBankInfoDRM;

            if ((row == null) || row.Closed == true)
                e.Accepted = false;
        }




        public void FilterText(object sender, TextChangedEventArgs e)
        {
            //_Accounts.Filter -= new FilterEventHandler(IncomeFilter);
        }
    }
}
