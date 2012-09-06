using System;
using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class TransactionLine
    {
        public TransactionModel Transaction { get; private set; }
        public LineItemModel LineItem { get; private set; }

        public TransactionLine(FFDataSet.LineItemRow lineRow)
        {
            if (lineRow == null)
                throw new Exception("Null Line Item Row.");

            FFDataSet.TransactionRow tRow;
            tRow = DataSetModel.Instance.getTransactionRowWithID(lineRow.transactionID);

            if (tRow == null)
                throw new Exception("Orphan Line Item Row.");

            this.Transaction = new TransactionModel(tRow);

            foreach (LineItemModel line in Transaction.LineItems)
                if (line.LineID == lineRow.id)
                    this.LineItem = line;

            if (this.LineItem == null)
                throw new Exception("No line with matching ID.");

        }
    }
}
