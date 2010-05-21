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
        //   Error Finder
        ///////////////////////////////////////////////////////////////////////
        private BackgroundWorker e_Finder;
        private List<AccountErrors> e_KnownErrors;


        private void findNewErrors()
        {
            if (!e_Finder.IsBusy)
                e_Finder.RunWorkerAsync();
        }

        private void setErrorFlags(BaseNode node)
        {
            switch (node.NodeType)
            {
                case MyNodes.EnvelopeGroup:
                case MyNodes.Envelope:
                case MyNodes.AENode:
                    return;
                case MyNodes.Root:
                    RootNode rNode = node as RootNode;
                    rNode.SetError(this.e_isCatagoryError(rNode.Catagory));

                    foreach (BaseNode child in node.Nodes)
                        setErrorFlags(child);

                    break;
                case MyNodes.AccountType:
                    TypeNode tNode = node as TypeNode;
                    tNode.SetError(this.e_isTypeError(tNode.Catagory, tNode.TypeID));

                    foreach (BaseNode child in node.Nodes)
                        setErrorFlags(child);

                    break;
                case MyNodes.Account:
                    AccountNode aNode = node as AccountNode;
                    aNode.SetError(this.e_isAccountError(aNode.AccountID));
                    break;
            }
        }



        private bool e_isCatagoryError(byte catagory)
        {
            foreach (AccountErrors er in this.e_KnownErrors)
                if (er.Catagory == catagory)
                    return true;

            return false;
        }

        private bool e_isTypeError(byte catagory, int typeID)
        {
            foreach (AccountErrors er in this.e_KnownErrors)
                if (er.TypeID == typeID && er.Catagory == catagory)
                    return true;

            return false;
        }

        private bool e_isAccountError(int accountID)
        {
            foreach (AccountErrors er in this.e_KnownErrors)
                if (er.AccountID == accountID)
                    return true;

            return false;
        }



        private void e_Finder_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = DBquery.getAccountErrors();
        }

        private void e_Finder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                throw new Exception("Finding account error exception", e.Error);

            this.e_KnownErrors = e.Result as List<AccountErrors>;

            for(int i = 0; i < this.Nodes.Count - 1; i++)
            {
                setErrorFlags(this.Nodes[i] as RootNode);
            }
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
            this.Images.Images.Add(Properties.Resources.TLVRedBank);
            this.Images.Images.Add(Properties.Resources.TLVRedEnvelope);
            this.Images.Images.Add(Properties.Resources.TLVBankAndFlag);


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
            this.findNewErrors();

            // Add the Type Nodes to the catagory Root nodes
            this.accountRootNode.HasChildren = true;
            this.Nodes.Add(this.accountRootNode);
            this.setErrorFlags(this.accountRootNode);

            if (this.showExpenseMenuItem.Checked == true)
            {
                this.expenseRootNode.HasChildren = true;
                this.Nodes.Add(this.expenseRootNode);
                this.setErrorFlags(this.expenseRootNode);
            }

            if (this.showIncomeMenuItem.Checked == true)
            {
                this.incomeRootNode.HasChildren = true;
                this.Nodes.Add(this.incomeRootNode);
                this.setErrorFlags(this.incomeRootNode);
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
            {
                // Fill with Envelope Groups
                foreach (var item in DBquery.getEnvelopeGroups())
                    rNode.Nodes.Add(new GroupNode(item.ID, item.Name));
            }
            else if (!groupEnv && cat == SpclAccountCat.ENVELOPE)
            {
                // Fill with All Envelope names
                foreach (var item in DBquery.getAllEnvelopeNames())
                    rNode.Nodes.Add(new EnvelopeNode(item.ID, item.Name));

                // Fill all Envelope balances
                foreach (var item in DBquery.getAllEnvelopeBalances())
                    this.updateBalanceRecurse(rNode, SpclAccount.NULL, item.ID, item.Balance);
            }
            else if (groupAcc)
            {
                // Fill with the Account Types
                foreach (var item in DBquery.getAccountTypes(cat))
                    rNode.Nodes.Add(new TypeNode(item.ID, item.Name, cat));
            }
            else if (cat == SpclAccountCat.ACCOUNT)
            {
                // Fill with All Account names
                foreach (var item in DBquery.getAccountNamesByCatagory(cat))
                    rNode.Nodes.Add(new AccountNode(cat, item.ID, item.Name, item.Envelopes));

                // Fill all Account balances
                foreach (var item in DBquery.getAccountBalancesByCatagory(cat))
                    this.updateBalanceRecurse(rNode, item.ID, SpclEnvelope.NULL, item.Balance);
            }
            else if (cat == SpclAccountCat.EXPENSE || cat == SpclAccountCat.INCOME)
            {
                // Fill with the Account names.  No balances
                foreach (var item in DBquery.getAccountNamesByCatagory(cat))
                    rNode.Nodes.Add(new AccountNode(cat, item.ID, item.Name, false));
            }

            this.setErrorFlags(rNode);
        }

        private void handleThisTypeNode(TypeNode tNode)
        {
            byte cat = tNode.Catagory;

            // Fill with account Names by type
            foreach (var item in DBquery.getAccountNamesByCatagoryAndType(cat, tNode.TypeID))
                tNode.Nodes.Add(new AccountNode(cat, item.ID, item.Name, item.Envelopes));

            // If this is an account add in the balances
            if (cat == SpclAccountCat.ACCOUNT)
            {
                foreach (var item in DBquery.getAccountBalancesByType(cat, tNode.TypeID))
                    this.updateBalanceRecurse(tNode, item.ID, SpclEnvelope.NULL, item.Balance);
            }

            this.setErrorFlags(tNode);
        }

        private void handleThisGroupNode(GroupNode gNode)
        {
            // Fill with names
            foreach (var item in DBquery.getEnvelopeNamesByGroup(gNode.GroupID))
                gNode.Nodes.Add(new EnvelopeNode(item.ID, item.Name));

            // Fill in the balances
            foreach (var item in DBquery.getEnvelopeBalancesByGroup(gNode.GroupID))
                this.updateBalanceRecurse(gNode, SpclAccount.NULL, item.ID, item.Balance);
        }

        private void handleThisAccountNode(AccountNode accNode)
        {
            int accountID = accNode.AccountID;

            if (accNode.Catagory == SpclAccountCat.ACCOUNT)
            {
                foreach (var item in DBquery.getSubAccountBalances(accountID))
                {
                    AENode node = new AENode(accountID, item.ID, item.Name, item.SubBalance);
                    node.ImageId = (int)NodeImage.Envelope;
                    accNode.Nodes.Add(node);
                }
            }

            this.setErrorFlags(accNode);
        }

        private void handleThisEnvelopeNode(EnvelopeNode envNode)
        {
            int envelopeID = envNode.EnvelopeID;

            foreach (var item in DBquery.getSubEnvelopeBalances(envelopeID))
            {
                AENode node = new AENode(item.ID, envelopeID, item.Name, item.SubBalance);
                node.ImageId = (int)NodeImage.Bank;
                envNode.Nodes.Add(node);
            }
        }


        private bool updateBalanceRecurse(BaseNode pNode, int accountID, int envelopeID, decimal newAmount)
        {
            switch (pNode.NodeType)
            {
                case MyNodes.Root:
                case MyNodes.AccountType:
                case MyNodes.EnvelopeGroup:
                    foreach (BaseNode child in pNode.Nodes)
                        if (updateBalanceRecurse(child, accountID, envelopeID, newAmount))
                            return true;
                    break;

                case MyNodes.Account:
                    AccountNode aNode = pNode as AccountNode;
                    if (aNode.AccountID == accountID)
                    {
                        if (envelopeID == SpclEnvelope.NULL)
                        {
                            aNode.setBalance(newAmount);
                            return true;
                        }
                        else
                            foreach (BaseNode child in pNode.Nodes)
                                if (updateBalanceRecurse(child, accountID, envelopeID, newAmount))
                                    return true;
                    }
                    break;

                case MyNodes.Envelope:
                    EnvelopeNode eNode = pNode as EnvelopeNode;
                    if (eNode.EnvelopeID == envelopeID)
                    {
                        if (accountID == SpclAccount.NULL)
                        {
                            eNode.setBalance(newAmount);
                            return true;
                        }
                        else
                            foreach (BaseNode child in pNode.Nodes)
                                if (updateBalanceRecurse(child, accountID, envelopeID, newAmount))
                                    return true;
                    }
                    break;

                case MyNodes.AENode:
                    AENode aeNode = pNode as AENode;
                    if (aeNode.EnvelopeID == envelopeID && aeNode.AccountID == accountID)
                    {
                        aeNode.setBalance(newAmount);
                        return true;
                    }
                    break;
            }
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

            e_Finder = new BackgroundWorker();
            e_Finder.RunWorkerCompleted += new RunWorkerCompletedEventHandler(e_Finder_RunWorkerCompleted);
            e_Finder.DoWork += new DoWorkEventHandler(e_Finder_DoWork);
            this.e_KnownErrors = new List<AccountErrors>();

            this.buildContextMenu();
            this.myInit();
            this.findNewErrors();
        }

        public void updateBalance(int accountID, int envelopeID)
        {
            decimal newBalance;

            if (accountID > SpclAccount.NULL && envelopeID > SpclEnvelope.NOENVELOPE)
            {
                newBalance = DBquery.getAccBalance(accountID);
                this.updateBalanceRecurse(this.accountRootNode, accountID, SpclEnvelope.NULL, newBalance);

                newBalance = DBquery.getAEBalance(accountID, envelopeID);
                this.updateBalanceRecurse(this.accountRootNode, accountID, envelopeID, newBalance);
                this.updateBalanceRecurse(this.envelopeRootNode, accountID, envelopeID, newBalance);

                newBalance = DBquery.getEnvBalance(envelopeID);
                this.updateBalanceRecurse(this.envelopeRootNode, SpclAccount.NULL, envelopeID, newBalance);
            }
            else if (accountID > SpclAccount.NULL)
            {
                newBalance = DBquery.getAccBalance(accountID);
                this.updateBalanceRecurse(this.accountRootNode, accountID, SpclEnvelope.NULL, newBalance);
            }

            findNewErrors();
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
