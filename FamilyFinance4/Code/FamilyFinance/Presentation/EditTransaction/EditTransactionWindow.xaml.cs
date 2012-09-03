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
        ///////////////////////////////////////////////////////////
        // local variables
        ///////////////////////////////////////////////////////////
        private EditTransactionVM transactionViewModel;


        ///////////////////////////////////////////////////////////
        // Events
        ///////////////////////////////////////////////////////////
        private void dataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            DataGrid grid = (DataGrid)sender;

            if (grid == this.sourceDataGrid)
            {
                this.transactionViewModel.sourceGridHasFocus();
                this.unselectFromDestinationDataGrid();
                //this.unselectFromEnvelopeDataGrid();
            }
            else if (grid == this.destinationDataGrid)
            {
                this.transactionViewModel.destinationGridHasFocus();
                this.unselectFromSourceDataGrid();
                //this.unselectFromEnvelopeDataGrid();
            }


        }


        ///////////////////////////////////////////////////////////
        // Private Functions
        ///////////////////////////////////////////////////////////
        private void unselectFromDestinationDataGrid()
        {
            this.destinationDataGrid.SelectedIndex = -1;
        }

        private void unselectFromSourceDataGrid()
        {
            this.sourceDataGrid.SelectedIndex = -1;
        }

        //private void unselectFromEnvelopeDataGrid()
        //{
        //    this.envelopeDataGrid.SelectedIndex = -1;
        //}


        ///////////////////////////////////////////////////////////
        // Public Functions
        ///////////////////////////////////////////////////////////
        public EditTransactionWindow() : this(1392)
        {
        }

        public EditTransactionWindow(int transactionID)
        {
            InitializeComponent();

            this.transactionViewModel = (EditTransactionVM)this.FindResource("editTransactionVM");
            this.transactionViewModel.loadTransaction(transactionID);

            this.unselectFromSourceDataGrid();
            this.unselectFromDestinationDataGrid();
            //this.unselectFromEnvelopeDataGrid();
        }

    }
}
