using FamilyFinance.Database;
using FamilyFinance.Model;

namespace FamilyFinance.EditTypes
{
    /// <summary>
    /// Models the Bank objects.
    /// </summary>
    class BankModel : ModelBase
    {
        /// <summary>
        /// Local referance to the account row this object is modeling.
        /// </summary>
        private FFDataSet.BankRow bankRow;

        /// <summary>
        /// Gets the ID of the Bank.
        /// </summary>
        public int ID
        {
            get
            {
                return this.bankRow.id;
            }
        }
  
        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        public string Name 
        {
            get 
            {
                return this.bankRow.name;
            }

            set
            {
                this.bankRow.name = value;

                this.saveRow();
                this.RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets the banks routing number.
        /// </summary>
        public string RoutingNumber
        {
            get
            {
                return this.bankRow.routingNumber;
            }

            set
            {
                this.bankRow.routingNumber = value;

                this.saveRow();
                this.RaisePropertyChanged("RoutingNumber");
                
            }
        }

        /// <summary>
        /// Creates the object and keeps a local referance to the given bank row.
        /// </summary>
        /// <param name="bRow"></param>
        public BankModel(FFDataSet.BankRow bRow)
        {
            this.bankRow = bRow;
        }

        /// <summary>
        /// Creates the object and keeps a reference to a new account type row.
        /// </summary>
        /// <param name="aRow"></param>
        public BankModel()
        {
            this.bankRow = MyData.getInstance().Bank.NewBankRow();
            MyData.getInstance().Bank.AddBankRow(this.bankRow);
            this.saveRow();
        }

        private void saveRow()
        {
            MyData.getInstance().saveRow(this.bankRow);
        }

    }
}
