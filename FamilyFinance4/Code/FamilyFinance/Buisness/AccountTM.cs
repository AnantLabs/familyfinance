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

        private ObservableCollection<AccountDRM> _FavoriteAccounts;
        public ObservableCollection<AccountDRM> FavoriteAccounts
        {
            get
            {
                _FavoriteAccounts = new ObservableCollection<AccountDRM>();

                foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
                    if (row.catagory == CatagoryCON.ACCOUNT.ID || row.id == AccountCON.NULL.ID)
                        _FavoriteAccounts.Add(new AccountDRM(row));

                return _FavoriteAccounts;
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
