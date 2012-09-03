using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Data
{
    class AccountTypeCON
    {

        /// <summary>
        /// The object to represent an NULL Account Type.
        /// </summary>
        public static AccountTypeCON NULL = new AccountTypeCON(-1, " ");

        /// <summary>
        /// The id value of the account type.
        /// </summary>
        private readonly int _ID;

        /// <summary>
        /// Amount the ID of the Line Type.
        /// </summary>
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        /// <summary>
        /// The name of the account
        /// </summary>
        private readonly string _Name;

        /// <summary>
        /// Amount the name of the account.
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
        private AccountTypeCON(int id, string name)
        {
            this._ID = id;
            this._Name = name;
        }

    }
}
