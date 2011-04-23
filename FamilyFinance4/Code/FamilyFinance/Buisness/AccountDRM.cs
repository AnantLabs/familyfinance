using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    class AccountDRM : DataRowModel
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Local referance to the account row this object is modeling.
        /// </summary>
        private FFDataSet.AccountRow accountRow;

        
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
        /// Gets the gatagory name forthis account.
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


        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////


        
        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public AccountDRM(FFDataSet.AccountRow aRow)
        {
            this.accountRow = aRow;
        }

        public AccountDRM()
        {
            this.accountRow = MyData.getInstance().Account.NewAccountRow();

            this.accountRow.id = MyData.getInstance().getNextID("Account");
            this.accountRow.name = "";
            this.accountRow.typeID = AccountTypeCON.NULL.ID;
            this.accountRow.catagory = CatagoryCON.ACCOUNT.ID;
            this.accountRow.closed = false;
            this.accountRow.envelopes = false;

            MyData.getInstance().Account.AddAccountRow(this.accountRow);
        }

    }
}
