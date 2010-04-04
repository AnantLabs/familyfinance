#pragma warning disable 67  // Event never used

using System;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Threading;
using System.Collections.Generic;

using Aga.Controls.Tree;


namespace FamilyFinance2.Forms.Main.RegistrySplit.TreeView
{
	public class AccountBrowserModel: ITreeModel
	{
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   ITreeModel Interface
        ////////////////////////////////////////////////////////////////////////////////////////////
        public event EventHandler<TreeModelEventArgs> NodesChanged;
        public event EventHandler<TreeModelEventArgs> NodesInserted;
        public event EventHandler<TreeModelEventArgs> NodesRemoved;
        public event EventHandler<TreePathEventArgs> StructureChanged;

        public System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            List<NodeItem> items = null;
            items = new List<NodeItem>();

            if (treePath.IsEmpty())
            {
                items.Add(new NodeItem("testig", -1, -1, -45.435m));
                items.Add(new NodeItem("testig2", -1, -1, -45.435m));
                items.Add(new NodeItem("testig3", -1, -1, -45.435m));
                //if (_cache.ContainsKey("ROOT"))
                //    items = _cache["ROOT"];
                //else
                //{
                //    items = new List<BaseItem>();
                //    _cache.Add("ROOT", items);
                //    foreach (string str in Environment.GetLogicalDrives())
                //        items.Add(new RootItem(str, this));
                //}
            }
            else
            {
                NodeItem parent = treePath.LastNode as NodeItem;
                //if (parent != null)
                //{
                //    if (_cache.ContainsKey(parent.ItemPath))
                //        items = _cache[parent.ItemPath];
                //    else
                //    {
                //        items = new List<BaseItem>();
                //        try
                //        {
                //            foreach (string str in Directory.GetDirectories(parent.ItemPath))
                //                items.Add(new FolderItem(str, parent, this));
                //            foreach (string str in Directory.GetFiles(parent.ItemPath))
                //            {
                //                FileItem item = new FileItem(str, parent, this);
                //                items.Add(item);
                //            }
                //        }
                //        catch (IOException)
                //        {
                //            return null;
                //        }
                //        _cache.Add(parent.ItemPath, items);
                //        _itemsToRead.AddRange(items);
                //        if (!bgWorker.IsBusy)
                //            bgWorker.RunWorkerAsync();
                //    }
                //}
            }
            return items;
        }

        public bool IsLeaf(TreePath treePath)
        {
            return treePath.LastNode is NodeItem;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Avriables
        ////////////////////////////////////////////////////////////////////////////////////////////
		private BackgroundWorker bgWorker;
		private List<NodeItem> _itemsToRead;
		//private Dictionary<string, List<BaseItem>> _cache = new Dictionary<string, List<BaseItem>>();


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void ReadFilesProperties(object sender, DoWorkEventArgs e)
        {
            //while (_itemsToRead.Count > 0)
            //{
            //    BaseItem item = _itemsToRead[0];
            //    _itemsToRead.RemoveAt(0);

            //    Thread.Sleep(50); //emulate time consuming operation
            //    if (item is FolderItem)
            //    {
            //        DirectoryInfo info = new DirectoryInfo(item.ItemPath);
            //        item.Date = info.CreationTime;
            //    }
            //    else if (item is FileItem)
            //    {
            //        FileInfo info = new FileInfo(item.ItemPath);
            //        item.Size = info.Length;
            //        item.Date = info.CreationTime;
            //        if (info.Extension.ToLower() == ".ico")
            //        {
            //            Icon icon = new Icon(item.ItemPath);
            //            item.Icon = icon.ToBitmap();
            //        }
            //        else if (info.Extension.ToLower() == ".bmp")
            //        {
            //            item.Icon = new Bitmap(item.ItemPath);
            //        }
            //    }
            //    bgWorker.ReportProgress(0, item);
            //}
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnNodesChanged(e.UserState as BaseItem);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private TreePath GetPath(NodeItem item)
        {
            if (item == null)
                return TreePath.Empty;
            else
            {
                Stack<object> stack = new Stack<object>();
                while (item != null)
                {
                    stack.Push(item);
                    item = item.Parent;
                }
                return new TreePath(stack.ToArray());
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public AccountBrowserModel()
		{
            _itemsToRead = new List<NodeItem>();

			bgWorker = new BackgroundWorker();
			bgWorker.WorkerReportsProgress = true;
			bgWorker.DoWork += new DoWorkEventHandler(ReadFilesProperties);
			bgWorker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
		}

		private void OnStructureChanged()
		{
			if (StructureChanged != null)
				StructureChanged(this, new TreePathEventArgs());
		}

        internal void OnNodesChanged(NodeItem item)
		{
			if (NodesChanged != null)
			{
				TreePath path = GetPath(item.Parent);
				NodesChanged(this, new TreeModelEventArgs(path, new object[] { item }));
			}
		}
	}
}
