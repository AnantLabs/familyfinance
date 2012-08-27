using System;
using System.Collections.ObjectModel;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class LineItemDRM : BindableObject, DataRowModel
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        // Local Variables
        ///////////////////////////////////////////////////////////////////////////////////////////
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

        public virtual int AccountID
        {
            get
            {
                return lineItemRow.accountID;
            }
            set
            {
                this.lineItemRow.accountID = value;

                this.reportPropertyChangedWithName("AccountID");
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
                this.reportPropertyChangedWithName("ConfirmationNumber");
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



        ///////////////////////////////////////////////////////////////////////////////////////////
        // Private Functions
        ///////////////////////////////////////////////////////////////////////////////////////////


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
        

        public EnvelopeLineDRM[] getEnvelopeLineRows()
        {
            FFDataSet.EnvelopeLineRow[] rawEnvLines= this.lineItemRow.GetEnvelopeLineRows();
            EnvelopeLineDRM[] envLines = new EnvelopeLineDRM[rawEnvLines.Length];

            for (int i = 0; i < rawEnvLines.Length; i++ )
                envLines[i] = new EnvelopeLineDRM(rawEnvLines[i]);
            
            return envLines;
        }

        public void setParentTransaction(TransactionDRM transaction)
        {
            if(this.lineItemRow.transactionID != transaction.TransactionID)
                this.lineItemRow.transactionID = transaction.TransactionID;
        }

        public bool supportsEnvelopeLines()
        {
            return this.lineItemRow.AccountRow.envelopes;
        }

        public void deleteRowFromDataset()
        {
            // Set amount to zero so there we don't have to listen to when rows are
            // added or removed. By setting the amount to zero before deleting it 
            // just listening to the amount and polarity changes will keep eveything syncronized.
            this.Amount = 0;
            this.lineItemRow.Delete();
        }

    
    }
}
