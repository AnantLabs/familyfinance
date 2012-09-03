using System;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class EnvelopeLineDRM : DataRowModel
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
                if (this.envelopeLineRow.envelopeID != value)
                {
                    this.envelopeLineRow.envelopeID = value;

                    this.reportPropertyChangedWithName("EnvelopeID");
                    this.reportPropertyChangedWithName("EnvelopeName");
                    this.reportPropertyChangedWithName("IsEnvelopeError");
                }
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
                if (this.envelopeLineRow.description != value)
                {
                    this.envelopeLineRow.description = value;
                    this.reportPropertyChangedWithName("Description");
                }
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
                if (this.envelopeLineRow.amount != value)
                {
                    this.envelopeLineRow.amount = Decimal.Round(value, 2);
                    this.reportPropertyChangedWithName("Amount");
                }
            }
        }


        public bool IsEnvelopeError
        {
            get
            {
                return EnvelopeCON.isSpecial(this.EnvelopeID);
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
