using FamilyFinance.Data;


namespace FamilyFinance.Buisness
{
    public class EnvelopeDRM : DataRowModel
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Local reference to the account row this object is modeling.
        /// </summary>
        private FFDataSet.EnvelopeRow envelopeRow;
       
        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the ID of the account.
        /// </summary>
        public int ID
        {
            get
            {
                return this.envelopeRow.id;
            }
        }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        public string Name 
        {
            get 
            {
                return this.envelopeRow.name;
            }

            set
            {
                this.envelopeRow.name = this.truncateIfNeeded(value, EnvelopeCON.NameMaxLength);
            }
        }

        /// <summary>
        /// Gets or sets the typeID of this account.
        /// </summary>
        public int GroupID
        {
            get 
            { 
                return this.envelopeRow.groupID; 
            }

            set
            {
                this.envelopeRow.groupID = value;
                this.RaisePropertyChanged("GroupName");
            }
        }

        /// <summary>
        /// Gets the type name of this account.
        /// </summary>
        public string GroupName
        {
            get
            {
                return this.envelopeRow.EnvelopeGroupRow.name;
            }
        }

        /// <summary>
        /// Gets or sets the Closed flag for this envelope. True if the envelope is closed, 
        /// false if the envelope is open.
        /// </summary>
        public bool Closed
        {
            get
            {
                return this.envelopeRow.closed;
            }

            set
            {
                this.envelopeRow.closed = value;
            }
        }

        /// <summary>
        /// Gets or sets the favorite account id.
        /// </summary>
        public int FavoriteAccountID
        {
            get 
            { 
                return this.envelopeRow.favoriteAccountID; 
            }

            set
            {
                this.envelopeRow.favoriteAccountID = value;
                this.RaisePropertyChanged("FavoriteAccountName");
            }
        }

        /// <summary>
        /// Gets the favorite account name.
        /// </summary>
        public string FavoriteAccountName
        {
            get
            {
                return this.envelopeRow.AccountRow.name;
            }
        }

        public int Priority
        {
            get 
            { 
                return this.envelopeRow.priority; 
            }

            set
            {
                this.envelopeRow.priority = value;
            }
        }

        public string Notes 
        {
            get 
            {
                return this.envelopeRow.notes;
            }

            set
            {
                this.envelopeRow.notes = this.truncateIfNeeded(value, EnvelopeCON.NotesMaxLength);
            }
        }

        public string Goal 
        {
            get 
            {
                return this.envelopeRow.goal;
            }

            set
            {
                this.envelopeRow.goal = this.truncateIfNeeded(value, EnvelopeCON.GoalMaxLength);
            }
        }
       
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////


        
        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public EnvelopeDRM(FFDataSet.EnvelopeRow eRow)
        {
            this.envelopeRow = eRow;
        }

        public EnvelopeDRM(string name, int groupID, int favoriteAccountID, bool closed)
        {
            this.envelopeRow = MyData.getInstance().Envelope.NewEnvelopeRow();

            this.envelopeRow.id = MyData.getInstance().getNextID("Envelope");
            this.Name = name;
            this.GroupID = groupID;
            this.FavoriteAccountID = favoriteAccountID;
            this.Closed = closed;
            this.Priority = this.envelopeRow.id;
            this.Notes = "";
            this.Goal = "";

            MyData.getInstance().Envelope.AddEnvelopeRow(this.envelopeRow);
        }

        public EnvelopeDRM() : this("", EnvelopeGroupCON.NULL.ID, AccountCON.NULL.ID, false)
        {
        }



    }
}
