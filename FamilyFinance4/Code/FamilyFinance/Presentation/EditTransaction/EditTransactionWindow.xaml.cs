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
using System.Windows.Shapes;

namespace FamilyFinance.Presentation.EditTransaction
{
    public partial class EditTransactionWindow : Window
    {
        private EditTransactionVM transactionViewModel;
        private DataGrid sourceDataGrid;
        private DataGrid destinationDataGrid;


        private void tellViewModelWhoItBelongsTo()
        {
            this.transactionViewModel.setParentWindow(this);
        }

        private void loadTransaction(int transID)
        {
            this.transactionViewModel.loadTransaction(transID);
        }

        private void grabTheObjectsFromTheWindowsResources()
        {
            this.transactionViewModel = (EditTransactionVM)this.FindResource("editTransactionVM");

            this.sourceDataGrid = (DataGrid)this.FindName("creditDataGrid");
            this.destinationDataGrid = (DataGrid)this.FindName("debitDataGrid");
        }

        ///////////////////////////////////////////////////////////
        // Public Functions
        ///////////////////////////////////////////////////////////
        public EditTransactionWindow() : this(1392)
        {
        }

        public EditTransactionWindow(int transactionID)
        {
            InitializeComponent();

            grabTheObjectsFromTheWindowsResources();
            loadTransaction(transactionID);
            tellViewModelWhoItBelongsTo();

            unselectFromSourceDataGRid();
            unselectFromDestinationDataGrid();
        }

        public void unselectFromDestinationDataGrid()
        {
            this.destinationDataGrid.SelectedIndex = -1;
        }

        public void unselectFromSourceDataGRid()
        {
            this.sourceDataGrid.SelectedIndex = -1;
        }


    }
}
