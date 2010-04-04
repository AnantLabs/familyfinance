using System;
using System.Collections;
using System.Windows.Forms;

namespace FamilyFinance2.Forms.Main.RegistrySplit.TreeView
{
	public class AccountItemSorter : IComparer
	{
        public AccountItemSorter()
		{
		}

		public int Compare(object x, object y)
		{
            return String.Compare((x as BaseItem).Name, (y as BaseItem).Name);
		}

		private string GetData(object x)
		{
			return (x as BaseItem).Name;
		}
	}
}
