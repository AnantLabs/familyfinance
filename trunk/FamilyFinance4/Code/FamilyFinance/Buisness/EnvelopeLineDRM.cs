using System;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class EnvelopeLineDRM : DataRowModel
    {
        private FFDataSet.EnvelopeLineRow _envelopeLineRow;

        public int EnvelopeLineID
        {
            get
            {
                return this._envelopeLineRow.id;
            }
        }

        public int LineItemID
        {
            get
            {
                return this._envelopeLineRow.lineItemID;
            }
        }

        public int TransactionID
        {
            get
            {
                return this._envelopeLineRow.LineItemRow.transactionID;
            }
        }

        public virtual string Description
        {
            get
            {
                return this._envelopeLineRow.description;
            }
            set
            {
                this._envelopeLineRow.description = value;
            }
        }

        public decimal Amount
        {
            get
            {
                return this._envelopeLineRow.amount;
            }
            set
            {
                // Round to 2 decimal places. 
                this._envelopeLineRow.amount = Decimal.Round(value, 2);
            }
        }

        public int EnvelopeID
        {
            get
            {
                return this._envelopeLineRow.envelopeID;
            }
            set
            {
                this._envelopeLineRow.envelopeID = value;
            }
        }

        public string EnvelopeName
        {
            get
            {
                return this._envelopeLineRow.EnvelopeRow.name;
            }
        }

        public EnvelopeLineDRM(FFDataSet.LineItemRow lineRow)
        {

        }
    }
}
