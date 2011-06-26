using System.Windows.Data;

namespace FamilyFinance.Presentation
{
    public abstract class ViewModel : FamilyFinance.Buisness.BindableObject
    {

        /// <summary>
        /// Commits any editing and then refreshs the given view. Typically used when filter parameters have changed.
        /// </summary>
        /// <param name="lcv"></param>
        static public void refreshViewFilter(ListCollectionView lcv)
        {
            if (lcv.IsEditingItem)
                lcv.CommitEdit();

            else if (lcv.IsAddingNew)
                lcv.CommitNew();

            lcv.Refresh();
        }

    }
}
