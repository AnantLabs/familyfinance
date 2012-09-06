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
        private LineItemModel currentLineItem;

        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////  
        public TransactionModel TransactionModel { get; private set; }


        public ListCollectionView TransactionTypesView { get; private set; }

        public ListCollectionView AccountsView { get; private set; }

        public ListCollectionView EnvelopesView { get; private set; }

        public ListCollectionView CreditsView { get; private set; }

        public ListCollectionView DebitsView { get; private set; }

        public ListCollectionView EnvelopeLinesView
        {
            get
            {
                ListCollectionView view = null;

                if (this.currentLineItem != null)
                {
                    int envLineCount = this.currentLineItem.EnvelopeLines.Count;
                    bool usesEnvelopes = this.currentLineItem.supportsEnvelopeLines();

                    if (envLineCount != 0 || usesEnvelopes)
                        view = new ListCollectionView(this.currentLineItem.EnvelopeLines);
                }

                return view;
            }
        }


        public bool IsCurrentLineError
        {
            get
            {
                if (this.currentLineItem == null)
                    return false;
                else
                    return this.currentLineItem.IsLineError;
            }
        }

        public decimal? CurrentEnvelopeLineSum
        {
            get
            {
                decimal? sum = null;

                if (this.currentLineItem != null)
                {
                    int envLineCount = this.currentLineItem.EnvelopeLines.Count;
                    bool usesEnvelopes = this.currentLineItem.supportsEnvelopeLines();
                    
                    if (envLineCount != 0 || usesEnvelopes)
                        sum = this.currentLineItem.EnvelopeLineSum;
                }

                return sum;
            }
        }



        ///////////////////////////////////////////////////////////
        // View Filters
        ///////////////////////////////////////////////////////////
        private bool CreditsFilter(object item)
        {
            LineItemModel lineRow = (LineItemModel)item;

            if (lineRow.Polarity == PolarityCON.CREDIT)
                return true;
            else
                return false;
        }

        private bool DebitsFilter(object item)
        {
            LineItemModel lineRow = (LineItemModel)item;

            if (lineRow.Polarity == PolarityCON.DEBIT)
                return true;
            else
                return false;
        }

        private bool AccountsFilter(object item)
        {
            AccountDRM account = (AccountDRM)item;
            bool keepItem = true;

            if (account.ID == AccountCON.MULTIPLE.ID)
                keepItem = false;

            return keepItem;
        }

        private bool EnvelopesFilter(object item)
        {
            EnvelopeDRM account = (EnvelopeDRM)item;
            bool keepItem = true;

            if (account.ID == EnvelopeCON.SPLIT.ID)
                keepItem = false;

            return keepItem;
        }



        ///////////////////////////////////////////////////////////
        // Event Functions
        ///////////////////////////////////////////////////////////
        private void CreditOrDebitView_CurrentChanged(object sender, EventArgs e)
        {
            ListCollectionView view = (ListCollectionView) sender;
            LineItemModel lineModel = (LineItemModel)view.CurrentItem;

            this.currentLineItem = lineModel;
            this.reportPropertyChangedWithName("EnvelopeLinesView");
            this.reportPropertyChangedWithName("IsCurrentLineError");
            this.reportPropertyChangedWithName("CurrentEnvelopeLineSum");
        }
       
        private void TransactionModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LineProperties")
            {
                this.reportPropertyChangedWithName("EnvelopeLinesView");
                this.reportPropertyChangedWithName("IsCurrentLineError");
                this.reportPropertyChangedWithName("CurrentEnvelopeLineSum");
            }
        }

        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private void setupViews()
        {
            this.TransactionTypesView = new ListCollectionView(DataSetModel.Instance.TransactionTypes);
            this.TransactionTypesView.CustomSort = new TransactionTypesComparer();

            this.AccountsView = new ListCollectionView(DataSetModel.Instance.Accounts);
            this.AccountsView.CustomSort = new AccountsCategoryNameComparer();
            this.AccountsView.Filter = new Predicate<Object>(AccountsFilter);

            this.EnvelopesView = new ListCollectionView(DataSetModel.Instance.Envelopes);
            this.EnvelopesView.CustomSort = new EnvelopesNameComparer();
            this.EnvelopesView.Filter = new Predicate<Object>(EnvelopesFilter);

            this.CreditsView = new ListCollectionView(this.TransactionModel.LineItems);
            this.CreditsView.Filter = new Predicate<Object>(CreditsFilter);
            this.CreditsView.CurrentChanged += new EventHandler(CreditOrDebitView_CurrentChanged);

            this.DebitsView = new ListCollectionView(this.TransactionModel.LineItems);
            this.DebitsView.Filter = new Predicate<Object>(DebitsFilter);
            this.DebitsView.CurrentChanged += new EventHandler(CreditOrDebitView_CurrentChanged);
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
            this.TransactionModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(TransactionModel_PropertyChanged);

            this.setupViews();
            this.reportAllPropertiesChanged();
        }



        public void sourceGridHasFocus()
        {
             LineItemModel.newLinesPolarity = PolarityCON.CREDIT;
        }

        public void destinationGridHasFocus()
        {
            LineItemModel.newLinesPolarity = PolarityCON.DEBIT;
        }

    }
}
