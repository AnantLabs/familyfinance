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
                this.envelopeRow.name = value;
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
                this.reportPropertyChangedWithName("GroupName");
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
                this.reportPropertyChangedWithName("FavoriteAccountName");
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
                this.envelopeRow.notes = value;
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
                this.envelopeRow.goal = value;
            }
        }
       
        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public EnvelopeDRM()
        {
            this.envelopeRow = DataSetModel.Instance.NewEnvelopeRow();
        }

        public EnvelopeDRM(FFDataSet.EnvelopeRow eRow)
        {
            this.envelopeRow = eRow;
        }

    }
}
