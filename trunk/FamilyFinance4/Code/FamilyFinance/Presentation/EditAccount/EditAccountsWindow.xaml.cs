using System.Windows;

namespace FamilyFinance.Presentation.EditAccount
{
    /// <summary>
    /// Interaction logic for AccountsWindow.xaml
    /// </summary>
    public partial class EditAccountsWindow : Window
    {
        public EditAccountsWindow()
        {
            InitializeComponent();
        }

        private void EditAccountTypes_Click(object sender, RoutedEventArgs e)
        {
            EditTypes.EditTypesWindow etWin = new EditTypes.EditTypesWindow(EditTypes.EditTypesVM.Table.AccountType);
            etWin.ShowInTaskbar = false;
            etWin.ShowDialog();
        }

        private void EditBanks_Click(object sender, RoutedEventArgs e)
        {
            EditTypes.EditTypesWindow etWin = new EditTypes.EditTypesWindow(EditTypes.EditTypesVM.Table.Bank);
            etWin.ShowInTaskbar = false;
            etWin.ShowDialog();
        }

    }
}
