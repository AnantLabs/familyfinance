using System.Collections.ObjectModel;
using System.Collections.Specialized;
using FamilyFinance.Data;



namespace FamilyFinance.Buisness
{
    public class TransactionModel : TransactionDRM
    {
        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        private ObservableCollection<LineItemDRM> lineItems;
        public ObservableCollection<LineItemDRM> LineItems
        {
            get
            {
                return lineItems;
            }
        }


        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private void newEmptyLineItemCollection()
        {
            this.lineItems = new ObservableCollection<LineItemDRM>();
        }

        private void listenForChangesToTheCollection()
        {
            this.lineItems.CollectionChanged += new NotifyCollectionChangedEventHandler(lineItems_CollectionChanged);
        }

        private void lineItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                setTheLinesTransaction(e.NewItems);
        }

        private void setTheLinesTransaction(System.Collections.IList iList)
        {
            foreach (LineItemDRM line in iList)
            {
                line.setParentTransaction(this);
            }
        }

        private void fillLineItemCollection(LineItemDRM[] lines)
        {
            foreach (LineItemDRM line in lines)
                this.lineItems.Add(line);

        }


        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public TransactionModel() : base()
        {
            newEmptyLineItemCollection();
            listenForChangesToTheCollection();
        }

        public TransactionModel(FFDataSet.TransactionRow tRow) : base(tRow)
        {
            newEmptyLineItemCollection();
            fillLineItemCollection(this.getWrappedTransactionLines());
            listenForChangesToTheCollection();
        }

        public TransactionModel(int transactionID) : base(transactionID)
        {
            newEmptyLineItemCollection();
            fillLineItemCollection(this.getWrappedTransactionLines());
            listenForChangesToTheCollection();
        }


    }
}
