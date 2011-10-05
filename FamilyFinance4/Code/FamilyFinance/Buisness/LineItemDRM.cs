using System;
using System.Collections.ObjectModel;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class LineItemDRM : DataRowModel
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        // Local Variables
        ///////////////////////////////////////////////////////////////////////////////////////////
        //public enum Property { 
        //    AccountID, 
        //    AccountName,
        //    ConfirmationNumber,
        //    Amoun,
        //    Polarity,
        //    IsLineError
        //};
        
        private FFDataSet.LineItemRow lineItemRow;


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////////////////////////
        public int LineID
        {
            get
            {
                return lineItemRow.id;
            }
        }

        public int TransactionID
        {
            get
            {
                return lineItemRow.transactionID;
            }
        }

        public int AccountID
        {
            get
            {
                return lineItemRow.accountID;
            }
            set
            {
                this.lineItemRow.accountID = value;
            }
        }

        public string AccountName
        {
            get
            {
                return lineItemRow.AccountRow.name;
            }
        }

        public string ConfirmationNumber
        {
            get
            {
                return lineItemRow.confirmationNumber;
            }
            set
            {
                this.lineItemRow.confirmationNumber = value;
            }
        }

        public decimal Amount
        {
            get
            {
                return lineItemRow.amount;
            }
            set
            {
                if (value < 0.0m)
                    value = Decimal.Negate(value);

                this.lineItemRow.amount = Decimal.Round(value, 2);
            }
        }

        public PolarityCON Polarity
        {
            get
            {
                return PolarityCON.GetPlolartiy(this.lineItemRow.polarity);
            }
            set
            {
                this.lineItemRow.polarity = value.Value;
            }
        }

        public TransactionStateCON State
        {
            get
            {
                return TransactionStateCON.GetState(this.lineItemRow.state);
            }
            set
            {
                this.lineItemRow.state = value.Value;
            }
        }

        public bool IsLineError
        {
            get
            {
                if (lineItemRow == null)
                    return false;

                else
                    return determineLineError();
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Private Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        private bool determineLineError()
        {
            decimal envLineSum = envelopeLineSum();
            bool accountUsesEnvelopes = lineItemRow.AccountRow.envelopes;

            if (accountUsesEnvelopes && lineItemRow.amount == envLineSum)
                return false;

            else if (!accountUsesEnvelopes && envLineSum == 0)
                return false;

            else
                return true;
        }

        private decimal envelopeLineSum()
        {
            decimal sum = 0;

            foreach (FFDataSet.EnvelopeLineRow envLine in lineItemRow.GetEnvelopeLineRows())
                sum += envLine.amount;

            return sum;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Public Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        public LineItemDRM()
        {
            this.lineItemRow = DataSetModel.Instance.NewLineItemRow();
        }
        
        public LineItemDRM(FFDataSet.LineItemRow lineRow)
        {
            this.lineItemRow = lineRow;
        }

        public LineItemDRM(TransactionDRM transaction)
        {
            this.lineItemRow = DataSetModel.Instance.NewLineItemRow(transaction);
        }

        //public void reportPropertyChanged(Property property)
        //{
        //    this.reportPropertyChangedWithName(property.ToString());
        //}

        public void setParentTransaction(TransactionDRM transaction)
        {
            this.lineItemRow.transactionID = transaction.TransactionID;
        }

    }
}
