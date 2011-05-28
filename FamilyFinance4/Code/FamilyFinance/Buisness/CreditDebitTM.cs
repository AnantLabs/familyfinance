using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    class CreditDebitTM : TableModel
    {
        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        public List<CreditDebitCON> List
        {
            get 
            {
                List<CreditDebitCON> temp = new List<CreditDebitCON>();

                temp.Add(CreditDebitCON.CREDIT);
                temp.Add(CreditDebitCON.DEBIT);

                return temp;
            }
        }

    }
}
