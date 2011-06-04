using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    class AccountTM : TableModel
    {

        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        private ObservableCollection<AccountDRM> _EditableAccounts;
        public ObservableCollection<AccountDRM> EditableAccounts
        {
            get 
            {
                _EditableAccounts = new ObservableCollection<AccountDRM>();

                foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
                    if(row.id > AccountCON.NULL.ID)
                        _EditableAccounts.Add(new AccountDRM(row));

                return _EditableAccounts; 
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////

        public AccountTM()
        {

        }

    }
}
