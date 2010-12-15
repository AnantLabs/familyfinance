using System.Windows;
using System.Windows.Controls;
using FamilyFinance.Database;


namespace FamilyFinance.Registry
{
    /// <summary>
    /// Interaction logic for RegistryControl.xaml
    /// </summary>
    public partial class RegistryGrid : UserControl
    {

        RegistryGridVM gridVM;


        ///////////////////////////////////////////////////////////////////////
        //   External Events
        ///////////////////////////////////////////////////////////////////////   
        public event AccountEnvelopeChangedEventHandler AccountEnvelopeChanged;
        private void OnAccountEnvelopeChanged(AccountEnvelopeChangedEventArgs e)
        {
            // Raises the event CloseMe
            if (AccountEnvelopeChanged != null)
                AccountEnvelopeChanged(this, e);
        }



        ///////////////////////////////////////////////////////////////////////
        //   Internal Events
        ///////////////////////////////////////////////////////////////////////  
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            gridVM.setCurrentAccountEnvelope(3, -1);
        }

        private void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            gridVM.registryRowEditEnding();
        }



        ///////////////////////////////////////////////////////////////////////
        //   Public Functions
        ///////////////////////////////////////////////////////////////////////          
        public RegistryGrid()
        {
            InitializeComponent();

            gridVM = (RegistryGridVM)this.Resources["gridVM"];
        }

        public void setCurrentAccountEnvelope(int aID, int eID)
        {
            this.gridVM.setCurrentAccountEnvelope(aID, eID);

            if (SpclEnvelope.isSpecial(eID))
                this.dataGrid.ItemsSource = this.gridVM.RegistryLines;
            else
                this.dataGrid.ItemsSource = this.gridVM.SubRegistryLines;

        }

        private void dataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LineItemRegModel row = (LineItemRegModel)this.dataGrid.SelectedItem;

            EditTransaction.EditTransaction et = new EditTransaction.EditTransaction(row.TransactionID);
            et.ShowDialog();
        }

    }
}
