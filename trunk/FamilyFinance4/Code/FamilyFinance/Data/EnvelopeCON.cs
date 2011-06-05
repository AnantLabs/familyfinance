using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Data
{
    class EnvelopeCON
    {


        public static int NameMaxLength = MyData.getInstance().Envelope.nameColumn.MaxLength;
        public static int NotesMaxLength = MyData.getInstance().Envelope.notesColumn.MaxLength;
        public static int GoalMaxLength = MyData.getInstance().Envelope.goalColumn.MaxLength;


        /// <summary>
        /// The object to represent an NULL envelope.
        /// </summary>
        public static EnvelopeCON NULL = new EnvelopeCON(-1, " ");
        public static EnvelopeCON SPLIT = new EnvelopeCON(-2, "-Split-");
        public static EnvelopeCON NO_ENVELOPE = new EnvelopeCON(0, "-No Envelope-");

        /// <summary>
        /// The id value of the envelope.
        /// </summary>
        private readonly int _ID;

        /// <summary>
        /// Gets the ID of the envelope.
        /// </summary>
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        /// <summary>
        /// The name of the envelope
        /// </summary>
        private readonly string _Name;

        /// <summary>
        /// Gets the name of the envelope.
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
        
        public static bool isSpecial(int id)
        {
            if (id == SPLIT.ID || id == NULL.ID || id == NO_ENVELOPE.ID)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Prevents outside instantiation of this class. This is esentially an Enum like the kind
        /// available in Java.
        /// </summary>
        /// <param name="id">The stored value of the envelope.</param>
        /// <param name="name">The name of the envelope.</param>
        private EnvelopeCON(int id, string name)
        {
            this._ID = id;
            this._Name = name;
        }

    }
}
