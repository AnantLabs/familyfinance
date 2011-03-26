using FamilyFinance.Database;
using FamilyFinance.Model;
using System.Collections.Generic;
using System.Text;

namespace FamilyFinance.Registry
{
    class TransactionModel : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Local referance to the transaction line this object encapselates.
        /// </summary>
        protected FFDataSet.TransactionRow transactionRow;

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
                return this.transactionRow.id;
            }
        }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        public System.DateTime Date
        {
            get
            {
                return (this.transactionRow == null) ? new System.DateTime(0) : this.transactionRow.date;
            }

            set
            {
                this.transactionRow.date = value;

                saveRow();
                this.RaisePropertyChanged("Date");
            }
        }

        /// <summary>
        /// Gets or sets the transaction type id.
        /// </summary>
        public int TypeID
        {
            get
            {
                return (this.transactionRow == null) ? SpclLineType.NULL : this.transactionRow.typeID;
            }

            set
            {
                this.transactionRow.typeID = value;

                saveRow();
                this.RaisePropertyChanged("TypeID");
                this.RaisePropertyChanged("TypeName");
            }
        }

        /// <summary>
        /// Gets the transaction type name.
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.transactionRow.LineTypeRow.name;
            }
        }

        /// <summary>
        /// Gets or sets the transaction description.
        /// </summary>
        public string Description
        {
            get
            {
                if (this.transactionRow.IsdescriptionNull())
                    return "";
                else 
                    return this.transactionRow.description;
            }

            set
            {
                this.transactionRow.description = value;

                this.saveRow();
                this.RaisePropertyChanged("Description");
            }
        }

        /// <summary>
        /// Gets or sets the transaction confirmation number.
        /// </summary>
        public string ConfirmationNumber
        {
            get
            {
                if (this.transactionRow.IsconfirmationNumberNull())
                    return "";
                else
                    return this.transactionRow.confirmationNumber;
            }

            set
            {
                this.transactionRow.confirmationNumber = value;

                this.saveRow();
                this.RaisePropertyChanged("ConfirmationNumber");
            }
        }

        /// <summary>
        /// Gets or sets state of the transaction.
        /// </summary>
        public string Complete
        {
            get
            {
                return this.transactionRow.complete;
            }

            set
            {
                this.transactionRow.complete = value;

                this.saveRow();
                this.RaisePropertyChanged("Complete");
            }
        }

        /// <summary>
        /// Gets a flag indicating if there is an error with this trasaction.
        /// </summary>
        public bool TransactionError
        {
            get
            {
                FFDataSet.LineItemRow[] rows = this.transactionRow.GetLineItemRows();
                decimal debitSum = 0;
                decimal creditSum = 0;
                bool result = false;


                foreach (FFDataSet.LineItemRow row in rows)
                {
                    if (row.accountID == SpclAccount.NULL)
                    {
                        result = true;
                        break;
                    }

                    if (row.creditDebit == LineCD.CREDIT)
                        creditSum += row.amount;
                    else
                        debitSum += row.amount;
                }

                if (debitSum != creditSum)
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
            MyData.getInstance().saveRow(this.transactionRow);
        }


        ///////////////////////////////////////////////////////////////////////
        // Protected functions
        ///////////////////////////////////////////////////////////////////////
        protected int determineOppAccountID(bool cd)
        {
            FFDataSet.LineItemRow[] rows = this.transactionRow.GetLineItemRows();
            int count = 0;
            int oppAccountID = SpclAccount.NULL;

            foreach (FFDataSet.LineItemRow row in rows)
                if (row.creditDebit != cd)
                {
                    count++;
                    oppAccountID = row.accountID;
                }

            // if count is 0 or 1 oppAccountID already has the right value
            // if count > 2 then change to multiple.
            if (count >= 2)
                oppAccountID = SpclAccount.MULTIPLE;

            return oppAccountID;
        }

        protected string determineOppAccountName(bool cd)
        {
            FFDataSet.LineItemRow[] rows = this.transactionRow.GetLineItemRows();
            List<int> oppAccounts = new List<int>();
            StringBuilder oppName = new StringBuilder();

            foreach (FFDataSet.LineItemRow row in rows)
                if (row.creditDebit != cd)
                    oppAccounts.Add(row.accountID);

            foreach (int id in oppAccounts)
            {
                // If this is NOT the first account add on the comma.
                if(oppAccounts[0] != id)
                    oppName.Append(", ");

                oppName.Append(MyData.getInstance().Account.FindByid(id).name);
            }


            return oppName.ToString();
        }

        protected bool setOppAccountID(bool cd, int newAccountID)
        {
            if (newAccountID == SpclAccount.MULTIPLE || newAccountID == SpclAccount.NULL)
                return false;

            FFDataSet.LineItemRow[] rows = this.transactionRow.GetLineItemRows();
            FFDataSet.LineItemRow oppLine = null;
            int count = 0;
            bool result = false;

            foreach (FFDataSet.LineItemRow row in rows)
                if (row.creditDebit != cd)
                {
                    count++;
                    oppLine = row;
                    result = true;
                }

            // if count is 0 or 1 oppAccountID already has the right value
            // if count > 2 then change to multiple.
            if (count == 1)
                oppLine.accountID = newAccountID;

            return result;
        }

        protected void setOppLineAmount(int lineID)
        {
            FFDataSet.LineItemRow[] rows = this.transactionRow.GetLineItemRows();
            FFDataSet.LineItemRow changedLine;
            FFDataSet.LineItemRow oppLine;

            // Only make changes to the oppLine if this is a smiple transaction.
            if (rows.Length == 2)
            {
                if (rows[0].id == lineID)
                {
                    changedLine = rows[0];
                    oppLine = rows[1];
                }
                else
                {
                    changedLine = rows[1];
                    oppLine = rows[0];
                }

                oppLine.creditDebit = !changedLine.creditDebit;
                oppLine.amount = changedLine.amount;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor. Adds a new transaction row to the table.
        /// </summary>
        public TransactionModel()
        {
            this.transactionRow = MyData.getInstance().Transaction.NewTransactionRow();
            MyData.getInstance().Transaction.AddTransactionRow(this.transactionRow);
            this.saveRow();
        }

        public TransactionModel(FFDataSet.TransactionRow row)
        {
            this.transactionRow = row;
        }


    }
}
