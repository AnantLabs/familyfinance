
using FamilyFinance.Data;
using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class AccountTypeDRM : DataRowModel
    {
        private FFDataSet.AccountTypeRow accountTypeRow;

        public int ID
        {
            get
            {
                return this.accountTypeRow.id;
            }
        }

        public string Name 
        {
            get 
            {
                return this.accountTypeRow.name;
            }

            set
            {
                this.accountTypeRow.name = this.truncateIfNeeded(value, AccountTypeCON.NameMaxLength);
            }
        }

        public AccountTypeDRM(FFDataSet.AccountTypeRow atRow)
        {
            this.accountTypeRow = atRow;
        }

        public AccountTypeDRM(string name)
        {
            this.accountTypeRow = DataSetModel.Instance.NewAccountTypeRow(name);
        }

        public AccountTypeDRM() : this("")
        {

        }

    }
}
