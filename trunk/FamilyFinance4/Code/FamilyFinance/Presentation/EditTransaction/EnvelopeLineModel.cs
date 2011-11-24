using System.Collections.ObjectModel;
using FamilyFinance.Buisness;
using FamilyFinance.Data;

namespace FamilyFinance.Presentation.EditTransaction
{
    class EnvelopeLineModel : EnvelopeLineDRM
    {
        private LineItemModel parentLine;



        public virtual decimal Amount
        {
            get
            {
                return base.Amount;
            }
            set
            {
                base.Amount = value;
                this.reportToParentTheDependantPropertyChanged();
            }
        }

        private void reportToParentTheDependantPropertyChanged()
        {
            this.parentLine.retportDependantPropertiesChanged();
        }

        public void setParentLine(LineItemModel lineItem)
        {
            base.setParentLine((LineItemDRM)lineItem);
            this.parentLine = lineItem;
        }




    }
}
