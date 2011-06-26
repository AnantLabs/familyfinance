using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class AccountDRM : DataRowModel
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Local reference to the account row this object is modeling.
        /// </summary>
        private FFDataSet.AccountRow accountRow;

        /// <summary>
        /// Local reference to the bank row this object is modeling.
        /// </summary>        
        private FFDataSet.BankInfoRow bankInfoRow;
        
        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the ID of the account.
        /// </summary>
        public int ID
        {
            get
            {
                return this.accountRow.id;
            }
        }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        public string Name 
        {
            get 
            {
                return this.accountRow.name;
            }

            set
            {
                this.accountRow.name = this.validLength(value, AccountCON.NameMaxLength);
            }
        }

        /// <summary>
        /// Gets or sets the typeID of this account.
        /// </summary>
        public int TypeID
        {
            get 
            { 
                return this.accountRow.typeID; 
            }

            set
            {
                this.accountRow.typeID = value;
            }
        }

        /// <summary>
        /// Gets the type name of this account.
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.accountRow.AccountTypeRow.name;
            }
        }

        /// <summary>
        /// Gets or sets the Catagory of this account.
        /// </summary>
        public byte CatagoryID
        {
            get
            {
                return this.accountRow.catagory;
            }

            set
            {
                this.accountRow.catagory = value;

                this.RaisePropertyChanged("CatagoryName");
                this.RaisePropertyChanged("UseEnvelopes");
            }
        }

        /// <summary>
        /// Gets the catagory name for this account.
        /// </summary>
        public string CatagoryName
        {
            get
            {
                return CatagoryCON.getName(this.accountRow.catagory);
            }
        }

        /// <summary>
        /// Gets or sets the Closed flag for this account. True if the account is closed, 
        /// false if the account is open.
        /// </summary>
        public bool Closed
        {
            get
            {
                return this.accountRow.closed;
            }

            set
            {
                this.accountRow.closed = value;
            }
        }

        /// <summary>
        /// Gets or sets the flag stating whether or not this account uses envelopes.
        /// </summary>
        public bool UsesEnvelopes
        {
            get
            {
                return this.accountRow.envelopes;
            }

            set
            {
                this.accountRow.envelopes = value;
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
                    this.bankInfoRow.bankID = BankCON.NULL.ID;
                    this.bankInfoRow.accountNumber = "";
                    this.bankInfoRow.creditDebit = CreditDebitCON.DEBIT.Value;

                    MyData.getInstance().BankInfo.AddBankInfoRow(this.bankInfoRow);
                }
                else if (value == false && this.bankInfoRow != null)
                {
                    this.bankInfoRow.Delete();
                    this.bankInfoRow = null;
                }

                this.RaisePropertyChanged("AccountNumber");
                this.RaisePropertyChanged("AccountNormal");
                this.RaisePropertyChanged("NormalName");
                this.RaisePropertyChanged("BankName");
                this.RaisePropertyChanged("RoutingNumber");
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
                    this.bankInfoRow.accountNumber = this.validLength(value, AccountCON.AccountNumberMaxLength);
                }
            }
        }

        public bool? AccountNormal
        {
            get
            {
                if (this.bankInfoRow == null)
                    return null;
                else
                    return this.bankInfoRow.creditDebit;
            }

            set
            {
                if (this.bankInfoRow != null)
                {
                    this.bankInfoRow.creditDebit = Convert.ToBoolean(value);
                }
            }
        }

        public string NormalName
        {
            get
            {
                if (this.bankInfoRow == null)
                    return "";

                else if (this.bankInfoRow.creditDebit == CreditDebitCON.CREDIT.Value)
                    return CreditDebitCON.CREDIT.Name;

                else
                    return CreditDebitCON.DEBIT.Name;
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

        

        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////


        
        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public AccountDRM(FFDataSet.AccountRow aRow)
        {
            this.accountRow = aRow;
            this.bankInfoRow = MyData.getInstance().BankInfo.FindByaccountID(this.ID);
        }

        public AccountDRM(string name, int typeID, byte catagory, bool closed, bool envelopes)
        {
            this.bankInfoRow = null;
            this.accountRow = MyData.getInstance().Account.NewAccountRow();

            this.accountRow.id = MyData.getInstance().getNextID("Account");
            this.Name = name;
            this.TypeID = typeID;
            this.CatagoryID = catagory;
            this.Closed = closed;
            this.UsesEnvelopes = envelopes;

            MyData.getInstance().Account.AddAccountRow(this.accountRow);
        }

        public AccountDRM() : this("", AccountTypeCON.NULL.ID, CatagoryCON.ACCOUNT.ID, false, false)
        {
        }
    }
}
