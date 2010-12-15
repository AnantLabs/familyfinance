using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class LineTypeCollectionVM : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        public List<IdName> LineTypesCollection 
        {
            get
            {
                List<IdName> types = new List<IdName>();

                foreach (FFDataSet.LineTypeRow row in MyData.getInstance().LineType)
                    types.Add(new IdName(row.id, row.name));

                types.Sort(new IdNameComparer());

                return types;
            }
        }


        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////
        private void LineType_LineTypeRowChanged(object sender, FFDataSet.LineTypeRowChangeEvent e)
        {
            this.RaisePropertyChanged("LineTypesCollection");
        }


        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public LineTypeCollectionVM()
        {
            MyData.getInstance().LineType.LineTypeRowChanged += new FFDataSet.LineTypeRowChangeEventHandler(LineType_LineTypeRowChanged);
        }
    }
}
