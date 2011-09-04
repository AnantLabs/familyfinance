using System;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    /// <summary>
    /// The Data Row Model wrapper for an envelope line row.
    /// </summary>
    public class EnvelopeLineDRM : DataRowModel
    {
        private FFDataSet.EnvelopeLineRow _envelopeLineRow;

        /// <summary>
        /// Gets the id of this envelopeLine
        /// </summary>
        public int EnvelopeLineID
        {
            get
            {
                return this._envelopeLineRow.id;
            }
        }

        /// <summary>
        /// Gets the id of the LineItem this belongs to.
        /// </summary>
        public int LineItemID
        {
            get
            {
                return this._envelopeLineRow.lineItemID;
            }
        }

        /// <summary>
        /// Gets the id of the transaction this belongs to.
        /// </summary>
        public int TransactionID
        {
            get
            {
                return this._envelopeLineRow.LineItemRow.transactionID;
            }
        }

        /// <summary>
        /// Gets or Sets the description of this envelope line.
        /// </summary>
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

        /// <summary>
        /// Gets or Sets the amount of this envelope line. Values are rounded to the nearest 2
        /// decimals and negative numbers are allowed.
        /// </summary>
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

        /// <summary>
        /// Gets or Sets the envelopeID of this envelope line.
        /// </summary>
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

        /// <summary>
        /// Amount the name of the envelope this envelope line is associated with.
        /// </summary>
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
