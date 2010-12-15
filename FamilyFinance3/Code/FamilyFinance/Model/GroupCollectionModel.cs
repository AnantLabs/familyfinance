using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class EnvelopeGroupCollectionModel : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        public List<IdName> EnvelopeGroupCollection
        {
            get
            {
                List<IdName> temp = new List<IdName>();

                foreach (FFDataSet.EnvelopeGroupRow row in MyData.getInstance().EnvelopeGroup)
                    temp.Add(new IdName(row.id, row.name));

                temp.Sort(new IdNameComparer());

                return temp;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////
        private void EnvelopeGroup_EnvelopeGroupRowChanged(object sender, FFDataSet.EnvelopeGroupRowChangeEvent e)
        {
            this.RaisePropertyChanged("EnvelopeGroupCollection");
        }


        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public EnvelopeGroupCollectionModel()
        {
            MyData.getInstance().EnvelopeGroup.EnvelopeGroupRowChanged += new FFDataSet.EnvelopeGroupRowChangeEventHandler(EnvelopeGroup_EnvelopeGroupRowChanged);
        }
    }
}
