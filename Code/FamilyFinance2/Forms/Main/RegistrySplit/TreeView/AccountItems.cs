using System;
using System.Drawing;
using System.IO;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main.RegistrySplit.TreeView
{
	public class NodeItem
	{
        public Image Icon;
        public BaseItem Parent;
		public string Name = "";
        public string ItemPath = "";

        private decimal balance = 0.0m;
        public string Balance
        {
            get
            {
                return balance.ToString("C");
            }
        }

        public int AccountID = SpclAccount.NULL;
        public int EnvelopeID = SpclEnvelope.NULL;

		public override string ToString()
		{
            return ItemPath;
		}

        public NodeItem(string name, int aID, int eID, decimal bal, BaseItem parent)
        {
            this.Name = name;
            this.AccountID = aID;
            this.EnvelopeID = eID;
            this.balance = bal;
        }

		/*public override bool Equals(object obj)
		{
			if (obj is BaseItem)
				return _path.Equals((obj as BaseItem).ItemPath);
			else
				return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return _path.GetHashCode();
		}*/
	}

    //public class RootItem : BaseItem
    //{
    //    //public RootItem(string name, AccountBrowserModel owner)
    //    //{
    //    //    ItemPath = name;
    //    //    Owner = owner;
    //    //}
    //}

    //public class FolderItem : BaseItem
    //{
    //    public override string Name
    //    {
    //        get
    //        {
    //            return Path.GetFileName(ItemPath);
    //        }
    //        set
    //        {
    //            string dir = Path.GetDirectoryName(ItemPath);
    //            string destination = Path.Combine(dir, value);
    //            Directory.Move(ItemPath, destination);
    //            ItemPath = destination;
    //        }
    //    }

    //    public FolderItem(string name, BaseItem parent, FolderBrowserModel owner)
    //    {
    //        ItemPath = name;
    //        Parent = parent;
    //        Owner = owner;
    //    }
    //}

    //public class FileItem : BaseItem
    //{
    //    public override string Name
    //    {
    //        get
    //        {
    //            return Path.GetFileName(ItemPath);
    //        }
    //        set
    //        {
    //            string dir = Path.GetDirectoryName(ItemPath);
    //            string destination = Path.Combine(dir, value);
    //            File.Move(ItemPath, destination);
    //            ItemPath = destination;
    //        }
    //    }

    //    public FileItem(string name, BaseItem parent, FolderBrowserModel owner)
    //    {
    //        ItemPath = name;
    //        Parent = parent;
    //        Owner = owner;
    //    }
    //}
}
