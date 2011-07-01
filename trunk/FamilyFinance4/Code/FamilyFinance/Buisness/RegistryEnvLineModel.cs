﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Buisness
{
    class RegistryEnvLineModel : EnvelopeLineDRM
    {
        /// <summary>
        /// Gets the complete code of the parent transaction.
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
        /// Gets the date of the parent transaction
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
        /// Gets the combined description of the transaction and the Envelope Line BUT only sets the envelopeLine description
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
        /// Gets the type of the parent transaction
        /// </summary>
        public string TypeName
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