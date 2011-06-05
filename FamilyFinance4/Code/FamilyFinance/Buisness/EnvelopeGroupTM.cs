using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    class EnvelopeGroupTM : TableModel
    {
                
        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        private ObservableCollection<EnvelopeGroupDRM> _EditableEnvelopeGroups;
        public ObservableCollection<EnvelopeGroupDRM> EditableEnvelopeGroups
        {
            get 
            {
                _EditableEnvelopeGroups = new ObservableCollection<EnvelopeGroupDRM>();

                foreach (FFDataSet.EnvelopeGroupRow row in MyData.getInstance().EnvelopeGroup)
                    if (row.id > EnvelopeGroupCON.NULL.ID)
                        _EditableEnvelopeGroups.Add(new EnvelopeGroupDRM(row));

                return _EditableEnvelopeGroups;
            }
        }

        private ObservableCollection<EnvelopeGroupDRM> _AllEnvelopeGroups;
        public ObservableCollection<EnvelopeGroupDRM> AllEnvelopeGroups
        {
            get
            {
                _AllEnvelopeGroups = new ObservableCollection<EnvelopeGroupDRM>();

                foreach (FFDataSet.EnvelopeGroupRow row in MyData.getInstance().EnvelopeGroup)
                    _AllEnvelopeGroups.Add(new EnvelopeGroupDRM(row));

                return _AllEnvelopeGroups;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////



        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public EnvelopeGroupTM()
        {

        }
    }
}
