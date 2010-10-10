using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Model;

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
        public ObservableCollection<EnvelopeModel> Envelopes { get; set; }

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
            this.Envelopes = CollectionBuilder.getEnvelopesEditable(this._ShowClosed, this._SearchText);

            this.RaisePropertyChanged("Envelopes");
        }

        public void reloadEnvelopeGroups()
        {
            this.Groups = CollectionBuilder.getEnvelopeGroupsAll();
            this.RaisePropertyChanged("Groups");
        }

        public void reloadAccounts()
        {
            this.Accounts = CollectionBuilder.getAccountAccount();
            this.RaisePropertyChanged("Accounts");
        }

        /// <summary>
        /// Creats the view model for editing accounts
        /// </summary>
        public EditEnvelopesVM()
        {
            this._SearchText = "";

            this.loadEnvelopes();
            this.reloadEnvelopeGroups();
            this.reloadAccounts();
        }
    
    }
}
