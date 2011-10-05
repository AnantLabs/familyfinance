
namespace FamilyFinance.Data
{
    class TransactionCON
    {
               
        ///////////////////////////////////////////////////////////
        // Java Style Enum Instances
        ///////////////////////////////////////////////////////////
        public static TransactionCON NULL = new TransactionCON(-1);


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

        ///////////////////////////////////////////////////////////
        // Private Functions
        ///////////////////////////////////////////////////////////
        private TransactionCON(int id)
        {
            this._ID = id;
        }


        ///////////////////////////////////////////////////////////
        // Public Functions
        ///////////////////////////////////////////////////////////
        public static bool isSpecial(int id)
        {
            if (id == NULL.ID)
                return true;
            else
                return false;
        }
    }
}
