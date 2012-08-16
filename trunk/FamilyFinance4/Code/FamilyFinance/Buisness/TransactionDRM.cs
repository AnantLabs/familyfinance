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
                if (isOneSidedTransaction() || isTransactionBalanced())
                    return false;

                else
                    return true;
            }
        }

        public decimal CreditSum
        {
            get
            {
                decimal creditSum = 0;

                foreach (FFDataSet.LineItemRow line in this.transactionRow.GetLineItemRows())
                    if (line.polarity == PolarityCON.CREDIT.Value)
                        creditSum += line.amount;

                return creditSum;
            }
        }

        public decimal DebitSum
        {
            get
            {
                decimal debitSum = 0;

                foreach (FFDataSet.LineItemRow line in this.transactionRow.GetLineItemRows())
                    if (line.polarity == PolarityCON.DEBIT.Value)
                        debitSum += line.amount;

                return debitSum;
            }
        }

        public bool canChangeOppositeAccount(LineItemDRM compareLine)
        {
            if (isOneSidedTransaction() || isOppositeSideSimple(compareLine))
                return true;
            else
                return false;
        }

        public void changeOppositeAccount(LineItemDRM compareLine)
        {
            FFDataSet.LineItemRow oppLine;

            if (isOneSidedTransaction())
            {
                oppLine = this.getLineItemRows()[0];
            }
            else if (isOppositeSideSimple(compareLine))
            {

            }
            else
                throw new Exception("Can't change the opposite account of a complex transaction.");
               
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

        public bool isOppositeSideSimple(LineItemDRM compareLine)
        {
            int count = 0;

            foreach (FFDataSet.LineItemRow line in this.transactionRow.GetLineItemRows())
                if (line.polarity != compareLine.Polarity.Value)
                    count++;

            if (count == 1)
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


        protected FFDataSet.LineItemRow[] getLineItemRows()
        {
            return this.transactionRow.GetLineItemRows();
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



        public void retportDependantLineBalanceChanged()
        {
            this.reportPropertyChangedWithName("CreditSum");
            this.reportPropertyChangedWithName("DebitSum");
            this.reportPropertyChangedWithName("IsTransactionError");
        }

        public void delete()
        {
            this.transactionRow.Delete();
        }

    }
}
