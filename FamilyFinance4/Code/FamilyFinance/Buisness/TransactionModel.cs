

using System.Collections.ObjectModel;
using System.Windows.Data;
using FamilyFinance.Data;


namespace FamilyFinance.Buisness
{
    class TransactionModel : TransactionDRM
    {

        private ObservableCollection<LineItemDRM> _lines;




        public decimal CreditsSum
        {
            get
            {
                decimal sum = 0.0m;

                foreach (LineItemDRM line in this._lines)
                {
                    if (line.Polarity != PolarityCON.CREDIT)
                    {
                        sum += line.Amount;
                    }
                }

                return sum;
            }
        }

        public decimal DebitsSum
        {
            get
            {
                decimal sum = 0.0m;

                foreach (LineItemDRM line in this._lines)
                {
                    if (line.Polarity != PolarityCON.DEBIT)
                    {
                        sum += line.Amount;
                    }
                }

                return sum;
            }
        }


        public TransactionModel(int transID)
            : base(transID)
        {
            this._lines = new ObservableCollection<LineItemDRM>();

            buildLinesOfTransaction();
        }

        private void buildLinesOfTransaction()
        {
            if (this._transactionRow != null)
            {
                FFDataSet.LineItemRow[] rows = this._transactionRow.GetLineItemRows();

                foreach (FFDataSet.LineItemRow line in rows)
                {
                    this._lines.Add(new LineItemDRM(line));
                }
            }
        }
    }
}
