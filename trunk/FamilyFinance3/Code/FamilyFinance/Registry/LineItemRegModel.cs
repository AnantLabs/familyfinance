using FamilyFinance.Database;
using System.Text;
using System.Collections.Generic;


namespace FamilyFinance.Registry
{
    class LineItemRegModel : TransactionModel
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////
        private FFDataSet.LineItemRow lineItemRow;

        private static int currentAccountID = SpclAccount.NULL;

        private decimal balAmount;

        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the lineItem's ID value. Override the ID property to be the LineItems id value instead of the transactions.
        /// </summary>
        public new int ID
        {
            get
            {
                return this.lineItemRow.id;
            }
        }

        /// <summary>
        /// Gets the transaciont ID.
        /// </summary>
        public int TransactionID
        {
            get
            {
                return this.lineItemRow.transactionID;
            }
        }

        /// <summary>
        /// Gets or sets this lines accountID
        /// </summary>
        public int AccountID
        {
            get
            {
                return this.lineItemRow.accountID;
            }

            set
            {
                if (SpclAccount.isNotSpecial(value))
                {
                    this.lineItemRow.accountID = value;

                    this.saveRow();

                    this.RaisePropertyChanged("TransactionError");
                }
            }
        }

        public decimal Amount
        {
            get
            {
                return this.lineItemRow.amount;
            }
        }

        public bool CreditDebit
        {
            get
            {
                return this.lineItemRow.creditDebit;
            }
        }

        /// <summary>
        /// Gets or sets this lines oppAccountID
        /// </summary>
        public int OppAccountID
        {
            get
            {
                return this.determineOppAccountID(this.lineItemRow.creditDebit);
            }

            set
            {
                if (this.setOppAccountID(this.lineItemRow.creditDebit, value))
                {
                    this.RaisePropertyChanged("OppAccountID");
                    this.RaisePropertyChanged("OppAccountName");
                    this.RaisePropertyChanged("TransactionError");
                }
            }
        }

        /// <summary>
        /// Gets the name of the account opposite from this line.
        /// </summary>
        public string OppAccountName
        {
            get
            {
                return this.determineOppAccountName(this.lineItemRow.creditDebit);
            }
        }

        public int EnvelopeID
        {
            get
            {
                int envelopeID = SpclEnvelope.NULL;

                // If this line account doesn't use envelopes return the null envelope reference.
                if (this.lineItemRow.AccountRow.envelopes == true)
                {
                    int count = this.lineItemRow.GetEnvelopeLineRows().Length;

                    if (count >= 2)
                        envelopeID = SpclEnvelope.SPLIT;

                    else if (count == 1)
                        envelopeID = this.lineItemRow.GetEnvelopeLineRows()[0].envelopeID;

                }

                return envelopeID;
            }

            set
            {
                // if this lines account uses envelopes save the value.
                if (this.lineItemRow.AccountRow.envelopes == true)
                {
                    FFDataSet.EnvelopeLineRow[] rows = this.lineItemRow.GetEnvelopeLineRows();
                    int count = rows.Length;

                    if (count == 1)
                    {
                        rows[0].envelopeID = value;
                        MyData.getInstance().saveRow(rows[0]);

                        this.RaisePropertyChanged("EnvelopeID");
                        this.RaisePropertyChanged("EnvelopeName");
                    }
                }
            }
        }

        public string EnvelopeName
        {
            get
            {
                FFDataSet.EnvelopeLineRow[] rows = this.lineItemRow.GetEnvelopeLineRows();
                StringBuilder name = new StringBuilder();

                foreach (FFDataSet.EnvelopeLineRow row in rows)
                {
                    // If this is NOT the first account add on the comma.
                    if (rows[0] != row)
                        name.Append(", ");

                    name.Append(row.EnvelopeRow.name);
                }

                return name.ToString();
            }
        }

        public decimal? CreditAmount
        {
            get
            {
                if (this.lineItemRow.creditDebit == LineCD.CREDIT && this.lineItemRow.amount > 0.0m)
                    return this.lineItemRow.amount;
                else
                    return null;
            }

            set
            {
                this.setAmount(value, LineCD.CREDIT);
                
                this.saveRow();
                this.RaisePropertyChanged("CreditAmount");
                this.RaisePropertyChanged("DebitAmount");
                this.RaisePropertyChanged("TransactionError");
            }
        }

        public decimal? DebitAmount
        {
            get
            {
                if (this.lineItemRow.creditDebit == LineCD.DEBIT && this.lineItemRow.amount > 0.0m)
                    return this.lineItemRow.amount;
                else
                    return null;
            }

            set
            {
                this.setAmount(value, LineCD.DEBIT);

                this.saveRow();
                this.RaisePropertyChanged("CreditAmount");
                this.RaisePropertyChanged("DebitAmount");
                this.RaisePropertyChanged("TransactionError");
            }
        }

        public decimal BalanceAmount
        {
            get
            {
                return balAmount;
            }

            set
            {
                this.balAmount = value;
                this.RaisePropertyChanged("BalanceAmount");
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////
        private void saveRow()
        {
            MyData.getInstance().saveRow(this.lineItemRow);
        }

        private void setAmount(decimal? amount, bool cd)
        {
            decimal newAmount = amount.Value;

            if (newAmount < 0.0m)
            {
                newAmount = decimal.Negate(newAmount);
                cd = !cd;
            }

            newAmount = decimal.Round(newAmount, 2);

            this.lineItemRow.amount = newAmount;
            this.lineItemRow.creditDebit = cd;

            this.setOppLineAmount(this.lineItemRow.id);
        }


        ///////////////////////////////////////////////////////////////////////
        // Protected functions
        ///////////////////////////////////////////////////////////////////////



        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public LineItemRegModel() : base()
        {
            // Build up the first lineItem.
            this.lineItemRow = MyData.getInstance().LineItem.NewLineItemRow();
            this.lineItemRow.transactionID = base.ID;
            this.lineItemRow.accountID = currentAccountID;
            this.lineItemRow.creditDebit = LineCD.CREDIT;
            MyData.getInstance().LineItem.AddLineItemRow(this.lineItemRow);
            MyData.getInstance().saveRow(this.lineItemRow);

            // Build up the opposite lineItem.
            FFDataSet.LineItemRow oppLine = MyData.getInstance().LineItem.NewLineItemRow();
            oppLine.transactionID = base.ID;
            oppLine.accountID = SpclAccount.NULL;
            oppLine.creditDebit = LineCD.DEBIT;
            MyData.getInstance().LineItem.AddLineItemRow(oppLine);
            MyData.getInstance().saveRow(oppLine);
        }

        public LineItemRegModel(FFDataSet.LineItemRow row) : base(row.TransactionRow)
        {
            this.lineItemRow = row;
        }

        public static void setAccount(int accountID)
        {
            currentAccountID = accountID;
        }

    }

    class RegistryComparer : System.Collections.Generic.IComparer<LineItemRegModel>
    {
        public int Compare(LineItemRegModel x, LineItemRegModel y)
        {
            int comp = x.Date.CompareTo(y.Date);

            if (comp == 0)
            {
                comp = y.CreditDebit.CompareTo(x.CreditDebit);

                if (comp == 0)
                {
                    comp = x.Amount.CompareTo(y.Amount);
                }
            }

            return comp;
        }
    }

}
