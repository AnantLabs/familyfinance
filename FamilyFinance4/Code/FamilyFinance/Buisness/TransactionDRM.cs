using FamilyFinance.Data;
using System;

namespace FamilyFinance.Buisness
{
    public class TransactionDRM : DataRowModel
    {
        private FFDataSet.TransactionRow transactionRow;


        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        public int TransactionID
        {
            get
            {
                return transactionRow.id;
            }
        }

        public DateTime Date
        {
            get
            {
                return this.transactionRow.date;
            }

            set
            {
                if (this.transactionRow.date != value)
                {
                    this.transactionRow.date = value;
                    this.reportPropertyChangedWithName("Date");
                }
            }
        }

        public int TypeID
        {
            get
            {
                return this.transactionRow.typeID;
            }

            set
            {
                if (this.transactionRow.typeID != value)
                {
                    this.transactionRow.typeID = value;
                    this.reportPropertyChangedWithName("TypeID");
                    this.reportPropertyChangedWithName("TypeName");
                }
            }
        }

        public string TypeName
        {
            get
            {
                return this.transactionRow.TransactionTypeRow.name;
            }
        }

        public string Description
        {
            get
            {
                return this.transactionRow.description;
            }
            set
            {
                if (this.transactionRow.description != value)
                {
                    this.transactionRow.description = value;
                    this.reportPropertyChangedWithName("Description");
                }
            }
        }



        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public TransactionDRM()
        {
            this.transactionRow = DataSetModel.Instance.NewTransactionRow();
        }

        public TransactionDRM(FFDataSet.TransactionRow tRow)
        {
            this.transactionRow = tRow;
        }

        public TransactionDRM(int transactionID)
        {
            this.transactionRow = DataSetModel.Instance.getTransactionRowWithID(transactionID);
        }



        public LineItemModel[] getLineItemModels()
        {
            FFDataSet.LineItemRow[] rawLines = this.transactionRow.GetLineItemRows();
            LineItemModel[] modelLines = new LineItemModel[rawLines.Length];

            for (int i = 0; i < rawLines.Length; i++)
                modelLines[i] = new LineItemModel(rawLines[i]);

            return modelLines;
        }

        public void deleteRowFromDataset()
        {
            this.transactionRow.Delete();
        }

    }
}
