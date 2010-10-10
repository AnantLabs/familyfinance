//using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;

using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class EnvelopeModel : ModelBase
    {
        /// <summary>
        /// Local referance to the account row this object is modeling.
        /// </summary>
        protected FFDataSet.EnvelopeRow envelopeRow;

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
                checkRowState();

                this.envelopeRow.name = value;
                MyData.getInstance().saveEnvelopeRow(this.envelopeRow);
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
                checkRowState();

                this.envelopeRow.groupID = value;
                MyData.getInstance().saveEnvelopeRow(this.envelopeRow);
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
                checkRowState();

                this.envelopeRow.closed = value;
                MyData.getInstance().saveEnvelopeRow(this.envelopeRow);
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
                checkRowState();

                this.envelopeRow.accountID = value;
                MyData.getInstance().saveEnvelopeRow(this.envelopeRow);
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
        }

        protected void checkRowState()
        {
            if (this.envelopeRow.RowState == System.Data.DataRowState.Detached)
                MyData.getInstance().Envelope.AddEnvelopeRow(this.envelopeRow);
        }


    }
}
