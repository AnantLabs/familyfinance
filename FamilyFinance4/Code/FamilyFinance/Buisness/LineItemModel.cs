using System.Collections.ObjectModel;
using FamilyFinance.Data;
using System.Collections.Specialized;

namespace FamilyFinance.Buisness
{
    public class LineItemModel : LineItemDRM
    {

        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        public ObservableCollection<EnvelopeLineDRM> EnvelopeLines { get; private set; }

        public override int AccountID
        {
            get
            {
                return base.AccountID;
            }
            set
            {
                if (DataSetModel.Instance.doesAccountUseEnvelopes(value) == false)
                    this.EnvelopeLines.Clear();

                base.AccountID = value;
            }
        }

        public bool IsLineError
        {
            get
            {
                decimal envLineSum = (decimal)this.EnvelopeLineSum;
                bool accountUsesEnvelopes = this.supportsEnvelopeLines();

                if (accountUsesEnvelopes && this.Amount == envLineSum)
                    return false;

                else if (!accountUsesEnvelopes && envLineSum == 0)
                    return false;

                else
                    return true;
            }
        }

        public decimal? EnvelopeLineSum
        {
            get
            {
                if (this.EnvelopeLines == null)
                    return null;

                decimal sum = 0;

                foreach (EnvelopeLineDRM envLine in this.EnvelopeLines)
                    sum += envLine.Amount;

                return sum;
            }
        }



        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private void EnvelopeLineLine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Amount")
            {
                reportPropertyChangedWithName("EnvelopeLineSum");
                reportPropertyChangedWithName("IsLineError");
            }
        }

        private void EnvelopeLines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                pointNewEnvelopeLinesToThisLineItem(e.NewItems);

            else if (e.Action == NotifyCollectionChangedAction.Remove)
                deleteEnvelopeLines(e.OldItems);
        }

        private void deleteEnvelopeLines(System.Collections.IList iList)
        {
            foreach (EnvelopeLineDRM oldELine in iList)
            {
                oldELine.deleteRowFromDataset();
                oldELine.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(EnvelopeLineLine_PropertyChanged);
            }
        }

        private void pointNewEnvelopeLinesToThisLineItem(System.Collections.IList iList)
        {
            foreach (EnvelopeLineDRM newEnvLine in iList)
            {
                newEnvLine.setParentLine(this);
                newEnvLine.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(EnvelopeLineLine_PropertyChanged);
            }
        }


        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public LineItemModel() : base()
        {
            this.EnvelopeLines = new ObservableCollection<EnvelopeLineDRM>();
            this.EnvelopeLines.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(EnvelopeLines_CollectionChanged);
        }

        public LineItemModel(FFDataSet.LineItemRow lRow) : base(lRow)
        {
            this.EnvelopeLines = new ObservableCollection<EnvelopeLineDRM>();
            this.EnvelopeLines.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(EnvelopeLines_CollectionChanged);

            // add the line this way so the collection(add/Remove) event can handle
            // subscribing and unsubscribing
            foreach (EnvelopeLineDRM envLine in this.getEnvelopeLineRows())
                this.EnvelopeLines.Add(envLine);
        }

        public void deleteLineAndEnvelopeLinesFromDataset()
        {
            this.EnvelopeLines.Clear();
            this.deleteRowFromDataset();
        }


    }
}
