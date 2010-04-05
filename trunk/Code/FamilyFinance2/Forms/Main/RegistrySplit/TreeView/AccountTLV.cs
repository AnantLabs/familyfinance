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
        private enum ImageID {Bank = 0, Envelope = 1, Money = 2, ErrorFlag = 3};
        
        private TreeListColumn nameColumn;
        private TreeListColumn balanceColumn;

        private CatagoryNode accountRootNode;
        private CatagoryNode expenseRootNode;
        private CatagoryNode incomeRootNode;
        private CatagoryNode envelopeRootNode;

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
            MyTreeListNode temp = this.FocusedNode as MyTreeListNode;

            int accountID = temp.AccountID;
            int envelopeID = temp.EnvelopeID;

            if (accountID != selectedAccountID || envelopeID != selectedEnvelopeID)
                OnSelectedAccountEnvelopeChanged(new SelectedAccountEnvelopeChangedEventArgs(accountID, envelopeID));
        }

        private void showIncomeMenuItem_Click(object sender, EventArgs e)
        {
            this.buildTheTree();
        }

        private void showExpenseMenuItem_Click(object sender, EventArgs e)
        {
            this.buildTheTree();
        }

        private void groupEnvelopesMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void groupAccountsMenuItem_Click(object sender, EventArgs e)
        {

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
            this.accountRootNode = new CatagoryNode("Accounts", SpclAccountCat.ACCOUNT);
            this.expenseRootNode = new CatagoryNode("Expenses", SpclAccountCat.EXPENSE);
            this.incomeRootNode = new CatagoryNode("Incomes", SpclAccountCat.INCOME);
            this.envelopeRootNode = new CatagoryNode("Envelopes", SpclAccountCat.NULL); 

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

        private void buildTheTree()
        {
            // Clear the nodes
            this.Nodes.Clear();

            // Add the Type Nodes to the catagory Root nodes
            this.Nodes.Add(this.accountRootNode);
            this.addAccountNodes();

            if (this.showExpenseMenuItem.Checked == true)
            {
                this.Nodes.Add(this.expenseRootNode);
                this.addInExNodes(this.expenseRootNode, SpclAccountCat.EXPENSE);
            }

            if (this.showIncomeMenuItem.Checked == true)
            {
                this.Nodes.Add(this.incomeRootNode);
                this.addInExNodes(this.incomeRootNode, SpclAccountCat.INCOME);
            }

            //Add the Envelope nodes
            this.Nodes.Add(this.envelopeRootNode);
            //this.addEnvelopeNodes(this.envelopeRootNode, SpclEnvelope.NULL);


            //Expand account and envelope Nodes
            
            //this.EndUpdate();
            //this.ResumeLayout();
        }


        private void updateCatagoryNode(CatagoryNode cNode)
        {
            // Clear any nodes that might already be here
            cNode.Nodes.Clear();

            // If we are grouping the accounts pass this on to the types to add the Account root node.
            if (this.groupAccountsMenuItem.Checked)
            {
                this.updateCatagoryNodeWithTypes(this.accountRootNode);
            }
            else if (cNode.Catagory == SpclAccountCat.ACCOUNT)
            {
                // Else we are going to add all the accounts to the Accounts ROOT node with their balances.
                List<AccountBalanceDetails> abdList = TreeQuery.getAccountsForCatagoryDetails();

                foreach (AccountBalanceDetails item in abdList)
                {
                    AccountNode accNode = new AccountNode(item.accountName, item.accountID, item.envelopes);
                    accNode[0] = item.accountName;
                    accNode[1] = item.balance.ToString("C2");

                    accNode.HasChildren = item.envelopes;
                    accNode.ImageId = (int)ImageID.Bank;
                    cNode.Nodes.Add(accNode);
                }
            }
            else
            {
                // Else this is an income or Expense catagory
                Dictionary<int, string> accList = TreeQuery.getInExForCatagory(cNode.Catagory);

                foreach (var item in accList)
                {
                    AccountNode accNode = new AccountNode(item.Value, item.Key, false);
                    accNode.HasChildren = false;
                    cNode.Nodes.Add(accNode);
                }
            }
        }

        private void updateCatagoryNodeWithTypes(CatagoryNode cNode)
        {
            cNode.Nodes.Clear();
            Dictionary<int, string> typeList = TreeQuery.getTypesForCatagory(cNode.Catagory);

            foreach (var item in typeList)
            {
                TypeNode tNode = new TypeNode(item.Value, cNode.Catagory, item.Key);
                tNode.HasChildren = true;
                cNode.Nodes.Add(tNode);
            }
        }

        private void addAccountNodes()
        {
            // Make the tree structure below.
            // Accounts  <- Main account ROOT node
            //  |-> AccountType
            //       |-> Account
            //            |-> Envelope

            // Clear any nodes that might already be hear
            //this.accountRootNode.Nodes.Clear();

            //// Get the list of accounts for the given catagory.
            //List<AccountBalanceDetails> accList;
            //accList = TreeQuery.myGetAccountBalanceDetails();

            //int currentTypeID = SpclAccountType.NULL;
            //MyTreeListNode typeNode = null;
                
            //// Add the account type nodes 
            //foreach (AccountBalanceDetails accItem in accList)
            //{
            //    // Add the type node when this is a new type
            //    if (accItem.typeID != currentTypeID)
            //    {
            //        if (typeNode != null)
            //            this.accountRootNode.Nodes.Add(typeNode);

            //        typeNode = this.makeNameNode(accItem.typeName);
            //        currentTypeID = accItem.typeID;
            //    }

            //    // Add the account to the type Node
            //    MyTreeListNode accNode = this.makeBalanceNode(accItem.accountName, accItem.balance, SpclEnvelope.NULL, accItem.accountID);
            //    typeNode.Nodes.Add(accNode);
            //    accNode.ImageId = (int)ImageID.Bank;

                
            //    // Add the subAccounts/subEnvelopes to this new accNode account.
            //    if(accItem.envelopes)
            //    {
            //        List<SubBalanceDetails> list = TreeQuery.myGetSubAccountBalanceDetails(accItem.accountID);
            //        foreach (SubBalanceDetails subBal in list)
            //        {
            //            MyTreeListNode subEnv = makeBalanceNode(subBal.name, subBal.subBalance, subBal.id, accItem.accountID);
            //            subEnv.ImageId = (int)ImageID.Envelope;
            //            accNode.Nodes.Add(subEnv);
            //        }
            //    }
                
            //}// END foreach (AccountBalanceDetails account in accList)
        }

        private void addInExNodes(CatagoryNode pNode, byte catagory)
        {
            // Make the tree structure below.
            // Incomes or expenses  <- Main account ROOT nodes
            //  |-> AccountType
            //       |-> Account
            //            |-> Envelope

            // Clear any nodes that might already be hear
            //pNode.Nodes.Clear();

            //// Get the list of accounts for the given catagory.
            //List<IncomeExpenseDetails> accList;
            //accList = TreeQuery.myGetIncomeExpenseDetails(catagory);

            //int currentTypeID = SpclAccountType.NULL;
            //MyTreeListNode typeNode = null;

            //// Add the account type nodes 
            //foreach (IncomeExpenseDetails ieItem in accList)
            //{
            //    // Add the type node when this is a new type
            //    if (ieItem.typeID != currentTypeID)
            //    {
            //        if (typeNode != null)
            //            pNode.Nodes.Add(typeNode);

            //        typeNode = this.makeNameNode(ieItem.typeName);
            //        currentTypeID = ieItem.typeID;
            //    }

            //    // Add the account to the type Node
            //    MyTreeListNode ieNode = this.makeNameNode(ieItem.accountName);
            //    typeNode.Nodes.Add(ieNode);

            //}
        }



        private void addEnvelopeNodes(CatagoryNode pNode)
        {
            //// Clear any nodes that might already be hear
            //pNode.Nodes.Clear();

            //// If inside an Envelope add the money node.
            //if (parentID != SpclEnvelope.NULL)
            //{
            //    string pName = (string)pNode[0];
            //    string pBal = (string)pNode[1];
            //    pBal = pBal.Replace("$", "");
            //    pBal = pBal.Replace("(", "");
            //    pBal = pBal.Replace(")", "");
            //    decimal bal = Convert.ToDecimal(pBal);
            //    MyTreeListNode money = makeBalanceNode(pName, bal, parentID, SpclAccount.NULL);
            //    money.ImageId = (int)ImageID.Money;
            //    pNode.Nodes.Add(money);

            //    // Add the subAccounts/subEnvelopes to this new money node.
            //    List<SubBalanceDetails> subList = FFDBDataSet.myGetSubEnvelopeBalanceDetails(parentID);
            //    foreach (SubBalanceDetails subBal in subList)
            //    {
            //        MyTreeListNode subEnv = makeBalanceNode(subBal.name, subBal.subCurrentBalance, parentID, subBal.id);
            //        subEnv.ImageId = (int)ImageID.Bank;
            //        money.Nodes.Add(subEnv);
            //    }
            //}

            //// Get the list of envelopes with the given parentID;
            //List<EnvelopeBalanceDetails> list = FFDBDataSet.myGetChildEnvelopeBalanceDetails(parentID);

            //foreach (EnvelopeBalanceDetails envelope in list)
            //{
            //    // Add all the envelopes to the parent Node
            //    MyTreeListNode childNode = makeBalanceNode(envelope.name, envelope.balance, envelope.envelopeID, SpclEnvelope.NULL);
            //    childNode.ImageId = (int)ImageID.Envelope;
            //    pNode.Nodes.Add(childNode);

            //    // Recurse and do the same for the children of this new child envelope
            //    addEnvelopeNodes(childNode, envelope.envelopeID);
            //}
        }



        private bool updateBalance(CatagoryNode pNode, int accountID, int envelopeID, decimal newAmount)
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

            this.accountRootNode.Expand();
            this.envelopeRootNode.Expand();
        }

        public void updateBalanceInTheTreeView(int accountID, int envelopeID, decimal newAmount)
        {
            bool found = false;

            foreach (MyTreeListNode child in this.Nodes)
            {
                found = updateBalance(child, accountID, envelopeID, newAmount);
                // Do not break-out if found, sub envelopes are in accounts and envleopes be in two places.
            }

            if (found == false)
                this.buildTheTree();
        }

        public void myRebuildTree()
        {
            this.buildTheTree();
        }

    }
}
