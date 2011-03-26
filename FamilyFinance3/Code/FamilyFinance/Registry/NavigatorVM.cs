using System;
using System.Collections.Generic;

using FamilyFinance.Model;
using FamilyFinance.Custom;
using FamilyFinance.Database;

namespace FamilyFinance.Registry
{
    class NavigatorVM : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        public MyObservableCollection<BalanceModel> Accounts { get; set; }
        public MyObservableCollection<BalanceModel> Envelopes { get; set; }
        public MyObservableCollection<BalanceModel> Expences { get; set; }
        public MyObservableCollection<BalanceModel> Incomes { get; set; }

        public void reloadAccountBalances()
        {
            MyObservableCollection<BalanceModel> tempAcc = new MyObservableCollection<BalanceModel>();
            MyObservableCollection<BalanceModel> tempIn = new MyObservableCollection<BalanceModel>();
            MyObservableCollection<BalanceModel> tempEx = new MyObservableCollection<BalanceModel>();

            foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
            {
                if (row.closed == false && row.id > 0)
                {
                    if (row.catagory == SpclAccountCat.ACCOUNT)
                    {
                        tempAcc.Add(new BalanceModel(row));
                    }
                    else if (row.catagory == SpclAccountCat.EXPENSE)
                    {
                        tempEx.Add(new BalanceModel(row));
                    }
                    else if (row.catagory == SpclAccountCat.INCOME)
                    {
                        tempIn.Add(new BalanceModel(row));
                    }
                }
            }

            tempAcc.Sort(new BalanceModelComparer());
            tempEx.Sort(new BalanceModelComparer());
            tempIn.Sort(new BalanceModelComparer());

            this.Accounts = tempAcc;
            this.Expences = tempEx;
            this.Incomes = tempIn;
            this.RaisePropertyChanged("Accounts");
            this.RaisePropertyChanged("Expences");
            this.RaisePropertyChanged("Incomes");
        }

        public void reloadEnvelopeBalances()
        {
            MyObservableCollection<BalanceModel> tempEnv = new MyObservableCollection<BalanceModel>();

            foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
            {
                if (row.closed == false && row.id > 0)
                {
                    tempEnv.Add(new BalanceModel(row));
                }
            }

            tempEnv.Sort(new BalanceModelComparer());

            this.Envelopes = tempEnv;
            this.RaisePropertyChanged("Envelopes");
        }

        public NavigatorVM()
        {
        }

    }
}
