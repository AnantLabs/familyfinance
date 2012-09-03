using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Data
{
    public class AccountCON
    {
        ///////////////////////////////////////////////////////////
        // Static Instances
        ///////////////////////////////////////////////////////////
        public static AccountCON NULL = new AccountCON(-1, " ");
        //public static AccountCON MULTIPLE = new AccountCON(-2, "-Multiple-");


        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        private readonly int _ID;
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        private readonly string _Name;
        public string Name
        {
            get
            {
                return this._Name;
            }
        }



        ///////////////////////////////////////////////////////////
        // Private Constructor
        ///////////////////////////////////////////////////////////
        private AccountCON(int id, string name)
        {
            this._ID = id;
            this._Name = name;
        }



        ///////////////////////////////////////////////////////////
        // Public Functions Constructor
        ///////////////////////////////////////////////////////////
        public override string ToString()
        {
            return this.Name;
        }

        public static bool isSpecial(int id)
        {
            if (id == NULL.ID) // || id == MULTIPLE.ID)
                return true;
            else
                return false;
        }



    }
}
