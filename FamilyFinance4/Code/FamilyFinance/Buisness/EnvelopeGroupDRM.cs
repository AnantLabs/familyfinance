
using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    public class EnvelopeGroupDRM : DataRowModel
    {
        private FFDataSet.EnvelopeGroupRow envelopeGroupRow;

        public int ID
        {
            get
            {
                return this.envelopeGroupRow.id;
            }
        }

        public string Name 
        {
            get 
            {
                return this.envelopeGroupRow.name;
            }

            set
            {
                this.envelopeGroupRow.name = value;
            }
        }

        public decimal MinPercentage
        {
            get
            {
                return this.envelopeGroupRow.minPercent;
            }

            set
            {
                this.envelopeGroupRow.minPercent = value;
            }
        }

        public decimal MaxPercentage
        {
            get
            {
                return this.envelopeGroupRow.maxPercent;
            }

            set
            {
                this.envelopeGroupRow.maxPercent = value;
            }
        }

        public EnvelopeGroupDRM()
        {
            this.envelopeGroupRow = DataSetModel.Instance.NewEnvelopeGroupRow();
        }

        public EnvelopeGroupDRM(string envelopeGroupName)
        {
            this.envelopeGroupRow = DataSetModel.Instance.NewEnvelopeGroupRow();
            this.Name = envelopeGroupName;
        }

        public EnvelopeGroupDRM(FFDataSet.EnvelopeGroupRow atRow)
        {
            this.envelopeGroupRow = atRow;
        }

        public void deleteRowFromDataset()
        {
            this.envelopeGroupRow.Delete();
        }


    }
}
