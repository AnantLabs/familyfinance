using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Data
{
    class LineCompleteCON
    {
        /// <summary>
        /// The initial value when an Account is made.
        /// </summary>
        public static LineCompleteCON CLEARED = new LineCompleteCON("C", "-Cleared-");

        /// <summary>
        /// The object to represent an account as an income.
        /// </summary>
        public static LineCompleteCON RECONSILED = new LineCompleteCON("R", "-Reconsiled-");

        /// <summary>
        /// The object to represent an account as an account.
        /// </summary>
        public static LineCompleteCON PENDING = new LineCompleteCON(" ", "-Pending-");

        /// <summary>
        /// The id value of the catagory.
        /// </summary>
        private readonly string _Value;

        /// <summary>
        /// Gets the ID of the catagory.
        /// </summary>
        public string Value
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
        /// <param name="id">The stored value of the line complete state.</param>
        /// <param name="name">The name of the line complete state.</param>
        private LineCompleteCON(string value, string name)
        {
            this._Value = value;
            this._Name = name;
        }
    }
}
