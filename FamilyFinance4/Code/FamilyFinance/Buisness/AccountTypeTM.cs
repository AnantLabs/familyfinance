using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    class AccountTypeTM : TableModel
    {

        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        private ObservableCollection<AccountTypeDRM> _EditableAccountTypes;
        public ObservableCollection<AccountTypeDRM> EditableAccountTypes
        {
            get 
            {
                _EditableAccountTypes = new ObservableCollection<AccountTypeDRM>();

                foreach (FFDataSet.AccountTypeRow row in MyData.getInstance().AccountType)
                    if(row.id > AccountTypeCON.NULL.ID)
                        _EditableAccountTypes.Add(new AccountTypeDRM(row));

                return _EditableAccountTypes; 
            }
        }

        private ObservableCollection<AccountTypeDRM> _AllAccountTypes;
        public ObservableCollection<AccountTypeDRM> AllAccountTypes
        {
            get
            {
                _AllAccountTypes = new ObservableCollection<AccountTypeDRM>();

                foreach (FFDataSet.AccountTypeRow row in MyData.getInstance().AccountType)
                    _AllAccountTypes.Add(new AccountTypeDRM(row));

                return _AllAccountTypes;
            }
        }
        


        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public AccountTypeTM()
        {

        }

    }
}
