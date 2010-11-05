using System.Windows;
using System.Windows.Controls;


namespace FamilyFinance.Registry
{
    /// <summary>
    /// Interaction logic for RegistryControl.xaml
    /// </summary>
    public partial class RegistryGrid : UserControl
    {
        RegistryGridVM gridVM;

        public RegistryGrid()
        {
            InitializeComponent();

            gridVM = (RegistryGridVM)this.Resources["gridVM"];
        }


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
            gridVM.reloadAccounts();
            gridVM.reloadEnvelopes();
            gridVM.reloadLineTypes();

            gridVM.setCurrentAccountEnvelope(3, -1);
        }

        private void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            gridVM.registryRowEditEnding();
        }

        ///////////////////////////////////////////////////////////////////////
        //   Public Functions
        ///////////////////////////////////////////////////////////////////////  
        public void setCurrentAccountEnvelope(int aID, int eID)
        {
            this.gridVM.setCurrentAccountEnvelope(aID, eID);
        }

    }
}
