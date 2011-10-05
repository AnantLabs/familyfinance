using System;
using System.Windows.Data;
using System.Collections.ObjectModel;

using FamilyFinance.Buisness;
using FamilyFinance.Data;
using System.Reflection;

namespace FamilyFinance.Presentation.EditTransaction
{
    public class EditTransactionVM : ViewModel
    {
        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        private TransactionModel transactionModel;
        public TransactionModel TransactionModel
        {
            get
            {
                return this.transactionModel;
            }
        }

        private ListCollectionView _CreditsView;
        public ListCollectionView CreditsView
        {
            get
            {
                return this._CreditsView;
            }
        }

        private ListCollectionView _DebitsView;
        public ListCollectionView DebitsView
        {
            get
            {
                return this._DebitsView;
            }
        }

        private ListCollectionView _TransactionTypesView;
        public ListCollectionView TransactionTypesView
        {
            get
            {
                return this._TransactionTypesView;
            }
        }


        public decimal EnvelopeLineSum
        {
            get
            {
                return 0;
            }
        }


        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private bool CreditsFilter(object item)
        {
            LineItemDRM lineRow = (LineItemDRM)item;
            bool keepItem = false;

            if (lineRow.Polarity == PolarityCON.CREDIT)
                keepItem = true;

            return keepItem;
        }

        private bool DebitsFilter(object item)
        {
            LineItemDRM lineRow = (LineItemDRM)item;
            bool keepItem = false; 

            if (lineRow.Polarity == PolarityCON.DEBIT)
                keepItem = true;

            return keepItem;
        }

        private void setupViews()
        {
            this._CreditsView = new ListCollectionView(this.transactionModel.LineItems);
            this._CreditsView.Filter = new Predicate<Object>(CreditsFilter);

            this._DebitsView = new ListCollectionView(this.transactionModel.LineItems);
            this._DebitsView.Filter = new Predicate<Object>(DebitsFilter);

            this._TransactionTypesView = new ListCollectionView(DataSetModel.Instance.TransactionTypes);
            //this._TransactionTypesView.Filter = new Predicate<Object>(DebitsFilter);
        }

        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public EditTransactionVM()
        {

        }

        public void loadTransaction(int transID)
        {
            this.transactionModel = new TransactionModel(transID);
            this.setupViews();
            this.reportAllPropertiesChanged();
        }


    }
}
