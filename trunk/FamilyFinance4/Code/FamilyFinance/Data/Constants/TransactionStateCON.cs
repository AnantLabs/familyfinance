using System;


namespace FamilyFinance.Data
{
    public class TransactionStateCON
    {
        /// <summary>
        /// The object representing a transactions in a cleared state.
        /// The cleared state means this transaction matches a transaction at the bank.
        /// </summary>
        public static TransactionStateCON CLEARED = new TransactionStateCON('C', "-Cleared-");

        /// <summary>
        /// The object representing a transactions in a reconsiled state.
        /// The reconsiled state means that a bunch of transaction add up to a balance that
        /// matches the transaction and balance at the bank.
        /// </summary>
        public static TransactionStateCON RECONSILED = new TransactionStateCON('R', "-Reconsiled-");

        /// <summary>
        /// The object representing a transactions in a pending state.
        /// The pending state means the transaction is waiting to be verified with what
        /// is at the bank.
        /// </summary>
        public static TransactionStateCON PENDING = new TransactionStateCON(' ', "-Pending-");

        /// <summary>
        /// The value of the Credit or Debit
        /// </summary>
        public char Value { get; private set; }

        /// <summary>
        /// The name of the credit or debit.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the static instance of the transactionState according to the 
        /// given value.
        /// </summary>
        /// <param name="va">Value of the Transactio state.</param>
        /// <returns></returns>
        public static TransactionStateCON GetState(char valueToDetermineState)
        {
            TransactionStateCON state;

            if (valueToDetermineState == CLEARED.Value)
                state = CLEARED;

            else if (valueToDetermineState == RECONSILED.Value)
                state = RECONSILED;

            else
                state = PENDING;

            return state;
        }


        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Prevents outside instantiation of this class. This is esentially an Enum like the kind
        /// available in Java.
        /// </summary>
        /// <param name="id">The stored value of the line complete state.</param>
        /// <param name="name">The name of the line complete state.</param>
        private TransactionStateCON(char value, string name)
        {
            this.Value = value;
            this.Name = name;
        }
    }
}
