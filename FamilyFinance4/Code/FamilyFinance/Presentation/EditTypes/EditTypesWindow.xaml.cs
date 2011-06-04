using System.Windows;

using FamilyFinance.Buisness;

namespace FamilyFinance.Presentation.EditTypes
{
    /// <summary>
    /// Interaction logic for EditTypesWindow.xaml
    /// </summary>
    public partial class EditTypesWindow : Window
    {

        public EditTypesWindow() : this(EditTypesVM.Table.AccountType)
        {
        }

        public EditTypesWindow(EditTypesVM.Table table)
        {
            InitializeComponent();

            this.DataContext = new EditTypesVM(table);

            // TODO: Find a way to move this to the EditTypesVM class. For some reason the properties
            // do not update to the set/changed values when in the EditTypesVM class.
            switch (table)
            {
                case EditTypesVM.Table.AccountType:
                    this.routingNumberColumn.Visibility = System.Windows.Visibility.Hidden;
                    this.minPercentColumn.Visibility = System.Windows.Visibility.Hidden;
                    this.maxPercentColumn.Visibility = System.Windows.Visibility.Hidden;
                    this.Width = 250;
                    break;

                case EditTypesVM.Table.TransactionType:
                    this.routingNumberColumn.Visibility = System.Windows.Visibility.Hidden;
                    this.minPercentColumn.Visibility = System.Windows.Visibility.Hidden;
                    this.maxPercentColumn.Visibility = System.Windows.Visibility.Hidden;
                    this.Width = 250;
                    break;

                case EditTypesVM.Table.Bank:
                    this.routingNumberColumn.Visibility = System.Windows.Visibility.Visible;
                    this.minPercentColumn.Visibility = System.Windows.Visibility.Hidden;
                    this.maxPercentColumn.Visibility = System.Windows.Visibility.Hidden;
                    this.Width = 400;
                    break;

                case EditTypesVM.Table.EnvelopeGroup:
                    this.routingNumberColumn.Visibility = System.Windows.Visibility.Hidden;
                    this.minPercentColumn.Visibility = System.Windows.Visibility.Visible;
                    this.maxPercentColumn.Visibility = System.Windows.Visibility.Visible;
                    this.Width = 400;
                    break;
            }
        }

    }
}
