using FamilyFinance.Data;

namespace FamilyFinance.Buisness.Sorters
{
    class EnvelopesNameComparer : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            EnvelopeDRM xEnv = (EnvelopeDRM)x;
            EnvelopeDRM yEnv = (EnvelopeDRM)y;

            int result;

            result = sortBySpecialCase(xEnv, yEnv);

            return result;
        }

        private int sortBySpecialCase(EnvelopeDRM xEnv, EnvelopeDRM yEnv)
        {
            int result;

            if (xEnv.IsSpecial() && yEnv.IsSpecial())
                result = compareByID(xEnv, yEnv);

            else if (xEnv.IsSpecial())
                result = -1;

            else if (yEnv.IsSpecial())
                result = 1;

            else
                result = compareByName(xEnv, yEnv);

            return result;
        }

        private static int compareByID(EnvelopeDRM xEnv, EnvelopeDRM yEnv)
        {
            int result;
            result = xEnv.ID.CompareTo(yEnv.ID);
            return result;
        }

        private static int compareByName(EnvelopeDRM xEnv, EnvelopeDRM yEnv)
        {
            return xEnv.Name.CompareTo(yEnv.Name);
        }

    }
}
