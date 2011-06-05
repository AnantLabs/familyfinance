using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    class EnvelopeTM : TableModel
    {

        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        private ObservableCollection<EnvelopeDRM> _EditableEnvelopes;
        public ObservableCollection<EnvelopeDRM> EditableEnvelopes
        {
            get 
            {
                _EditableEnvelopes = new ObservableCollection<EnvelopeDRM>();

                foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
                    if(row.id > EnvelopeCON.NO_ENVELOPE.ID)
                        _EditableEnvelopes.Add(new EnvelopeDRM(row));

                return _EditableEnvelopes; 
            }
        }

        private ObservableCollection<EnvelopeDRM> _AllEnvelopes;
        public ObservableCollection<EnvelopeDRM> AllEnvelopes
        {
            get 
            {
                _AllEnvelopes = new ObservableCollection<EnvelopeDRM>();

                foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
                    _AllEnvelopes.Add(new EnvelopeDRM(row));

                return _AllEnvelopes; 
            }
        }


        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////

        public EnvelopeTM()
        {

        }

    }
}
