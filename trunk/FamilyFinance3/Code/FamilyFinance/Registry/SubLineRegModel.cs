using FamilyFinance.Database;

namespace FamilyFinance.Registry
{
    class SubLineRegModel : LineItemRegModel
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////
        private FFDataSet.EnvelopeLineRow eLineRow;

        //private static int currentAccountID = SpclAccount.NULL;

        private decimal balAmount;

        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        public new int ID
        {
            get
            {
                return this.eLineRow.id;
            }
        }

        public int LineItemID
        {
            get
            {
                return this.eLineRow.lineItemID;
            }
        }

        public string Source
        {
            get
            {
                if (this.eLineRow.LineItemRow.creditDebit == LineCD.CREDIT && this.eLineRow.amount >= 0.0m)
                    return this.eLineRow.LineItemRow.AccountRow.name;

                else
                    return this.OppAccountName;
            }
        }

        public string Destination
        {
            get
            {
                if (this.eLineRow.LineItemRow.creditDebit == LineCD.DEBIT && this.eLineRow.amount >= 0.0m)
                    return this.OppAccountName;

                else
                    return this.eLineRow.LineItemRow.AccountRow.name;
            }
        }
        
        public string SubDescription
        {
            get
            {
                if (this.eLineRow.IsdescriptionNull())
                    return "";
                else
                    return this.eLineRow.description;
            }

            set
            {
                this.eLineRow.description = value;

                this.saveRow();
                this.RaisePropertyChanged("SubDescription");
            }
        }

        public new decimal Amount
        {
            get
            {
                return this.eLineRow.amount;
            }
        }

        public new int EnvelopeID
        {
            get
            {
                return this.eLineRow.envelopeID;
            }

            set
            {
                this.eLineRow.envelopeID = value;
                this.saveRow();

                this.RaisePropertyChanged("EnvelopeID");
                this.RaisePropertyChanged("EnvelopeName");
            }
        }

        public new string EnvelopeName
        {
            get
            {
                return this.eLineRow.EnvelopeRow.name;
            }
        }

        public new decimal? CreditAmount
        {
            get
            {
                if (this.eLineRow.LineItemRow.creditDebit == LineCD.CREDIT && this.eLineRow.amount > 0.0m)
                    return this.eLineRow.amount;

                else if (this.eLineRow.LineItemRow.creditDebit == LineCD.DEBIT && this.eLineRow.amount < 0.0m)
                    return decimal.Negate(this.eLineRow.amount);

                else
                    return null;
            }

            set
            {
                //this.setAmount(value, LineCD.CREDIT);
                
                //this.saveRow();
                //this.RaisePropertyChanged("CreditAmount");
                //this.RaisePropertyChanged("DebitAmount");
                //this.RaisePropertyChanged("TransactionError");
            }
        }

        public new decimal? DebitAmount
        {
            get
            {
                if (this.eLineRow.LineItemRow.creditDebit == LineCD.DEBIT && this.eLineRow.amount > 0.0m)
                    return this.eLineRow.amount;

                else if (this.eLineRow.LineItemRow.creditDebit == LineCD.CREDIT && this.eLineRow.amount < 0.0m)
                    return decimal.Negate(this.eLineRow.amount);

                else
                    return null;
            }

            set
            {
                //this.setAmount(value, LineCD.DEBIT);

                //this.saveRow();
                //this.RaisePropertyChanged("CreditAmount");
                //this.RaisePropertyChanged("DebitAmount");
                //this.RaisePropertyChanged("TransactionError");
            }
        }

        public new decimal BalanceAmount
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
            MyData.getInstance().saveRow(this.eLineRow);
        }

        //private void setAmount(decimal? amount, bool cd)
        //{
        //    decimal newAmount = amount.Value;

        //    if (newAmount < 0.0m)
        //    {
        //        newAmount = decimal.Negate(newAmount);
        //        cd = !cd;
        //    }

        //    newAmount = decimal.Round(newAmount, 2);

        //    this.eLineRow.amount = newAmount;
        //    this.eLineRow.creditDebit = cd;

        //    this.setOppLineAmount(this.eLineRow.id);
        //}


        ///////////////////////////////////////////////////////////////////////
        // Protected functions
        ///////////////////////////////////////////////////////////////////////



        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public SubLineRegModel(FFDataSet.EnvelopeLineRow row) : base(row.LineItemRow)
        {
            this.eLineRow = row;
        }

    }

    class SubLineRegModelComparer : System.Collections.Generic.IComparer<SubLineRegModel>
    {
        public int Compare(SubLineRegModel x, SubLineRegModel y)
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
