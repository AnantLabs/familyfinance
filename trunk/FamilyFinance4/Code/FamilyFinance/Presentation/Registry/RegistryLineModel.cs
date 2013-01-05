using System;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class RegistryLineModel : BindableObject
    {
        private TransactionModel transaction;
        private LineItemModel lineItem;
        private LineItemModel oppositeLine;

        public static int CurrentAccountID;


        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        public int LineID
        {
            get
            {
                return lineItem.LineID;
            }
        }

        public int TransactionID
        {
            get
            {
                return transaction.TransactionID;
            }
        }
        
        public DateTime Date
        {
            get
            {
                return this.transaction.Date;
            }

            set
            {
                this.transaction.Date = value;
            }
        }

        public int TypeID
        {
            get
            {
                return this.transaction.TypeID;
            }

            set
            {
                this.transaction.TypeID = value;
            }
        }

        public string TypeName
        {
            get
            {
                return this.transaction.TypeName;
            }
        }

        public int AccountID
        {
            get
            {
                return lineItem.AccountID;
            }
            set
            {
                this.lineItem.AccountID = value;
            }
        }

        public string AccountName
        {
            get
            {
                return lineItem.AccountName;
            }
        }
        
        public int OppositeAccountID
        {
            get
            {
                int oppCount = this.getOppositeLineCount;

                if (oppCount == 0)
                    return AccountCON.NULL.ID;
                if (oppCount == 1)
                    return this.oppositeLine.AccountID;
                else
                    return AccountCON.MULTIPLE.ID;
            }
            set
            {
                if (this.oppositeLine != null)
                    this.oppositeLine.AccountID = value;
            }
        }

        public string OppositeAccountName
        {
            get
            {
                int oppCount = this.getOppositeLineCount;

                if (oppCount == 0)
                    return AccountCON.NULL.Name;
                if (oppCount == 1)
                    return this.oppositeLine.AccountName;
                else
                    return AccountCON.MULTIPLE.Name;
            }
        }

        public string Description
        {
            get
            {
                return this.transaction.Description;
            }
            set
            {
                this.transaction.Description = value;
            }
        }

        public string ConfirmationNumber
        {
            get
            {
                return lineItem.ConfirmationNumber;
            }
            set
            {
                this.lineItem.ConfirmationNumber = value;
            }
        }
        
        public int EnvelopeID
        {
            get
            {
                return lineItem.AccountID;
            }
            set
            {
                this.lineItem.AccountID = value;
            }
        }

        public string EnvelopeName
        {
            get
            {
                return lineItem.AccountName;
            }
        }
        
        public decimal? CreditAmount
        {
            get
            {
                if (this.lineItem.Polarity == PolarityCON.CREDIT && this.lineItem.Amount > 0)
                    return lineItem.Amount;
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    this.lineItem.Polarity = PolarityCON.CREDIT;
                    this.lineItem.Amount = (decimal)value;
                }
            }
        }

        public TransactionStateCON State
        {
            get
            {
                return this.lineItem.State;
            }
            set
            {
                this.lineItem.State = value;
            }
        }

        public decimal? DebitAmount
        {
            get
            {
                if (this.lineItem.Polarity == PolarityCON.DEBIT && this.lineItem.Amount > 0)
                    return lineItem.Amount;
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    this.lineItem.Polarity = PolarityCON.DEBIT;
                    this.lineItem.Amount = (decimal)value;
                }
            }
        }

        public decimal? RunningTotal { get; set; }




        ///////////////////////////////////////////////////////////////////////////////////////////
        // Private Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        private int getOppositeLineCount
        {
            get
            {
                int count = 0;

                foreach (LineItemModel line in this.transaction.LineItems)
                    if (line.Polarity != lineItem.Polarity)
                        count++;

                return count;
            }
        }


                
        ///////////////////////////////////////////////////////////////////////////////////////////
        //  Public Functions
        ///////////////////////////////////////////////////////////////////////////////////////////
        public RegistryLineModel()
        {
            this.lineItem = new LineItemModel();
            this.lineItem.AccountID = RegistryLineModel.CurrentAccountID;
            this.lineItem.Polarity = PolarityCON.CREDIT;

            this.oppositeLine = new LineItemModel();
            this.oppositeLine.Polarity = PolarityCON.DEBIT;

            this.transaction = new TransactionModel();
            this.transaction.LineItems.Add(this.lineItem);
            this.transaction.LineItems.Add(this.oppositeLine);
        }

        public RegistryLineModel(TransactionLine tLine)
        {
            this.transaction = tLine.Transaction;
            this.lineItem = tLine.LineItem;


            if (this.getOppositeLineCount == 1)
            {
                foreach (LineItemModel line in this.transaction.LineItems)
                    if (line.Polarity != this.lineItem.Polarity)
                        this.oppositeLine = line;
            }
        }

        public decimal getAmount()
        {
            if (this.lineItem.Polarity == PolarityCON.DEBIT)
                return lineItem.Amount;
            else
                return Decimal.Negate(lineItem.Amount);
        }

        
    }
}
