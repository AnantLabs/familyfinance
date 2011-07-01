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
        /// <summary>
        /// Gets the id for the transaction.
        /// </summary>
        public int TransactionID
        {
            get
            {
                return this._transactionRow.id;
            }
        }

        /// <summary>
        /// gets or sets the date for the transaction.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the transaction type id.
        /// </summary>
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

        /// <summary>
        /// Gets the name of the Transaction Type.
        /// </summary>
        public string TypeName
        {
            get
            {
                return this._transactionRow.TransactionTypeRow.name;
            }
        }

        /// <summary>
        /// gets or sets the transaction description.
        /// </summary>
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

        /// <summary>
        /// gets or sets the complete status of the transaction. See CompleteCON for values.
        /// </summary>
        public string CompleteCode
        {
            get
            {
                return this._transactionRow.complete;
            }
            set
            {
                this._transactionRow.complete = this.truncateIfNeeded(value, TransactionCON.CompleteMaxLength);
            }
        }

        /// <summary>
        /// Determins if the transaction has an error where the credits and debits do not add up.
        /// </summary>
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


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Public Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a DRM with the given transaction row.
        /// </summary>
        public TransactionDRM(FFDataSet.TransactionRow tRow)
        {
            this._transactionRow = tRow;
        }

        /// <summary>
        /// Creates a DRM with the given transactioID if it exists.
        /// </summary>
        protected TransactionDRM(int transID)
        {
            this._transactionRow = MyData.getInstance().Transaction.FindByid(transID);
        }

        /// <summary>
        /// Creates a new transaction data row with default values.
        /// </summary>
        public TransactionDRM() : this(DateTime.Today, TransactionTypeCON.NULL.ID, "", LineCompleteCON.PENDING)
        {
        }

        /// <summary>
        /// Creates a new transaction data row with the given values
        /// </summary>
        public TransactionDRM(DateTime date, int typeID, string description, LineCompleteCON complete)
        {
            this._transactionRow = MyData.getInstance().Transaction.NewTransactionRow();

            this._transactionRow.id = MyData.getInstance().getNextID("IsTransactionError");
            this.Date = date;
            this.TypeID = typeID;
            this.Description = description;
            this.CompleteCode = complete.Value;

            MyData.getInstance().Transaction.AddTransactionRow(this._transactionRow);
        }

    }
}
