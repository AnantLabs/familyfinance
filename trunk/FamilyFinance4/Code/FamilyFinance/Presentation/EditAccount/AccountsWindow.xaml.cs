using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FamilyFinance.Presentation.EditAccount
{
    /// <summary>
    /// Interaction logic for AccountsWindow.xaml
    /// </summary>
    public partial class AccountsWindow : Window
    {

        private EditAccountsVM eaViewModel;

        public AccountsWindow()
        {
            InitializeComponent();
            this.eaViewModel = (EditAccountsVM)(this.Resources["eaViewModel"]);
        }

        private void IncludeIncomes(object sender, RoutedEventArgs e)
        {
            //this.eaViewModel.IncludeIncomes(sender, e);
        }

        private void FilterIncomes(object sender, RoutedEventArgs e)
        {
            //this.eaViewModel.FilterIncomes(sender, e);
        }

        private void IncludeAccounts(object sender, RoutedEventArgs e)
        {
            //if(this.eaViewModel != null)
            //    this.eaViewModel.IncludeAccounts(sender, e);
        }

        private void FilterAccounts(object sender, RoutedEventArgs e)
        {
            //this.eaViewModel.FilterAccounts(sender, e);
        }

        private void IncludeExpenses(object sender, RoutedEventArgs e)
        {
            //this.eaViewModel.IncludeExpenses(sender, e);
        }

        private void FilterExpenses(object sender, RoutedEventArgs e)
        {
            //this.eaViewModel.FilterExpenses(sender, e);
        }

        private void IncludeClosed(object sender, RoutedEventArgs e)
        {
            //this.eaViewModel.IncludeClosed(sender, e);
        }

        private void FilterClosed(object sender, RoutedEventArgs e)
        {
            //this.eaViewModel.FilterClosed(sender, e);
        }

        private void FilterText(object sender, TextChangedEventArgs e)
        {
            //this.eaViewModel.FilterText(sender, e);
        }


    }
}
