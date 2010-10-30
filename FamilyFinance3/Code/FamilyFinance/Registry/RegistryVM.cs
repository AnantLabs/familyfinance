using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Model;
using FamilyFinance.Database;
using FamilyFinance.Custom;

namespace FamilyFinance.Registry
{
    class RegistryVM : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////


        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets the collection of accounts.
        /// </summary>
        public MyObservableCollection<LineItemRegModel> RegistryLines { get; set; }

        public ObservableCollection<BalanceModel> AccountBalances { get; set; }
        public ObservableCollection<BalanceModel> EnvelopeBalances { get; set; }
        public ObservableCollection<BalanceModel> ExpenceBalances { get; set; }
        public ObservableCollection<BalanceModel> IncomeBalances { get; set; }


        /// <summary>
        /// Gets or sets the collection of account types.
        /// </summary>
        public List<IdName> LineTypesCBList { get; set; }

        /// <summary>
        /// Gets or sets the collection of account types.
        /// </summary>
        public List<IdNameCat> AccountsCBList { get; set; }

        /// <summary>
        /// Gets or sets the collection of account types.
        /// </summary>
        public List<IdName> EnvelopesCBList { get; set; }




        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////



        private void calcAccountBalance(int aID)
        {
            decimal bal = 0.0m;
            bool cd = LineCD.DEBIT;

            FFDataSet.BankInfoRow bInfo = MyData.getInstance().BankInfo.FindByaccountID(aID);
            LineItemRegModel line;

            if (bInfo != null)
                cd = bInfo.creditDebit;

            if (cd == LineCD.DEBIT)
            {
                for (int i = 0; i < this.RegistryLines.Count; i++)
                {
                    line = this.RegistryLines[i];
                    bal = (line.CreditDebit) ? bal += line.Amount : bal -= line.Amount;
                    line.BalanceAmount = bal;
                }
            }
            else
            {
                for (int i = 0; i < this.RegistryLines.Count; i++)
                {
                    line = this.RegistryLines[i];
                    bal = (line.CreditDebit) ? bal -= line.Amount : bal += line.Amount;
                    line.BalanceAmount = bal;
                }
            }

        }


        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public RegistryVM()
        {
            this.reloadAccountTypesCBList();
            this.reloadAccountsCBList();
            this.reloadEnvelopesCBList();
            this.reloadAccountBalances();
            this.reloadEnvelopeBalances();

            this.setCurrentAccountEnvelope(3, -1);
        }

        public void reloadAccountTypesCBList()
        {
            List<IdName> types = new List<IdName>();

            foreach (FFDataSet.LineTypeRow row in MyData.getInstance().LineType)
                types.Add(new IdName(row.id, row.name));

            types.Sort(new IdNameComparer());

            this.LineTypesCBList = types;
            this.RaisePropertyChanged("LineTypesCBList");
        }

        public void reloadEnvelopesCBList()
        {
            List<IdName> envelopes = new List<IdName>();

            foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
                envelopes.Add(new IdName(row.id, row.name));

            envelopes.Sort(new IdNameComparer());

            this.EnvelopesCBList = envelopes;
            this.RaisePropertyChanged("EnvelopesCBList");
        }

        public void reloadAccountsCBList()
        {
            List<IdNameCat> acc = new List<IdNameCat>();

            foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
                acc.Add(new IdNameCat(row.id, row.name, CatagoryModel.getShortName(row.catagory)));

            acc.Sort(new IdNameCatComparer());

            this.AccountsCBList = acc;

            this.RaisePropertyChanged("AccountsCBList");
        }

        public void reloadAccountBalances()
        {
            ObservableCollection<BalanceModel> tempAcc = new ObservableCollection<BalanceModel>();
            ObservableCollection<BalanceModel> tempIn = new ObservableCollection<BalanceModel>();
            ObservableCollection<BalanceModel> tempEx = new ObservableCollection<BalanceModel>();

            foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
            {
                if (row.closed == false)
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

            this.AccountBalances = tempAcc;
            this.ExpenceBalances = tempEx;
            this.IncomeBalances = tempIn;
            this.RaisePropertyChanged("AccountBalances");
            this.RaisePropertyChanged("ExpenceBalances");
            this.RaisePropertyChanged("IncomeBalances");
        }

        public void reloadEnvelopeBalances()
        {
            ObservableCollection<BalanceModel> tempEnv = new ObservableCollection<BalanceModel>();

            foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
            {
                if (row.closed == false && row.id > 0)
                {
                    tempEnv.Add(new BalanceModel(row));
                }
            }

            this.EnvelopeBalances = tempEnv;
            this.RaisePropertyChanged("EnvelopeBalances");
        }

        public void setCurrentAccountEnvelope(int aID, int eID)
        {
            LineItemRegModel.setAccount(aID);

            MyObservableCollection<LineItemRegModel> reg = new MyObservableCollection<LineItemRegModel>();

            FFDataSet.LineItemRow[] lines = MyData.getInstance().Account.FindByid(aID).GetLineItemRows();

            foreach (FFDataSet.LineItemRow line in lines)
                reg.Add(new LineItemRegModel(line));

            this.RegistryLines = reg;
            this.RegistryLines.sort(new RegistryComparer());
            this.RaisePropertyChanged("RegistryLines");
            this.calcAccountBalance(aID);
        }

        public void registryRowEditEnding()
        {

        }

    }
}
