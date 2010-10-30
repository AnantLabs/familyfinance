using System.Windows;
using System.Windows.Controls;


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


        private DataGrid prevMajorGrid;
        private DataGrid prevSubGrid;

        private void sub_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                BalanceModel bModle = e.AddedItems[0] as BalanceModel;
                DataGrid thisGrid = sender as DataGrid;

                this.prevSubGrid = thisGrid;

                if (bModle != null)
                    rVM.setCurrentAccountEnvelope(bModle.AccountID, bModle.EnvelopeID);

                e.Handled = true;
            }
        }

        private void major_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                BalanceModel bModle = e.AddedItems[0] as BalanceModel;
                DataGrid thisGrid = sender as DataGrid;

                if (bModle != null)
                {
                    rVM.setCurrentAccountEnvelope(bModle.AccountID, bModle.EnvelopeID);
                    e.Handled = true;

                    // Deselect the sub grid if it's not null
                    if (this.prevSubGrid != null)
                        this.prevSubGrid.SelectedItem = null;

                    // 
                    if (thisGrid != this.prevMajorGrid)
                    {
                        if (this.prevMajorGrid != null)
                            this.prevMajorGrid.SelectedItem = null;

                        this.prevMajorGrid = thisGrid;
                    }
                }
            }
        }

    }
}
