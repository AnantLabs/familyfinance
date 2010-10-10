using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class EnvelopeGroupModel : ModelBase
    {
        /// <summary>
        /// Local referance to the envelope group row this object is modeling.
        /// </summary>
        private FFDataSet.EnvelopeGroupRow envelopeGroupRow;

        /// <summary>
        /// Gets the ID of the Envelope Group.
        /// </summary>
        public int ID
        {
            get
            {
                return this.envelopeGroupRow.id;
            }
        }

        /// <summary>
        /// Gets or sets the name of the envelope group.
        /// </summary>
        public string Name 
        {
            get 
            {
                return this.envelopeGroupRow.name;
            }

            set
            {
                this.checkRowState();

                this.envelopeGroupRow.name = value;

                if (this.envelopeGroupRow.RowState == System.Data.DataRowState.Detached)
                    MyData.getInstance().EnvelopeGroup.AddEnvelopeGroupRow(this.envelopeGroupRow);

                MyData.getInstance().saveEnvelopeGroupRow(this.envelopeGroupRow);
                this.RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Creates the object and keeps a local referance to the given envelope group row.
        /// </summary>
        /// <param name="aRow"></param>
        public EnvelopeGroupModel(FFDataSet.EnvelopeGroupRow egRow)
        {
            this.envelopeGroupRow = egRow;
        }

        /// <summary>
        /// Creates the object and keeps a reference to a new envelope group row.
        /// </summary>
        /// <param name="aRow"></param>
        public EnvelopeGroupModel()
        {
            this.envelopeGroupRow = MyData.getInstance().EnvelopeGroup.NewEnvelopeGroupRow();
        }

        private void checkRowState()
        {
            if (this.envelopeGroupRow.RowState == System.Data.DataRowState.Detached)
                MyData.getInstance().EnvelopeGroup.AddEnvelopeGroupRow(this.envelopeGroupRow);
        }



    }
}
