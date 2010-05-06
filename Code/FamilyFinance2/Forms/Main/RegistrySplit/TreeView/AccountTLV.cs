using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TreeList;
using FamilyFinance2.SharedElements;
using FamilyFinance2.Forms.Main.RegistrySplit.TreeView;

namespace FamilyFinance2.Forms.Main.RegistrySplit.TreeView
{
    public class AccountTLV : TreeListView
    {    
        ///////////////////////////////////////////////////////////////////////
        //   Local Variables
        ///////////////////////////////////////////////////////////////////////
        
        private TreeListColumn nameColumn;
        private TreeListColumn balanceColumn;

        private RootNode accountRootNode;
        private RootNode expenseRootNode;
        private RootNode incomeRootNode;
        private RootNode envelopeRootNode;

        private ToolStripMenuItem showIncomeMenuItem;
        private ToolStripMenuItem showExpenseMenuItem;
        private ToolStripMenuItem groupAccountsMenuItem;
        private ToolStripMenuItem groupEnvelopesMenuItem;


        ///////////////////////////////////////////////////////////////////////
        //   Properties
        ///////////////////////////////////////////////////////////////////////
        private int selectedAccountID;
        public int SelectedAccountID
        {
            get { return selectedAccountID; }
        }

        private int selectedEnvelopeID;
        public int SelectedEnvelopeID
        {
            get { return selectedEnvelopeID; }
        }



        ///////////////////////////////////////////////////////////////////////
        //   External Events
        ///////////////////////////////////////////////////////////////////////   
        public event SelectedAccountEnvelopeChangedEventHandler SelectedAccountEnvelopeChanged;
        public void OnSelectedAccountEnvelopeChanged(SelectedAccountEnvelopeChangedEventArgs e)
        {
            selectedAccountID = e.AccountID;
            selectedEnvelopeID = e.EnvelopeID;

            // Raises the event CloseMe
            if (SelectedAccountEnvelopeChanged != null)
                SelectedAccountEnvelopeChanged(this, e);
        }



        ///////////////////////////////////////////////////////////////////////
        //   Internal Events
        ///////////////////////////////////////////////////////////////////////
        private void theTreeListView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int accountID = -1;
            int envelopeID = -1;

            MyNodes nodeType = (this.FocusedNode as BaseNode).NodeType;

            switch (nodeType)
            {
                case MyNodes.AENode:
                    AENode aeNode = this.FocusedNode as AENode;
                    accountID = aeNode.AccountID;
                    envelopeID = aeNode.EnvelopeID;
                    break;

                case MyNodes.Account:
                    accountID = (this.FocusedNode as AccountNode).AccountID;
                    break;

                case MyNodes.Envelope:
                    envelopeID = (this.FocusedNode as EnvelopeNode).EnvelopeID;
                    break;

                case MyNodes.Root:
                case MyNodes.AccountType:
                case MyNodes.EnvelopeGroup:
                    break;
            }

            if (accountID != selectedAccountID || envelopeID != selectedEnvelopeID)
                OnSelectedAccountEnvelopeChanged(new SelectedAccountEnvelopeChangedEventArgs(accountID, envelopeID));
        }

        private void AccountTLV_NotifyBeforeExpand(Node node, bool isExpanding)
        {
            if (!isExpanding || node.Nodes.Count > 0)
                return;

            MyNodes nodeType = (node as BaseNode).NodeType;

            switch (nodeType)
            {
                case MyNodes.Root:
                    this.handleThisRootNode(node as RootNode);
                    break;

                case MyNodes.AccountType:
                    this.handleThisTypeNode(node as TypeNode);
                    break;

                case MyNodes.EnvelopeGroup:
                    this.handleThisGroupNode(node as GroupNode);
                    break;

                case MyNodes.Account:
                    this.handleThisAccountNode(node as AccountNode);
                    break;

                case MyNodes.Envelope:
                    this.handleThisEnvelopeNode(node as EnvelopeNode);
                    break;
            }

            if (node.Nodes.Count > 0)
                node.HasChildren = true;
            else
                node.HasChildren = false;
        }

        private void showIncomeMenuItem_Click(object sender, EventArgs e)
        {
            this.incomeRootNode.Nodes.Clear();
            this.incomeRootNode.Collapse();
            this.rePlantTheRoots();
        }

        private void showExpenseMenuItem_Click(object sender, EventArgs e)
        {
            this.expenseRootNode.Nodes.Clear();
            this.expenseRootNode.Collapse();
            this.rePlantTheRoots();
        }

        private void groupEnvelopesMenuItem_Click(object sender, EventArgs e)
        {
            this.myRebuildEnvelopes();
        }

        private void groupAccountsMenuItem_Click(object sender, EventArgs e)
        {
            this.myRebuildAccounts();
        }

        
 
        ///////////////////////////////////////////////////////////////////////
        //   Functions Private
        ///////////////////////////////////////////////////////////////////////
        private void myInit()
        {
            this.SuspendLayout();

            // Build Image list
            this.Images = new ImageList();
            this.Images.Images.Add(Properties.Resources.TLVBank);
            this.Images.Images.Add(Properties.Resources.TLVEnvelope);
            this.Images.Images.Add(Properties.Resources.TLVMoney);
            this.Images.Images.Add(Properties.Resources.TLVRedFlag);

            // Build theTreeListView
            this.MultiSelect = false;
            this.RowOptions.ShowHeader = false;
            this.ColumnsOptions.HeaderHeight = 0;
            this.ViewOptions.ShowGridLines = false;
            this.ViewOptions.ShowLine = true;
            this.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
            this.RowOptions.ItemHeight = 20;
            this.ViewOptions.Indent = 10;
            this.Dock = DockStyle.Fill;
            this.AfterSelect += new TreeViewEventHandler(theTreeListView_AfterSelect);
            this.NotifyBeforeExpand += new NotifyBeforeExpandHandler(AccountTLV_NotifyBeforeExpand);

            // Build the columns
            this.nameColumn = new TreeListColumn("nameColumn");
            this.nameColumn.Caption = "Name";
            this.nameColumn.AutoSize = true;
            this.nameColumn.AutoSizeRatio = 66.0F;

            this.balanceColumn = new TreeListColumn("endingBalance");
            this.balanceColumn.Caption = "Balance";
            this.balanceColumn.AutoSize = true;
            this.balanceColumn.AutoSizeRatio = 33.0F;
            this.balanceColumn.CellFormat.TextAlignment = ContentAlignment.MiddleRight;

            // Add the columns
            this.Columns.Add(nameColumn);
            this.Columns.Add(balanceColumn);

            // Make the ROOT nodes
            this.accountRootNode = new RootNode(SpclAccountCat.ACCOUNT, "Accounts");
            this.expenseRootNode = new RootNode(SpclAccountCat.EXPENSE, "Expenses");
            this.incomeRootNode = new RootNode(SpclAccountCat.INCOME, "Incomes");
            this.envelopeRootNode = new RootNode(SpclAccountCat.ENVELOPE, "Envelopes"); 

            this.ResumeLayout();
        }

        private void buildContextMenu()
        {

            // showIncome
            this.showIncomeMenuItem = new ToolStripMenuItem();
            this.showIncomeMenuItem.Name = "showIncomeMenuItem";
            this.showIncomeMenuItem.Text = "Show Incomes";
            this.showIncomeMenuItem.CheckOnClick = true;
            this.showIncomeMenuItem.Checked = false;
            this.showIncomeMenuItem.Click += new EventHandler(showIncomeMenuItem_Click);

            // showExpenses
            this.showExpenseMenuItem = new ToolStripMenuItem();
            this.showExpenseMenuItem.Name = "showExpenseMenuItem";
            //this.showExpenseMenuItem.Size = new System.Drawing.Size(170, 22);
            this.showExpenseMenuItem.Text = "Show Expences";
            this.showExpenseMenuItem.CheckOnClick = true;
            this.showExpenseMenuItem.Checked = false;
            this.showExpenseMenuItem.Click += new EventHandler(showExpenseMenuItem_Click);

            // groupAccountsMenuItem
            this.groupAccountsMenuItem = new ToolStripMenuItem();
            this.groupAccountsMenuItem.Name = "groupAccountsMenuItem";
            this.groupAccountsMenuItem.Text = "Group Accounts";
            this.groupAccountsMenuItem.CheckOnClick = true;
            this.groupAccountsMenuItem.Checked = true;
            this.groupAccountsMenuItem.Click += new EventHandler(groupAccountsMenuItem_Click);

            // groupEnvelopesMenuItem
            this.groupEnvelopesMenuItem = new ToolStripMenuItem();
            this.groupEnvelopesMenuItem.Name = "groupEnvelopesMenuItem";
            this.groupEnvelopesMenuItem.Text = "Group Envelopes";
            this.groupEnvelopesMenuItem.CheckOnClick = true;
            this.groupEnvelopesMenuItem.Checked = true;
            this.groupEnvelopesMenuItem.Click += new EventHandler(groupEnvelopesMenuItem_Click);

            // Context Menu for the AccountTreeListView
            this.ContextMenuStrip = new ContextMenuStrip();
            this.ContextMenuStrip.Items.Add(this.showIncomeMenuItem);
            this.ContextMenuStrip.Items.Add(this.showExpenseMenuItem);
            this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            this.ContextMenuStrip.Items.Add(this.groupAccountsMenuItem);
            this.ContextMenuStrip.Items.Add(this.groupEnvelopesMenuItem);


        }

        private void rePlantTheRoots()
        {
            // Clear the nodes
            this.Nodes.Clear();

            // Add the Type Nodes to the catagory Root nodes
            this.accountRootNode.HasChildren = true;
            this.Nodes.Add(this.accountRootNode);

            if (this.showExpenseMenuItem.Checked == true)
            {
                this.expenseRootNode.HasChildren = true;
                this.Nodes.Add(this.expenseRootNode);
            }

            if (this.showIncomeMenuItem.Checked == true)
            {
                this.incomeRootNode.HasChildren = true;
                this.Nodes.Add(this.incomeRootNode);
            }

            //Add the Envelope nodes
            this.envelopeRootNode.HasChildren = true;
            this.Nodes.Add(this.envelopeRootNode);

        }


        private void handleThisRootNode(RootNode rNode)
        {
            Boolean groupAcc = this.groupAccountsMenuItem.Checked;
            Boolean groupEnv = this.groupEnvelopesMenuItem.Checked;
            byte cat = rNode.Catagory;

            if (groupEnv && cat == SpclAccountCat.ENVELOPE)
                this.fillWithTypeOrGroupNodes(rNode);

            else if (!groupEnv && cat == SpclAccountCat.ENVELOPE)
                this.fillWithEnvelopeNodes(rNode, SpclEnvelopeGroup.NULL);

            else if (groupAcc)
                this.fillWithTypeOrGroupNodes(rNode);

            else if (cat == SpclAccountCat.ACCOUNT || cat == SpclAccountCat.EXPENSE || cat == SpclAccountCat.INCOME)
                this.fillWithAccountNodes(rNode, cat, SpclAccountType.NULL);
        }

        private void handleThisTypeNode(TypeNode tNode)
        {
            byte cat = tNode.Catagory;
            int typeID = tNode.TypeID;
            this.fillWithAccountNodes(tNode, cat, typeID);
        }

        private void handleThisGroupNode(GroupNode gNode)
        {
            int groupID = gNode.GroupID;
            this.fillWithEnvelopeNodes(gNode, groupID);
        }

        private void handleThisAccountNode(AccountNode accNode)
        {
            int accountID = accNode.AccountID;
            byte cat = accNode.Catagory;
            bool usesEnvelopes = accNode.Envelopes;

            if (!usesEnvelopes)
            {
                accNode.HasChildren = false;
                return;
            }

            List<SubBalanceDetails> envList = TreeQuery.getSubAccountDetails(accountID);

            foreach (var item in envList)
            {
                AENode aeNode = new AENode(accountID, item.id, item.name, item.subBalance);
                aeNode.ImageId = (int)NodeImage.Envelope;
                aeNode.HasChildren = false;
                accNode.Nodes.Add(aeNode);
            }
        }

        private void handleThisEnvelopeNode(EnvelopeNode envNode)
        {
            int envelopeID = envNode.EnvelopeID;
            envNode.Nodes.Clear();

            List<SubBalanceDetails> envList = TreeQuery.getSubEnvelopeDetails(envelopeID);

            foreach (var item in envList)
            {
                AENode aeNode = new AENode(item.id, envelopeID, item.name, item.subBalance);
                aeNode.ImageId = (int)NodeImage.Bank;
                aeNode.HasChildren = false;
                envNode.Nodes.Add(aeNode);
            }
        }


        private void fillWithTypeOrGroupNodes(RootNode rNode)
        {
            byte cat = rNode.Catagory;

            if (cat == SpclAccountCat.ENVELOPE)
                foreach (var item in TreeQuery.getGroups())
                {
                    GroupNode gNode = new GroupNode(item.Key, item.Value);
                    gNode.HasChildren = true;
                    rNode.Nodes.Add(gNode);
                }
            else
                foreach (var item in TreeQuery.getAccountTypes(cat))
                {
                    TypeNode tNode = new TypeNode(item.Key, item.Value, cat);
                    tNode.HasChildren = true;
                    rNode.Nodes.Add(tNode);
                }
        }

        private void fillWithAccountNodes(BaseNode pNode, byte cat, int typeID)
        {
            if (cat == SpclAccountCat.ACCOUNT)
            {
                List<AccountBalanceDetails> accList = TreeQuery.getRealAccountsDetails(typeID);

                foreach (var item in accList)
                {
                    AccountNode accNode = new AccountNode(item.accountID, item.accountName, item.envelopes, item.balance);
                    accNode.ImageId = (int)NodeImage.Bank;
                    accNode.HasChildren = item.envelopes;
                    pNode.Nodes.Add(accNode);
                }
            }
            else
            {
                Dictionary<int, string> inList = TreeQuery.getAccountNamesByCatAndType(cat, typeID);

                foreach (var item in inList)
                {
                    AccountNode accNode = new AccountNode(cat, item.Key, item.Value);
                    accNode.HasChildren = false;
                    pNode.Nodes.Add(accNode);
                }
            }
        }

        private void fillWithEnvelopeNodes(BaseNode pNode, int groupID)
        {
            pNode.Nodes.Clear();
            List<EnvelopeBalanceDetails> envList = TreeQuery.getEnvelopeDetails(groupID);

            foreach (var item in envList)
            {
                EnvelopeNode eNode = new EnvelopeNode(item.envelopeID, item.envelopeName, item.balance);
                eNode[0] = item.envelopeName;
                eNode[1] = item.balance.ToString("C2");
                eNode.ImageId = (int)NodeImage.Envelope;
                eNode.HasChildren = true;
                pNode.Nodes.Add(eNode);
            }
        }



        private bool updateBalance(BaseNode pNode, int accountID, int envelopeID, decimal newAmount)
        {
            //foreach (MyTreeListNode child in pNode.Nodes)
            //{
            //    int aID = child.AccountID;
            //    int eID = child.EnvelopeID;

            //    if (aID == accountID && eID == envelopeID)
            //    {
            //        child[1] = newAmount.ToString("C2");
            //        return true;
            //    }

            //    if (updateBalance(child, accountID, envelopeID, newAmount))
            //        return true;
            //    else
            //        return false;
            //}

            return false;
        }


        ///////////////////////////////////////////////////////////////////////
        //   Functions Public
        ///////////////////////////////////////////////////////////////////////
        public AccountTLV()
        {
            // Set Defaults
            selectedAccountID = SpclAccount.NULL;
            selectedEnvelopeID = SpclEnvelope.NULL;

            this.buildContextMenu();
            this.myInit();

        }

        public void updateBalanceInTheTreeView(int accountID, int envelopeID, decimal newAmount)
        {
            bool found = false;

            //foreach (MyTreeListNode child in this.Nodes)
            //{
            //    found = updateBalance(child, accountID, envelopeID, newAmount);
            //    // Do not break-out if found, sub envelopes are in accounts and envleopes be in two places.
            //}

            if (found == false)
                this.rePlantTheRoots();
        }

        public void myRebuildAccounts()
        {
            this.accountRootNode.Nodes.Clear();
            this.accountRootNode.Collapse();

            this.incomeRootNode.Nodes.Clear();
            this.incomeRootNode.Collapse();

            this.expenseRootNode.Nodes.Clear();
            this.expenseRootNode.Collapse();

            this.rePlantTheRoots();
        }

        public void myRebuildEnvelopes()
        {
            this.envelopeRootNode.Nodes.Clear();
            this.envelopeRootNode.Collapse();
            this.rePlantTheRoots();
        }

        public void myRebuildAccountType()
        {
            this.accountRootNode.Nodes.Clear();
            this.accountRootNode.Collapse();

            this.incomeRootNode.Nodes.Clear();
            this.incomeRootNode.Collapse();

            this.expenseRootNode.Nodes.Clear();
            this.expenseRootNode.Collapse();

            this.rePlantTheRoots();
        }

        public void myRebuildTree()
        {
            this.rePlantTheRoots();
        }
    }
}
