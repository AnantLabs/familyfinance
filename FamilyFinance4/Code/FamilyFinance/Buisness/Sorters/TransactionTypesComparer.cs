using FamilyFinance.Data;

namespace FamilyFinance.Buisness.Sorters
{
    class TransactionTypesComparer : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            TransactionTypeDRM xType  = (TransactionTypeDRM)x;
            TransactionTypeDRM yType = (TransactionTypeDRM)y;

            if (xType.IsSpecial() && yType.IsSpecial())
                return xType.ID.CompareTo(yType.ID);

            else if (xType.IsSpecial())
                return -1;

            else if (yType.IsSpecial())
                return 1;

            else
                return xType.Name.CompareTo(yType.Name);
        }
    }
}
