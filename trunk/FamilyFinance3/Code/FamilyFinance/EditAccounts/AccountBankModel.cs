using FamilyFinance.Database;
using FamilyFinance.Model;

namespace FamilyFinance.EditAccounts
{
    class AccountBankModel : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Local referance to the account row this object is modeling.
        /// </summary>
        private FFDataSet.AccountRow accountRow;
        
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
                this.accountRow.name = value;

                MyData.getInstance().saveRow(this.accountRow);
                this.RaisePropertyChanged("Name");
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

                MyData.getInstance().saveRow(this.accountRow);
                this.RaisePropertyChanged("TypeID");
                this.RaisePropertyChanged("TypeName");
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

                MyData.getInstance().saveRow(this.accountRow);
                this.RaisePropertyChanged("CatagoryID");
                this.RaisePropertyChanged("CatagoryName");
                this.RaisePropertyChanged("CanUseEnvelopes");
            }
        }

        /// <summary>
        /// Gets the gatagory name forthis account.
        /// </summary>
        public string CatagoryName
        {
            get
            {
                return CatagoryModel.getName(this.accountRow.catagory);
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

                MyData.getInstance().saveRow(this.accountRow);
                this.RaisePropertyChanged("Closed");
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

                MyData.getInstance().saveRow(this.accountRow);
                this.RaisePropertyChanged("UsesEnvelopes");
            }
        }

        public bool CanUseEnvelopes
        {
            get
            {
                return (this.accountRow.catagory == SpclAccountCat.ACCOUNT);
            }
        }
        

        public int BankID
        {
            get
            {
                if (this.bankInfoRow == null)
                    return SpclBank.NULL;
                else
                    return this.bankInfoRow.bankID;
            }

            set
            {
                if (this.bankInfoRow != null)
                {
                    this.bankInfoRow.bankID = value;

                    MyData.getInstance().saveRow(this.bankInfoRow);
                    this.RaisePropertyChanged("BankID");
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

        public string BankAccountNumber
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

                    MyData.getInstance().saveRow(this.bankInfoRow);
                    this.RaisePropertyChanged("BankAccountNumber");
                }
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

        public bool AccountNormal
        {
            get
            {
                if (this.bankInfoRow == null)
                    return LineCD.DEBIT;
                else
                    return this.bankInfoRow.creditDebit;
            }

            set
            {
                if (this.bankInfoRow != null)
                {
                    this.bankInfoRow.creditDebit = value;

                    MyData.getInstance().saveRow(this.bankInfoRow);
                    this.RaisePropertyChanged("AccountNormal");
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
                    this.bankInfoRow.accountID = this.accountRow.id;
                    MyData.getInstance().BankInfo.AddBankInfoRow(this.bankInfoRow);
                    MyData.getInstance().saveRow(this.bankInfoRow);
                }
                else if (value == false && this.bankInfoRow != null)
                {
                    this.bankInfoRow.Delete();
                    MyData.getInstance().saveRow(this.bankInfoRow);
                    this.bankInfoRow = null;
                }

                this.RaisePropertyChanged("HasBankInfo");
            }
        }


        
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////


        
        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public AccountBankModel(int aID)
        {
            this.accountRow = MyData.getInstance().Account.FindByid(aID);
            this.bankInfoRow = MyData.getInstance().BankInfo.FindByaccountID(aID);
        }
        
        public AccountBankModel(FFDataSet.AccountRow aRow)
        {
            this.accountRow = aRow;
            this.bankInfoRow = MyData.getInstance().BankInfo.FindByaccountID(this.ID);
        }

        public AccountBankModel()
        {
            this.accountRow = MyData.getInstance().Account.NewAccountRow();
            MyData.getInstance().Account.AddAccountRow(this.accountRow);
            MyData.getInstance().saveRow(this.accountRow);

            this.bankInfoRow = null;
        }


    }
}
