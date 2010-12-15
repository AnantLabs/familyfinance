using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class CreditDebitCollectionModel : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        public List<CreditDebitModel> CreditDebitCollection
        {
            get
            {
                List<CreditDebitModel> temp = new List<CreditDebitModel>(2);

                temp.Add(CreditDebitModel.CREDIT);
                temp.Add(CreditDebitModel.DEBIT);

                return temp;
            }
        }
    }
}
