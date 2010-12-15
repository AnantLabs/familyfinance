using System.Windows;

using FamilyFinance.EditTypes;

namespace FamilyFinance.EditAccounts
{
    /// <summary>
    /// Interaction logic for EditAccountsWindow.xaml
    /// </summary>
    public partial class EditAccountsWindow : Window
    {
        public EditAccountsWindow()
        {
            InitializeComponent();

        }

        private void editBanksButton_Click(object sender, RoutedEventArgs e)
        {
            EditTypesWindow win = new EditTypesWindow(Table.Bank);
            win.ShowDialog();

            //((EditAccountsVM)(this.Resources["eaViewModel"])).reloadBanks();
        }

        private void editAccountTypesButton_Click(object sender, RoutedEventArgs e)
        {
            EditTypesWindow win = new EditTypesWindow(Table.AccountType);
            win.ShowDialog();

            //((EditAccountsVM)(this.Resources["eaViewModel"])).reloadAccountTypes();
        }

    }
}
