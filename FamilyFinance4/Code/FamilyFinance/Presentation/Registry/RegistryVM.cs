using System;
using System.Windows.Data;
using FamilyFinance.Buisness;
using FamilyFinance.Data;

namespace FamilyFinance.Presentation.Registry
{
    class RegistryVM : ViewModel
    {
        //private LineItemModel currentLine;
        //private EditTransactionWindow parentWindow;


        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        public ListCollectionView AccountsView { get; private set; }

        public ListCollectionView IncomesView { get; private set; }

        public ListCollectionView ExpencesView { get; private set; }

        public ListCollectionView EnvelopesView { get; private set; }


        private ListCollectionView _RegistryLinesView;
        public ListCollectionView RegistryLinesView
        {
            get
            {
                return this._RegistryLinesView;
            }

            private set
            {
                this._RegistryLinesView = value;
                this.reportPropertyChangedWithName("RegistryLinesView");
                this.reportPropertyChangedWithName("EndingBalance");
            }
        }

        public decimal EndingBalance
        {
            get
            {
                //if (this.currentLine == null)
                //    return 0;
                //else
                //    return this.currentLine.EnvelopeLineSum;
                return 0;
            }
        }


        ///////////////////////////////////////////////////////////
        // View Filters
        ///////////////////////////////////////////////////////////
        private bool AccountsFilter(object item)
        {
            RegistryAccountModel accountRow = (RegistryAccountModel)item;
            bool keepItem = false;

            if (accountRow.Catagory == CatagoryCON.ACCOUNT)
                keepItem = true;

            return keepItem;
        }


        ///////////////////////////////////////////////////////////
        // Event Functions
        ///////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private void setupViews()
        {
            this.AccountsView = new ListCollectionView(DataSetModel.Instance.Accounts);
            //this.AccountsView.Filter = new Predicate<Object>(AccountsFilter);
            //this.CreditsView.CurrentChanged += new EventHandler(CreditOrDebitView_CurrentChanged);

        }

        
        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public RegistryVM()
        {

        }

        public void loadData()
        {
            //this.TransactionModel = new TransactionModel(transID);

            this.setupViews();

            this.reportAllPropertiesChanged();
        }

        public void setParentWindow(RegistryWindow registryWindow)
        {
            //this.parentWindow = editWindow;
        }

        public void reportDependantEndingBalancesChanged()
        {
            //this.reportPropertyChangedWithName("EnvelopeLineSum");
        }

    }
}
