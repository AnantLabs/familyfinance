using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    /// <summary>
    /// A modle of an account row in the dataset. This also manages a bankInfo row, if the
    /// user wants to attach bank information to the account row.
    /// </summary>
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
        /// Amount the ID of the account.
        /// </summary>
        public int ID
        {
            get
            {
                return this.accountRow.id;
            }
        }

        /// <summary>
        /// Amount or sets the name of the account.
        /// </summary>
        public string Name 
        {
            get 
            {
                return this.accountRow.name;
            }

            set
            {
                this.accountRow.name = this.truncateIfNeeded(value, AccountCON.NameMaxLength);
            }
        }

        /// <summary>
        /// Amount or sets the typeID of this account.
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
        /// Amount the type name of this account.
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.accountRow.AccountTypeRow.name;
            }
        }

        /// <summary>
        /// Amount or sets the Catagory of this account.
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
                this.RaisePropertyChanged("UsesEnvelopes");
                this.RaisePropertyChanged("CanUseEnvelopes");
            }
        }

        /// <summary>
        /// Amount the catagory name for this account.
        /// </summary>
        public string CatagoryName
        {
            get
            {
                return CatagoryCON.getName(this.accountRow.catagory);
            }
        }

        /// <summary>
        /// Amount or sets the Closed flag for this account. True if the account is closed, 
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
        /// Amount or sets the flag stating whether or not this account uses envelopes.
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

        public bool CanUseEnvelopes
        {
            get
            {
                return (this.accountRow.catagory == CatagoryCON.ACCOUNT.ID);
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
                    this.bankInfoRow.polarity = PolarityCON.DEBIT.Value;

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
                    this.bankInfoRow.accountNumber = this.truncateIfNeeded(value, AccountCON.AccountNumberMaxLength);
                }
            }
        }

        public PolarityCON AccountNormal
        {
            get
            {
                if (this.bankInfoRow == null)
                    return null;
                else
                    return PolarityCON.GetPlolartiy(this.bankInfoRow.polarity);
            }

            set
            {
                if (this.bankInfoRow != null)
                {
                    this.bankInfoRow.polarity = value.Value;
                }
            }
        }

        public string NormalName
        {
            get
            {
                if (this.bankInfoRow == null)
                    return "";

                else
                    return PolarityCON.GetPlolartiy(this.bankInfoRow.polarity).Name;
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

        public AccountDRM(string name, int typeID, CatagoryCON catagory, bool closed, bool envelopes)
        {
            this.bankInfoRow = null;
            this.accountRow = MyData.getInstance().Account.NewAccountRow();

            this.accountRow.id = MyData.getInstance().getNextID("Account");
            this.Name = name;
            this.TypeID = typeID;
            this.CatagoryID = catagory.ID;
            this.Closed = closed;
            this.UsesEnvelopes = envelopes;

            MyData.getInstance().Account.AddAccountRow(this.accountRow);
        }

        public AccountDRM() : this("", AccountTypeCON.NULL.ID, CatagoryCON.ACCOUNT, false, false)
        {
        }
    }
}
