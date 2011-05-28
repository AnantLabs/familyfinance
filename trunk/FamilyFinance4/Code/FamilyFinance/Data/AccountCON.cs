using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Data
{
    class AccountCON
    {

        public static int NameMaxLength = MyData.getInstance().Account.nameColumn.MaxLength;
        public static int AccountNumberMaxLength = MyData.getInstance().BankInfo.accountNumberColumn.MaxLength;

        /// <summary>
        /// The object to represent an NULL account.
        /// </summary>
        public static AccountCON NULL = new AccountCON(-1, " ");
        public static AccountCON MULTIPLE = new AccountCON(-2, "-Multiple-");

        /// <summary>
        /// The id value of the account.
        /// </summary>
        private readonly int _ID;

        /// <summary>
        /// Gets the ID of the account.
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
        /// Gets the name of the account.
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


        public static bool isNotSpecial(int id)
        {
            if (id > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Prevents outside instantiation of this class. This is esentially an Enum like the kind
        /// available in Java.
        /// </summary>
        /// <param name="id">The stored value of the account.</param>
        /// <param name="name">The name of the account.</param>
        private AccountCON(int id, string name)
        {
            this._ID = id;
            this._Name = name;
        }

    }
}
