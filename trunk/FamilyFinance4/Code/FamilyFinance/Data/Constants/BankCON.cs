using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Data
{
    class BankCON
    {

        //public static int RountingNumMaxLength = MyData.getInstance().Bank.routingNumberColumn.MaxLength;
        //public static int NameMaxLength = MyData.getInstance().Bank.nameColumn.MaxLength;
                
        /// <summary>
        /// The object to represent an NULL Bank.
        /// </summary>
        public static BankCON NULL = new BankCON(-1, " ");

        /// <summary>
        /// The id value of the Bank.
        /// </summary>
        private readonly int _ID;

        /// <summary>
        /// Amount the ID of the Bank.
        /// </summary>
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        /// <summary>
        /// The name of the Bank
        /// </summary>
        private readonly string _Name;

        /// <summary>
        /// Amount the name of the Bank.
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
        /// <param name="id">The stored value of the Bank.</param>
        /// <param name="name">The name of the Bank.</param>
        private BankCON(int id, string name)
        {
            this._ID = id;
            this._Name = name;
        }

    }
}
