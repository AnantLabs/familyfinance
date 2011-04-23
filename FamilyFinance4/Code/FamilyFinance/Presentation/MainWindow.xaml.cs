using System;
using System.Windows;

namespace FamilyFinance.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EditAccounts_Click(object sender, RoutedEventArgs e)
        {
            EditAccount.AccountsWindow aWin = new EditAccount.AccountsWindow();
            aWin.ShowInTaskbar = false;
            aWin.ShowDialog();
        }

        private void EditAccountTypes_Click(object sender, RoutedEventArgs e)
        {
            EditTypes.EditTypesWindow etWin= new EditTypes.EditTypesWindow(EditTypes.EditTypesVM.Table.AccountType);
            etWin.ShowInTaskbar = false;
            etWin.ShowDialog();
        }

        private void EditTransactionTypes_Click(object sender, RoutedEventArgs e)
        {
            EditTypes.EditTypesWindow etWin = new EditTypes.EditTypesWindow(EditTypes.EditTypesVM.Table.TransactionType);
            etWin.ShowInTaskbar = false;
            etWin.ShowDialog();
        }

        private void EditBanks_Click(object sender, RoutedEventArgs e)
        {
            EditTypes.EditTypesWindow etWin = new EditTypes.EditTypesWindow(EditTypes.EditTypesVM.Table.Bank);
            etWin.ShowInTaskbar = false;
            etWin.ShowDialog();
        }

        private void EditEnvelopeGroups_Click(object sender, RoutedEventArgs e)
        {
            EditTypes.EditTypesWindow etWin = new EditTypes.EditTypesWindow(EditTypes.EditTypesVM.Table.EnvelopeGroup);
            etWin.ShowInTaskbar = false;
            etWin.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            FamilyFinance.Data.MyData.getInstance().readData();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            FamilyFinance.Data.MyData.getInstance().saveData();
        }

    }
}
