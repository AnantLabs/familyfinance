using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class TransactionTypeDRM : DataRowModel
    {
        private FFDataSet.TransactionTypeRow TransactionTypeRow;

        public int ID
        {
            get
            {
                return this.TransactionTypeRow.id;
            }
        }

        public string Name 
        {
            get 
            {
                return this.TransactionTypeRow.name;
            }

            set
            {
                this.TransactionTypeRow.name = this.truncateIfNeeded(value, TransactionTypeCON.NameMaxLength);
            }
        }

        public TransactionTypeDRM(FFDataSet.TransactionTypeRow ttRow)
        {
            this.TransactionTypeRow = ttRow;
        }

        public TransactionTypeDRM()
        {
            this.TransactionTypeRow = DataSetModel.Instance.NewTransactionTypeRow();
        }
    }
}
