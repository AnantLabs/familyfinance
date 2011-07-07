﻿using System;
using System.Collections.ObjectModel;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class LineItemDRM : DataRowModel
    {

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Local Variables
        ///////////////////////////////////////////////////////////////////////////////////////////
        private FFDataSet.LineItemRow _lineItemRow;


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////////////////////////
        public int LineID
        {
            get
            {
                return this._lineItemRow.id;
            }
        }

        public int TransactionID
        {
            get
            {
                return this._lineItemRow.transactionID;
            }
            set
            {
                this._lineItemRow.transactionID = value;
            }
        }

        public int AccountID
        {
            get
            {
                return this._lineItemRow.accountID;
            }

            set
            {
                this._lineItemRow.accountID = value;
            }
        }

        public string AccountName
        {
            get
            {
                return this._lineItemRow.AccountRow.name;
            }
        }

        public string ConfirmationNumber
        {
            get
            {
                return this._lineItemRow.confirmationNumber;
            }
            set
            {
                this._lineItemRow.confirmationNumber = value.Substring(0, LineItemCON.ConfirmationNumberMaxLength);
            }
        }

        public decimal Amount
        {
            get
            {
                return this._lineItemRow.amount;
            }
            set
            {
                if(value < 0.0m)
                    value = Decimal.Negate(value);

                this._lineItemRow.amount = Decimal.Round(value, 2);
            }
        }

        public PolarityCON Polarity
        {
            get
            {
                return PolarityCON.GetPlolartiy(this._lineItemRow.polarity);
            }
            set
            {
                this._lineItemRow.polarity = value.Value;
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

            this._lineItemRow = linRow;
        }

        //public LineItemDRM(int transactionID) 
        //    : this(transactionID, AccountCON.NULL.ID, "", 0.0m, PolarityCON.CREDIT)
        //{
        //}

        //public LineItemDRM() 
        //    : this(LineItemDRM._TransactionID, AccountCON.NULL.ID, "", 0.0m, PolarityCON.CREDIT)
        //{
        //}

        public LineItemDRM(TransactionDRM transDRM, int accountID, string confrimationNum, decimal amount, PolarityCON polarity) 
        {
            InputValidator.CheckNotNull(transDRM, "TransactionDRM");

            this._lineItemRow = MyData.getInstance().LineItem.NewLineItemRow();

            this._lineItemRow.id = MyData.getInstance().getNextID("LineItem");
            this._lineItemRow.transactionID = transDRM.TransactionID;
            this.AccountID = accountID;
            this.ConfirmationNumber = confrimationNum;
            this.Amount = amount;
            this.Polarity = polarity;

            MyData.getInstance().LineItem.AddLineItemRow(this._lineItemRow);
        }


    }
}