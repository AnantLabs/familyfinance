using System;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    /// <summary>
    /// 
    /// </summary>
    public class TransactionDRM : DataRowModel
    {
        ///////////////////////////////////////////////////////////////////////////////////////////
        // Local Variables
        ///////////////////////////////////////////////////////////////////////////////////////////
        private FFDataSet.TransactionRow transactionRow;


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////////////////////////
        public int TransactionID
        {
            get
            {
                return this.transactionRow.id;
            }
        }

        public DateTime Date
        {
            get
            {
                return this.transactionRow.date;
            }

            set
            {
                this.transactionRow.date = value;
            }
        }

        public int TypeID
        {
            get
            {
                return this.transactionRow.typeID;
            }

            set
            {
                this.transactionRow.typeID = value;
            }
        }

        public string TypeName
        {
            get
            {
                return this.transactionRow.TransactionTypeRow.name;
            }
        }

        public string Description
        {
            get
            {
                return this.transactionRow.description;
            }
            set
            {
                this.transactionRow.description = value;
            }
        }

        public TransactionStateCON State
        {
            get
            {
                return TransactionStateCON.GetState(this.transactionRow.state);
            }
            set
            {
                this.transactionRow.state = value.Value;
            }
        }

        public bool IsTransactionError
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Public Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        public TransactionDRM()
        {
            this.transactionRow = DataSetModel.Instance.NewTransactionRow();
        }

        public TransactionDRM(FFDataSet.TransactionRow tRow)
        {
            this.transactionRow = tRow;
        }

    }
}
