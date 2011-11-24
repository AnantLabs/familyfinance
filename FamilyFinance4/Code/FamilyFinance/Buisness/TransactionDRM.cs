using FamilyFinance.Data;
using System;

namespace FamilyFinance.Buisness
{
    public class TransactionDRM : DataRowModel
    {
        private FFDataSet.TransactionRow transactionRow;


        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        public int TransactionID
        {
            get
            {
                return transactionRow.id;
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
                this.reportPropertyChangedWithName("TypeName");
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


        public bool IsTransactionError
        {
            get
            {
                return this.isTransactionError();
            }
        }

        public decimal CreditSum
        {
            get
            {
                return this.getCreditSum();
            }
        }

        public decimal DebitSum
        {
            get
            {
                return this.getDebitSum();
            }
        }



        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private bool isOneSidedTransaction()
        {
            int size = transactionRow.GetLineItemRows().Length;

            if (size == 1)
                return true;
            else
                return false;
        }

        private bool isTransactionBalanced()
        {
            decimal creditSum = 0;
            decimal debitSum = 0;

            foreach (FFDataSet.LineItemRow line in this.transactionRow.GetLineItemRows())
            {
                if (line.polarity == PolarityCON.CREDIT.Value)
                    creditSum += line.amount;
                else
                    debitSum += line.amount;
            }

            if (creditSum == debitSum)
                return true;
            else
                return false;
        }

        private bool isTransactionError()
        {
            if (isOneSidedTransaction() || isTransactionBalanced())
                return false;

            else
                return true;
        }

        private decimal getCreditSum()
        {
               decimal creditSum = 0;

                foreach (FFDataSet.LineItemRow line in this.transactionRow.GetLineItemRows())
                    if (line.polarity == PolarityCON.CREDIT.Value)
                        creditSum += line.amount;

                return creditSum;
        }

        private decimal getDebitSum()
        {
            decimal debitSum = 0;

            foreach (FFDataSet.LineItemRow line in this.transactionRow.GetLineItemRows())
                if (line.polarity == PolarityCON.DEBIT.Value)
                    debitSum += line.amount;

            return debitSum;
        }



        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public TransactionDRM()
        {
            this.transactionRow = DataSetModel.Instance.NewTransactionRow();
        }

        public TransactionDRM(FFDataSet.TransactionRow tRow)
        {
            this.transactionRow = tRow;
        }

        public TransactionDRM(int transactionID)
        {
            this.transactionRow = DataSetModel.Instance.getTransactionRowWithID(transactionID);
        }


        protected FFDataSet.LineItemRow[] getLineItemRows()
        {
            return this.transactionRow.GetLineItemRows();
        }

        public void retportDependantPropertiesChanged()
        {
            this.reportPropertyChangedWithName("IsTransactionError");
            this.reportPropertyChangedWithName("CreditSum");
            this.reportPropertyChangedWithName("DebitSum");
        }

        public void delete()
        {
            this.transactionRow.Delete();
        }

    }
}
