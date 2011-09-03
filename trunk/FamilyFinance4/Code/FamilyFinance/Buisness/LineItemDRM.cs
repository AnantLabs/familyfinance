using System;
using System.Collections.ObjectModel;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class LineItemDRM : DataRowModel
    {

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Local Variables
        ///////////////////////////////////////////////////////////////////////////////////////////
        private FFDataSet.LineItemRow lineItemRow;


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////////////////////////
        public int LineID
        {
            get
            {
                return this.lineItemRow.id;
            }
        }

        public int TransactionID
        {
            get
            {
                return this.lineItemRow.transactionID;
            }
            set
            {
                this.lineItemRow.transactionID = value;
            }
        }

        public int AccountID
        {
            get
            {
                return this.lineItemRow.accountID;
            }

            set
            {
                this.lineItemRow.accountID = value;
            }
        }

        public string AccountName
        {
            get
            {
                return this.lineItemRow.AccountRow.name;
            }
        }

        public string ConfirmationNumber
        {
            get
            {
                return this.lineItemRow.confirmationNumber;
            }
            set
            {
                this.lineItemRow.confirmationNumber = value.Substring(0, LineItemCON.ConfirmationNumberMaxLength);
            }
        }

        public decimal Amount
        {
            get
            {
                return this.lineItemRow.amount;
            }
            set
            {
                if(value < 0.0m)
                    value = Decimal.Negate(value);

                this.lineItemRow.amount = Decimal.Round(value, 2);
            }
        }

        public PolarityCON Polarity
        {
            get
            {
                return PolarityCON.GetPlolartiy(this.lineItemRow.polarity);
            }
            set
            {
                this.lineItemRow.polarity = value.Value;
            }
        }

        public bool IsLineError
        {
            get
            {
                return false;
            }
        }


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Public Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        public LineItemDRM(FFDataSet.LineItemRow linRow)
        {
            InputValidator.CheckNotNull(linRow, "FFDataSet.LineItemRow");

            this.lineItemRow = linRow;
        }

        public LineItemDRM(FFDataSet.TransactionRow transactionRow) 
        {
            InputValidator.CheckNotNull(transactionRow, "FFDataSet.TransactionRow");

            this.lineItemRow = DataSetModel.Instance.NewLineItemRow(transactionRow);
        }


    }
}
