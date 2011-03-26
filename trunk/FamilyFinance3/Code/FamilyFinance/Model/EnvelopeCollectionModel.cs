using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class EnvelopeCollectionModel : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        public List<IdName> EnvelopeCollection
        {
            get
            {
                List<IdName> temp = new List<IdName>();

                foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
                    temp.Add(new IdName(row.id, row.name));

                temp.Sort(new IdNameComparer());

                return temp;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////
        private void Envelope_EnvelopeRowChanged(object sender, FFDataSet.EnvelopeRowChangeEvent e)
        {
            this.RaisePropertyChanged("EnvelopeCollection");
        }

        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public EnvelopeCollectionModel()
        {
            MyData.getInstance().Envelope.EnvelopeRowChanged +=new FFDataSet.EnvelopeRowChangeEventHandler(Envelope_EnvelopeRowChanged);
        }
    }
}
