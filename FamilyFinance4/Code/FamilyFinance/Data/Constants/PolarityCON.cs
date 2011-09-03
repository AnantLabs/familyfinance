using System;

namespace FamilyFinance.Data
{
    public class PolarityCON
    {

        /// <summary>
        /// The static polarity instance of Credit.
        /// </summary>
        public static PolarityCON CREDIT = new PolarityCON(false, "Credit");

        /// <summary>
        /// The static polarity instance of Debit.
        /// </summary>
        public static PolarityCON DEBIT = new PolarityCON(true, "Debit");

        /// <summary>
        /// Gets the static instance of the Credit or debit according to the 
        /// given boolean.
        /// </summary>
        /// <param name="value">Value of the polarity.</param>
        /// <returns></returns>
        public static PolarityCON GetPlolartiy(bool value)
        {
            PolarityCON polarity;

            if (value == CREDIT.Value)
                polarity = CREDIT;

            else
                polarity = DEBIT;

            return polarity;
        }

        /// <summary>
        /// The value of the Credit or Debit
        /// </summary>
        public bool Value { get; private set; }

        /// <summary>
        /// The name of the credit or debit.
        /// </summary>
        public string Name { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Prevents outside instantiation of this class. This is a Java style enum.
        /// </summary>
        /// <param name="id">The value of the polarity.</param>
        /// <param name="name">The name of the polarity.</param>
        private PolarityCON(bool value, string name)
        {
            this.Value = value;
            this.Name = name;
        }
    }
}
