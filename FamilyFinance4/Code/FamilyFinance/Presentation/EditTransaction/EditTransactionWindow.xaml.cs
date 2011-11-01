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

        public EditTransactionWindow()
        {
            InitializeComponent();

            grabTheTransactionViewModelFromTheWindowsResources();
            loadTransaction(62);
        }

        public EditTransactionWindow(int transactionID)
        {
            InitializeComponent();

            grabTheTransactionViewModelFromTheWindowsResources();
            loadTransaction(transactionID);
        }

        private void grabTheTransactionViewModelFromTheWindowsResources()
        {
            this.transactionViewModel = (EditTransactionVM)this.FindResource("editTransactionVM");
        }

        private void loadTransaction(int transID)
        {
            this.transactionViewModel.loadTransaction(transID);
        }


    }
}
