using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    class AccountCatagoryTM : TableModel
    {
        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        public List<CatagoryCON> Catagories
        {
            get 
            { 
                List<CatagoryCON> temp = new List<CatagoryCON>();

                //temp.Add(CatagoryCON.NULL);
                temp.Add(CatagoryCON.ACCOUNT);
                temp.Add(CatagoryCON.EXPENSE);
                temp.Add(CatagoryCON.INCOME);

                return temp;
            }
        }

    }
}
