using FamilyFinance.Database;
using FamilyFinance.Model;

namespace FamilyFinance.EditTypes
{
    /// <summary>
    /// Models the account type elements for reading and changing values.
    /// </summary>
    class AccountTypeModel : ModelBase
    {
        /// <summary>
        /// Local referance to the account type row this object is modeling.
        /// </summary>
        private FFDataSet.AccountTypeRow accountTypeRow;

        /// <summary>
        /// Gets the ID of the account type.
        /// </summary>
        public int ID
        {
            get
            {
                return this.accountTypeRow.id;
            }
        }

        /// <summary>
        /// Gets or sets the name of the account type.
        /// </summary>
        public string Name 
        {
            get 
            {
                return this.accountTypeRow.name;
            }

            set
            {
                this.accountTypeRow.name = value;

                this.saveRow();
                this.RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Creates the object and keeps a local referance to the given account type row.
        /// </summary>
        /// <param name="aRow"></param>
        public AccountTypeModel(FFDataSet.AccountTypeRow atRow)
        {
            this.accountTypeRow = atRow;
        }

        /// <summary>
        /// Creates the object and keeps a reference to a new account type row.
        /// </summary>
        /// <param name="aRow"></param>
        public AccountTypeModel()
        {
            this.accountTypeRow = MyData.getInstance().AccountType.NewAccountTypeRow();
            MyData.getInstance().AccountType.AddAccountTypeRow(this.accountTypeRow);
            this.saveRow();
        }

        private void saveRow()
        {
            MyData.getInstance().saveRow(this.accountTypeRow);
        }


    }
}
