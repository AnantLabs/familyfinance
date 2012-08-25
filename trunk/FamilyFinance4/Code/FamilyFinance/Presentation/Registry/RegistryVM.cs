using System;
using System.Windows.Data;
using FamilyFinance.Buisness;
using FamilyFinance.Data;
using System.Collections.ObjectModel;

namespace FamilyFinance.Presentation.Registry
{
    class RegistryVM : ViewModel
    {
        private AccountDRM currentAccount;
        private EnvelopeDRM currentEnvelope;
        //private ObservableCollection<RegistryLineItemModel> currentLineItems;




        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        public string RegistryTitle
        {
            get
            {
                string title = "Registry Title";

                if (this.currentAccount != null)
                {
                    title = this.currentAccount.Name 
                            + "     " + this.currentAccount.TypeName
                            + "     " + this.currentAccount.CatagoryName;
                }
                else if (this.currentEnvelope != null)
                {
                    title = this.currentEnvelope.Name
                            + "     " + this.currentEnvelope.GroupName
                            + "     " + "Envelope";
                }

                return title;
            }
        }

        public string EndingBalance
        {
            get
            {
                if (this.currentAccount != null)
                {
                    return "Ending Balance " + this.currentAccount.getEndingBalance().ToString("C2");
                }
                else if (this.currentEnvelope != null)
                {
                    return "Ending Balance " + this.currentEnvelope.EndingBalance.ToString("C2");
                }
                else
                    return "Ending Balance";
            }
        }

        public string ClearedBalance
        {
            get
            {
                if (this.currentAccount != null)
                {
                    return "Cleared " + this.currentAccount.getClearedBalance().ToString("C2");
                }
                else
                    return "";
            }
        }

        public string ReconciledBalance
        {
            get
            {
                if (this.currentAccount != null)
                {
                    return "Reconsiled " + this.currentAccount.getReconciledBalance().ToString("C2");
                }
                else
                    return "";
            }
        }

        public ListCollectionView AccountsView { get; private set; }

        public ListCollectionView IncomesView { get; private set; }

        public ListCollectionView ExpencesView { get; private set; }

        public ListCollectionView EnvelopesView { get; private set; }



        ///////////////////////////////////////////////////////////
        // View Filters
        ///////////////////////////////////////////////////////////
        private bool AccountsFilter(object item)
        {
            AccountDRM row = (AccountDRM)item;

            if (row == null)
                return false;

            if (row.Catagory != CatagoryCON.ACCOUNT)
                return false;

            if (row.Closed == true)
                return false;

            return true;
        }
        
        private bool IncomesFilter(object item)
        {
            AccountDRM row = (AccountDRM)item;

            if (row == null)
                return false;

            if (row.Catagory != CatagoryCON.INCOME)
                return false;

            if (row.Closed == true)
                return false;

            return true;
        }

        private bool ExpencesFilter(object item)
        {
            AccountDRM row = (AccountDRM)item;

            if (row == null)
                return false;

            if (row.Catagory != CatagoryCON.EXPENSE)
                return false;

            if (row.Closed == true)
                return false;

            return true;
        }

        private bool EnvelopesFilter(object item)
        {
            EnvelopeDRM row = (EnvelopeDRM)item;

            if (row == null || row.IsSpecial())
                return false;

            if (row.EndingBalance != 0 && row.Closed == true)
                return false;

            return true;
        }



        ///////////////////////////////////////////////////////////
        // Event Functions
        ///////////////////////////////////////////////////////////
        private void AccountsView_CurrentChanged(object sender, EventArgs e)
        {
            this.switchToSelectedAccount();
        }

        private void IncomesView_CurrentChanged(object sender, EventArgs e)
        {
            this.switchToSelectedIncome();
        }

        private void ExpencesView_CurrentChanged(object sender, EventArgs e)
        {
            this.switchToSelectedExpence();
        }

        private void EnvelopesView_CurrentChanged(object sender, EventArgs e)
        {
            switchToSelectedEnvelope();
        }



        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private void setupViews()
        {
            this.AccountsView = new ListCollectionView(DataSetModel.Instance.Accounts);
            this.AccountsView.Filter = new Predicate<Object>(AccountsFilter);
            this.AccountsView.CurrentChanged += new EventHandler(AccountsView_CurrentChanged);

            this.IncomesView = new ListCollectionView(DataSetModel.Instance.Accounts);
            this.IncomesView.Filter = new Predicate<object>(IncomesFilter);
            this.IncomesView.CurrentChanged += new EventHandler(IncomesView_CurrentChanged);

            this.ExpencesView = new ListCollectionView(DataSetModel.Instance.Accounts);
            this.ExpencesView.Filter = new Predicate<object>(ExpencesFilter);
            this.ExpencesView.CurrentChanged += new EventHandler(ExpencesView_CurrentChanged);

            this.EnvelopesView = new ListCollectionView(DataSetModel.Instance.Envelopes);
            this.EnvelopesView.Filter = new Predicate<Object>(EnvelopesFilter);
            this.EnvelopesView.CurrentChanged += new EventHandler(EnvelopesView_CurrentChanged);

        }

        private void reportSummaryPropertiesChanged()
        {
            this.reportPropertyChangedWithName("RegistryTitle");
            this.reportPropertyChangedWithName("EndingBalance");
            this.reportPropertyChangedWithName("ReconciledBalance");
            this.reportPropertyChangedWithName("ClearedBalance");
        }


        
        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public RegistryVM()
        {
            this.setupViews();

            this.currentAccount = (AccountDRM)this.AccountsView.CurrentItem;
            this.currentEnvelope = null;
        }

        public void switchToSelectedAccount()
        {
            this.currentAccount = (AccountDRM)this.AccountsView.CurrentItem;
            this.currentEnvelope = null;

            this.reportSummaryPropertiesChanged();
        }

        public void switchToSelectedIncome()
        {
            this.currentAccount = (AccountDRM)this.IncomesView.CurrentItem;
            this.currentEnvelope = null;

            this.reportSummaryPropertiesChanged();
        }

        public void switchToSelectedExpence()
        {
            this.currentAccount = (AccountDRM)this.ExpencesView.CurrentItem;
            this.currentEnvelope = null;

            this.reportSummaryPropertiesChanged();
        }

        public void switchToSelectedEnvelope()
        {
            this.currentEnvelope = (EnvelopeDRM)this.EnvelopesView.CurrentItem;
            this.currentAccount = null;

            this.reportSummaryPropertiesChanged();
        }

    }
}
