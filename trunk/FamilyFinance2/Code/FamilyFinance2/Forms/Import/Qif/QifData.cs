using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Import.Qif
{
    class QifAccount
    {
        public const decimal NULL_DECIMAL = -1.0m;

        public string Name = "";
        public string Type = "";
        public string Description = "";
        public decimal StatementBalance = NULL_DECIMAL;
        public decimal CreditLimit = NULL_DECIMAL;
        public DateTime StatementBalanceDate = new DateTime(1, 1, 1);
        public List<QifTransaction> Transactions;

        public QifAccount()
        {
            this.Transactions = new List<QifTransaction>();
        }
    }

    /// <summary>
    /// This holds all the data for a transaction from a qif file.
    /// </summary>
    class QifTransaction
    {
        public DateTime Date;
        public decimal Amount;
        public string ClearedStatus = "";
        public string Num = "";
        public string Memo = "";
        public string Payee = "";
        public string Address = "";
        public string Catagory = "";
        public List<QifSplit> EnvLines;
        public List<QifSplit> OppLines;

        /// <summary>
        /// AccountID that is used in Family finance. Not to be filled at creation.
        /// </summary>
        public int AccountID = SpclAccount.NULL;
        public int OppAccountID = SpclAccount.NULL;

        public QifTransaction()
        {
            this.EnvLines = new List<QifSplit>();
            this.OppLines = new List<QifSplit>(0);
        }

        public bool isOpposite(QifTransaction trans)
        {
            if (this.AccountID.Equals(trans.OppAccountID)
                && this.OppAccountID.Equals(trans.AccountID)
                && this.Date.Equals(trans.Date)
                && this.Amount.Equals(Decimal.Negate(trans.Amount)))
                return true;

            return false;
        }

    }

    class QifSplit
    {
        public string Catagory = "";
        public string Memo = "";
        public decimal Amount;
        public int OppAccountID;

        public QifSplit()
        {
        }

        public QifSplit(string catagory, string memo, decimal amount)
        {
            this.Catagory = catagory;
            this.Memo = memo;
            this.Amount = amount;
            this.OppAccountID = SpclAccount.NULL;
        }
    }
}
