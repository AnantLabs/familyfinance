using FamilyFinance.Database;

namespace FamilyFinance.Model
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
        /// Gets the ID of the account type.
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
                this.checkRowState();

                this.lineTypeRow.name = value;

                if (this.lineTypeRow.RowState == System.Data.DataRowState.Detached)
                    MyData.getInstance().LineType.AddLineTypeRow(this.lineTypeRow);

                MyData.getInstance().saveLineTypeRow(this.lineTypeRow);
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
        }

        private void checkRowState()
        {
            if (this.lineTypeRow.RowState == System.Data.DataRowState.Detached)
                MyData.getInstance().LineType.AddLineTypeRow(this.lineTypeRow);
        }


    }
}
