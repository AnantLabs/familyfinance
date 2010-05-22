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

        private int currentID;

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void accountBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.accountBindingSource.EndEdit();
            this.eADataSet.myUpdateAccountDB();
            DBquery.deleteOrphanELines();
            this.buildAccountTree();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            this.nameTextBox.Enabled = true;
            this.accountTypeIDComboBox.Enabled = true;
            this.catagoryIDComboBox.Enabled = true;
            this.closedCheckBox.Enabled = true;
            this.accountNormalComboBox.Enabled = true;
            this.envelopesCheckBox.Enabled = true;
        }

        private void modifyAccountTypesTSB_Click(object sender, EventArgs e)
        {
            AccountTypeForm atf = new AccountTypeForm();
            atf.ShowDialog();

            this.eADataSet.myFillAccountTypeTable();
        }

        private void accountTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MyTreeNode node = e.Node as MyTreeNode;
            currentID = node.ID;

            // End, save 
            this.accountBindingSource.EndEdit();
            this.eADataSet.myUpdateAccountDB();

            // Enable or disable the combo box
            EADataSet.AccountRow aRow = this.eADataSet.Account.FindByid(currentID);
            if (aRow == null || currentID == SpclAccount.NULL)
            {
                this.nameTextBox.Enabled = false;
                this.accountTypeIDComboBox.Enabled = false;
                this.catagoryIDComboBox.Enabled = false;
                this.closedCheckBox.Enabled = false;
                this.accountNormalComboBox.Enabled = false;
                this.envelopesCheckBox.Enabled = false;
            }
            else if (aRow.closed)
            {
                this.nameTextBox.Enabled = false;
                this.accountTypeIDComboBox.Enabled = false;
                this.catagoryIDComboBox.Enabled = false;
                this.closedCheckBox.Enabled = true;
                this.accountNormalComboBox.Enabled = false;
                this.envelopesCheckBox.Enabled = false;
            }
            else if (aRow.catagory == SpclAccountCat.EXPENSE || aRow.catagory == SpclAccountCat.INCOME)
            {
                this.nameTextBox.Enabled = true;
                this.accountTypeIDComboBox.Enabled = true;
                this.catagoryIDComboBox.Enabled = true;
                this.closedCheckBox.Enabled = true;
                this.accountNormalComboBox.Enabled = false;
                this.envelopesCheckBox.Enabled = false;

                this.envelopesCheckBox.Checked = false;
            }
            else
            {
                this.nameTextBox.Enabled = true;
                this.accountTypeIDComboBox.Enabled = true;
                this.catagoryIDComboBox.Enabled = true;
                this.closedCheckBox.Enabled = true;
                this.accountNormalComboBox.Enabled = true;
                this.envelopesCheckBox.Enabled = true;
            }


            // Set Selected Account to the selected Account
            this.accountBindingSource.Filter = "id = " + currentID.ToString();
        }

        private void EditAccountsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.accountBindingSource.EndEdit();
            this.eADataSet.myUpdateAccountDB();
            DBquery.deleteOrphanELines();
        }


        private void envelopesCheckBox_Click(object sender, EventArgs e)
        {
            string text;
            string caption;
            DialogResult res;

            // If still using envelopes return
            if (envelopesCheckBox.Checked == true)
                return;

            int count = DBquery.getELineCount(currentID);

            if (count < 1)
                return;

            caption = "Delete Envelope Lines?";

            text = "By removing the use of envelopes from this account \n";
            text += count.ToString() + " envelope lines will be deleted. \n\n";
            text += "Click YES if you want this to happen.";

            res = MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button3);

            if (res != DialogResult.Yes)
                this.envelopesCheckBox.Checked = true;
        }

        private void closedCheckBox_Click(object sender, EventArgs e)
        {
            string text;
            string caption;
            DialogResult res;

            // If "opening" an account return.
            if (closedCheckBox.Checked == false)
                return;

            if (this.eADataSet.Account.FindByid(this.currentID).catagory != SpclAccountCat.ACCOUNT)
                return;

            decimal balance = DBquery.getAccBalance(currentID);
            int count = DBquery.getAccountErrorCount(currentID);

            if (balance == 0.0m && count == 0)
                return;

            caption = "Zero balance needed";

            text = "This account has a balance of " + balance.ToString("C2") + ". \n";
            text += "This account has " + count.ToString() + " error(s). \n\n";
            text += "Please get the balance to zero and resolve the errors before closing.";

            res = MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            closedCheckBox.Checked = false;
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
                //this.accountRootNode.Expand();
            }

            // Add the ExpenceRootNode if needed
            if (expenseRootNode == null)
            {
                this.expenseRootNode = new MyTreeNode("Expenses", SpclAccount.NULL);
                this.expenseRootNode.NodeFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.expenseRootNode.ForeColor = System.Drawing.Color.Black;
                this.accountTreeView.Nodes.Add(expenseRootNode);
                //this.expenseRootNode.Expand();
            }

            // Add the IncomeRootNode if needed
            if (incomeRootNode == null)
            {
                this.incomeRootNode = new MyTreeNode("Incomes", SpclAccount.NULL);
                this.incomeRootNode.NodeFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.incomeRootNode.ForeColor = System.Drawing.Color.Black;
                this.accountTreeView.Nodes.Add(incomeRootNode);
                //this.incomeRootNode.Expand();
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

                else if (SpclAccountCat.INCOME == acc.catagory)
                    this.addToRootNode(ref this.incomeRootNode, acc.AccountTypeRow.name, acc.name, acc.id);

                else if (SpclAccountCat.EXPENSE == acc.catagory)
                    this.addToRootNode(ref this.expenseRootNode, acc.AccountTypeRow.name, acc.name, acc.id);

                else if (SpclAccountCat.ACCOUNT == acc.catagory)
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


            // Set the max on the nameTextbox
            this.nameTextBox.MaxLength = this.eADataSet.Account.nameColumn.MaxLength;

            // Build the tree
            this.accountTreeView.Nodes.Clear();
            this.buildAccountTree();

            // Setup the Binding sources and the Account Catagory list/combobox
            this.currentID = -100;
            this.accountBindingSource.Filter = "id = " + this.currentID.ToString(); // Initially blank

            this.accountTypeBindingSource.Sort = "name";

            this.catagoryIDComboBox.DataSource = this.eADataSet.AccountCatagoryList;
            this.catagoryIDComboBox.DisplayMember = "Name";
            this.catagoryIDComboBox.ValueMember = "Id";


            this.accountNormalComboBox.DataSource = this.eADataSet.AccountNormalList;
            this.accountNormalComboBox.DisplayMember = "Name";
            this.accountNormalComboBox.ValueMember = "Id";
        }


    }
}
