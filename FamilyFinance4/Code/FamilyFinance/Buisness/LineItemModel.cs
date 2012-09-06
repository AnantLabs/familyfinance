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

                this.reportPropertyChangedWithName("IsLineError");
                this.reportPropertyChangedWithName("EnvelopeLineSum");
            }
        }

        public static PolarityCON newLinesPolarity { get; set; }


        public bool IsLineError
        {
            get
            {
                bool result = true;

                if (this.isLineNull())
                    result = false;

                else
                {
                    int envLineCount = this.EnvelopeLines.Count;
                    decimal envLineSum = this.EnvelopeLineSum;
                    bool usesEnvelopes = this.supportsEnvelopeLines();

                    if (usesEnvelopes && envLineCount > 0 && this.Amount == envLineSum)
                        result = false;

                    if (!usesEnvelopes && envLineCount == 0)
                        result = false;
                }

                return result;
            }
        }

        public decimal EnvelopeLineSum
        {
            get
            {
                decimal sum = 0;

                foreach (EnvelopeLineDRM envLine in this.EnvelopeLines)
                    sum += envLine.Amount;

                return sum;
            }
        }
        
        
        ///////////////////////////////////////////////////////////
        // Events
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


        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private void deleteEnvelopeLines(System.Collections.IList iList)
        {
            foreach (EnvelopeLineDRM oldELine in iList)
            {
                oldELine.deleteRowFromDataset();
                oldELine.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(EnvelopeLineLine_PropertyChanged);
            }

            this.reportPropertyChangedWithName("IsLineError");
            this.reportPropertyChangedWithName("EnvelopeLineSum");
        }

        private void pointNewEnvelopeLinesToThisLineItem(System.Collections.IList iList)
        {
            foreach (EnvelopeLineDRM newEnvLine in iList)
            {
                newEnvLine.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(EnvelopeLineLine_PropertyChanged);

                if (newEnvLine.LineItemID != this.LineID)
                {
                    newEnvLine.setParentLine(this);
                    newEnvLine.Amount = suggestedEnvelopeAmount();
                }
            }
            
            this.reportPropertyChangedWithName("IsLineError");
            this.reportPropertyChangedWithName("EnvelopeLineSum");
        }

        private decimal suggestedEnvelopeAmount()
        {
            return this.Amount - this.EnvelopeLineSum;
        }



        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public LineItemModel() : base()
        {
            if (newLinesPolarity == null)
                newLinesPolarity = PolarityCON.CREDIT; // good default
                
            this.Polarity = newLinesPolarity;
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

        public bool hasEnvelopeLines()
        {
            if(this.EnvelopeLines.Count > 0)
                return true;
            else 
                return false;
        }


    }
}
