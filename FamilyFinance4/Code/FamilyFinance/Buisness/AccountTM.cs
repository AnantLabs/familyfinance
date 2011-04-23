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
        private ObservableCollection<AccountBankInfoDRM> _EditableAccounts;
        public ObservableCollection<AccountBankInfoDRM> EditableAccounts
        {
            get 
            {
                _EditableAccounts = new ObservableCollection<AccountBankInfoDRM>();

                foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
                    if(row.id > AccountCON.NULL.ID)
                        _EditableAccounts.Add(new AccountBankInfoDRM(row));

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
