using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class LineItemModel : LineItemDRM
    {
        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        public ObservableCollection<EnvelopeLineDRM> EnvelopeLines { get; private set; }


        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private void newEmptyEnvelopeLineCollection()
        {
            this.EnvelopeLines = new ObservableCollection<EnvelopeLineDRM>();
        }

        private void fillEnvelopeLineCollection(FFDataSet.EnvelopeLineRow[] envLines)
        {
            foreach (FFDataSet.EnvelopeLineRow envLine in envLines)
                this.EnvelopeLines.Add(new EnvelopeLineDRM(envLine));
        }



        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public LineItemModel() : base()
        {
            newEmptyEnvelopeLineCollection();
        }

        public LineItemModel(FFDataSet.LineItemRow lRow) : base(lRow)
        {
            newEmptyEnvelopeLineCollection();
            fillEnvelopeLineCollection(this.getEnvelopeLineRows());
        }

        public LineItemModel(TransactionDRM transaction) : base(transaction)
        {
            newEmptyEnvelopeLineCollection();
            fillEnvelopeLineCollection(this.getEnvelopeLineRows());
        }

    }
}
