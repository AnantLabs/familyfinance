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
        public ObservableCollection<LineItemModel> LineItems { get; private set; }

        public bool IsTransactionError
        {
            get
            {
                if (isOneSidedTransaction() || isTransactionBalanced())
                    return false;

                else
                    return true;
            }
        }

        public decimal CreditSum
        {
            get
            {
                decimal creditSum = 0;

                foreach (LineItemModel line in this.LineItems)
                    if (line.Polarity == PolarityCON.CREDIT)
                        creditSum += line.Amount;

                return creditSum;
            }
        }

        public decimal DebitSum
        {
            get
            {
                decimal debitSum = 0;

                foreach (LineItemModel line in this.LineItems)
                    if (line.Polarity == PolarityCON.DEBIT)
                        debitSum += line.Amount;

                return debitSum;
            }
        }



        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private void LineItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Amount" || e.PropertyName == "Polarity")
            {
                reportPropertyChangedWithName("CreditSum");
                reportPropertyChangedWithName("DebitSum");
                reportPropertyChangedWithName("IsTransactionError");
            }
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
            {
                oldLine.deleteLineAndEnvelopeLinesFromDataset();
                oldLine.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(LineItem_PropertyChanged);
            }
        }

        private void pointNewLinesToThisTransaction(System.Collections.IList iList)
        {
            foreach (LineItemModel newLine in iList)
            {
                newLine.setParentTransaction(this);
                newLine.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(LineItem_PropertyChanged);
            }
        }

        private bool isOneSidedTransaction()
        {
            int size = this.LineItems.Count;

            if (size == 1)
                return true;
            else
                return false;
        }

        private bool isTransactionBalanced()
        {
            decimal creditSum = 0;
            decimal debitSum = 0;

            foreach (LineItemDRM line in this.LineItems)
            {
                if (line.Polarity == PolarityCON.CREDIT)
                    creditSum += line.Amount;
                else
                    debitSum += line.Amount;
            }

            if (creditSum == debitSum)
                return true;
            else
                return false;
        }




        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public TransactionModel() : base()
        {
            this.LineItems = new ObservableCollection<LineItemModel>();
            this.LineItems.CollectionChanged += new NotifyCollectionChangedEventHandler(LineItems_CollectionChanged);
        }

        public TransactionModel(FFDataSet.TransactionRow tRow) : base(tRow)
        {
            this.LineItems = new ObservableCollection<LineItemModel>(this.getLineItemModels());
            this.LineItems.CollectionChanged += new NotifyCollectionChangedEventHandler(LineItems_CollectionChanged);

            foreach (LineItemModel newLine in this.getLineItemModels())
                this.LineItems.Add(newLine);
        }

        public TransactionModel(int transactionID) : base(transactionID)
        {
            this.LineItems = new ObservableCollection<LineItemModel>();
            this.LineItems.CollectionChanged += new NotifyCollectionChangedEventHandler(LineItems_CollectionChanged);

            foreach (LineItemModel newLine in this.getLineItemModels())
                this.LineItems.Add(newLine);
        }


        public void deleteTransactionLinesAndEnvelopeLinesFromDataset() 
        {
            this.LineItems.Clear();
            this.deleteRowFromDataset();
        }




    }
}
