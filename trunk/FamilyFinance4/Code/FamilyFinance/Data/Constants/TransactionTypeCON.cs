

namespace FamilyFinance.Data
{
    public class TransactionTypeCON
    {

        //public static int NameMaxLength = MyData.getInstance().TransactionType.nameColumn.MaxLength;
                
        /// <summary>
        /// The object to represent an NULL Line Type.
        /// </summary>
        public static TransactionTypeCON NULL = new TransactionTypeCON(-1, " ");

        /// <summary>
        /// The id value of the line type.
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
        /// The name of the catagory
        /// </summary>
        private readonly string _Name;

        /// <summary>
        /// Amount the name of the catagory.
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
        private TransactionTypeCON(int id, string name)
        {
            this._ID = id;
            this._Name = name;
        }

    }
}
