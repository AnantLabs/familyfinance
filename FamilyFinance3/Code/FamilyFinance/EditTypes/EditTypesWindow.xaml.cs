using System.Windows;

using FamilyFinance.Model;

namespace FamilyFinance.EditTypes
{
    /// <summary>
    /// Interaction logic for EditTypesWindow.xaml
    /// </summary>
    public partial class EditTypesWindow : Window
    {
        private EditTypesVM editTypeVM;

        public EditTypesWindow()
        {
            this.myInit(Table.AccountType);
        }

        public EditTypesWindow(Table table)
        {
            this.myInit(table);
        }

        private void myInit(Table table)
        {
            InitializeComponent();

            this.editTypeVM = new EditTypesVM();

            switch (table)
            {
                case Table.AccountType:
                    this.typeDataGrid.ItemsSource = this.editTypeVM.AccountTypes;
                    this.Title = "Account Types";
                    break;

                case Table.EnvelopeGroup:
                    this.typeDataGrid.ItemsSource = this.editTypeVM.EnvelopeGroups;
                    this.Title = "Envelope Groups";
                    break;

                case Table.LineType:
                    this.typeDataGrid.ItemsSource = this.editTypeVM.LineTypes;
                    this.Title = "Line Types";
                    break;

                case Table.Bank:
                    this.typeDataGrid.ItemsSource = this.editTypeVM.Banks;
                    this.Title = "Banks";
                    this.routingNumberColumn.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }


    }

            
    public enum Table { AccountType, EnvelopeGroup, LineType, Bank};
}
