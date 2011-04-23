using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    class TransactionTypeTM : TableModel
    {

        ///////////////////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////////////////
        private ObservableCollection<TransactionTypeDRM> _EditableTransactionType;
        public ObservableCollection<TransactionTypeDRM> EditableAccountType
        {
            get 
            {
                _EditableTransactionType = new ObservableCollection<TransactionTypeDRM>();

                foreach (FFDataSet.TransactionTypeRow row in MyData.getInstance().TransactionType)
                    if(row.id > TransactionTypeCON.NULL.ID)
                        _EditableTransactionType.Add(new TransactionTypeDRM(row));

                return _EditableTransactionType; 
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public TransactionTypeTM()
        {

        }

    }
}
