using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// Creates the object and keeps a local referance to the given account type row.
        /// </summary>
        /// <param name="aRow"></param>
        public EnvelopeGroupDRM(FFDataSet.EnvelopeGroupRow atRow)
        {
            this.EnvelopeGroupRow = atRow;
        }

        /// <summary>
        /// Creates the object and keeps a reference to a new account type row.
        /// </summary>
        /// <param name="aRow"></param>
        public EnvelopeGroupDRM()
        {
            this.EnvelopeGroupRow = MyData.getInstance().EnvelopeGroup.NewEnvelopeGroupRow();

            this.EnvelopeGroupRow.id = MyData.getInstance().getNextID("EnvelopeGroup");
            this.EnvelopeGroupRow.name = "";
            this.EnvelopeGroupRow.minPercent = 0m;
            this.EnvelopeGroupRow.maxPercent = 0m;

            MyData.getInstance().EnvelopeGroup.AddEnvelopeGroupRow(this.EnvelopeGroupRow);
        }
    }
}
