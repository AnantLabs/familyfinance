using System.Windows.Controls;

namespace FamilyFinance.Registry
{
    /// <summary>
    /// Interaction logic for RegistryPanel.xaml
    /// </summary>
    public partial class RegistryPanel : UserControl
    {
        public RegistryPanel()
        {
            InitializeComponent();
        }

        private void Navigator_AccountEnvelopeChanged(object sender, AccountEnvelopeChangedEventArgs e)
        {
            grid.setCurrentAccountEnvelope(e.AccountID, e.EnvelopeID);
        }

        private void grid_AccountEnvelopeChanged(object sender, AccountEnvelopeChangedEventArgs e)
        {
            nav.updateAccountEnvelope(e.AccountID, e.EnvelopeID);
        }
    }
}
