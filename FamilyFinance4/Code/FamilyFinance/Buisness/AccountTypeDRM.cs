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
                this.accountTypeRow.name = value;
            }
        }

        public AccountTypeDRM()
        {
            this.accountTypeRow = DataSetModel.Instance.NewAccountTypeRow();
        }

        public AccountTypeDRM(FFDataSet.AccountTypeRow atRow)
        {
            this.accountTypeRow = atRow;
        }

        public AccountTypeDRM(string accountTypeName)
        {
            this.accountTypeRow = DataSetModel.Instance.NewAccountTypeRow();
            this.Name = accountTypeName;
        }


        public void deleteRowFromDataset()
        {
            this.accountTypeRow.Delete();
        }
    }
}
