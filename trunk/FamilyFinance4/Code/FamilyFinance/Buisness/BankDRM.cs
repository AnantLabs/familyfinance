using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class BankDRM : DataRowModel
    {

        /// <summary>
        /// Local referance to the account row this object is modeling.
        /// </summary>
        private FFDataSet.BankRow bankRow;

        /// <summary>
        /// Gets the ID of the Bank.
        /// </summary>
        public int ID
        {
            get
            {
                return this.bankRow.id;
            }
        }
  
        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        public string Name 
        {
            get 
            {
                return this.bankRow.name;
            }

            set
            {
                this.bankRow.name = this.validLength(value, BankCON.NameMaxLength);
            }
        }

        /// <summary>
        /// Gets or sets the banks routing number.
        /// </summary>
        public string RoutingNumber
        {
            get
            {
                return this.bankRow.routingNumber;
            }

            set
            {
                this.bankRow.routingNumber = this.validLength(value, BankCON.RountingNumMaxLength);
                
            }
        }

        /// <summary>
        /// Creates the object and keeps a local referance to the given bank row.
        /// </summary>
        /// <param name="bRow"></param>
        public BankDRM(FFDataSet.BankRow bRow)
        {
            this.bankRow = bRow;
        }

        /// <summary>
        /// Creates the object and keeps a reference to a new account type row.
        /// </summary>
        /// <param name="aRow"></param>
        public BankDRM()
        {
            this.bankRow = MyData.getInstance().Bank.NewBankRow();

            this.bankRow.id = MyData.getInstance().getNextID("Bank");
            this.Name = "";
            this.RoutingNumber = "";

            MyData.getInstance().Bank.AddBankRow(this.bankRow);
        }

    }
}
