
using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class EnvelopeGroupDRM : DataRowModel
    {
        /// <summary>
        /// Local referance to the account type row this object is modeling.
        /// </summary>
        private FFDataSet.EnvelopeGroupRow EnvelopeGroupRow;

        /// <summary>
        /// Gets the ID of the account type.
        /// </summary>
        public int ID
        {
            get
            {
                return this.EnvelopeGroupRow.id;
            }
        }

        /// <summary>
        /// Gets or sets the name of the account type.
        /// </summary>
        public string Name 
        {
            get 
            {
                return this.EnvelopeGroupRow.name;
            }

            set
            {
                this.EnvelopeGroupRow.name = this.validLength(value, EnvelopeGroupCON.NameMaxLength);
            }
        }

        /// <summary>
        /// Gets or sets the minimum percentage for this envelope group.
        /// </summary>
        public decimal MinPercentage
        {
            get
            {
                return this.EnvelopeGroupRow.minPercent;
            }

            set
            {
                this.EnvelopeGroupRow.minPercent = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum percentage for this envelope group.
        /// </summary>
        public decimal MaxPercentage
        {
            get
            {
                return this.EnvelopeGroupRow.maxPercent;
            }

            set
            {
                this.EnvelopeGroupRow.maxPercent = value;
            }
        }






        /// <summary>
        /// Creates the object and keeps a local referance to the given Envelope Group row.
        /// </summary>
        /// <param name="aRow"></param>
        public EnvelopeGroupDRM(FFDataSet.EnvelopeGroupRow atRow)
        {
            this.EnvelopeGroupRow = atRow;
        }

        /// <summary>
        /// Creates the object and keeps a reference to a new Envelope Group row.
        /// </summary>
        /// <param name="name">The name of the new Envelope Group.</param>
        public EnvelopeGroupDRM(string name) : this(name, 0.0m, 0.0m)
        {
        }

        /// <summary>
        /// Creates the object and keeps a reference to a new Envelope Group row.
        /// </summary>
        /// <param name="name">The name of the new Envelope Group.</param>
        /// <param name="minPer">The minimum bound of the Envelope Group.</param>
        /// <param name="maxPer">The maximum bound of the Envelope Group.</param>
        public EnvelopeGroupDRM(string name, decimal minPer, decimal maxPer)
        {
            this.EnvelopeGroupRow = MyData.getInstance().EnvelopeGroup.NewEnvelopeGroupRow();

            this.EnvelopeGroupRow.id = MyData.getInstance().getNextID("EnvelopeGroup");
            this.Name = name;
            this.MinPercentage = minPer;
            this.MaxPercentage = maxPer;

            MyData.getInstance().EnvelopeGroup.AddEnvelopeGroupRow(this.EnvelopeGroupRow);
        }
    }
}
