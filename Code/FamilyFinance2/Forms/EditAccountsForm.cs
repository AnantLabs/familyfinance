using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2.Forms
{
    public partial class EditAccountsForm : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private MyTreeNode accountRootNode;
        private MyTreeNode expenseRootNode;
        private MyTreeNode incomeRootNode;

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void EditAccountsForm_Load(object sender, EventArgs e)
        {
            this.accountCatagoryTableAdapter.Fill(this.fFDBDataSet.AccountCatagory);
            this.accountTypeTableAdapter.Fill(this.fFDBDataSet.AccountType);
            this.accountTableAdapter.Fill(this.fFDBDataSet.Account);

            this.accountTreeView.Nodes.Clear();

            this.buildAccountTree();

            this.accountBindingSource.Filter = "id = -100";
        }

        private void accountBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            saveChanges();
        }

        private void modifyAccountTypesTSB_Click(object sender, EventArgs e)
        {
            AccountTypeForm atf = new AccountTypeForm();
            atf.ShowDialog();
            this.accountTypeTableAdapter.Fill(this.fFDBDataSet.AccountType);
        }

        private void accountTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MyTreeNode node = e.Node as MyTreeNode;
            short accountID = node.ID;

            if(accountID > 0)
                this.accountBindingSource.Filter = "id = " + accountID.ToString();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildAccountTree()
        {
            // Add the AccountRootNode if needed
            if (accountRootNode == null)
            {
                accountRootNode = new MyTreeNode();
                accountRootNode.Text = "Accounts";
                accountRootNode.ID = SpclAccount.NULL;
                this.accountTreeView.Nodes.Add(accountRootNode);
                this.accountRootNode.Expand();
            }

            // Add the ExpenceRootNode if needed
            if (expenseRootNode == null)
            {
                expenseRootNode = new MyTreeNode();
                expenseRootNode.Text = "Expenses";
                expenseRootNode.ID = SpclAccount.NULL;
                this.accountTreeView.Nodes.Add(expenseRootNode);
                this.expenseRootNode.Expand();
            }

            // Add the IncomeRootNode if needed
            if (incomeRootNode == null)
            {
                incomeRootNode = new MyTreeNode();
                incomeRootNode.Text = "Incomes";
                incomeRootNode.ID = SpclAccount.NULL;
                this.accountTreeView.Nodes.Add(incomeRootNode);
                this.incomeRootNode.Expand();
            }

            // Clear all the nodes under the roots
            this.accountRootNode.Nodes.Clear();
            this.expenseRootNode.Nodes.Clear();
            this.incomeRootNode.Nodes.Clear();

            // Add all the Type nodes under the catagory roots
            this.addTypeBranch(SpclAccountCat.ACCOUNT, ref this.accountRootNode);
            this.addTypeBranch(SpclAccountCat.EXPENSE, ref this.expenseRootNode);
            this.addTypeBranch(SpclAccountCat.INCOME, ref this.incomeRootNode);

            this.accountTreeView.Sort();
        }

        private void addTypeBranch(byte catID, ref MyTreeNode branch)
        {
            foreach (FFDBDataSet.AccountTypeRow typeRow in fFDBDataSet.AccountType)
            {
                if (typeRow.id > 0)
                {
                    MyTreeNode node = new MyTreeNode();
                    node.Text = this.fFDBDataSet.AccountType.FindByid(typeRow.id).name;
                    node.ID = SpclAccount.NULL;
                    this.addAccountBranch(catID, typeRow.id, ref node);

                    if(node.Nodes.Count > 0)
                        branch.Nodes.Add(node);
                }
            }
        }

        private void addAccountBranch(byte catID, short accType, ref MyTreeNode accNode)
        {
            foreach (FFDBDataSet.AccountRow row in fFDBDataSet.Account)
            {
                if (row.id > 0 && row.catagoryID == catID && row.accountTypeID == accType)
                {
                    MyTreeNode node = new MyTreeNode();
                    node.Text = this.fFDBDataSet.Account.FindByid(row.id).name;
                    node.ID = row.id;

                    accNode.Nodes.Add(node);
                }
            }
        }

        private void saveChanges()
        {
            this.Validate();
            this.accountBindingSource.EndEdit();
            this.accountTableAdapter.Update(this.fFDBDataSet.Account);

            this.buildAccountTree();
        }

        

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public EditAccountsForm()
        {
            InitializeComponent();
        }







    }
}
