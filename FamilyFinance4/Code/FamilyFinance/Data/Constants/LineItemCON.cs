
namespace FamilyFinance.Data
{
    class LineItemCON
    {
                
        ///////////////////////////////////////////////////////////
        // Java Style Enum Instances
        ///////////////////////////////////////////////////////////
        public static LineItemCON NULL = new LineItemCON(-1);


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
        private LineItemCON(int id)
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
