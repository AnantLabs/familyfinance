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
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void EditAccountsForm_Load(object sender, EventArgs e)
        {
            this.accountCatagoryTableAdapter.Fill(this.fFDBDataSet.AccountCatagory);
            this.accountTypeTableAdapter.Fill(this.fFDBDataSet.AccountType);
            this.accountTableAdapter.Fill(this.fFDBDataSet.Account);

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
            // Add all the root envelopes

            this.accountTreeView.Nodes.Clear();

            foreach ( FFDBDataSet.AccountCatagoryRow catRow in fFDBDataSet.AccountCatagory )
            {
                if (catRow.id > 0)
                {
                    MyTreeNode node = new MyTreeNode();
                    node.Text = this.fFDBDataSet.AccountCatagory.FindByid(catRow.id).name;
                    node.ID = SpclAccount.NULL;
                    this.addTypeBranch(catRow.id, ref node);
                    this.accountTreeView.Nodes.Add(node);
                }
            }

            if (this.accountTreeView.Nodes.Count > 0)
                this.accountTreeView.ExpandAll();

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
