using FamilyFinance.Database;
using FamilyFinance.Model;


namespace FamilyFinance.EditTransaction
{
    class LineItemModel : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Local referance to the transaction line this object encapselates.
        /// </summary>
        private FFDataSet.LineItemRow lineItemRow;

        static public int TransactionID;

        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Gets the transaction id.
        /// </summary>
        public int ID
        {
            get
            {
                return this.lineItemRow.id;
            }
        }

        /// <summary>
        /// Gets or sets the account ID.
        /// </summary>
        public int AccountID
        {
            get
            {
                return this.lineItemRow.accountID;
            }

            set
            {
                this.lineItemRow.accountID = value;

                saveRow();
                this.RaisePropertyChanged("AccountID");
                this.RaisePropertyChanged("AccountName");
            }
        }

        /// <summary>
        /// Gets the account name.
        /// </summary>
        public string AccountName
        {
            get
            {
                return this.lineItemRow.AccountRow.name;
            }
        }


        /// <summary>
        /// Gets or sets state of the transaction.
        /// </summary>
        public decimal? Amount
        {
            get
            {
                if (this.lineItemRow.amount > 0.0m)
                    return this.lineItemRow.amount;
                else
                    return null;
            }

            set
            {
                decimal newValue = (value == null) ? decimal.Zero : value.Value;

                if (newValue < decimal.Zero)
                    newValue = decimal.Negate(newValue);

                this.lineItemRow.amount = decimal.Round(newValue, 2);
                
                this.saveRow();
                this.RaisePropertyChanged("Amount");
                this.RaisePropertyChanged("LineError");
            }
        }

        public bool CreditDebit
        {
            get
            {
                return this.lineItemRow.creditDebit;
            }

            set
            {
                this.lineItemRow.creditDebit = value;
                this.saveRow();
            }
        }

        /// <summary>
        /// Gets a flag indicating if there is a line error with this line.
        /// </summary>
        public bool LineError
        {
            get
            {
                FFDataSet.EnvelopeLineRow[] rows = this.lineItemRow.GetEnvelopeLineRows();
                decimal sum = 0;
                bool result = false;


                foreach (FFDataSet.EnvelopeLineRow row in rows)
                {
                    if (row.envelopeID == SpclEnvelope.NULL)
                    {
                        result = true;
                        break;
                    }

                    sum += row.amount;
                }

                if (sum != this.lineItemRow.amount)
                    result = true;

                return result;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Sates changes to the row to the database.
        /// </summary>
        private void saveRow()
        {
            MyData.getInstance().saveRow(this.lineItemRow);
        }


        ///////////////////////////////////////////////////////////////////////
        // Protected functions
        ///////////////////////////////////////////////////////////////////////



        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor. Adds a new transaction row to the table.
        /// </summary>
        public LineItemModel()
        {
            this.lineItemRow = MyData.getInstance().LineItem.NewLineItemRow();

            this.lineItemRow.transactionID = TransactionID;

            MyData.getInstance().LineItem.AddLineItemRow(this.lineItemRow);
            this.saveRow();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        public LineItemModel(FFDataSet.LineItemRow row)
        {
            this.lineItemRow = row;
        }

    }
}
