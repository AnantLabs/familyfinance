using System;
using System.Collections.Generic;

using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class AccountListModel : ModelBase
    {
        private List<IdNameCat> aList;

        public List<IdNameCat> AccountList
        {
            get
            {
                if (aList == null)
                    reloadAccount();

                return aList;
            }

        }

        public void reloadAccount()
        {
            List<IdNameCat> temp = new List<IdNameCat>();

            foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
                temp.Add(new IdNameCat(row.id, row.name, CatagoryModel.getShortName(row.catagory)));

            temp.Sort(new IdNameCatComparer());

            aList = temp;
            RaisePropertyChanged("AccountList");
        }
    }
}
