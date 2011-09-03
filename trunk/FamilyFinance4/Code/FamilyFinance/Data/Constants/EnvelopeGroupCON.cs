using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Data
{
    class EnvelopeGroupCON
    {

        //public static int NameMaxLength = MyData.getInstance().EnvelopeGroup.nameColumn.MaxLength;
                
        /// <summary>
        /// The object to represent an NULL envelope group.
        /// </summary>
        public static EnvelopeGroupCON NULL = new EnvelopeGroupCON(-1, " ");

        /// <summary>
        /// The id value of the envelope group.
        /// </summary>
        private readonly int _ID;

        /// <summary>
        /// Amount the ID of the envelope group.
        /// </summary>
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        /// <summary>
        /// The name of the envelope group
        /// </summary>
        private readonly string _Name;

        /// <summary>
        /// Amount the name of the envelope group.
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
        /// <param name="id">The stored value of the envelope group.</param>
        /// <param name="name">The name of the envelope group.</param>
        private EnvelopeGroupCON(int id, string name)
        {
            this._ID = id;
            this._Name = name;
        }

    }
}
