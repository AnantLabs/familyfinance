using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Model;
using FamilyFinance.Database;
using FamilyFinance.EditAccounts;

namespace FamilyFinance.Registry
{
    class RegistryVM : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////
        private int currentAccountID;


        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or sets the collection of accounts.
        /// </summary>
        public ObservableCollection<LineItemRegModel> RegistryLines { get; set; }

        public ObservableCollection<AccountBankModel> AccountBalances { get; set; }



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


        /// <summary>
        /// Optimised sort for the registry. Based off the bubble sort but works from the bottom up.
        /// This is because the registry will usually be sorted and new items will be added to the end.
        /// So let that new item bubble up ito place.
        /// </summary>
        private void sortRegistry()
        {
            bool moved = false;

            // Start at end and push the smallestvalues to the top
            for (int i = 0; i < this.RegistryLines.Count - 1; i++)
            {
                moved = false;

                for (int j = this.RegistryLines.Count - 1; j > i; j--)
                {
                    int comp = this.RegistryLines[j].CompareTo(this.RegistryLines[j - 1]);

                    if (comp < 0)
                    {
                        this.RegistryLines.Move(j, j - 1);
                        moved = true;
                    }
                }

                if (!moved)
                    break;
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


        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public RegistryVM()
        {
            this.reloadAccountTypesCBList();
            this.reloadAccountsCBList();
            this.reloadEnvelopesCBList();
            this.reloadRegistryLines(3);
            this.reloadAccountBalances();
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
            ObservableCollection<AccountBankModel> temp = new ObservableCollection<AccountBankModel>();

            foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
            {
                if (row.closed == false && row.catagory == SpclAccountCat.ACCOUNT)
                    temp.Add(new AccountBankModel(row));
            }
            
            this.AccountBalances = temp;
            this.RaisePropertyChanged("AccountBalances");
        }

        public void reloadRegistryLines(int aID)
        {
            this.currentAccountID = aID;
            LineItemRegModel.setAccount(aID);

            ObservableCollection<LineItemRegModel> reg = new ObservableCollection<LineItemRegModel>();

            foreach (FFDataSet.LineItemRow line in MyData.getInstance().LineItem)
                if (line.accountID == aID)
                    reg.Add(new LineItemRegModel(line));

            this.RegistryLines = reg;
            this.registryRowEditEnding();
        }

        public void registryRowEditEnding()
        {
            this.sortRegistry();
            this.RaisePropertyChanged("RegistryLines");
            this.calcBalance();
        }


    }
}
