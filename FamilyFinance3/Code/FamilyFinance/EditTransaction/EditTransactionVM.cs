using System;
using System.Collections.Generic;

using FamilyFinance.Model;
using FamilyFinance.Custom;
using FamilyFinance.Database;

namespace FamilyFinance.EditTransaction
{
    class EditTransactionVM : ModelBase
    {
        private int transID;

        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        public MyObservableCollection<Decimal> Credits { get; private set; }
        public MyObservableCollection<Decimal> Debits { get; private set; }



        public EditTransactionVM()
        {
        }

        public EditTransactionVM(int transID)
        {
            this.transID = transID;
        }


    }
}
