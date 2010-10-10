using System.Windows;

using FamilyFinance.EditAccounts;
using FamilyFinance.EditTypes;
using FamilyFinance.EditEnvelopes;

namespace FamilyFinance
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            EditAccountsWindow win = new EditAccountsWindow();
            win.ShowDialog();
        }



        private void button2_Click(object sender, RoutedEventArgs e)
        {
            EditTypesWindow win = new EditTypesWindow(Table.AccountType);
            win.ShowDialog();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            EditTypesWindow win = new EditTypesWindow(Table.LineType);
            win.ShowDialog();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            EditTypesWindow win = new EditTypesWindow(Table.EnvelopeGroup);
            win.ShowDialog();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            EditTypesWindow win = new EditTypesWindow(Table.Bank);
            win.ShowDialog();
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            EditEnvelopesWindow win = new EditEnvelopesWindow();
            win.ShowDialog();
        }

    }
}
