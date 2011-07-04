using System;


using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    class RegistryEnvelopeLineModel : EnvelopeLineDRM
    {
        /// <summary>
        /// Amount the complete code of the parent transaction.
        /// </summary>
        public string CompleteCode
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
        /// Amount the date of the parent transaction
        /// </summary>
        public DateTime Date
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
        /// Amount the combined description of the transaction and the Envelope Line BUT only sets the envelopeLine description
        /// </summary>
        public override string Description
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
        /// Amount the type of the parent transaction
        /// </summary>
        public string TypeName
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }




        public RegistryEnvelopeLineModel(FFDataSet.LineItemRow lineRow) : base(lineRow)
        {

        }
    }
}
