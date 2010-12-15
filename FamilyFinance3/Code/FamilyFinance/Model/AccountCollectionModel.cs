using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class AccountCollectionModel : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        public List<IdNameCat> AccountCollection_IAE
        {
            get
            {
                List<IdNameCat> temp = new List<IdNameCat>();

                foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
                    temp.Add(new IdNameCat(row.id, row.name, CatagoryModel.getShortName(row.catagory)));

                temp.Sort(new IdNameCatComparer());

                return temp;
            }
        }

        public List<IdName> AccountCollection_A
        {
            get
            {
                List<IdName> temp = new List<IdName>();

                foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
                {
                    if ((row.catagory == SpclAccountCat.ACCOUNT && row.closed == false ) || row.id == SpclAccount.NULL)
                        temp.Add(new IdName(row.id, row.name));
                }

                temp.Sort(new IdNameComparer());

                return temp;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////
        private void Account_AccountRowChanged(object sender, FFDataSet.AccountRowChangeEvent e)
        {
            this.RaisePropertyChanged("AccountCollection_A");
            this.RaisePropertyChanged("AccountCollection_IAE");
        }


        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public AccountCollectionModel()
        {
            MyData.getInstance().Account.AccountRowChanged += new FFDataSet.AccountRowChangeEventHandler(Account_AccountRowChanged);
        }
    }
}
