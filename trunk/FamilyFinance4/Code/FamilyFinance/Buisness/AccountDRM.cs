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
        private FFDataSet.AccountRow accountRow;
        private FFDataSet.BankInfoRow bankInfoRow;
        
        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        public int ID
        {
            get
            {
                return this.accountRow.id;
            }
        }

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

        public string TypeName
        {
            get
            {
                return this.accountRow.AccountTypeRow.name;
            }
        }

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

        public string CatagoryName
        {
            get
            {
                return CatagoryCON.getName(this.accountRow.catagory);
            }
        }

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
                return (this.bankInfoRow != null);
            }

            set
            {
                if (value == true && this.bankInfoRow == null)
                {
                    this.bankInfoRow = DataSetModel.Instance.NewBankInfoRow(
                        this.accountRow, BankCON.NULL.ID, "", PolarityCON.DEBIT);
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
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public AccountDRM() : this("", AccountTypeCON.NULL.ID, CatagoryCON.ACCOUNT, false, false)
        {
        }

        public AccountDRM(string name, int typeID, CatagoryCON catagory, bool closed, bool useEnvelopes)
        {
            this.accountRow = DataSetModel.Instance.NewAccountRow(name, typeID, catagory, closed, useEnvelopes);
            this.bankInfoRow = null;
        }

        public AccountDRM(FFDataSet.AccountRow accountRow, FFDataSet.BankInfoRow bankRow)
        {
            this.accountRow = accountRow;
            this.bankInfoRow = bankRow;
        }
    }
}
