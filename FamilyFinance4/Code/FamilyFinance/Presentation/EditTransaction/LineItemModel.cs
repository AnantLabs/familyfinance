using System.Collections.ObjectModel;
using FamilyFinance.Buisness;
using FamilyFinance.Data;
using System.Collections.Specialized;

namespace FamilyFinance.Presentation.EditTransaction
{
    public class LineItemModel : LineItemDRM
    {
        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        public ObservableCollection<EnvelopeLineDRM> EnvelopeLines { get; private set; }


        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private void newEmptyEnvelopeLineCollection()
        {
            this.EnvelopeLines = new ObservableCollection<EnvelopeLineDRM>();
        }

        private void fillEnvelopeLineCollection(FFDataSet.EnvelopeLineRow[] envLines)
        {
            foreach (FFDataSet.EnvelopeLineRow envLine in envLines)
                this.EnvelopeLines.Add(new EnvelopeLineDRM(envLine, this));
        }


        private void listenToCollectionChanges()
        {
            this.EnvelopeLines.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(EnvelopeLines_CollectionChanged);
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
                oldELine.delete();
        }

        private void pointNewEnvelopeLinesToThisLineItem(System.Collections.IList iList)
        {
            foreach (EnvelopeLineDRM newEnvLine in iList)
                newEnvLine.setParentLine(this);
        }




        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public LineItemModel() : base()
        {
            newEmptyEnvelopeLineCollection();
            this.listenToCollectionChanges();
        }

        public LineItemModel(FFDataSet.LineItemRow lRow, TransactionDRM parentTransaction) : base(lRow, parentTransaction)
        {
            newEmptyEnvelopeLineCollection();
            fillEnvelopeLineCollection(this.getEnvelopeLineRows());
            listenToCollectionChanges();
        }

        


    }
}
