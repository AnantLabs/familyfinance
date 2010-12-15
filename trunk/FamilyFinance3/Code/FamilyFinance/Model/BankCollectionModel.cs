using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class BankCollectionModel : ModelBase
    {
        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        public List<IdName> BankCollection
        {
            get
            {
                List<IdName> temp = new List<IdName>();

                foreach (FFDataSet.BankRow row in MyData.getInstance().Bank)
                    temp.Add(new IdName(row.id, row.name));

                temp.Sort(new IdNameComparer());

                return temp;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////
        private void Bank_BankRowChanged(object sender, FFDataSet.BankRowChangeEvent e)
        {
            this.RaisePropertyChanged("BankCollection");
        }


        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public BankCollectionModel()
        {
            MyData.getInstance().Bank.BankRowChanged += new FFDataSet.BankRowChangeEventHandler(Bank_BankRowChanged);
        }
    }
}
