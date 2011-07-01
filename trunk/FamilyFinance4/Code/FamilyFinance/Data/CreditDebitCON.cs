using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Data
{
    public class CreditDebitCON
    {

        /// <summary>
        /// The Credit instance.
        /// </summary>
        public static CreditDebitCON CREDIT = new CreditDebitCON(false, "Credit");

        /// <summary>
        /// The Debit instance.
        /// </summary>
        public static CreditDebitCON DEBIT = new CreditDebitCON(true, "Debit");

        public static CreditDebitCON GetPlolartiy(bool? value)
        {
            CreditDebitCON polarity;

            if (value == null)
                polarity = null;

            else if (value == CREDIT.Value)
                polarity = CREDIT;

            else
                polarity = DEBIT;

            return polarity;
        }

        /// <summary>
        /// The value of the Credit or Debit
        /// </summary>
        private readonly bool _Value;

        /// <summary>
        /// Gets the ID of the catagory.
        /// </summary>
        public bool Value
        {
            get
            {
                return this._Value;
            }
        }

        /// <summary>
        /// The name of the catagory
        /// </summary>
        private readonly string _Name;

        /// <summary>
        /// Gets the name of the catagory.
        /// </summary>
        public string Name
        {
            get
            {
                return this._Name;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Prevents outside instantiation of this class. This is a Java style enum.
        /// </summary>
        /// <param name="id">The id of the catagory.</param>
        /// <param name="name">The name of the catagory.</param>
        private CreditDebitCON(bool value, string name)
        {
            this._Value = value;
            this._Name = name;
        }
    }
}
