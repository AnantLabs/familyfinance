using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Buisness
{
    public class EnvelopeLineDRM : DataRowModel
    {
        private EnvelopeLineRow _envelopeLineRow;

        /// <summary>
        /// Gets the id of this envelopeLine
        /// </summary>
        public int EnvelopeLineID
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets the id of the LineItem this belongs to.
        /// </summary>
        public int LineItemID
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets the id of the transaction this belongs to.
        /// </summary>
        public int TransactionID
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets or Sets the description of this envelope line.
        /// </summary>
        public string Description
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets or Sets the amount of this envelope line.
        /// </summary>
        public decimal Amount
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets or Sets the envelopeID of this envelope line.
        /// </summary>
        public int EnvelopeID
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets the name of the envelope this envelope line is associated with.
        /// </summary>
        public string EnvelopeName
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
