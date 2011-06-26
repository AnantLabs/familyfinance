using System;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class TransactionDRM : DataRowModel
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        // Local Variables
        ///////////////////////////////////////////////////////////////////////////////////////////
        protected FFDataSet.TransactionRow _transactionRow;


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////////////////////////
        public int TransactionID
        {
            get
            {
                return this._transactionRow.id;
            }
        }

        public DateTime Date
        {
            get
            {
                return this._transactionRow.date;
            }

            set
            {
                this._transactionRow.date = value;
            }
        }

        public int TypeID
        {
            get
            {
                return this._transactionRow.typeID;
            }

            set
            {
                this._transactionRow.typeID = value;
            }
        }

        public string TypeName
        {
            get
            {
                return this._transactionRow.TransactionTypeRow.name;
            }
        }

        public string Description
        {
            get
            {
                return this._transactionRow.description;
            }
            set
            {
                this._transactionRow.description = this.validLength(value, TransactionCON.DescriptionMaxLength);
            }
        }

        public string Complete
        {
            get
            {
                return this._transactionRow.complete;
            }
            set
            {
                this._transactionRow.complete = this.validLength(value, TransactionCON.CompleteMaxLength);
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Public Functions
        ///////////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Public Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        public TransactionDRM(FFDataSet.TransactionRow tRow)
        {
            this._transactionRow = tRow;
        }

        protected TransactionDRM(int transID)
        {
            this._transactionRow = MyData.getInstance().Transaction.FindByid(transID);
        }

        public TransactionDRM() : this(DateTime.Today, TransactionTypeCON.NULL.ID, "", LineCompleteCON.PENDING.Value)
        {
        }

        public TransactionDRM(DateTime date, int typeID, string description, string complete)
        {
            this._transactionRow = MyData.getInstance().Transaction.NewTransactionRow();

            this._transactionRow.id = MyData.getInstance().getNextID("Transaction");
            this.Date = date;
            this.TypeID = typeID;
            this.Description = description;
            this.Complete = complete;

            MyData.getInstance().Transaction.AddTransactionRow(this._transactionRow);
        }

    }
}
