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

namespace FamilyFinance.EditTransaction
{
    /// <summary>
    /// Interaction logic for EditTransaction.xaml
    /// </summary>
    public partial class EditTransaction : Window
    {
        private EditTransactionVM eTVM;
        private int _transID;

        public EditTransaction() : this(4)
        {
        }

        public EditTransaction(int transID)
        {
            InitializeComponent();

            this._transID = transID;
            this.Resources["eTVM"] = eTVM = new EditTransactionVM(transID);
            //this.InvalidateVisual();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
