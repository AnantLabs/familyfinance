using FamilyFinance.Model;
using FamilyFinance.Database;

namespace FamilyFinance.EditEnvelopes
{
    class EnvelopeModel : ModelBase
    {
        /// <summary>
        /// Local referance to the account row this object is modeling.
        /// </summary>
        private FFDataSet.EnvelopeRow envelopeRow;

        /// <summary>
        /// Gets the ID of the envelope.
        /// </summary>
        public int ID
        {
            get
            {
                return this.envelopeRow.id;
            }
        }

        /// <summary>
        /// Gets or sets the name of the envelope.
        /// </summary>
        public string Name 
        {
            get 
            {
                return this.envelopeRow.name;
            }

            set
            {
                this.envelopeRow.name = value;

                saveRow();
                this.RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets the groupID of this envelope.
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

                saveRow();
                this.RaisePropertyChanged("GroupID");
                this.RaisePropertyChanged("GroupName");
            }
        }

        /// <summary>
        /// Gets the group name of this envelope.
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
        /// false if the account is open.
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

                saveRow();
                this.RaisePropertyChanged("Closed");
            }
        }

        /// <summary>
        /// Gets or sets the Catagory of this account.
        /// </summary>
        public int AccountID
        {
            get
            {
                return this.envelopeRow.accountID;
            }

            set
            {
                this.envelopeRow.accountID = value;

                saveRow();
                this.RaisePropertyChanged("AccountID");
                this.RaisePropertyChanged("AccountName");
            }
        }
        
        /// <summary>
        /// Gets the favorite account name of this envelope.
        /// </summary>
        public string AccountName
        {
            get
            {
                return this.envelopeRow.AccountRow.name;
            }
        }

        public int PriorityOrder
        {
            get
            {
                return this.envelopeRow.priorityOrder;
            }

            set
            {
                this.envelopeRow.priorityOrder = value;

                this.saveRow();
                this.RaisePropertyChanged("PriorityOrder");
            }
        }

        public string Notes
        {
            get
            {
                if (this.envelopeRow.IsnotesNull())
                    return "";
                else
                    return this.envelopeRow.notes;
            }

            set
            {
                this.envelopeRow.notes = value;

                this.saveRow();
                this.RaisePropertyChanged("Notes");
            }
        }

        public decimal Step
        {
            get
            {
                return this.envelopeRow.step;
            }

            set
            {
                this.envelopeRow.step = value;

                this.saveRow();
                this.RaisePropertyChanged("Step");
            }
        }

        public decimal Cap
        {
            get
            {
                return this.envelopeRow.cap;
            }

            set
            {
                this.envelopeRow.cap = value;

                this.saveRow();
                this.RaisePropertyChanged("Cap");
            }
        }

        public System.DateTime NextDate
        {
            get
            {
                return this.envelopeRow.nextDate;
            }

            set
            {
                this.envelopeRow.nextDate = value;

                this.saveRow();
                this.RaisePropertyChanged("NextDate");
            }
        }

        public System.DateTime IntervalDate
        {
            get
            {
                return this.envelopeRow.intervalDate;
            }

            set
            {
                this.envelopeRow.intervalDate = value;

                this.saveRow();
                this.RaisePropertyChanged("IntervalDate");
            }
        }

        /// <summary>
        /// Creates the object and keeps a local referance to the given account row.
        /// </summary>
        /// <param name="aRow"></param>
        public EnvelopeModel(FFDataSet.EnvelopeRow eRow)
        {
            this.envelopeRow = eRow;
        }

        /// <summary>
        /// Creates the object with a referance to a new account row.
        /// </summary>
        public EnvelopeModel()
        {
            this.envelopeRow = MyData.getInstance().Envelope.NewEnvelopeRow();
            MyData.getInstance().Envelope.AddEnvelopeRow(this.envelopeRow);
            this.saveRow();
        }

        private void saveRow()
        {
            MyData.getInstance().saveRow(this.envelopeRow);
        }
    }
}
