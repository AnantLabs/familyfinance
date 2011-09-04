using System;
using System.Windows.Data;
using System.Collections.ObjectModel;

using FamilyFinance.Buisness;
using FamilyFinance.Data;

namespace FamilyFinance.Presentation.EditTransaction
{
    /// <summary>
    /// This class inherits from the TransactionDRM instead of strictly from the ViewModel class
    /// because this "is a" transaction we are editing, it comes from the Bindable object which is the important part.
    /// </summary>
    public class EditTransactionVM : ViewModel
    {

        ///////////////////////////////////////////////////////////
        // Private variables
        ///////////////////////////////////////////////////////////
        private TransactionModel transactionModel;


        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        private ListCollectionView _CreditsView;
        public ListCollectionView CreditsView
        {
            get
            {
                if (this._CreditsView == null)
                {
                    this._CreditsView = new ListCollectionView(this.transactionModel.LineItems);
                    this._CreditsView.Filter = new Predicate<Object>(CreditsFilter);
                }

                return this._CreditsView;
            }
        }

        private ListCollectionView _DebitsView;
        public ListCollectionView DebitsView
        {
            get
            {
                if (this._DebitsView == null)
                {
                    this._DebitsView = new ListCollectionView(this.transactionModel.LineItems);
                    this._DebitsView.Filter = new Predicate<Object>(DebitsFilter);
                }

                return this._DebitsView;
            }
        }

        private ListCollectionView _TransactionTypesView;
        public ListCollectionView TransactionTypesView
        {
            get
            {
                if (this._TransactionTypesView == null)
                {
                    this._TransactionTypesView = new ListCollectionView(DataSetModel.Instance.TransactionTypes);
                    //this._TransactionTypesView.Filter = new Predicate<Object>(DebitsFilter);
                }

                return this._TransactionTypesView;
            }
        }

        public ListCollectionView EnvelopeLinesView
        {
            get
            {
                ObservableCollection<EnvelopeLineDRM> envColl = new ObservableCollection<EnvelopeLineDRM>();
                FFDataSet.LineItemRow currentLine = MyData.getInstance().LineItem.FindByid(2);

                if (currentLine != null)
                {
                    FFDataSet.EnvelopeLineRow[] rows = currentLine.GetEnvelopeLineRows();

                    foreach (FFDataSet.EnvelopeLineRow envLine in rows)
                    {
                        //envColl.Add(new EnvelopeLineDRM(envLine));
                    }
                }

                ListCollectionView envView = new ListCollectionView(envColl);
                envView.SortDescriptions.Add(new System.ComponentModel.SortDescription("EnvelopeName", System.ComponentModel.ListSortDirection.Ascending));

                return envView;
            }
        }


        public decimal EnvelopeLineSum
        {
            get
            {
                decimal sum = 0;

                //foreach (EnvelopeLineDRM envLine in _envelopeLines)
                //{
                //    sum += envLine.Amount;
                //}

                return sum;
            }
        }


        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private bool CreditsFilter(object item)
        {
            LineItemDRM lineRow = (LineItemDRM)item;
            bool keepItem = false; // Assume the item will NOT be shown in the list

            // Keep the item if it is a credit
            if (lineRow.Polarity == PolarityCON.CREDIT)
                keepItem = true;

            return keepItem;
        }

        private bool DebitsFilter(object item)
        {
            LineItemDRM lineRow = (LineItemDRM)item;
            bool keepItem = false; // Assume the item will NOT be shown in the list

            // Keep the item if it is a debit
            if (lineRow.Polarity == PolarityCON.DEBIT)
                keepItem = true;

            return keepItem;
        }


        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public EditTransactionVM(int transID) 
        {


        }

        public EditTransactionVM()
        {
            this.transactionModel = new TransactionModel();
        }
    }
}
