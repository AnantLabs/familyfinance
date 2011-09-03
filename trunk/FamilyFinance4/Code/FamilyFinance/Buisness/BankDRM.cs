using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class BankDRM : DataRowModel
    {
        private FFDataSet.BankRow bankRow;

        public int ID
        {
            get
            {
                return this.bankRow.id;
            }
        }
  
        public string Name 
        {
            get 
            {
                return this.bankRow.name;
            }

            set
            {
                this.bankRow.name = this.truncateIfNeeded(value, BankCON.NameMaxLength);
                this.RaisePropertyChanged("Name");
            }
        }

        public string RoutingNumber
        {
            get
            {
                return this.bankRow.routingNumber;
            }

            set
            {
                this.bankRow.routingNumber = this.truncateIfNeeded(value, BankCON.RountingNumMaxLength);
            }
        }

        public BankDRM()
        {
            this.bankRow = DataSetModel.Instance.NewBankRow("", "");
        }

        public BankDRM(FFDataSet.BankRow bRow)
        {
            this.bankRow = bRow;
        }

    }
}
