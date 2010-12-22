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
                this.RaisePropertyChanged("Envelopes");
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
                this.RaisePropertyChanged("Envelopes");
            }
        }

        /// <summary>
        /// Gets or sets the collection of envelopes.
        /// </summary>
        public ObservableCollection<EnvelopeModel> Envelopes 
        {
            get
            {
                ObservableCollection<EnvelopeModel> envelopes = new ObservableCollection<EnvelopeModel>();

                foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
                {
                    bool validID = row.id > 0;
                    bool inSearch = row.name.ToLower().Contains(this._SearchText.ToLower());
                    bool doShow = this._ShowClosed || !row.closed;

                    if (validID && inSearch && doShow)
                        envelopes.Add(new EnvelopeModel(row));
                }

                return envelopes;
            }
        }

        /// <summary>
        /// Creats the view model for editing accounts
        /// </summary>
        public EditEnvelopesVM()
        {
            this._SearchText = "";
            this._ShowClosed = false;
        }
    
    }
}
