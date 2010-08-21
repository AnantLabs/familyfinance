using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using TreeList;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main.RegistrySplit
{
    public class AccountTLV
    {    
        ///////////////////////////////////////////////////////////////////////
        //   Local Variables
        ///////////////////////////////////////////////////////////////////////
        private TreeListView accTLV;

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
        private void OnSelectedAccountEnvelopeChanged(SelectedAccountEnvelopeChangedEventArgs e)
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
        private void AccountTLV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int accountID = -1;
            int envelopeID = -1;

            MyNodes nodeType = (this.accTLV.FocusedNode as BaseNode).NodeType;

            switch (nodeType)
            {
                case MyNodes.AENode:
                    AENode aeNode = this.accTLV.FocusedNode as AENode;
                    accountID = aeNode.AccountID;
                    envelopeID = aeNode.EnvelopeID;
                    break;

                case MyNodes.Account:
                    accountID = (this.accTLV.FocusedNode as AccountNode).AccountID;
                    break;

                case MyNodes.Envelope:
                    envelopeID = (this.accTLV.FocusedNode as EnvelopeNode).EnvelopeID;
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
            this.rebuildEnvelopes();
        }

        private void groupAccountsMenuItem_Click(object sender, EventArgs e)
        {
            this.rebuildAccounts();
        }



        ///////////////////////////////////////////////////////////////////////
        //   Error Finder
        ///////////////////////////////////////////////////////////////////////
        private BackgroundWorker e_Finder;
        private List<AccountError> e_KnownErrors;


        private void findNewErrors()
        {
            if (!e_Finder.IsBusy)
                e_Finder.RunWorkerAsync();
        }

        private bool setErrorFlag(BaseNode node, int accountID, bool error)
        {
            switch (node.NodeType)
            {
                case MyNodes.EnvelopeGroup:
                case MyNodes.Envelope:
                case MyNodes.AENode:
                    break;
                case MyNodes.Root:
                    RootNode rNode = node as RootNode;
                    rNode.SetError(this.e_isCatagoryError(rNode.Catagory));

                    foreach (BaseNode child in node.Nodes)
                        if (setErrorFlag(child, accountID, error))
                            return true;
                    break;

                case MyNodes.AccountType:
                    TypeNode tNode = node as TypeNode;
                    tNode.SetError(this.e_isTypeError(tNode.Catagory, tNode.TypeID));

                    foreach (BaseNode child in node.Nodes)
                        if (setErrorFlag(child, accountID, error))
                            return true;
                    break;

                case MyNodes.Account:
                    AccountNode aNode = node as AccountNode;
                    if (aNode.AccountID == accountID)
                    {
                        aNode.SetError(error);
                        return true;
                    }
                    break;
            }
            return false;
        }

        private void setErrorFlag(BaseNode node)
        {
            switch (node.NodeType)
            {
                case MyNodes.EnvelopeGroup:
                case MyNodes.Envelope:
                case MyNodes.AENode:
                    break;
                case MyNodes.Root:
                    RootNode rNode = node as RootNode;
                    rNode.SetError(this.e_isCatagoryError(rNode.Catagory));
                    foreach (BaseNode child in node.Nodes)
                        setErrorFlag(child);
                    break;

                case MyNodes.AccountType:
                    TypeNode tNode = node as TypeNode;
                    tNode.SetError(this.e_isTypeError(tNode.Catagory, tNode.TypeID));
                    foreach (BaseNode child in node.Nodes)
                        setErrorFlag(child);
                    break;

                case MyNodes.Account:
                    AccountNode aNode = node as AccountNode;
                    aNode.SetError(this.e_isAccountError(aNode.AccountID));
                    break;
            }
        }


        private bool e_isCatagoryError(byte catagory)
        {
            foreach (AccountError er in this.e_KnownErrors)
                if (er.Catagory == catagory)
                    return true;

            return false;
        }

        private bool e_isTypeError(byte catagory, int typeID)
        {
            foreach (AccountError er in this.e_KnownErrors)
                if (er.TypeID == typeID && er.Catagory == catagory)
                    return true;

            return false;
        }

        private bool e_isAccountError(int accountID)
        {
            foreach (AccountError er in this.e_KnownErrors)
                if (er.AccountID == accountID)
                    return true;

            return false;
        }


        private void e_Finder_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = DBquery.getAccountErrors();
        }

        private bool e_doesNotContain(ref List<AccountError> list, ref AccountError item)
        {
            bool notFound = true;

            foreach (AccountError alpha in list)
                if (alpha.AccountID == item.AccountID
                    && alpha.Catagory == item.Catagory
                    && alpha.TypeID == item.TypeID)
                    notFound = false;

            return notFound;
        }

        private void e_Finder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                throw new Exception("Finding account error exception", e.Error);

            List<AccountError> newErrors = e.Result as List<AccountError>;

            // Add the new error to the new list and set the flag
            for (int i = 0; i < newErrors.Count; i++)
            {
                AccountError aError = newErrors[i];
                if (e_doesNotContain(ref e_KnownErrors, ref aError))
                {
                    this.e_KnownErrors.Add(aError);
                    switch (aError.Catagory)
                    {
                        case SpclAccountCat.ACCOUNT:
                            this.setErrorFlag(this.accountRootNode, aError.AccountID, true);
                            break;
                        case SpclAccountCat.EXPENSE:
                            this.setErrorFlag(this.expenseRootNode, aError.AccountID, true);
                            break;
                        case SpclAccountCat.INCOME:
                            this.setErrorFlag(this.incomeRootNode, aError.AccountID, true);
                            break;
                    }
                }
            }

            // Remove the fixed errors. Errors not in the new list.
            for (int i = 0; i < this.e_KnownErrors.Count; i++)
            {
                AccountError aError = this.e_KnownErrors[i];
                if (e_doesNotContain(ref newErrors, ref aError))
                {
                    this.e_KnownErrors.RemoveAt(i);
                    i--;
                    switch (aError.Catagory)
                    {
                        case SpclAccountCat.ACCOUNT:
                            this.setErrorFlag(this.accountRootNode, aError.AccountID, false);
                            break;
                        case SpclAccountCat.EXPENSE:
                            this.setErrorFlag(this.expenseRootNode, aError.AccountID, false);
                            break;
                        case SpclAccountCat.INCOME:
                            this.setErrorFlag(this.incomeRootNode, aError.AccountID, false);
                            break;
                    }
                }
            }// END for loop

        }// END e_Finder_RunWorkerCompleted()

 
        ///////////////////////////////////////////////////////////////////////
        //   Functions Private
        ///////////////////////////////////////////////////////////////////////
        private void myInit()
        {
            this.accTLV.SuspendLayout();

            this.accTLV.Text = "accountTLV";
            this.accTLV.Dock = DockStyle.Fill;

            // Build Image list
            this.accTLV.Images = new ImageList();
            this.accTLV.Images.Images.Add(Properties.Resources.TLVBank);
            this.accTLV.Images.Images.Add(Properties.Resources.TLVEnvelope);
            this.accTLV.Images.Images.Add(Properties.Resources.TLVMoney);
            this.accTLV.Images.Images.Add(Properties.Resources.TLVRedFlag);
            this.accTLV.Images.Images.Add(Properties.Resources.TLVRedBank);
            this.accTLV.Images.Images.Add(Properties.Resources.TLVRedEnvelope);
            this.accTLV.Images.Images.Add(Properties.Resources.TLVBankAndFlag);


            // Build theTreeListView
            this.accTLV.MultiSelect = false;
            this.accTLV.RowOptions.ShowHeader = false;
            this.accTLV.ColumnsOptions.HeaderHeight = 0;
            this.accTLV.ViewOptions.ShowGridLines = false;
            this.accTLV.ViewOptions.ShowLine = true;
            this.accTLV.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
            this.accTLV.RowOptions.ItemHeight = 20;
            this.accTLV.ViewOptions.Indent = 10;
            this.accTLV.Dock = DockStyle.Fill;
            this.accTLV.AfterSelect += new TreeViewEventHandler(AccountTLV_AfterSelect);
            this.accTLV.NotifyBeforeExpand += new TreeListView.NotifyBeforeExpandHandler(AccountTLV_NotifyBeforeExpand);


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
            this.accTLV.Columns.Add(nameColumn);
            this.accTLV.Columns.Add(balanceColumn);

            // Make the ROOT nodes
            this.accountRootNode = new RootNode(SpclAccountCat.ACCOUNT, "Accounts");
            this.expenseRootNode = new RootNode(SpclAccountCat.EXPENSE, "Expenses");
            this.incomeRootNode = new RootNode(SpclAccountCat.INCOME, "Incomes");
            this.envelopeRootNode = new RootNode(SpclAccountCat.ENVELOPE, "Envelopes");

            this.accTLV.ResumeLayout();
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
            this.groupAccountsMenuItem.Checked = false;
            this.groupAccountsMenuItem.Click += new EventHandler(groupAccountsMenuItem_Click);

            // groupEnvelopesMenuItem
            this.groupEnvelopesMenuItem = new ToolStripMenuItem();
            this.groupEnvelopesMenuItem.Name = "groupEnvelopesMenuItem";
            this.groupEnvelopesMenuItem.Text = "Group Envelopes";
            this.groupEnvelopesMenuItem.CheckOnClick = true;
            this.groupEnvelopesMenuItem.Checked = false;
            this.groupEnvelopesMenuItem.Click += new EventHandler(groupEnvelopesMenuItem_Click);

            // Context Menu for the AccountTreeListView
            this.accTLV.ContextMenuStrip = new ContextMenuStrip();
            this.accTLV.ContextMenuStrip.Items.Add(this.showIncomeMenuItem);
            this.accTLV.ContextMenuStrip.Items.Add(this.showExpenseMenuItem);
            this.accTLV.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            this.accTLV.ContextMenuStrip.Items.Add(this.groupAccountsMenuItem);
            this.accTLV.ContextMenuStrip.Items.Add(this.groupEnvelopesMenuItem);


        }

        private void rePlantTheRoots()
        {
            // Clear the nodes
            this.accTLV.Nodes.Clear();
            //this.findNewErrors();

            // Add the Type Nodes to the catagory Root nodes
            this.accountRootNode.HasChildren = true;
            this.accTLV.Nodes.Add(this.accountRootNode);
            this.setErrorFlag(this.accountRootNode);
            this.accountRootNode.Expand();

            if (this.showIncomeMenuItem.Checked == true)
            {
                this.incomeRootNode.HasChildren = true;
                this.accTLV.Nodes.Add(this.incomeRootNode);
                this.setErrorFlag(this.incomeRootNode);
            }

            if (this.showExpenseMenuItem.Checked == true)
            {
                this.expenseRootNode.HasChildren = true;
                this.accTLV.Nodes.Add(this.expenseRootNode);
                this.setErrorFlag(this.expenseRootNode);
            }

            //Add the Envelope nodes
            this.envelopeRootNode.HasChildren = true;
            this.accTLV.Nodes.Add(this.envelopeRootNode);
            this.envelopeRootNode.Expand();

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

            this.setErrorFlag(rNode);
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

            this.setErrorFlag(tNode);
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

            this.setErrorFlag(accNode);
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
                        {
                            foreach (BaseNode child in pNode.Nodes)
                            {
                                if (updateBalanceRecurse(child, accountID, envelopeID, newAmount))
                                    return true;
                            }

                            // If we get here the AENode was not found so handle this Account Node.
                            bool open = aNode.Expanded;

                            aNode.Nodes.Clear();
                            this.handleThisAccountNode(aNode);

                            aNode.Expanded = open;
                            return true;
                        }
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
                        {
                            foreach (BaseNode child in pNode.Nodes)
                            {
                                if (updateBalanceRecurse(child, accountID, envelopeID, newAmount))
                                    return true;
                            }

                            // If we get here the AENode was not found so handle this Account Node.
                            bool open = eNode.Expanded;

                            eNode.Nodes.Clear();
                            this.handleThisEnvelopeNode(eNode);

                            eNode.Expanded = open;
                            return true;
                        }
                    }
                    break;

                case MyNodes.AENode:
                    AENode aeNode = pNode as AENode;
                    if (aeNode.EnvelopeID == envelopeID && aeNode.AccountID == accountID)
                    {
                        if (newAmount == 0.00m)
                        {
                            // If we get here delete this node because it is zero balance.
                            // To do that in a simple way just return false and the previous iteration
                            // will think we didn't find the aeNode and do a update of all the aeNode.
                            // which will be the same as deleting it here.
                            return false;
                        }
                        else
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
            this.accTLV = new TreeListView();

            // Set Defaults
            this.selectedAccountID = SpclAccount.NULL;
            this.selectedEnvelopeID = SpclEnvelope.NULL;

            this.e_KnownErrors = new List<AccountError>();
            this.e_Finder = new BackgroundWorker();
            this.e_Finder.RunWorkerCompleted += new RunWorkerCompletedEventHandler(e_Finder_RunWorkerCompleted);
            this.e_Finder.DoWork += new DoWorkEventHandler(e_Finder_DoWork);

            this.buildContextMenu();
            this.myInit();
            this.rePlantTheRoots();
            this.findNewErrors();
        }

        public Control getControls()
        {
            return this.accTLV;
        }

        public void updateBalance(List<AEPair> aeChanges)
        {
            decimal newBalance;

            foreach (AEPair pair in aeChanges)
            {
                if (pair.AccountID > SpclAccount.NULL && pair.EnvelopeID > SpclEnvelope.NOENVELOPE)
                {
                    newBalance = DBquery.getAccBalance(pair.AccountID);
                    this.updateBalanceRecurse(this.accountRootNode, pair.AccountID, SpclEnvelope.NULL, newBalance);

                    newBalance = DBquery.getAEBalance(pair.AccountID, pair.EnvelopeID);
                    this.updateBalanceRecurse(this.accountRootNode, pair.AccountID, pair.EnvelopeID, newBalance);
                    this.updateBalanceRecurse(this.envelopeRootNode, pair.AccountID, pair.EnvelopeID, newBalance);

                    newBalance = DBquery.getEnvBalance(pair.EnvelopeID);
                    this.updateBalanceRecurse(this.envelopeRootNode, SpclAccount.NULL, pair.EnvelopeID, newBalance);
                }
                else if (pair.AccountID > SpclAccount.NULL)
                {
                    newBalance = DBquery.getAccBalance(pair.AccountID);
                    this.updateBalanceRecurse(this.accountRootNode, pair.AccountID, SpclEnvelope.NULL, newBalance);
                }
            }

            this.accTLV.Refresh();
            findNewErrors();
        }

        public void rebuildAccounts()
        {
            this.accountRootNode.Nodes.Clear();
            this.accountRootNode.Collapse();

            this.incomeRootNode.Nodes.Clear();
            this.incomeRootNode.Collapse();

            this.expenseRootNode.Nodes.Clear();
            this.expenseRootNode.Collapse();

            this.rePlantTheRoots();
            this.findNewErrors();
        }

        public void rebuildEnvelopes()
        {
            this.envelopeRootNode.Nodes.Clear();
            this.envelopeRootNode.Collapse();
            this.rePlantTheRoots();
        }

        public void rebuildAccountType()
        {
            this.accountRootNode.Nodes.Clear();
            this.accountRootNode.Collapse();

            this.incomeRootNode.Nodes.Clear();
            this.incomeRootNode.Collapse();

            this.expenseRootNode.Nodes.Clear();
            this.expenseRootNode.Collapse();

            this.rePlantTheRoots();
        }



    }
}
