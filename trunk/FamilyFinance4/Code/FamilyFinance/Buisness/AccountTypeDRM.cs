﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class AccountTypeDRM : DataRowModel
    {
        /// <summary>
        /// Local referance to the account type row this object is modeling.
        /// </summary>
        private FFDataSet.AccountTypeRow accountTypeRow;

        /// <summary>
        /// Gets the ID of the account type.
        /// </summary>
        public int ID
        {
            get
            {
                return this.accountTypeRow.id;
            }
        }

        /// <summary>
        /// Gets or sets the name of the account type.
        /// </summary>
        public string Name 
        {
            get 
            {
                return this.accountTypeRow.name;
            }

            set
            {
                this.accountTypeRow.name = this.truncateIfNeeded(value, AccountTypeCON.NameMaxLength);
            }
        }

        /// <summary>
        /// Creates the object and keeps a local referance to the given account type row.
        /// </summary>
        /// <param name="aRow"></param>
        public AccountTypeDRM(FFDataSet.AccountTypeRow atRow)
        {
            this.accountTypeRow = atRow;
        }

        /// <summary>
        /// Creates the object and keeps a reference to a new account type row.
        /// </summary>
        /// <param name="name">The name of the new AccountType.</param>
        public AccountTypeDRM(string name)
        {
            this.accountTypeRow = MyData.getInstance().AccountType.NewAccountTypeRow();

            this.accountTypeRow.id = MyData.getInstance().getNextID("AccountType");
            this.Name = name;

            MyData.getInstance().AccountType.AddAccountTypeRow(this.accountTypeRow);
        }

        public AccountTypeDRM() : this("")
        {

        }

    }
}