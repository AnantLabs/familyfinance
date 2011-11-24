using System;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    /// <summary>
    /// This class "is a" LineItemDRM and "has a" TransactionDRM for making easy changes
    /// like in a registry setting.
    /// </summary>
    class RegistryLineModel : LineItemDRM
    {

        private TransactionDRM _transactionDRM;

        /// <summary>
        /// Gets or sets the datetime of the parent transaction
        /// </summary>
        public DateTime Date
        {
            get
            {
                return this._transactionDRM.Date;
            }
            set
            {
                this._transactionDRM.Date = value;
            }
        }

        /// <summary>
        /// Gets or sets the type ID of the parent transaction
        /// </summary>
        public int TypeID
        {
            get
            {
                return this._transactionDRM.TypeID;
            }
            set
            {
                this._transactionDRM.TypeID = value;
            }
        }

        /// <summary>
        /// GEts the name of the transaction type of the parent transation
        /// </summary>
        public string TypeName
        {
            get
            {
                return this._transactionDRM.TypeName;
            }
        }

        /// <summary>
        /// Gets or sets the description of the parent transaction
        /// </summary>
        public string Description
        {
            get
            {
                return this._transactionDRM.Description;
            }
            set
            {
                this._transactionDRM.Description = value;
            }
        }

        /// <summary>
        /// Gets or sets the complete code for the parent transaction
        /// </summary>
        public TransactionStateCON Complete
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the account id of the line items on the opposite side of the transaction.
        /// </summary>
        public int OppositeAccountID
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string OppositeAccountName
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }



        //public int OppAccountID
        //{
        //    get
        //    {
        //        return this.getOppAccountID();
        //    }

        //    set
        //    {
        //        this.setOppAccountID(value);
        //    }
        //}

        //public string OppAccountName
        //{
        //    get
        //    {
        //        return this.getOppAccountName();
        //    }
        //}


        ///////////////////////////////////////////////////////////////////////////////////////////
        // Private Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        //private void setOppAccountID(int newOppAccID)
        //{
        //    int oppLinesCount = 0;
        //    FFDataSet.LineItemRow[] lines = this._lineItemRow.TransactionRow.GetLineItemRows();
        //    FFDataSet.LineItemRow oppLine = null;

        //    foreach (FFDataSet.LineItemRow line in lines)
        //    {
        //        if (line.creditDebit != this._lineItemRow.creditDebit)
        //        {
        //            oppLinesCount++;
        //            oppLine = line;
        //        }
        //    }

        //    if (oppLinesCount == 0)
        //    {
        //        // We could do something here but it is not trivial to do.
        //        //   If we get here and if there is only 1 line in this transaction we could duplicate 
        //        //      the existing line to make an opposite line.
        //        //   If we get here and there are multiple lines on one side and non on the opposite side
        //        //      it means we are working with a one-sided complex transaction and this is not the 
        //        //      class to handle this situation.
        //    }
        //    else if (oppLinesCount == 1)
        //    {
        //        oppLine.accountID = newOppAccID;
        //    }
        //    else if (oppLinesCount >= 2)
        //    {
        //        // This is a complex transaction, so don't commit the change because we don't know which
        //        // opposite line to update.
        //    }
        //}

        //private int getOppAccountID()
        //{
        //    int oppLinesCount = 0;
        //    int oppAccountID = AccountCON.NULL.ID;
        //    FFDataSet.LineItemRow[] lines = this._lineItemRow.TransactionRow.GetLineItemRows();

        //    foreach (FFDataSet.LineItemRow line in lines)
        //    {
        //        if (line.creditDebit != this._lineItemRow.creditDebit)
        //        {
        //            oppLinesCount++;
        //            oppAccountID = line.accountID;
        //        }
        //    }

        //    if (oppLinesCount >= 2)
        //    {
        //        oppAccountID = AccountCON.MULTIPLE.ID;
        //    }

        //    return oppAccountID;
        //}

        //private string getOppAccountName()
        //{
        //    int oppLinesCount = 0;
        //    string oppAccountName = AccountCON.NULL.Name;
        //    FFDataSet.LineItemRow[] lines = this._lineItemRow.TransactionRow.GetLineItemRows();

        //    foreach (FFDataSet.LineItemRow line in lines)
        //    {
        //        if (line.creditDebit != this._lineItemRow.creditDebit)
        //        {
        //            oppLinesCount++;
        //            oppAccountName = line.AccountRow.name;
        //        }
        //    }

        //    if (oppLinesCount >= 2)
        //    {
        //        oppAccountName = AccountCON.MULTIPLE.Name;
        //    }

        //    return oppAccountName;
        //}



                
        /////////////////////////////////////////////////////////////////////////////////////////////
        //// Public Functions
        /////////////////////////////////////////////////////////////////////////////////////////////
        //public RegistryLineModel(FFDataSet.LineItemRow lRow)
        //{
        //    this._lineItemRow = lRow;
        //}

        //        public LineItemDRM() : this(AccountCON.NULL.ID, AccountCON.NULL.ID, "",  0.0m, CreditDebitCON.CREDIT)
        //{
        //}

        //public LineItemDRM(int accountID, int oppAccountID, string confrimationNum, decimal amount, CreditDebitCON creditDebit)
        //    : base()
        //{
        //    // Make the first line of the transaction.
        //    this._lineItemRow = MyData.getInstance().LineItem.NewLineItemRow();

        //    this._lineItemRow.id = MyData.getInstance().getNextID("LineItem");
        //    this._lineItemRow.transactionID = this.TransactionID;
        //    this.AccountID = accountID;
        //    this.ConfirmationNumber = confrimationNum;
        //    this.Amount = amount;
        //    this.Polarity = creditDebit.Value;

        //    MyData.getInstance().LineItem.AddLineItemRow(this._lineItemRow);

        //    // Make the second or opposite line of the transaction.
        //    FFDataSet.LineItemRow oppLine = MyData.getInstance().LineItem.NewLineItemRow();

        //    oppLine.id = this._lineItemRow.id + 1;
        //    oppLine.transactionID = this._lineItemRow.transactionID;
        //    oppLine.accountID = oppAccountID;
        //    oppLine.confirmationNumber = this._lineItemRow.confirmationNumber;
        //    oppLine.amount = this._lineItemRow.amount;
        //    oppLine.creditDebit = !this._lineItemRow.creditDebit;

        //    MyData.getInstance().LineItem.AddLineItemRow(oppLine);
        //}

        //public RegistryLineModel(TransactionDRM transDRM, FFDataSet.LineItemRow lineRow)
        //    : base(lineRow)
        //{

        //    this._transactionDRM = transDRM;
        //}
        
    }
}
