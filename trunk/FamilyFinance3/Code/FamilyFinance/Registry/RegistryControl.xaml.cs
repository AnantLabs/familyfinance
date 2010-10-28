using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FamilyFinance.Registry
{
    /// <summary>
    /// Interaction logic for RegistryControl.xaml
    /// </summary>
    public partial class RegistryControl : UserControl
    {
        RegistryVM rVM;

        public RegistryControl()
        {
            InitializeComponent();

            rVM = (RegistryVM)this.Resources["rVM"];
        }

        private void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            rVM.registryRowEditEnding();
        }

        private void ae_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BalanceModel bModle = e.AddedItems[0] as BalanceModel;

            if (bModle != null)
                rVM.setCurrentAccountEnvelope(bModle.AccountID, bModle.EnvelopeID);

        }

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            int i = 0;
        }

        private void DataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            int i = 0;
        }
    }
}
