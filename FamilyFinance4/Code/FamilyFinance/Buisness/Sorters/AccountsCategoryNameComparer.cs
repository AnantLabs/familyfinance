using FamilyFinance.Data;

namespace FamilyFinance.Buisness.Sorters
{
    class AccountsCategoryNameComparer : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            AccountDRM xAcc = (AccountDRM)x;
            AccountDRM yAcc = (AccountDRM)y;

            int result;

            result = sortBySpecialCase(xAcc, yAcc);

            return result;
        }

        private int sortBySpecialCase(AccountDRM xAcc, AccountDRM yAcc)
        {
            int result;

            if (xAcc.IsSpecial() && yAcc.IsSpecial())
                result = xAcc.ID.CompareTo(yAcc.ID);
            
            else if (xAcc.IsSpecial())
                result = -1;

            else if (yAcc.IsSpecial())
                result = 1;

            else
                result = compareByCategory(xAcc, yAcc);

            return result;
        }

        private static int compareByCategory(AccountDRM xAcc, AccountDRM yAcc)
        {
            int result = xAcc.CatagoryName.CompareTo(yAcc.CatagoryName);

            if (result == 0)
                result = compareByName(xAcc, yAcc);

            return result;
        }

        private static int compareByName(AccountDRM xAcc, AccountDRM yAcc)
        {
            return xAcc.Name.CompareTo(yAcc.Name);
        }

    }
}
