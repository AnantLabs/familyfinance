using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;

using FamilyFinance2.Forms.AccountType;

namespace FamilyFinance2.Forms.EditAccounts
{
    public partial class EditAccountsForm : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private MyTreeNode accountRootNode;
        private MyTreeNode expenseRootNode;
        private MyTreeNode incomeRootNode;
        private MyTreeNode closedRootNode;

        public Changes Changes;

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void accountBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.accountBindingSource.EndEdit();
            this.eADataSet.myUpdateAccountDB();
            this.buildAccountTree();
        }

        private void modifyAccountTypesTSB_Click(object sender, EventArgs e)
        {
            AccountTypeForm atf = new AccountTypeForm();
            atf.ShowDialog();

            this.Changes.Copy(atf.Changes);

            this.eADataSet.myFillAccountTypeTable();
        }

        private void accountTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MyTreeNode node = e.Node as MyTreeNode;
            int accountID = node.ID;

            this.accountBindingSource.EndEdit(); // Required so that things save correctly.

            if (accountID > 0)
                this.accountBindingSource.Filter = "id = " + accountID.ToString();
        }

        private void EditAccountsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.accountBindingSource.EndEdit();
            this.eADataSet.myUpdateAccountDB();

            this.Changes.AddTable(DBTables.Account);
        }

        private void debitRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (debitRadioButton.Checked == true)
                creditRadioButton.Checked = false;
            else
                creditRadioButton.Checked = true;

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildAccountTree()
        {
            // Add the AccountRootNode if needed
            if (accountRootNode == null)
            {
                this.accountRootNode = new MyTreeNode("Accounts", SpclAccount.NULL);
                this.accountRootNode.NodeFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.accountRootNode.ForeColor = System.Drawing.Color.Black;
                this.accountTreeView.Nodes.Add(accountRootNode);
                this.accountRootNode.Expand();
            }

            // Add the ExpenceRootNode if needed
            if (expenseRootNode == null)
            {
                this.expenseRootNode = new MyTreeNode("Expenses", SpclAccount.NULL);
                this.expenseRootNode.NodeFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.expenseRootNode.ForeColor = System.Drawing.Color.Black;
                this.accountTreeView.Nodes.Add(expenseRootNode);
                this.expenseRootNode.Expand();
            }

            // Add the IncomeRootNode if needed
            if (incomeRootNode == null)
            {
                this.incomeRootNode = new MyTreeNode("Incomes", SpclAccount.NULL);
                this.incomeRootNode.NodeFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.incomeRootNode.ForeColor = System.Drawing.Color.Black;
                this.accountTreeView.Nodes.Add(incomeRootNode);
                this.incomeRootNode.Expand();
            }

            // Add the ClosedRootNode if needed
            if (closedRootNode == null)
            {
                this.closedRootNode = new MyTreeNode("Closed Accounts", SpclAccount.NULL);
                this.closedRootNode.NodeFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.closedRootNode.ForeColor = System.Drawing.Color.DarkSlateGray;
                this.accountTreeView.Nodes.Add(closedRootNode);
            }

            // Clear all the nodes under the roots
            this.accountRootNode.Nodes.Clear();
            this.expenseRootNode.Nodes.Clear();
            this.incomeRootNode.Nodes.Clear();
            this.closedRootNode.Nodes.Clear();

            // Add all the Accounts to an appropriate root node
            foreach (EADataSet.AccountRow acc in this.eADataSet.Account)
            {
                if (true == acc.closed)
                    this.addToRootNode(ref this.closedRootNode, acc.AccountTypeRow.name, acc.name, acc.id);

                else if (SpclAccountCat.INCOME == acc.catagoryID)
                    this.addToRootNode(ref this.incomeRootNode, acc.AccountTypeRow.name, acc.name, acc.id);

                else if (SpclAccountCat.EXPENSE == acc.catagoryID)
                    this.addToRootNode(ref this.expenseRootNode, acc.AccountTypeRow.name, acc.name, acc.id);

                else if (SpclAccountCat.ACCOUNT == acc.catagoryID)
                    this.addToRootNode(ref this.accountRootNode, acc.AccountTypeRow.name, acc.name, acc.id);
            }

            // Sort the tree, remove the closed node and put it at the end.
            //this.accountTreeView.Sort();
        }

        private void addToRootNode(ref MyTreeNode rootNode, string accType, string accName, int accID)
        {
            MyTreeNode newAcc = new MyTreeNode(accName, accID);
            newAcc.NodeFont = rootNode.NodeFont;
            newAcc.ForeColor = rootNode.ForeColor;

            // Go throu each node in the given root node and add the new account to the corrolating Account Type
            // If it doesn't exsist add a new accounttype node.
            foreach (MyTreeNode typeNode in rootNode.Nodes)
            {
                if(typeNode.Text == accType) 
                {
                    // We have found the appropriate node add the Account node, there is nothing else to do.
                    typeNode.Nodes.Add(newAcc);
                    return;
                }
            }

            // If we get here there was no type node matching the new account.   
            MyTreeNode newTypeNode = new MyTreeNode(accType, SpclAccount.NULL);
            newTypeNode.NodeFont = rootNode.NodeFont;
            newTypeNode.ForeColor = rootNode.ForeColor;
            newTypeNode.Nodes.Add(newAcc);

            rootNode.Nodes.Add(newTypeNode);
        }

        

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public EditAccountsForm()
        {
            InitializeComponent();

            // Setup Datasset
            this.eADataSet.myInit();
            this.eADataSet.myFillAccountTable();
            this.eADataSet.myFillAccountTypeTable();

            this.Changes = new Changes();

            // Set the max on the nameTextbox
            this.nameTextBox.MaxLength = this.eADataSet.Account.nameColumn.MaxLength;

            // Build the tree
            this.accountTreeView.Nodes.Clear();
            this.buildAccountTree();

            // Setup the Binding sources and the Account Catagory list/combobox
            this.accountBindingSource.Filter = "id = -100"; // Initially blank

            this.accountTypeBindingSource.Sort = "name";

            this.catagoryIDComboBox.DataSource = this.eADataSet.AccountCatagoryList;
            this.catagoryIDComboBox.DisplayMember = "Name";
            this.catagoryIDComboBox.ValueMember = "ID";
               

        }
    }
}
