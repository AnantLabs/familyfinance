using System.Collections.ObjectModel;
using System.Collections.Specialized;

using FamilyFinance.Data;
using FamilyFinance.Buisness;



namespace FamilyFinance.Presentation.EditTransaction
{
    public class TransactionModel : TransactionDRM
    {
        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        public ObservableCollection<LineItemModel> LineItems { get; private set; }


        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private void newEmptyLineItemCollection()
        {
            this.LineItems = new ObservableCollection<LineItemModel>();
        }

        private void fillLineItemCollection(FFDataSet.LineItemRow[] lines)
        {
            foreach (FFDataSet.LineItemRow line in lines)
                this.LineItems.Add(new LineItemModel(line));
        }


        private void listenToCollectionChanges()
        {
            this.LineItems.CollectionChanged += new NotifyCollectionChangedEventHandler(LineItems_CollectionChanged);
        }

        private void LineItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                pointNewLinesToThisTransaction(e.NewItems);

            else if (e.Action == NotifyCollectionChangedAction.Remove)
                deleteLineItems(e.OldItems);
        }

        private void deleteLineItems(System.Collections.IList iList)
        {
            foreach (LineItemModel oldLine in iList)
                oldLine.Delete();
        }

        private void pointNewLinesToThisTransaction(System.Collections.IList iList)
        {
            foreach(LineItemModel newLine in iList)
                newLine.setParentTransaction(this);
        }



        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public TransactionModel() : base()
        {
            newEmptyLineItemCollection();
            listenToCollectionChanges();
        }

        public TransactionModel(FFDataSet.TransactionRow tRow) : base(tRow)
        {
            newEmptyLineItemCollection();
            fillLineItemCollection(this.getLineItemRows());
            listenToCollectionChanges();
        }

        public TransactionModel(int transactionID) : base(transactionID)
        {
            newEmptyLineItemCollection();
            fillLineItemCollection(this.getLineItemRows());
            listenToCollectionChanges();
        }

    }
}
