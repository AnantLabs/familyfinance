using System;
using System.Windows.Data;
using System.Collections;
using System.Collections.ObjectModel;

using FamilyFinance.Buisness;
using FamilyFinance.Buisness.Sorters;
using FamilyFinance.Data;

namespace FamilyFinance.Presentation.EditTransaction
{
    public class EditTransactionVM : ViewModel
    {
        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        public TransactionModel TransactionModel { get; private set; }

        public ListCollectionView TransactionTypesView { get; private set; }

        public ListCollectionView CreditsView { get; private set; }

        public ListCollectionView DebitsView { get; private set; }

        private ListCollectionView _EnvelopeLinesView;
        public ListCollectionView EnvelopeLinesView 
        {
            get
            {
                return this._EnvelopeLinesView;
            }

            private set
            {
                this._EnvelopeLinesView = value;
                this.reportPropertyChangedWithName("EnvelopeLinesView");
            }
        } 

        public decimal EnvelopeLineSum
        {
            get
            {
                return 0;
            }
        }


        public ListCollectionView AccountsView 
        {
            get
            {
                return getANewSortedViewOfAccounts();
            }
        }

        public ListCollectionView EnvelopesView
        {
            get
            {
                return new ListCollectionView(DataSetModel.Instance.Envelopes);
            }
        }


        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private void setupViews()
        {            
            this.TransactionTypesView = new ListCollectionView(DataSetModel.Instance.TransactionTypes);
            this.TransactionTypesView.CustomSort = new TransactionTypesComparer();

            this.CreditsView = new ListCollectionView(this.TransactionModel.LineItems);
            this.CreditsView.Filter = new Predicate<Object>(CreditsFilter);
            this.CreditsView.CurrentChanged += new EventHandler(CreditOrDebitView_CurrentChanged);

            this.DebitsView = new ListCollectionView(this.TransactionModel.LineItems);
            this.DebitsView.Filter = new Predicate<Object>(DebitsFilter);
            this.DebitsView.CurrentChanged += new EventHandler(CreditOrDebitView_CurrentChanged);
            
        }

        private bool CreditsFilter(object item)
        {
            LineItemModel lineRow = (LineItemModel)item;
            bool keepItem = false;

            if (lineRow.Polarity == PolarityCON.CREDIT)
                keepItem = true;

            return keepItem;
        }

        private bool DebitsFilter(object item)
        {
            LineItemModel lineRow = (LineItemModel)item;
            bool keepItem = false; 

            if (lineRow.Polarity == PolarityCON.DEBIT)
                keepItem = true;

            return keepItem;
        }

        private void CreditOrDebitView_CurrentChanged(object sender, EventArgs e)
        {
            ListCollectionView view = (ListCollectionView) sender;

            if (view.IsAddingNew)
            {
                LineItemModel newLine = (LineItemModel)view.CurrentAddItem;

                newLine.Amount = suggestedAmountDependingOnView(view);
                newLine.Polarity = determinePolarityDependingOnView(view);

                removeGhostLine(view);
            }

            LineItemModel currentLine = (LineItemModel)view.CurrentItem;

            if (currentLine != null)
            {
                if (currentLine.supportsEnvelopeLines())
                {
                    this.EnvelopeLinesView = new ListCollectionView(currentLine.EnvelopeLines);
                }
                else
                {
                    this.EnvelopeLinesView = null;
                }
            }
            else
            {
                this.EnvelopeLinesView = null;
            }

        }

        private decimal suggestedAmountDependingOnView(ListCollectionView view)
        {
            decimal suggestedAmount;

            if(view == DebitsView)
                suggestedAmount = TransactionModel.CreditSum - TransactionModel.DebitSum;
            else
                suggestedAmount = TransactionModel.DebitSum - TransactionModel.CreditSum;

            if (suggestedAmount < 0)
                suggestedAmount = 0;

            return suggestedAmount;
        }

        private PolarityCON determinePolarityDependingOnView(ListCollectionView view)
        {
            if (view == DebitsView)
                return PolarityCON.DEBIT;
            else
                return PolarityCON.CREDIT;
        }

        private void removeGhostLine(ListCollectionView view)
        {
            // When adding a new line the credit or debit filter might be applied
            // too soon and a ghost copy of the line might appear in the opposite
            // view or datagrid. So when an item is added to the view and after 
            // the polarity is set refresh the opposite view to remove the ghost 
            // line.
            if (view == DebitsView)
                this.CreditsView.Refresh();
            else
                this.DebitsView.Refresh();
        }


        private ListCollectionView getANewSortedViewOfAccounts()
        {

            ListCollectionView listView = new ListCollectionView(DataSetModel.Instance.Accounts);

            listView.CustomSort = new AccountsCategoryNameComparer();
            listView.Filter = new Predicate<Object>(AccountsFilter);

            return listView;
        }

        private bool AccountsFilter(object item)
        {
            AccountDRM account = (AccountDRM)item;
            bool keepItem = true;

            if (account.ID == AccountCON.MULTIPLE.ID)
                keepItem = false;

            return keepItem;
        }


        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public EditTransactionVM()
        {

        }

        public void loadTransaction(int transID)
        {
            this.TransactionModel = new TransactionModel(transID);
            this.setupViews();

            this.reportAllPropertiesChanged();
        }


    }
}
