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
    public class EditTransactionVM : TransactionDRM
    {

        ///////////////////////////////////////////////////////////
        // Private variables
        ///////////////////////////////////////////////////////////
        private ObservableCollection<LineItemDRM> _lines;


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
                    this._CreditsView = new ListCollectionView(this._lines);
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
                    this._DebitsView = new ListCollectionView(this._lines);
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
                    this._TransactionTypesView = new ListCollectionView(DataSetModel.getInstance().TransactionTypes);
                    this._TransactionTypesView.Filter = new Predicate<Object>(DebitsFilter);
                }

                return this._TransactionTypesView;
            }
        }

        private int _currentLineID;
        private int CurrentLineID
        {
            get
            {
                return this._currentLineID;
            }
            set
            {
                if (this._currentLineID != value)
                {
                    this._currentLineID = value;
                }
            }
        }

        public ListCollectionView EnvelopeLinesView
        {
            get
            {
                ObservableCollection<EnvelopeLineDRM> envColl = new ObservableCollection<EnvelopeLineDRM>();
                FFDataSet.LineItemRow currentLine = MyData.getInstance().LineItem.FindByid(CurrentLineID);

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

        public decimal CreditsSum
        {
            get
            {
                decimal sum = 0.0m;

                foreach (LineItemDRM line in this._lines)
                {
                    if (line.Polarity != PolarityCON.CREDIT)
                    {
                        sum += line.Amount;
                    }
                }

                return sum;
            }
        }

        public decimal DebitsSum
        {
            get
            {
                decimal sum = 0.0m;

                foreach (LineItemDRM line in this._lines)
                {
                    if (line.Polarity != PolarityCON.DEBIT)
                    {
                        sum += line.Amount;
                    }
                }

                return sum;
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
        public EditTransactionVM(int transID) : base(transID)
        {
            this._currentLineID = 0;
            this._lines = new ObservableCollection<LineItemDRM>();

            if (this._transactionRow != null)
            {
                FFDataSet.LineItemRow[] rows = this._transactionRow.GetLineItemRows();

                foreach (FFDataSet.LineItemRow line in rows)
                {
                    this._lines.Add(new LineItemDRM(line));
                }
            }
        }

        public EditTransactionVM()
        {
            this._currentLineID = 0;
            this._lines = new ObservableCollection<LineItemDRM>();
        }
    }
}
