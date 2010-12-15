using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class CreditDebitModel
    {
        /// <summary>
        /// The Credit instance.
        /// </summary>
        public static CreditDebitModel CREDIT = new CreditDebitModel(LineCD.CREDIT, "Credit");

        /// <summary>
        /// The Debit instance.
        /// </summary>
        public static CreditDebitModel DEBIT = new CreditDebitModel(LineCD.DEBIT, "Debit");

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
        /// Prevents outside instantiation of this class. This is esentially an Enum like the kind
        /// available in Java.
        /// </summary>
        /// <param name="id">The id of the catagory.</param>
        /// <param name="name">The name of the catagory.</param>
        private CreditDebitModel(bool value, string name)
        {
            this._Value = value;
            this._Name = name;
        }

    }
}
