using System;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class EnvelopeLineDRM : DataRowModel
    {
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

        public virtual string Description
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
                // Round to 2 decimal places. 
                this.envelopeLineRow.amount = Decimal.Round(value, 2);
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
            }
        }

        public string EnvelopeName
        {
            get
            {
                return this.envelopeLineRow.EnvelopeRow.name;
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

        public EnvelopeLineDRM(LineItemDRM lineItem)
        {
            this.envelopeLineRow = DataSetModel.Instance.NewEnvelopeLineRow(lineItem);
        }


        public void setParentLine(LineItemDRM lineitem)
        {
            this.envelopeLineRow.lineItemID = lineitem.LineID;
        }

        public void Delete()
        {
            this.Amount = 0;
            this.envelopeLineRow.Delete();
        }

    }
}
