using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FamilyFinance.Data
{
    public class EnvelopeCON
    {
        ///////////////////////////////////////////////////////////
        // Java Style Enum Instances
        ///////////////////////////////////////////////////////////
        public static EnvelopeCON NULL = new EnvelopeCON(-1, " ");
        //public static EnvelopeCON SPLIT = new EnvelopeCON(-2, "-Split-");
        public static EnvelopeCON NO_ENVELOPE = new EnvelopeCON(0, "-No Envelope-");


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
        // Private Functions
        ///////////////////////////////////////////////////////////
        private EnvelopeCON(int id, string name)
        {
            this._ID = id;
            this._Name = name;
        }


        ///////////////////////////////////////////////////////////
        // Public Functions
        ///////////////////////////////////////////////////////////
        public override string ToString()
        {
            return this.Name;
        }
        
        public static bool isSpecial(int id)
        {
            //if (id == SPLIT.ID || id == NULL.ID || id == NO_ENVELOPE.ID)
            if (id == NULL.ID || id == NO_ENVELOPE.ID)
                return true;
            else
                return false;
        }

    }
}
