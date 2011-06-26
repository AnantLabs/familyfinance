using System;
using System.Collections.ObjectModel;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class LineItemDRM : TransactionDRM
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        // Local Variables
        ///////////////////////////////////////////////////////////////////////////////////////////
        private FFDataSet.LineItemRow _lineItemRow;


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////////////////////////
        public int LineID
        {
            get
            {
                return this._lineItemRow.id;
            }
        }

        public int LineTransactionID
        {
            get
            {
                return this._lineItemRow.transactionID;
            }
            set
            {
                this._lineItemRow.transactionID = value;
            }
        }

        public int AccountID
        {
            get
            {
                return this._lineItemRow.accountID;
            }

            set
            {
                this._lineItemRow.accountID = value;
            }
        }

        public string AccountName
        {
            get
            {
                return this._lineItemRow.AccountRow.name;
            }
        }

        public int OppAccountID
        {
            get
            {
                return this.getOppAccountID();
            }

            set
            {
                this.setOppAccountID(value);
            }
        }

        public string OppAccountName
        {
            get
            {
                return this.getOppAccountName();
            }
        }

        public string ConfirmationNumber
        {
            get
            {
                return this._lineItemRow.confirmationNumber;
            }
            set
            {
                this._lineItemRow.confirmationNumber = value.Substring(0, LineItemCON.ConfirmationNumberMaxLength);
            }
        }

        public decimal Amount
        {
            get
            {
                return this._lineItemRow.amount;
            }
            set
            {
                if(value < 0.0m)
                    value = Decimal.Negate(value);

                this._lineItemRow.amount = Decimal.Round(value, 2);
            }
        }

        public bool CreditDebit
        {
            get
            {
                return this._lineItemRow.creditDebit;
            }
            set
            {
                this._lineItemRow.creditDebit = value;
            }
        }

        public ObservableCollection<EnvelopeLineDRM> EnvelopeLines
        {
            get
            {
                ObservableCollection<EnvelopeLineDRM> temp = new ObservableCollection<EnvelopeLineDRM>();
                FFDataSet.EnvelopeLineRow[] rows = this._lineItemRow.GetEnvelopeLineRows();

                foreach (FFDataSet.EnvelopeLineRow envLine in rows)
                {
                    //temp.Add(new EnvelopeLineDRM(envLine));
                }

                return temp;
            }
        }



        ///////////////////////////////////////////////////////////////////////////////////////////
        // Private Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void setOppAccountID(int newOppAccID)
        {
            int oppLinesCount = 0;
            FFDataSet.LineItemRow[] lines = this._lineItemRow.TransactionRow.GetLineItemRows();
            FFDataSet.LineItemRow oppLine = null;

            foreach (FFDataSet.LineItemRow line in lines)
            {
                if (line.creditDebit != this._lineItemRow.creditDebit)
                {
                    oppLinesCount++;
                    oppLine = line;
                }
            }

            if (oppLinesCount == 0)
            {
                // We could do something here but it is not trivial to do.
                //   If we get here and if there is only 1 line in this transaction we could duplicate 
                //      the existing line to make an opposite line.
                //   If we get here and there are multiple lines on one side and non on the opposite side
                //      it means we are working with a one-sided complex transaction and this is not the 
                //      class to handle this situation.
            }
            else if (oppLinesCount == 1)
            {
                oppLine.accountID = newOppAccID;
            }
            else if (oppLinesCount >= 2)
            {
                // This is a complex transaction, so don't commit the change because we don't know which
                // opposite line to update.
            }
        }

        private int getOppAccountID()
        {
            int oppLinesCount = 0;
            int oppAccountID = AccountCON.NULL.ID;
            FFDataSet.LineItemRow[] lines = this._lineItemRow.TransactionRow.GetLineItemRows();

            foreach (FFDataSet.LineItemRow line in lines)
            {
                if (line.creditDebit != this._lineItemRow.creditDebit)
                {
                    oppLinesCount++;
                    oppAccountID = line.accountID;
                }
            }

            if (oppLinesCount >= 2)
            {
                oppAccountID = AccountCON.MULTIPLE.ID;
            }

            return oppAccountID;
        }

        private string getOppAccountName()
        {
            int oppLinesCount = 0;
            string oppAccountName = AccountCON.NULL.Name;
            FFDataSet.LineItemRow[] lines = this._lineItemRow.TransactionRow.GetLineItemRows();

            foreach (FFDataSet.LineItemRow line in lines)
            {
                if (line.creditDebit != this._lineItemRow.creditDebit)
                {
                    oppLinesCount++;
                    oppAccountName = line.AccountRow.name;
                }
            }

            if (oppLinesCount >= 2)
            {
                oppAccountName = AccountCON.MULTIPLE.Name;
            }

            return oppAccountName;
        }




        ///////////////////////////////////////////////////////////////////////////////////////////
        // Public Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        public LineItemDRM(FFDataSet.LineItemRow lRow)
        {
            this._lineItemRow = lRow;
        }

        public LineItemDRM() : this(AccountCON.NULL.ID, AccountCON.NULL.ID, "",  0.0m, CreditDebitCON.CREDIT.Value)
        {
        }

        public LineItemDRM(int accountID, int oppAccountID, string confrimationNum, decimal amount, bool creditDebit) : base()
        {
            // Make the first line of the transaction.
            this._lineItemRow = MyData.getInstance().LineItem.NewLineItemRow();

            this._lineItemRow.id = MyData.getInstance().getNextID("LineItem");
            this._lineItemRow.transactionID = this.TransactionID;
            this.AccountID = accountID;
            this.ConfirmationNumber = confrimationNum;
            this.Amount = amount;
            this.CreditDebit = creditDebit;

            MyData.getInstance().LineItem.AddLineItemRow(this._lineItemRow);

            // Make the second or opposite line of the transaction.
            FFDataSet.LineItemRow oppLine = MyData.getInstance().LineItem.NewLineItemRow();

            oppLine.id = this._lineItemRow.id + 1;
            oppLine.transactionID = this._lineItemRow.transactionID;
            oppLine.accountID = oppAccountID;
            oppLine.confirmationNumber = this._lineItemRow.confirmationNumber;
            oppLine.amount = this._lineItemRow.amount;
            oppLine.creditDebit = !this._lineItemRow.creditDebit;

            MyData.getInstance().LineItem.AddLineItemRow(oppLine);
        }

    }
}
