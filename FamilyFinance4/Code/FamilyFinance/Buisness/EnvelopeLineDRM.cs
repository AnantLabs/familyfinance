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
        private LineItemDRM parentLine;


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

        public virtual decimal Amount
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


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Private Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        private void reportToParentThatADependantPropertyHasChanged()
        {
            if (this.parentLine != null)
                this.parentLine.retportDependantPropertiesChanged();
        }



        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public EnvelopeLineDRM()
        {
            this.envelopeLineRow = DataSetModel.Instance.NewEnvelopeLineRow();
        }

        public EnvelopeLineDRM(FFDataSet.EnvelopeLineRow envLineRow, LineItemDRM parentLine)
        {
            this.envelopeLineRow = envLineRow;
            this.parentLine = parentLine;
        }

        public EnvelopeLineDRM(LineItemDRM parentLine)
        {
            this.envelopeLineRow = DataSetModel.Instance.NewEnvelopeLineRow(parentLine);
            this.parentLine = parentLine;
        }


        public void setParentLine(LineItemDRM parentLine)
        {
            this.envelopeLineRow.lineItemID = parentLine.LineID;
            this.parentLine = parentLine;
            this.reportToParentThatADependantPropertyHasChanged();
        }

        public void delete()
        {
            this.Amount = 0;
            this.envelopeLineRow.Delete();
        }

    }
}
