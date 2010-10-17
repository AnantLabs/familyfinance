using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Model;
using FamilyFinance.Database;

namespace FamilyFinance.Registry
{
    class RegistryVM : ModelBase
    {
        /// <summary>
        /// Gets or sets the collection of accounts.
        /// </summary>
        public ObservableCollection<LineItemRegModel> RegistryLines { get; set; }

        /// <summary>
        /// Gets or sets the collection of account types.
        /// </summary>
        public List<IdName> LineTypes { get; set; }

        /// <summary>
        /// Gets or sets the collection of account types.
        /// </summary>
        public List<IdNameCat> Accounts { get; set; }

        /// <summary>
        /// Gets or sets the collection of account types.
        /// </summary>
        public List<IdName> Envelopes { get; set; }

        private int currentAccountID;

        public void reloadRegistryLines(int aID)
        {
            this.currentAccountID = aID;
            this.RegistryLines = CollectionBuilder.getRegistryLinesEditable(aID);

            LineItemRegModel.setAccount(aID);
            this.sortRegistry();

            this.RaisePropertyChanged("RegistryLines");
            this.calcBalance();
        }

        private void sortRegistry()
        {
            for (int i = 0; i < this.RegistryLines.Count - 1; i++)
            {
                for (int j = i + 1; j < this.RegistryLines.Count; j++)
                {
                    int comp = this.RegistryLines[i].CompareTo(this.RegistryLines[j]);

                    if(comp > 0)
                        this.RegistryLines.Move(j, i);
                }
            }

        }

        private void calcBalance()
        {
            decimal bal = 0.0m;
            bool cd = LineCD.DEBIT;

            FFDataSet.BankInfoRow bInfo = MyData.getInstance().BankInfo.FindByaccountID(currentAccountID);
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

        public void reloadAccountTypes()
        {
            this.LineTypes = CollectionBuilder.getLineTypesAll();
            this.RaisePropertyChanged("LineTypes");
        }

        public void reloadEnvelopes()
        {
            this.Envelopes = CollectionBuilder.getEnvelopesAll();
            this.RaisePropertyChanged("Envelopes");
        }

        public void reloadAccounts()
        {
            this.Accounts = CollectionBuilder.getAccountAll();
            this.RaisePropertyChanged("Accounts");
        }


        public RegistryVM()
        {
            this.reloadAccountTypes();
            this.reloadAccounts();
            this.reloadEnvelopes();
            this.reloadRegistryLines(3);
        }

    }
}
