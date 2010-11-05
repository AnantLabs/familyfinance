using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Model;
using FamilyFinance.Database;
using FamilyFinance.Custom;
using FamilyFinance.EditAccounts;

namespace FamilyFinance.Registry
{
    class RegistryGridVM : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////
        private int currentAccountID;
        //private int currentEnvelopeID;

        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        private MyObservableCollection<LineItemRegModel> _RegistryLines;
        public MyObservableCollection<LineItemRegModel> RegistryLines
        {
            get
            {
                return this._RegistryLines;
            }
            private set
            {
                this._RegistryLines = value;
                this.RaisePropertyChanged("RegistryLines");
            }
        }

        private List<IdName> _LineTypesList;
        public List<IdName> LineTypesList 
        {
            get
            {
                return this._LineTypesList;
            }
            private set
            {
                this._LineTypesList = value;
                this.RaisePropertyChanged("LineTypesList");
            }
        }

        private List<IdNameCat> _AccountsList;
        public List<IdNameCat> AccountsList
        {
            get
            {
                return this._AccountsList;
            }
            private set
            {
                this._AccountsList = value;
                this.RaisePropertyChanged("AccountsList");
            }
        }

        private List<IdName> _EnvelopesList;
        public List<IdName> EnvelopesList
        {
            get
            {
                return this._EnvelopesList;
            }
            private set
            {
                this._EnvelopesList = value;
                this.RaisePropertyChanged("EnvelopesList");
            }
        }

        private string _AccountName;
        public string AccountName
        {
            get
            {
                return this._AccountName;
            }
            private set
            {
                this._AccountName = value;
                this.RaisePropertyChanged("AccountName");
            }
        }

        private decimal _EndingBalance;
        public decimal EndingBalance
        {
            get
            {
                return this._EndingBalance;
            }
            private set
            {
                this._EndingBalance = value;
                this.RaisePropertyChanged("EndingBalance");
            }
        }

        private decimal _TodaysBalance;
        public decimal TodaysBalance
        {
            get
            {
                return this._TodaysBalance;
            }
            private set
            {
                this._TodaysBalance = value;
                this.RaisePropertyChanged("TodaysBalance");
            }
        }

        public DateTime Date
        {
            get
            {
                return DateTime.Today;
            }
        }

        //private decimal _BanksBalance;
        //public decimal BanksBalance
        //{
        //    get
        //    {
        //        return this._BanksBalance;
        //    }
        //    private set
        //    {
        //        this._BanksBalance = value;
        //        this.RaisePropertyChanged("BanksBalance");
        //    }
        //}



        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////
        private void calcAccountBalance(int aID)
        {
            DateTime today = DateTime.Today;
            decimal tBal = 0.0m;
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

                    if (line.Date <= today)
                        tBal = bal;
                }
            }
            else
            {
                for (int i = 0; i < this.RegistryLines.Count; i++)
                {
                    line = this.RegistryLines[i];
                    bal = (line.CreditDebit) ? bal -= line.Amount : bal += line.Amount;
                    line.BalanceAmount = bal;

                    if (line.Date <= today)
                        tBal = bal;
                }
            }

            this.EndingBalance = bal;
            this.TodaysBalance = tBal;
        }


        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public RegistryGridVM()
        {
        }

        public void reloadLineTypes()
        {
            List<IdName> types = new List<IdName>();

            foreach (FFDataSet.LineTypeRow row in MyData.getInstance().LineType)
                types.Add(new IdName(row.id, row.name));

            types.Sort(new IdNameComparer());
            this.LineTypesList = types;
        }

        public void reloadEnvelopes()
        {
            List<IdName> envelopes = new List<IdName>();

            foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
                envelopes.Add(new IdName(row.id, row.name));

            envelopes.Sort(new IdNameComparer());
            this.EnvelopesList = envelopes;
        }

        public void reloadAccounts()
        {
            List<IdNameCat> acc = new List<IdNameCat>();

            foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
                acc.Add(new IdNameCat(row.id, row.name, CatagoryModel.getShortName(row.catagory)));

            acc.Sort(new IdNameCatComparer());
            this.AccountsList = acc;
        }

        public void setCurrentAccountEnvelope(int aID, int eID)
        {
            string name = MyData.getInstance().Account.FindByid(aID).AccountTypeRow.name;
            name += " : " + MyData.getInstance().Account.FindByid(aID).name;

            this.currentAccountID = aID;
            this.AccountName = name;
            LineItemRegModel.setAccount(aID);

            MyObservableCollection<LineItemRegModel> reg = new MyObservableCollection<LineItemRegModel>();

            FFDataSet.LineItemRow[] lines = MyData.getInstance().Account.FindByid(aID).GetLineItemRows();

            foreach (FFDataSet.LineItemRow line in lines)
                reg.Add(new LineItemRegModel(line));

            reg.sort(new RegistryComparer());
            this.RegistryLines = reg;
            this.calcAccountBalance(aID);
        }

        public void registryRowEditEnding()
        {
            this.RegistryLines.sort(new RegistryComparer());
            this.calcAccountBalance(this.currentAccountID);
        }

    }
}
