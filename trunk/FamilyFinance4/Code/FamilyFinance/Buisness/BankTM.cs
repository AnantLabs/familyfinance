using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    class BankTM : TableModel
    {
                
        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        private ObservableCollection<BankDRM> _EditableBanks;
        public ObservableCollection<BankDRM> EditableBanks
        {
            get 
            {
                _EditableBanks = new ObservableCollection<BankDRM>();

                foreach (FFDataSet.BankRow row in MyData.getInstance().Bank)
                    if(row.id > BankCON.NULL.ID)
                        _EditableBanks.Add(new BankDRM(row));

                return _EditableBanks;
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////



        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public BankTM()
        {

        }


    }
}
