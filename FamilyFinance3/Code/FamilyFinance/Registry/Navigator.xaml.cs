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
    /// Interaction logic for Navigator.xaml
    /// </summary>
    public partial class Navigator : UserControl
    {
        private NavigatorVM navVM;

        public Navigator()
        {
            InitializeComponent();

            navVM = (NavigatorVM)this.Resources["navVM"];
        }

        ///////////////////////////////////////////////////////////////////////
        //   Internal Events
        ///////////////////////////////////////////////////////////////////////  
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            navVM.reloadAccountBalances();
            navVM.reloadEnvelopeBalances();
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
                    this.OnAccountEnvelopeChanged(new AccountEnvelopeChangedEventArgs(bModle.AccountID, bModle.EnvelopeID));

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
                    this.OnAccountEnvelopeChanged(new AccountEnvelopeChangedEventArgs(bModle.AccountID, bModle.EnvelopeID));

                    //gridVM.setCurrentAccountEnvelope(bModle.AccountID, bModle.EnvelopeID);
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
        //   Public Functions
        ///////////////////////////////////////////////////////////////////////  
        public void updateAccountEnvelope(int aID, int eID)
        {
        }

    }

}
