using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    class AccountBankInfoDRM : AccountDRM
    {
                
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Local referance to the account row this object is modeling.
        /// </summary>        
        private FFDataSet.BankInfoRow bankInfoRow;

        
        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the ID of the account.
        /// </summary>
        public int AccountID
        {
            get
            {
                return this.bankInfoRow.accountID;
            }
        }


        public int BankID
        {
            get
            {
                if (this.bankInfoRow == null)
                    return BankCON.NULL.ID;
                else
                    return this.bankInfoRow.bankID;
            }

            set
            {
                if (this.bankInfoRow != null)
                {
                    this.bankInfoRow.bankID = value;

                    this.RaisePropertyChanged("BankName");
                    this.RaisePropertyChanged("RoutingNumber");
                }
            }
        }

        public string BankName
        {
            get
            {
                if (this.bankInfoRow == null)
                    return "";
                else
                    return this.bankInfoRow.BankRow.name;
            }
        }

        public string RoutingNumber
        {
            get
            {
                if (this.bankInfoRow == null)
                    return "";
                else
                    return this.bankInfoRow.BankRow.routingNumber;
            }
        }

        public string AccountNumber
        {
            get
            {
                if (this.bankInfoRow == null)
                    return "";
                else
                    return this.bankInfoRow.accountNumber;
            }

            set
            {
                if (this.bankInfoRow != null)
                {
                    this.bankInfoRow.accountNumber = value;
                }
            }
        }


        public bool AccountNormal
        {
            get
            {
                if (this.bankInfoRow == null)
                    return CreditDebitCON.DEBIT.Value;
                else
                    return this.bankInfoRow.creditDebit;
            }

            set
            {
                if (this.bankInfoRow != null)
                {
                    this.bankInfoRow.creditDebit = value;
                }
            }
        }

        public bool HasBankInfo
        {
            get
            {
                if (this.bankInfoRow == null)
                    return false;
                else
                    return true;
            }

            set
            {
                if (value == true && this.bankInfoRow == null)
                {
                    this.bankInfoRow = MyData.getInstance().BankInfo.NewBankInfoRow();
                    this.bankInfoRow.accountID = this.ID;
                    MyData.getInstance().BankInfo.AddBankInfoRow(this.bankInfoRow);
                }
                else if (value == false && this.bankInfoRow != null)
                {
                    this.bankInfoRow.Delete();
                    this.bankInfoRow = null;
                }

            }
        }


        
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////


        
        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public AccountBankInfoDRM(FFDataSet.AccountRow aRow) : base(aRow)
        {
            this.bankInfoRow = MyData.getInstance().BankInfo.FindByaccountID(this.ID);
        }

        public AccountBankInfoDRM() : base()
        {
            this.bankInfoRow = null;
        }



    }
}
