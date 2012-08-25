using System;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class EnvelopeLineDRM : BindableObject, DataRowModel
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        // Local Variables
        ///////////////////////////////////////////////////////////////////////////////////////////
        private FFDataSet.EnvelopeLineRow envelopeLineRow;


        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        public int EnvelopeLineID
        {
            get
            {
                return this.envelopeLineRow.id;
            }
        }

        public int LineItemID
        {
            get
            {
                return this.envelopeLineRow.lineItemID;
            }
        }

        public int TransactionID
        {
            get
            {
                return this.envelopeLineRow.LineItemRow.transactionID;
            }
        }

        public int EnvelopeID
        {
            get
            {
                return this.envelopeLineRow.envelopeID;
            }
            set
            {
                this.envelopeLineRow.envelopeID = value;

                this.reportPropertyChangedWithName("EnvelopeName");
                this.reportPropertyChangedWithName("IsEnvelopeError");
            }
        }

        public string EnvelopeName
        {
            get
            {
                return this.envelopeLineRow.EnvelopeRow.name;
            }
        }

        public string Description
        {
            get
            {
                return this.envelopeLineRow.description;
            }
            set
            {
                this.envelopeLineRow.description = value;
            }
        }

        public decimal Amount
        {
            get
            {
                return this.envelopeLineRow.amount;
            }
            set
            {
                this.envelopeLineRow.amount = Decimal.Round(value, 2);

                //this.reportToParentLineBalanceHasChanged();
            }
        }


        public bool IsEnvelopeError
        {
            get
            {
                return envelopeLineRow.envelopeID == EnvelopeCON.NULL.ID;
            }
        }




        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public EnvelopeLineDRM()
        {
            this.envelopeLineRow = DataSetModel.Instance.NewEnvelopeLineRow();
        }

        public EnvelopeLineDRM(FFDataSet.EnvelopeLineRow envLineRow)
        {
            this.envelopeLineRow = envLineRow;
        }

        public EnvelopeLineDRM(LineItemDRM parentLine)
        {
            this.envelopeLineRow = DataSetModel.Instance.NewEnvelopeLineRow(parentLine);
        }


        public void setParentLine(LineItemDRM parentLine)
        {
            if(this.envelopeLineRow.lineItemID != parentLine.LineID)
                this.envelopeLineRow.lineItemID = parentLine.LineID;
        }

        public virtual void deleteRowFromDataset()
        {
            // Set amount to zero so there we don't have to listen to when rows are
            // added or removed. By setting the amount to zero before deleting it 
            // just listening to the amount and polarity changes will keep eveything syncronized.
            this.Amount = 0;
            this.envelopeLineRow.Delete();
        }


    }
}
