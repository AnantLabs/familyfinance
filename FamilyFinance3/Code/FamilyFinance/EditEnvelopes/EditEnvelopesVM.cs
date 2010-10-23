using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Model;
using FamilyFinance.Database;

namespace FamilyFinance.EditEnvelopes
{
    class EditEnvelopesVM : ModelBase
    {
        private string _SearchText;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                this._SearchText = value;
                this.loadEnvelopes();
            }
        }

        private bool _ShowClosed;
        public bool ShowClosed
        {
            get
            {
                return this._ShowClosed;
            }
            set
            {
                this._ShowClosed = value;
                loadEnvelopes();
            }
        }

        /// <summary>
        /// Gets or sets the collection of envelopes.
        /// </summary>
        public ObservableCollection<EnvelopeGoalModel> Envelopes { get; set; }

        /// <summary>
        /// Gets or sets the collection of accounts.
        /// </summary>
        public List<IdName> Groups { get; set; }

        /// <summary>
        /// Gets or sets the collection of account types.
        /// </summary>
        public List<IdName> Accounts { get; set; }

        /// <summary>
        /// Loads the Envelopes collection with data.
        /// </summary>
        private void loadEnvelopes()
        {
            ObservableCollection<EnvelopeGoalModel> envelopes = new ObservableCollection<EnvelopeGoalModel>();

            foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
            {
                bool validID = row.id > 0;
                bool inSearch = row.name.ToLower().Contains(this._SearchText.ToLower());
                bool doShow = this._ShowClosed || !row.closed;

                if (validID && inSearch && doShow)
                    envelopes.Add(new EnvelopeGoalModel(row));
            }


            this.Envelopes = envelopes;

            this.RaisePropertyChanged("Envelopes");
        }

        public void reloadEnvelopeGroups()
        {
            List<IdName> types = new List<IdName>();

            foreach (FFDataSet.EnvelopeGroupRow row in MyData.getInstance().EnvelopeGroup)
                types.Add(new IdName(row.id, row.name));

            types.Sort(new IdNameComparer());

            this.Groups = types;
            this.RaisePropertyChanged("Groups");
        }

        public void reloadAccounts()
        {
            List<IdName> accounts = new List<IdName>();

            foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
            {
                if (row.catagory == SpclAccountCat.ACCOUNT || row.id == SpclAccount.NULL)
                    accounts.Add(new IdName(row.id, row.name));
            }

            accounts.Sort(new IdNameComparer());

            this.Accounts = accounts;
            this.RaisePropertyChanged("Accounts");
        }

        /// <summary>
        /// Creats the view model for editing accounts
        /// </summary>
        public EditEnvelopesVM()
        {
            this._SearchText = "";
            this._ShowClosed = false;

            this.loadEnvelopes();
            this.reloadEnvelopeGroups();
            this.reloadAccounts();
        }
    
    }
}
