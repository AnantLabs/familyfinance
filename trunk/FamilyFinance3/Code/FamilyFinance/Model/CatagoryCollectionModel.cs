using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class CatagoryCollectionModel : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        public List<CatagoryModel> CatagoryCollection
        {
            get
            {
                List<CatagoryModel> temp = new List<CatagoryModel>(3);

                temp.Add(CatagoryModel.INCOME);
                temp.Add(CatagoryModel.ACCOUNT);
                temp.Add(CatagoryModel.EXPENCE);

                return temp;
            }
        }
    }
}
