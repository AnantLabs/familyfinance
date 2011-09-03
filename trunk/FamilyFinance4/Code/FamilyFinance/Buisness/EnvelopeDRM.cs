using FamilyFinance.Data;


namespace FamilyFinance.Buisness
{
    public class EnvelopeDRM : DataRowModel
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////
        private FFDataSet.EnvelopeRow envelopeRow;
       
        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        public int ID
        {
            get
            {
                return this.envelopeRow.id;
            }
        }

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

        public string GroupName
        {
            get
            {
                return this.envelopeRow.EnvelopeGroupRow.name;
            }
        }

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
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public EnvelopeDRM() : this("", EnvelopeGroupCON.NULL.ID, AccountCON.NULL.ID, false)
        {
        }

        public EnvelopeDRM(string name, int groupID, int favoriteAccountID, bool closed)
        {
            this.envelopeRow = DataSetModel.Instance.NewEnvelopeRow(name, groupID, favoriteAccountID, closed, "", "");
        }

        public EnvelopeDRM(FFDataSet.EnvelopeRow eRow)
        {
            this.envelopeRow = eRow;
        }

    }
}
