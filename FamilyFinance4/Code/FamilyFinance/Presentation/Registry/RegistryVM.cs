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
        private ObservableCollection<RegistryLineModel> currentLineItems;


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
                    return "";
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

        public ListCollectionView RegistryLinesView
        {
            get
            {
                if (this.currentAccount == null)
                    return null;
                else
                {
                    this.currentLineItems = new ObservableCollection<RegistryLineModel>(this.currentAccount.getTransactionLines());
                    return new ListCollectionView(this.currentLineItems);
                }
            }
        }


        ///////////////////////////////////////////////////////////
        // View Filters
        ///////////////////////////////////////////////////////////
        private bool AccountsFilter(object item)
        {
            AccountDRM row = (AccountDRM)item;
            bool keep = false;

            if (row.Catagory == CatagoryCON.ACCOUNT)
            {
                if (row.Closed == false || row.getEndingBalance() != 0)
                    keep = true;
            }

            return keep;
        }
        
        private bool IncomesFilter(object item)
        {
            AccountDRM row = (AccountDRM)item;
            bool keep = false;

            if (row.Catagory == CatagoryCON.INCOME && row.Closed == false)
                keep = true;

            return keep;
        }

        private bool ExpencesFilter(object item)
        {
            AccountDRM row = (AccountDRM)item;
            bool keep = false;

            if (row.Catagory == CatagoryCON.EXPENSE && row.Closed == false)
                keep = true;

            return keep;
        }

        private bool EnvelopesFilter(object item)
        {
            EnvelopeDRM row = (EnvelopeDRM)item;
            bool keep = false;

            if (row.Closed == false || row.EndingBalance != 0)
                    keep = true;

            return keep;
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
            this.switchToSelectedEnvelope();
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
            this.reportPropertyChangedWithName("RegistryLinesView");
            this.reportPropertyChangedWithName("RegistryTitle");
            this.reportPropertyChangedWithName("EndingBalance");
            this.reportPropertyChangedWithName("ReconciledBalance");
            this.reportPropertyChangedWithName("ClearedBalance");
        }

        private void switchToSelectedAccount(ListCollectionView view)
        {
            this.currentAccount = (AccountDRM)view.CurrentItem;
            this.currentEnvelope = null;
           
            if(this.currentAccount != null)
                RegistryLineModel.CurrentAccountID = this.currentAccount.ID;

            this.reportSummaryPropertiesChanged();
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
            this.switchToSelectedAccount(this.AccountsView);
        }

        public void switchToSelectedIncome()
        {
            this.switchToSelectedAccount(this.IncomesView);
        }

        public void switchToSelectedExpence()
        {
            this.switchToSelectedAccount(this.ExpencesView);
        }

        public void switchToSelectedEnvelope()
        {
            this.currentEnvelope = (EnvelopeDRM)this.EnvelopesView.CurrentItem;
            this.currentAccount = null;

            this.reportSummaryPropertiesChanged();
        }

    }
}
