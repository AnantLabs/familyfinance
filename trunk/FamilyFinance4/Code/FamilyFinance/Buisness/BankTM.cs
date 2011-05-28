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

        private ObservableCollection<BankDRM> _AllBanks;
        public ObservableCollection<BankDRM> AllBanks
        {
            get
            {
                _AllBanks = new ObservableCollection<BankDRM>();

                foreach (FFDataSet.BankRow row in MyData.getInstance().Bank)
                    _AllBanks.Add(new BankDRM(row));

                return _AllBanks;
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
