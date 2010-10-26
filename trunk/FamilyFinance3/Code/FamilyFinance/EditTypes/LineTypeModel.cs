using FamilyFinance.Database;
using FamilyFinance.Model;

namespace FamilyFinance.EditTypes
{
    /// <summary>
    /// Models the account type elements for reading and changing values.
    /// </summary>
    class LineTypeModel : ModelBase
    {
        /// <summary>
        /// Local referance to the account type row this object is modeling.
        /// </summary>
        private FFDataSet.LineTypeRow lineTypeRow;

        /// <summary>
        /// Gets the ID of this line type
        /// </summary>
        public int ID
        {
            get
            {
                return this.lineTypeRow.id;
            }
        }

        /// <summary>
        /// Gets or sets the name of the account type.
        /// </summary>
        public string Name
        {
            get
            {
                return this.lineTypeRow.name;
            }

            set
            {
                this.lineTypeRow.name = value;

                this.saveRow();
                this.RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Creates the object and keeps a local referance to the given account type row.
        /// </summary>
        /// <param name="aRow"></param>
        public LineTypeModel(FFDataSet.LineTypeRow row)
        {
            this.lineTypeRow = row;
        }

        /// <summary>
        /// Creates the object and keeps a reference to a new account type row.
        /// </summary>
        /// <param name="aRow"></param>
        public LineTypeModel()
        {
            this.lineTypeRow = MyData.getInstance().LineType.NewLineTypeRow();
            MyData.getInstance().LineType.AddLineTypeRow(this.lineTypeRow);
            this.saveRow();
        }

        private void saveRow()
        {
            MyData.getInstance().saveRow(this.lineTypeRow);
        }


    }
}
