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
        private int currentEnvelopeID;

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

        private MyObservableCollection<SubLineRegModel> _SubRegistryLines;
        public MyObservableCollection<SubLineRegModel> SubRegistryLines
        {
            get
            {
                return this._SubRegistryLines;
            }
            private set
            {
                this._SubRegistryLines = value;
                this.RaisePropertyChanged("SubRegistryLines");
            }
        }

        private string _Title;
        public string Title
        {
            get
            {
                return this._Title;
            }
            private set
            {
                this._Title = value;
                this.RaisePropertyChanged("Title");
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


        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////
        private void calcAccountBalance()
        {
            DateTime today = DateTime.Today;
            decimal tBal = 0.0m;
            decimal bal = 0.0m;
            bool cd = LineCD.DEBIT;

            FFDataSet.BankInfoRow bInfo = MyData.getInstance().BankInfo.FindByaccountID(this.currentAccountID);
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

        private void calcEnvelopeBalance()
        {
            DateTime today = DateTime.Today;
            decimal tBal = 0.0m;
            decimal bal = 0.0m;

            SubLineRegModel subLine;

            for (int i = 0; i < this.SubRegistryLines.Count; i++)
            {
                subLine = this.SubRegistryLines[i];
                bal = (subLine.CreditDebit) ? bal += subLine.Amount : bal -= subLine.Amount;
                subLine.BalanceAmount = bal;

                if (subLine.Date <= today)
                    tBal = bal;
            }

            this.EndingBalance = bal;
            this.TodaysBalance = tBal;
        }

        private void setTitle()
        {
            string accName = MyData.getInstance().Account.FindByid(this.currentAccountID).name;
            string envName = MyData.getInstance().Envelope.FindByid(this.currentEnvelopeID).name;

            string title;

            if (!String.IsNullOrWhiteSpace(accName) && !String.IsNullOrWhiteSpace(envName))
                title = accName + " : " + envName;

            else if (!String.IsNullOrWhiteSpace(accName))
                title = accName;

            else title = envName;

            this.Title = title;
        }

        private void fillRegistryLines()
        {
            LineItemRegModel.setAccount(this.currentAccountID);

            MyObservableCollection<LineItemRegModel> reg = new MyObservableCollection<LineItemRegModel>();

            FFDataSet.LineItemRow[] lines = MyData.getInstance().Account.FindByid(this.currentAccountID).GetLineItemRows();

            foreach (FFDataSet.LineItemRow line in lines)
                reg.Add(new LineItemRegModel(line));

            reg.Sort(new RegistryComparer());
            this.RegistryLines = reg;
            this.calcAccountBalance();
        }

        private void fillEnvelopeLines()
        {
            MyObservableCollection<SubLineRegModel> reg = new MyObservableCollection<SubLineRegModel>();

            FFDataSet.EnvelopeLineRow[] lines = MyData.getInstance().Envelope.FindByid(this.currentEnvelopeID).GetEnvelopeLineRows();

            foreach (FFDataSet.EnvelopeLineRow line in lines)
                reg.Add(new SubLineRegModel(line));

            reg.Sort(new SubLineRegModelComparer());
            this.SubRegistryLines = reg;
            this.calcEnvelopeBalance();
        }

        private void fillAccountEnvelopeLines()
        {
            MyObservableCollection<SubLineRegModel> reg = new MyObservableCollection<SubLineRegModel>();

            FFDataSet.EnvelopeLineRow[] lines = MyData.getInstance().Envelope.FindByid(this.currentEnvelopeID).GetEnvelopeLineRows();

            foreach (FFDataSet.EnvelopeLineRow line in lines)
                if (line.LineItemRow.accountID == this.currentAccountID)
                    reg.Add(new SubLineRegModel(line));

            reg.Sort(new SubLineRegModelComparer());
            this.SubRegistryLines = reg;
            this.calcEnvelopeBalance();
        }


        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public RegistryGridVM()
        {
        }

        public void setCurrentAccountEnvelope(int aID, int eID)
        {
            this.currentAccountID = aID;
            this.currentEnvelopeID = eID;

            this.setTitle();

            if (!SpclEnvelope.isSpecial(eID) && SpclAccount.isNotSpecial(aID))
                this.fillAccountEnvelopeLines();

            else if (!SpclEnvelope.isSpecial(eID))
                this.fillEnvelopeLines();

            else
                this.fillRegistryLines();

        }

        public void registryRowEditEnding()
        {
            this.RegistryLines.Sort(new RegistryComparer());
            this.calcAccountBalance();
        }

    }
}
