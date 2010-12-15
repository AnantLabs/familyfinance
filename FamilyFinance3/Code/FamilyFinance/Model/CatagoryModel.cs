namespace FamilyFinance.Model
{
    class CatagoryModel
    {

        /// <summary>
        /// The initial value when an Account is made.
        /// </summary>
        public static CatagoryModel NULL = new CatagoryModel(0, "", "");

        /// <summary>
        /// The object to represent an account as an income.
        /// </summary>
        public static CatagoryModel INCOME = new CatagoryModel(1, "Income", "INC");

        /// <summary>
        /// The object to represent an account as an account.
        /// </summary>
        public static CatagoryModel ACCOUNT = new CatagoryModel(2, "Account", "ACC");

        /// <summary>
        /// The object to represent an account as an expence.
        /// </summary>
        public static CatagoryModel EXPENCE = new CatagoryModel(3, "Expence", "EXP");



        /// <summary>
        /// Gets the name of the given catagory id.
        /// </summary>
        /// <param name="id">The id of the catagory.</param>
        /// <returns>The name of the catagory.</returns>
        public static string getName(byte id)
        {

            if (id == CatagoryModel.NULL.ID)
                return CatagoryModel.NULL.Name;

            if (id == CatagoryModel.INCOME.ID)
                return CatagoryModel.INCOME.Name;

            if (id == CatagoryModel.ACCOUNT.ID)
                return CatagoryModel.ACCOUNT.Name;

            if (id == CatagoryModel.EXPENCE.ID)
                return CatagoryModel.EXPENCE.Name;

            throw new System.Exception("Invalid catagory id:" + id);
        }

        
        /// <summary>
        /// Gets the short name of the given catagory id.
        /// </summary>
        /// <param name="id">The id of the catagory.</param>
        /// <returns>The short name of the catagory.</returns>
        public static string getShortName(byte id)
        {

            if (id == CatagoryModel.NULL.ID)
                return CatagoryModel.NULL.ShortName;

            if (id == CatagoryModel.INCOME.ID)
                return CatagoryModel.INCOME.ShortName;

            if (id == CatagoryModel.ACCOUNT.ID)
                return CatagoryModel.ACCOUNT.ShortName;

            if (id == CatagoryModel.EXPENCE.ID)
                return CatagoryModel.EXPENCE.ShortName;

            throw new System.Exception("Invalid catagory id:" + id);
        }
        /// <summary>
        /// The id value of the catagory.
        /// </summary>
        public readonly byte _ID;

        /// <summary>
        /// Gets the ID of the catagory.
        /// </summary>
        public byte ID
        {
            get
            {
                return this._ID;
            }
        }

        /// <summary>
        /// The name of the catagory
        /// </summary>
        public readonly string _Name;

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

        /// <summary>
        /// The short name of the catagory
        /// </summary>
        public readonly string _ShortName;

        /// <summary>
        /// Gets the short name of the catagory.
        /// </summary>
        public string ShortName
        {
            get
            {
                return this._ShortName;
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
        /// <param name="id">The id of the catagory.</param>
        /// <param name="name">The name of the catagory.</param>
        /// <param name="sName">The short name of the catagory</param>
        private CatagoryModel(byte id, string name, string sName)
        {
            this._ID = id;
            this._Name = name;
            this._ShortName = sName;
        }
    }
}
