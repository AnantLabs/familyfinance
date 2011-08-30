using System;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    /// <summary>
    /// 
    /// </summary>
    public class TransactionDRM : DataRowModel
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        // Local Variables
        ///////////////////////////////////////////////////////////////////////////////////////////
        private FFDataSet.TransactionRow _transactionRow;


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
                this._transactionRow.description = this.truncateIfNeeded(value, TransactionCON.DescriptionMaxLength);
            }
        }

        public TransactionStateCON State
        {
            get
            {
                return TransactionStateCON.GetState(this._transactionRow.state);
            }
            set
            {
                this._transactionRow.state = value.Value;
            }
        }

        public bool IsTransactionError
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }


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

        public TransactionDRM() : this(DateTime.Today, TransactionTypeCON.NULL.ID, "", TransactionStateCON.PENDING)
        {
        }

        /// <summary>
        /// Creates a new transaction data row with the given values
        /// </summary>
        public TransactionDRM(DateTime date, int typeID, string description, TransactionStateCON state)
        {
            this._transactionRow = MyData.getInstance().Transaction.NewTransactionRow();

            this._transactionRow.id = MyData.getInstance().getNextID("Transaction");
            this.Date = date;
            this.TypeID = typeID;
            this.Description = description;
            this.State = state;

            MyData.getInstance().Transaction.AddTransactionRow(this._transactionRow);
        }

    }
}
