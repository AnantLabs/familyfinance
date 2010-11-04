using System;
using System.Collections.Generic;

using FamilyFinance.Model;
using FamilyFinance.Custom;

namespace FamilyFinance.Registry
{
    class NavigatorVM : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        public MyObservableCollection<BalanceModel> AccountBalances { get; set; }
        public MyObservableCollection<BalanceModel> EnvelopeBalances { get; set; }
        public MyObservableCollection<BalanceModel> ExpenceBalances { get; set; }
        public MyObservableCollection<BalanceModel> IncomeBalances { get; set; }


    }
}
