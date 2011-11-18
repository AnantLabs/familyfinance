

namespace FamilyFinance.Data
{
    public class TransactionTypeCON
    {

        ///////////////////////////////////////////////////////////
        // Static Instances
        ///////////////////////////////////////////////////////////
        public static TransactionTypeCON NULL = new TransactionTypeCON(-1, " ");


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

        public override string ToString()
        {
            return this.Name;
        }


        ///////////////////////////////////////////////////////////
        // Private Constructor
        ///////////////////////////////////////////////////////////
        private TransactionTypeCON(int id, string name)
        {
            this._ID = id;
            this._Name = name;
        }


        ///////////////////////////////////////////////////////////
        // Public static functions
        ///////////////////////////////////////////////////////////
        public static bool IsSpecial(int transTypeID)
        {
            if (transTypeID == TransactionTypeCON.NULL.ID)
                return true;
            else
                return false;
        }

    }
}
