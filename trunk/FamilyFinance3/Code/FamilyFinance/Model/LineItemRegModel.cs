using FamilyFinance.Database;

namespace FamilyFinance.Model
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
                if (this.lineItemRow.AccountRowByFK_Line_accountID.envelopes == true)
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
                if (this.lineItemRow.AccountRowByFK_Line_accountID.envelopes == true)
                {
                    FFDataSet.EnvelopeLineRow[] rows = this.lineItemRow.GetEnvelopeLineRows();
                    int count = rows.Length;

                    if (count == 1)
                    {
                        rows[0].envelopeID = value;
                        MyData.getInstance().saveEnvelopeLineRow(rows[0]);

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
                int count = this.lineItemRow.GetEnvelopeLineRows().Length;

                if (count >= 2)
                    return MyData.getInstance().Envelope.FindByid(-2).name;

                else if (count == 1)
                    return this.lineItemRow.GetEnvelopeLineRows()[0].EnvelopeRow.name;

                else
                    return "";
            }
        }

        public decimal CreditAmount
        {
            get
            {
                if (this.lineItemRow.creditDebit == LineCD.CREDIT)
                    return this.lineItemRow.amount;
                else
                    return 0.0m;
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

        public decimal DebitAmount
        {
            get
            {
                if (this.lineItemRow.creditDebit == LineCD.DEBIT)
                    return this.lineItemRow.amount;
                else
                    return 0.0m;
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
            MyData.getInstance().saveLineItemRow(this.lineItemRow);
        }

        private void setAmount(decimal amount, bool cd)
        {
            if (amount < 0.0m)
            {
                amount = decimal.Negate(amount);
                cd = !cd;
            }

            amount = decimal.Round(amount, 2);

            this.lineItemRow.amount = amount;
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
            // Create two new lines
            this.lineItemRow = MyData.getInstance().LineItem.NewLineItemRow();
            FFDataSet.LineItemRow oppLine = MyData.getInstance().LineItem.NewLineItemRow();

            // Set the transactionID.
            this.lineItemRow.transactionID = base.ID;
            oppLine.transactionID = base.ID;

            // Set the accountID's
            this.lineItemRow.accountID = currentAccountID;
            oppLine.accountID = SpclAccount.NULL;

            // Make them opposite, assume this is a credit (purchase)
            this.lineItemRow.creditDebit = LineCD.CREDIT;
            this.lineItemRow.creditDebit = LineCD.DEBIT;

            // Add them to the table
            MyData.getInstance().LineItem.AddLineItemRow(this.lineItemRow);
            MyData.getInstance().LineItem.AddLineItemRow(oppLine);

            // Save these new linees to the database.
            MyData.getInstance().saveLineItemRow(this.lineItemRow);
            MyData.getInstance().saveLineItemRow(oppLine);
        }

        public LineItemRegModel(FFDataSet.LineItemRow row) : base(row.TransactionRow)
        {
            this.lineItemRow = row;
        }

        public static void setAccount(int accountID)
        {
            currentAccountID = accountID;
        }

        public int CompareTo(LineItemRegModel value)
        {
            return this.Date.CompareTo(value.Date);
        }


    }

}
