using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TreeList;

namespace FamilyFinance2
{
    public partial class AccountTLV : TreeListView
    {    
        ///////////////////////////////////////////////////////////////////////
        //   Local Variables
        ///////////////////////////////////////////////////////////////////////
        private enum ImageID {Bank = 0, Envelope = 1, Money = 2, ErrorFlag = 3};
        
        private TreeListColumn nameColumn;
        private TreeListColumn balanceColumn;

        private MyTreeListNode accountRootNode;
        private MyTreeListNode expenseRootNode;
        private MyTreeListNode incomeRootNode;
        private MyTreeListNode envelopeRootNode;

        private ToolStripMenuItem showIncomeMenuItem;
        private ToolStripMenuItem showExpenseMenuItem;


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

            short accountID = temp.AccountID;
            short envelopeID = temp.EnvelopeID;

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


 
        ///////////////////////////////////////////////////////////////////////
        //   Functions Private
        ///////////////////////////////////////////////////////////////////////
        private void myInit()
        {
            this.SuspendLayout();

            // Defaults

            // Build theTreeListView
            this.Images = new ImageList();
            this.Images.Images.Add(Properties.Resources.TLVBank);
            this.Images.Images.Add(Properties.Resources.TLVEnvelope);
            this.Images.Images.Add(Properties.Resources.TLVMoney);
            this.Images.Images.Add(Properties.Resources.TLVRedFlag);

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


            ///////////////////////////////////////////////////////////////////
            // Clear and setup the columns
            this.Columns.Clear();

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

            // Make and Add the root nodes
            this.accountRootNode = makeNameNode("Accounts");
            this.expenseRootNode = makeNameNode("Expenses");
            this.incomeRootNode = makeNameNode("Incomes");
            this.envelopeRootNode = makeNameNode("Envelopes"); 

            this.buildTheTree();           

            this.ResumeLayout();
        }

        private void buildContextMenu()
        {

            // showIncome
            showIncomeMenuItem = new ToolStripMenuItem();
            showIncomeMenuItem.Name = "envelopesToolStripMenuItem";
            showIncomeMenuItem.Text = "Show Incomes";
            showIncomeMenuItem.CheckOnClick = true;
            showIncomeMenuItem.Checked = false;
            showIncomeMenuItem.Click += new EventHandler(showIncomeMenuItem_Click);

            // showExpenses
            showExpenseMenuItem = new ToolStripMenuItem();
            showExpenseMenuItem.Name = "envelopesToolStripMenuItem";
            showExpenseMenuItem.Size = new System.Drawing.Size(170, 22);
            showExpenseMenuItem.Text = "Show Expences";
            showExpenseMenuItem.CheckOnClick = true;
            showExpenseMenuItem.Checked = false;
            showExpenseMenuItem.Click += new EventHandler(showExpenseMenuItem_Click);

            // Context Menu for the AccountTreeListView
            this.ContextMenuStrip = new ContextMenuStrip();
            this.ContextMenuStrip.Items.Add(showIncomeMenuItem);
            this.ContextMenuStrip.Items.Add(showExpenseMenuItem);


        }

        private void buildTheTree()
        {
            // Clear the nodes
            this.Nodes.Clear();

            // Add the Type Nodes to the catagory Root nodes
            this.Nodes.Add(this.accountRootNode);
            this.addTypeNodes(this.accountRootNode, SpclAccountCat.ACCOUNT);

            if (this.showExpenseMenuItem.Checked == true)
            {
                this.Nodes.Add(this.expenseRootNode);
                this.addTypeNodes(this.expenseRootNode, SpclAccountCat.EXPENSE);
            }

            if (this.showIncomeMenuItem.Checked == true)
            {
                this.Nodes.Add(this.incomeRootNode);
                this.addTypeNodes(this.incomeRootNode, SpclAccountCat.INCOME);
            }

            //Add the Envelope nodes
            this.Nodes.Add(this.envelopeRootNode);
            this.addEnvelopeNodes(this.envelopeRootNode, SpclEnvelope.NULL);


            //Expand account and envelope Nodes
            
            //this.EndUpdate();
            //this.ResumeLayout();
        }



        private void addTypeNodes(MyTreeListNode pNode, byte catagory)
        {
            MyTreeListNode typeNode = new MyTreeListNode();
            short thisTypeID = SpclAccountType.NULL;
            List<AccountBalanceDetails> accList;

            // Clear any nodes that might already be hear
            pNode.Nodes.Clear();

            // Get the list of accounts for the given catagory.
            accList = FFDBDataSet.myGetAccountBalanceDetails(catagory);
                
            // Add the account type nodes 
            foreach (AccountBalanceDetails account in accList)
            {
                MyTreeListNode accNode;

                // Create and add a new type node if needed.
                if (thisTypeID != account.typeID)
                {
                    thisTypeID = account.typeID;
                    typeNode = makeNameNode(account.typeName);
                    pNode.Nodes.Add(typeNode);
                }

                // Add the account to the type
                accNode = makeBalanceNode(account.accountName, account.currentBalance, account.accountID, SpclEnvelope.NULL);
                typeNode.Nodes.Add(accNode);

                // Add the appropriate image
                if (!account.error)
                {
                    switch (catagory)
                    {
                        case SpclAccountCat.ACCOUNT:
                            accNode.ImageId = (int)ImageID.Bank;
                            break;

                        case SpclAccountCat.EXPENSE:
                            break;

                        case SpclAccountCat.INCOME:
                            break;
                    }
                }
                else
                {
                    accNode.ImageId = (int)ImageID.ErrorFlag;
                }

                // Add the subAccounts/subEnvelopes to this new accNode account.
                if (catagory == SpclAccountCat.ACCOUNT)
                {
                    List<SubBalanceDetails> list = FFDBDataSet.myGetSubAccountBalanceDetails(account.accountID);
                    foreach (SubBalanceDetails subBal in list)
                    {
                        MyTreeListNode subEnv = makeBalanceNode(subBal.name, subBal.subCurrentBalance, account.accountID, subBal.id);
                        subEnv.ImageId = (int)ImageID.Envelope;
                        accNode.Nodes.Add(subEnv);
                    }
                }
            }// END foreach (AccountBalanceDetails account in accList)
        }// END private void addTypeNodes(MyTreeListNode pNode, byte catagory)

        private void addEnvelopeNodes(MyTreeListNode pNode, short parentID)
        {
            // Clear any nodes that might already be hear
            pNode.Nodes.Clear();

            // If inside an Envelope add the money node.
            if (parentID != SpclEnvelope.NULL)
            {
                string pName = (string)pNode[0];
                string pBal = (string)pNode[1];
                pBal = pBal.Replace("$", "");
                pBal = pBal.Replace("(", "");
                pBal = pBal.Replace(")", "");
                decimal bal = Convert.ToDecimal(pBal);
                MyTreeListNode money = makeBalanceNode(pName, bal, SpclAccount.NULL, parentID);
                money.ImageId = (int)ImageID.Money;
                pNode.Nodes.Add(money);

                // Add the subAccounts/subEnvelopes to this new money node.
                List<SubBalanceDetails> subList = FFDBDataSet.myGetSubEnvelopeBalanceDetails(parentID);
                foreach (SubBalanceDetails subBal in subList)
                {
                    MyTreeListNode subEnv = makeBalanceNode(subBal.name, subBal.subCurrentBalance, subBal.id, parentID);
                    subEnv.ImageId = (int)ImageID.Bank;
                    money.Nodes.Add(subEnv);
                }
            }

            // Get the list of envelopes with the given parentID;
            List<EnvelopeBalanceDetails> list = FFDBDataSet.myGetChildEnvelopeBalanceDetails(parentID);

            foreach (EnvelopeBalanceDetails envelope in list)
            {
                // Add all the envelopes to the parent Node
                MyTreeListNode childNode = makeBalanceNode(envelope.name, envelope.balance, SpclEnvelope.NULL, envelope.envelopeID);
                childNode.ImageId = (int)ImageID.Envelope;
                pNode.Nodes.Add(childNode);

                // Recurse and do the same for the children of this new child envelope
                addEnvelopeNodes(childNode, envelope.envelopeID);
            }
        }
                     
        private MyTreeListNode makeBalanceNode(string name, decimal balance, short accID, short envID)
        {
            MyTreeListNode newNode = new MyTreeListNode();

            newNode[0] = name;
            newNode[1] = balance.ToString("C2");
            newNode.AccountID = accID;
            newNode.EnvelopeID = envID;

            return newNode;
        }

        private MyTreeListNode makeNameNode(string name)
        {
            MyTreeListNode newNode = new MyTreeListNode();

            newNode[0] = name;
            newNode[1] = null;
            newNode.AccountID = SpclAccount.NULL;
            newNode.EnvelopeID = SpclEnvelope.NULL;

            return newNode;
        }


        
        private bool updateBalance(MyTreeListNode pNode, int accountID, int envelopeID, decimal newAmount)
        {
            foreach (MyTreeListNode child in pNode.Nodes)
            {
                int aID = child.AccountID;
                int eID = child.EnvelopeID;

                if (aID == accountID && eID == envelopeID)
                {
                    child[1] = newAmount.ToString("C2");
                    return true;
                }

                if (updateBalance(child, accountID, envelopeID, newAmount))
                    return true;
                else
                    return false;
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
