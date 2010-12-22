using System.Windows;

using FamilyFinance.EditTypes;
using FamilyFinance.EditAccounts;

namespace FamilyFinance.EditEnvelopes
{
    /// <summary>
    /// Interaction logic for EditEnvelopesWindow.xaml
    /// </summary>
    public partial class EditEnvelopesWindow : Window
    {
        public EditEnvelopesWindow()
        {
            InitializeComponent();
        }

        private void envelopeGroupsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditTypesWindow win = new EditTypesWindow(Table.EnvelopeGroup);
            win.ShowDialog();
        }

        private void accountMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditAccountsWindow win = new EditAccountsWindow();
            win.ShowDialog();
        }
    }
}
