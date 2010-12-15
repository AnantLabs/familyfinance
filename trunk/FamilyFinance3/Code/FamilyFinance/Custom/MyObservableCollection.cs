using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace FamilyFinance.Custom
{
    class MyObservableCollection<T> : ObservableCollection<T>
    {

        /// <summary>
        /// A bubble sort that actually works from the bottom up.
        /// This is because after the initial sort new things are added to the end.
        /// So move that new item into the right place and be done.
        /// </summary>
        public void Sort(IComparer<T> comp)
        {
            bool moved = false;

            // Start at end and push the smallest values to the top
            for (int i = 0; i < this.Count - 1; i++)
            {
                moved = false;

                for (int j = this.Count - 1; j > i; j--)
                {
                    int value = comp.Compare(this[j], this[j - 1]);

                    if (value < 0)
                    {
                        this.Move(j, j - 1);
                        moved = true;
                    }
                }

                if (!moved)
                    break;
            }
        }

    }
}
