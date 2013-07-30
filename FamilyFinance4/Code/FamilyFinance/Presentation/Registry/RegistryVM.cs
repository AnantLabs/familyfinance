using System;
using System.Windows.Data;
using FamilyFinance.Buisness;
using FamilyFinance.Buisness.Sorters;
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
                    title = this.currentAccount.CatagoryName
                            + "     " + this.currentAccount.Name
                            + "     " + this.currentAccount.TypeName;
                }
                else if (this.currentEnvelope != null)
                {
                    title = "Envelope"
                            + "     " + this.currentEnvelope.Name
                            + "     " + this.currentEnvelope.GroupName;
                }

                return title;
            }
        }

        private decimal endingBalance;
        public string EndingBalanceString
        {
            get
            {
                if (this.currentAccount == null && this.currentEnvelope == null)
                    return "";
                else
                    return "Ending Balance " + this.endingBalance.ToString("C2");
            }
        }

        private decimal clearedBalance;
        public string ClearedBalanceString
        {
            get
            {
                if (this.currentAccount == null)
                    return "";
                else
                    return "Cleared " + clearedBalance.ToString("C2");
            }
        }

        private decimal reconciledBalance;
        public string ReconciledBalanceString
        {
            get
            {
                if (this.currentAccount == null)
                    return "";
                else
                    return "Reconsiled " + this.reconciledBalance.ToString("C2");
            }
        }


        public ListCollectionView AccountsView { get; private set; }

        public ListCollectionView IncomesView { get; private set; }

        public ListCollectionView ExpencesView { get; private set; }

        public ListCollectionView EnvelopesView { get; private set; }

        public ListCollectionView RegistryLinesView { get; private set; }




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

            if (row.ID == EnvelopeCON.NULL.ID || row.ID == EnvelopeCON.SPLIT.ID)
                return false;

            if (row.Closed == true && row.EndingBalance == 0)
                return false;

            return true;
        }

        private bool RegistryFilter(object item)
        {
            RegistryLineModel row = (RegistryLineModel)item;
            bool keep = false;

            if (row.AccountID == this.currentAccount.ID)
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

            this.switchToSelectedAccount(this.AccountsView);
        }

        private void updateBalanceValues()
        {
            decimal value;
            
            this.endingBalance = Decimal.Zero;
            this.clearedBalance = Decimal.Zero;
            this.reconciledBalance = Decimal.Zero;

            if (this.RegistryLinesView == null)
                return;

            foreach (RegistryLineModel line in this.RegistryLinesView)
            {
                value = line.getAmount();
                this.endingBalance += value;
                line.RunningTotal = this.endingBalance;

                if (line.State == TransactionStateCON.RECONSILED)
                {
                    this.clearedBalance += value;
                    this.reconciledBalance += value;
                }
                else if (line.State == TransactionStateCON.CLEARED)
                {
                    this.clearedBalance += value;
                }
            }
        }

        private void reportSummaryPropertiesChanged()
        {
            this.reportPropertyChangedWithName("RegistryTitle");
            this.reportPropertyChangedWithName("EndingBalanceString");
            this.reportPropertyChangedWithName("ReconciledBalanceString");
            this.reportPropertyChangedWithName("ClearedBalanceString");
        }

        private void switchToSelectedAccount(ListCollectionView view)
        {
            this.currentAccount = (AccountDRM)view.CurrentItem;
            this.currentEnvelope = null;

            if (this.currentAccount != null)
            {
                RegistryLineModel.CurrentAccountID = this.currentAccount.ID;
                
                this.currentLineItems = new ObservableCollection<RegistryLineModel>(this.currentAccount.getTransactionLines());
                //this.currentLineItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(currentLineItems_CollectionChanged);

                this.RegistryLinesView = new ListCollectionView(this.currentLineItems);
                this.RegistryLinesView.Filter = new Predicate<Object>(RegistryFilter);
                this.RegistryLinesView.CustomSort = new RegistryLinesComparer();
            }
            else
            {

            }

           


            this.updateBalanceValues();
            this.reportSummaryPropertiesChanged();
            this.reportPropertyChangedWithName("RegistryLinesView");
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

            //this.updateBalanceValues();
            //this.reportSummaryPropertiesChanged();
        }

    }
}
