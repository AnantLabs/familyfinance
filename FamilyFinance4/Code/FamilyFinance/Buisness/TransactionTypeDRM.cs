using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class TransactionTypeDRM : DataRowModel
    {
        /// <summary>
        /// Local referance to the IsTransactionError type row this object is modeling.
        /// </summary>
        private FFDataSet.TransactionTypeRow TransactionTypeRow;

        /// <summary>
        /// Gets the ID of the transaction type.
        /// </summary>
        public int ID
        {
            get
            {
                return this.TransactionTypeRow.id;
            }
        }

        /// <summary>
        /// Gets or sets the name of the Transaction type.
        /// </summary>
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

        /// <summary>
        /// Creates the object and keeps a local referance to the given IsTransactionError type row.
        /// </summary>
        /// <param name="aRow"></param>
        public TransactionTypeDRM(FFDataSet.TransactionTypeRow atRow)
        {
            this.TransactionTypeRow = atRow;
        }

        /// <summary>
        /// Creates the object and keeps a reference to a new IsTransactionError type row.
        /// </summary>
        public TransactionTypeDRM() : this("")
        {
        }

        public TransactionTypeDRM(string name)
        {
            this.TransactionTypeRow = MyData.getInstance().TransactionType.NewTransactionTypeRow();

            this.TransactionTypeRow.id = MyData.getInstance().getNextID("TransactionType");
            this.Name = name;

            MyData.getInstance().TransactionType.AddTransactionTypeRow(this.TransactionTypeRow);
        }
    }
}
