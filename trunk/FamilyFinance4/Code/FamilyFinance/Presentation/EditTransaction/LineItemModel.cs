using System.Collections.ObjectModel;
using FamilyFinance.Buisness;
using FamilyFinance.Data;

namespace FamilyFinance.Presentation.EditTransaction
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

        public LineItemModel(FFDataSet.LineItemRow lRow, TransactionDRM parentTransaction) : base(lRow, parentTransaction)
        {
            newEmptyEnvelopeLineCollection();
            fillEnvelopeLineCollection(this.getEnvelopeLineRows());
        }

        //public LineItemModel(TransactionDRM parentTransaction) : base(parentTransaction)
        //{
        //    newEmptyEnvelopeLineCollection();
        //    fillEnvelopeLineCollection(this.getEnvelopeLineRows());
        //}


        public void setParentTransaction(TransactionModel transaction)
        {
            base.setParentTransaction((TransactionDRM) transaction);
        }


        public void retportDependantPropertiesChanged()
        {
            this.reportPropertyChangedWithName("IsLineError");
            this.reportPropertyChangedWithName("EnvelopeLineSum");
        }


    }
}
