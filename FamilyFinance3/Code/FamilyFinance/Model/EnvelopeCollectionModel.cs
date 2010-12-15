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
        public List<IdNameCat> EnvelopeCollection
        {
            get
            {
                List<IdNameCat> temp = new List<IdNameCat>();

                foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
                    temp.Add(new IdNameCat(row.id, row.name, row.EnvelopeGroupRow.name));

                temp.Sort(new IdNameCatComparer());

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
