using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class AccountTypeCollectionModel : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        public List<IdName> AccountTypeCollection
        {
            get
            {
                List<IdName> temp = new List<IdName>();

                foreach (FFDataSet.AccountTypeRow row in MyData.getInstance().AccountType)
                    temp.Add(new IdName(row.id, row.name));

                temp.Sort(new IdNameComparer());

                return temp;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////
        private void AccountType_AccountTypeRowChanged(object sender, FFDataSet.AccountTypeRowChangeEvent e)
        {
            this.RaisePropertyChanged("AccountTypeCollection");
        }


        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public AccountTypeCollectionModel()
        {
            MyData.getInstance().AccountType.AccountTypeRowChanged += new FFDataSet.AccountTypeRowChangeEventHandler(AccountType_AccountTypeRowChanged);
        }
    }
}
