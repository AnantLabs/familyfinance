

using System.Collections.ObjectModel;
using System.Windows.Data;
using FamilyFinance.Data;


namespace FamilyFinance.Buisness
{
    class TransactionModel : TransactionDRM
    {
        public ObservableCollection<LineItemDRM> LineItems
        {
            get;
            private set;
        }



        public decimal CreditsSum
        {
            get
            {
                decimal sum = 0.0m;

                foreach (LineItemDRM line in this.LineItems)
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

                foreach (LineItemDRM line in this.LineItems)
                {
                    if (line.Polarity != PolarityCON.DEBIT)
                    {
                        sum += line.Amount;
                    }
                }

                return sum;
            }
        }

        public TransactionModel()
        {
        }
    }
}
