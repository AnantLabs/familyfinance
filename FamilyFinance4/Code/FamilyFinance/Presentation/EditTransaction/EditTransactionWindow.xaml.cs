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

        private bool allReadyInSelectionChanged = false;
        private void sourceOrDestinationDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allReadyInSelectionChanged)
                return;

            allReadyInSelectionChanged = true;
            //this.transactionViewModel.

            DataGrid currentDG = (DataGrid)sender;

            if (currentDG == sourceDataGrid)
                this.unselectFromDestinationDataGrid();

            else if (currentDG == destinationDataGrid)
                this.unselectFromSourceDataGRid();


            allReadyInSelectionChanged = false;
        }

        private void unselectFromDestinationDataGrid()
        {
            this.destinationDataGrid.SelectedIndex = -1;
        }

        private void unselectFromSourceDataGRid()
        {
            this.sourceDataGrid.SelectedIndex = -1;
        }

        private void grabTheObjectsFromTheWindowsResources()
        {
            this.transactionViewModel = (EditTransactionVM)this.FindResource("editTransactionVM");

            this.sourceDataGrid = (DataGrid)this.FindName("creditDataGrid");
            this.destinationDataGrid = (DataGrid)this.FindName("debitDataGrid");
        }

        private void loadTransaction(int transID)
        {
            this.transactionViewModel.loadTransaction(transID);
        }


        public EditTransactionWindow() : this(1392)
        {
        }

        public EditTransactionWindow(int transactionID)
        {
            InitializeComponent();

            grabTheObjectsFromTheWindowsResources();
            loadTransaction(1392);


            this.sourceDataGrid.SelectionChanged += new SelectionChangedEventHandler(sourceOrDestinationDataGrid_SelectionChanged);
            this.destinationDataGrid.SelectionChanged += new SelectionChangedEventHandler(sourceOrDestinationDataGrid_SelectionChanged);

            unselectFromSourceDataGRid();
            unselectFromDestinationDataGrid();
        }
    }
}
