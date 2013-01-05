using FamilyFinance.Buisness;

namespace FamilyFinance.Buisness.Sorters
{
    class RegistryLinesComparer : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            RegistryLineModel xLine = (RegistryLineModel)x;
            RegistryLineModel yLine = (RegistryLineModel)y;

            int result;

            result = xLine.Date.CompareTo(yLine.Date);

            return result;
        }

        //private int sortBySpecialCase(RegistryLineModel xEnv, RegistryLineModel yEnv)
        //{
        //    //int result;

        //    //if (xEnv.IsSpecial() && yEnv.IsSpecial())
        //    //    result = compareByID(xEnv, yEnv);

        //    //else if (xEnv.IsSpecial())
        //    //    result = -1;

        //    //else if (yEnv.IsSpecial())
        //    //    result = 1;

        //    //else
        //    //    result = compareByName(xEnv, yEnv);

        //    //return result;
        //}

        //private static int compareByID(RegistryLineModel xEnv, RegistryLineModel yEnv)
        //{
        //    int result;
        //    result = xEnv.ID.CompareTo(yEnv.ID);
        //    return result;
        //}

        //private static int compareByName(RegistryLineModel xEnv, RegistryLineModel yEnv)
        //{
        //    //return xEnv.Name.CompareTo(yEnv.Name);
        //}

    }
}
