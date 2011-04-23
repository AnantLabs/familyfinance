using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Data
{
    public class CatagoryCON
    {
        /// <summary>
        /// The initial value when an Account is made.
        /// </summary>
        public static CatagoryCON NULL = new CatagoryCON(0, "", "");

        /// <summary>
        /// The object to represent an account as an income.
        /// </summary>
        public static CatagoryCON INCOME = new CatagoryCON(1, "Income", "INC");

        /// <summary>
        /// The object to represent an account as an account.
        /// </summary>
        public static CatagoryCON ACCOUNT = new CatagoryCON(2, "Account", "ACC");

        /// <summary>
        /// The object to represent an account as an expence.
        /// </summary>
        public static CatagoryCON EXPENCE = new CatagoryCON(3, "Expence", "EXP");



        /// <summary>
        /// Gets the name of the given catagory id.
        /// </summary>
        /// <param name="id">The id of the catagory.</param>
        /// <returns>The name of the catagory.</returns>
        public static string getName(byte id)
        {

            if (id == CatagoryCON.NULL.ID)
                return CatagoryCON.NULL.Name;

            if (id == CatagoryCON.INCOME.ID)
                return CatagoryCON.INCOME.Name;

            if (id == CatagoryCON.ACCOUNT.ID)
                return CatagoryCON.ACCOUNT.Name;

            if (id == CatagoryCON.EXPENCE.ID)
                return CatagoryCON.EXPENCE.Name;

            throw new System.Exception("Invalid catagory id:" + id);
        }


        /// <summary>
        /// Gets the short name of the given catagory id.
        /// </summary>
        /// <param name="id">The id of the catagory.</param>
        /// <returns>The short name of the catagory.</returns>
        public static string getShortName(byte id)
        {

            if (id == CatagoryCON.NULL.ID)
                return CatagoryCON.NULL.ShortName;

            if (id == CatagoryCON.INCOME.ID)
                return CatagoryCON.INCOME.ShortName;

            if (id == CatagoryCON.ACCOUNT.ID)
                return CatagoryCON.ACCOUNT.ShortName;

            if (id == CatagoryCON.EXPENCE.ID)
                return CatagoryCON.EXPENCE.ShortName;

            throw new System.Exception("Invalid catagory id:" + id);
        }
        /// <summary>
        /// The id value of the catagory.
        /// </summary>
        private readonly byte _ID;

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

        /// <summary>
        /// The short name of the catagory
        /// </summary>
        private readonly string _ShortName;

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
        private CatagoryCON(byte id, string name, string sName)
        {
            this._ID = id;
            this._Name = name;
            this._ShortName = sName;
        }
    }
}
