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
        private FFDataSet.LineItemRow lineItemRow;
        private TransactionDRM parentTransaction;


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
                this.reportPropertyChangedWithName("AccountName");
                this.reportPropertyChangedWithName("IsAccountError");
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
                this.reportPropertyChangedWithName("Amount");
                this.reportToParentThatADependantPropertyHasChanged();
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
                this.reportPropertyChangedWithName("Polarity");
                this.reportToParentThatADependantPropertyHasChanged();
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



        public bool IsAccountError
        {
            get
            {
                if (this.lineItemRow.accountID == AccountCON.NULL.ID)
                    return true;
                else
                    return false;
            }
        }

        public bool IsLineError
        {
            get
            {
                decimal envLineSum = this.EnvelopeLineSum;
                bool accountUsesEnvelopes = lineItemRow.AccountRow.envelopes;

                if (accountUsesEnvelopes && lineItemRow.amount == envLineSum)
                    return false;

                else if (!accountUsesEnvelopes && envLineSum == 0)
                    return false;

                else
                    return true;
            }
        }

        public decimal EnvelopeLineSum
        {
            get
            {
                decimal sum = 0;

                foreach (FFDataSet.EnvelopeLineRow envLine in lineItemRow.GetEnvelopeLineRows())
                    sum += envLine.amount;

                return sum;
            }
        }




        ///////////////////////////////////////////////////////////////////////////////////////////
        // Private Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void reportToParentThatADependantPropertyHasChanged()
        {
            if (this.parentTransaction != null)
                this.parentTransaction.retportDependantPropertiesChanged();
        }

        protected FFDataSet.EnvelopeLineRow[] getEnvelopeLineRows()
        {
            return this.lineItemRow.GetEnvelopeLineRows();
        }



        ///////////////////////////////////////////////////////////////////////////////////////////
        // Public Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        public LineItemDRM()
        {
            this.lineItemRow = DataSetModel.Instance.NewLineItemRow();
        }
        
        public LineItemDRM(FFDataSet.LineItemRow lineRow, TransactionDRM parentTransaction)
        {
            this.lineItemRow = lineRow;
            this.parentTransaction = parentTransaction;
        }

        public LineItemDRM(TransactionDRM parentTransaction)
        {
            this.lineItemRow = DataSetModel.Instance.NewLineItemRow(parentTransaction);
            this.parentTransaction = parentTransaction;
        }



        public void setParentTransaction(TransactionDRM transaction)
        {
            this.lineItemRow.transactionID = transaction.TransactionID;
            this.parentTransaction = transaction;
            this.reportToParentThatADependantPropertyHasChanged();
        }

        public bool supportsEnvelopeLines()
        {
            return this.lineItemRow.AccountRow.envelopes;
        }

        public void retportDependantPropertiesChanged()
        {
            this.reportPropertyChangedWithName("IsLineError");
            this.reportPropertyChangedWithName("EnvelopeLineSum");
        }

        public void delete()
        {
            // Set amount to zero so there we don't have to listen to when rows are
            // added or removed. By setting the amount to zero before deleting it 
            // just listening to the column changes will keep eveything syncronized.
            this.Amount = 0;
            this.lineItemRow.Delete();
        }

    }
}
