namespace FamilyFinance2.Forms
{
    partial class EditAccountsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label nameLabel;
            System.Windows.Forms.Label accountTypeIDLabel;
            System.Windows.Forms.Label catagoryLabel;
            System.Windows.Forms.Label closedLabel;
            System.Windows.Forms.Label creditDebitLabel;
            System.Windows.Forms.Label envelopesLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditAccountsForm));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.accountTreeView = new System.Windows.Forms.TreeView();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.accountBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.fFDBDataSet = new FamilyFinance2.FFDBDataSet();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.accountTypeIDComboBox = new System.Windows.Forms.ComboBox();
            this.accountTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.catagoryComboBox = new System.Windows.Forms.ComboBox();
            this.accountCatagoryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.closedCheckBox = new System.Windows.Forms.CheckBox();
            this.envelopesCheckBox = new System.Windows.Forms.CheckBox();
            this.accountBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.accountBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.modifyAccountTypesTSB = new System.Windows.Forms.ToolStripButton();
            this.accountTableAdapter = new FamilyFinance2.FFDBDataSetTableAdapters.AccountTableAdapter();
            this.accountTypeTableAdapter = new FamilyFinance2.FFDBDataSetTableAdapters.AccountTypeTableAdapter();
            this.accountCatagoryTableAdapter = new FamilyFinance2.FFDBDataSetTableAdapters.AccountCatagoryTableAdapter();
            nameLabel = new System.Windows.Forms.Label();
            accountTypeIDLabel = new System.Windows.Forms.Label();
            catagoryLabel = new System.Windows.Forms.Label();
            closedLabel = new System.Windows.Forms.Label();
            creditDebitLabel = new System.Windows.Forms.Label();
            envelopesLabel = new System.Windows.Forms.Label();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accountBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fFDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountCatagoryBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountBindingNavigator)).BeginInit();
            this.accountBindingNavigator.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(51, 35);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(38, 13);
            nameLabel.TabIndex = 1;
            nameLabel.Text = "Name:";
            // 
            // accountTypeIDLabel
            // 
            accountTypeIDLabel.AutoSize = true;
            accountTypeIDLabel.Location = new System.Drawing.Point(55, 88);
            accountTypeIDLabel.Name = "accountTypeIDLabel";
            accountTypeIDLabel.Size = new System.Drawing.Size(34, 13);
            accountTypeIDLabel.TabIndex = 3;
            accountTypeIDLabel.Text = "Type:";
            // 
            // catagoryLabel
            // 
            catagoryLabel.AutoSize = true;
            catagoryLabel.Location = new System.Drawing.Point(37, 61);
            catagoryLabel.Name = "catagoryLabel";
            catagoryLabel.Size = new System.Drawing.Size(52, 13);
            catagoryLabel.TabIndex = 5;
            catagoryLabel.Text = "Catagory:";
            // 
            // closedLabel
            // 
            closedLabel.AutoSize = true;
            closedLabel.Location = new System.Drawing.Point(47, 176);
            closedLabel.Name = "closedLabel";
            closedLabel.Size = new System.Drawing.Size(42, 13);
            closedLabel.TabIndex = 7;
            closedLabel.Text = "Closed:";
            // 
            // creditDebitLabel
            // 
            creditDebitLabel.AutoSize = true;
            creditDebitLabel.Location = new System.Drawing.Point(3, 117);
            creditDebitLabel.Name = "creditDebitLabel";
            creditDebitLabel.Size = new System.Drawing.Size(86, 13);
            creditDebitLabel.TabIndex = 9;
            creditDebitLabel.Text = "Account Normal:";
            // 
            // envelopesLabel
            // 
            envelopesLabel.AutoSize = true;
            envelopesLabel.Location = new System.Drawing.Point(29, 146);
            envelopesLabel.Name = "envelopesLabel";
            envelopesLabel.Size = new System.Drawing.Size(60, 13);
            envelopesLabel.TabIndex = 11;
            envelopesLabel.Text = "Envelopes:";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.accountTreeView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.AutoScroll = true;
            this.splitContainer.Panel2.Controls.Add(this.checkBox1);
            this.splitContainer.Panel2.Controls.Add(nameLabel);
            this.splitContainer.Panel2.Controls.Add(this.nameTextBox);
            this.splitContainer.Panel2.Controls.Add(accountTypeIDLabel);
            this.splitContainer.Panel2.Controls.Add(this.accountTypeIDComboBox);
            this.splitContainer.Panel2.Controls.Add(catagoryLabel);
            this.splitContainer.Panel2.Controls.Add(this.catagoryComboBox);
            this.splitContainer.Panel2.Controls.Add(closedLabel);
            this.splitContainer.Panel2.Controls.Add(this.closedCheckBox);
            this.splitContainer.Panel2.Controls.Add(creditDebitLabel);
            this.splitContainer.Panel2.Controls.Add(envelopesLabel);
            this.splitContainer.Panel2.Controls.Add(this.envelopesCheckBox);
            this.splitContainer.Panel2.Controls.Add(this.accountBindingNavigator);
            this.splitContainer.Size = new System.Drawing.Size(566, 376);
            this.splitContainer.SplitterDistance = 248;
            this.splitContainer.TabIndex = 0;
            // 
            // accountTreeView
            // 
            this.accountTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountTreeView.FullRowSelect = true;
            this.accountTreeView.HideSelection = false;
            this.accountTreeView.Location = new System.Drawing.Point(0, 0);
            this.accountTreeView.Name = "accountTreeView";
            this.accountTreeView.Size = new System.Drawing.Size(248, 376);
            this.accountTreeView.TabIndex = 0;
            this.accountTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.accountTreeView_AfterSelect);
            // 
            // checkBox1
            // 
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.accountBindingSource, "creditDebit", true));
            this.checkBox1.Location = new System.Drawing.Point(95, 112);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(121, 24);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Debit";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // accountBindingSource
            // 
            this.accountBindingSource.DataMember = "Account";
            this.accountBindingSource.DataSource = this.fFDBDataSet;
            // 
            // fFDBDataSet
            // 
            this.fFDBDataSet.DataSetName = "FFDBDataSet";
            this.fFDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.accountBindingSource, "name", true));
            this.nameTextBox.Location = new System.Drawing.Point(95, 32);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(207, 20);
            this.nameTextBox.TabIndex = 2;
            // 
            // accountTypeIDComboBox
            // 
            this.accountTypeIDComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.accountTypeIDComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.accountBindingSource, "accountTypeID", true));
            this.accountTypeIDComboBox.DataSource = this.accountTypeBindingSource;
            this.accountTypeIDComboBox.DisplayMember = "name";
            this.accountTypeIDComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.accountTypeIDComboBox.FormattingEnabled = true;
            this.accountTypeIDComboBox.Location = new System.Drawing.Point(95, 85);
            this.accountTypeIDComboBox.Name = "accountTypeIDComboBox";
            this.accountTypeIDComboBox.Size = new System.Drawing.Size(207, 21);
            this.accountTypeIDComboBox.TabIndex = 4;
            this.accountTypeIDComboBox.ValueMember = "id";
            // 
            // accountTypeBindingSource
            // 
            this.accountTypeBindingSource.DataMember = "AccountType";
            this.accountTypeBindingSource.DataSource = this.fFDBDataSet;
            this.accountTypeBindingSource.Filter = "id > 0";
            // 
            // catagoryComboBox
            // 
            this.catagoryComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.catagoryComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.accountBindingSource, "catagoryID", true));
            this.catagoryComboBox.DataSource = this.accountCatagoryBindingSource;
            this.catagoryComboBox.DisplayMember = "name";
            this.catagoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.catagoryComboBox.FormattingEnabled = true;
            this.catagoryComboBox.Location = new System.Drawing.Point(95, 58);
            this.catagoryComboBox.Name = "catagoryComboBox";
            this.catagoryComboBox.Size = new System.Drawing.Size(207, 21);
            this.catagoryComboBox.TabIndex = 6;
            this.catagoryComboBox.ValueMember = "id";
            // 
            // accountCatagoryBindingSource
            // 
            this.accountCatagoryBindingSource.DataMember = "AccountCatagory";
            this.accountCatagoryBindingSource.DataSource = this.fFDBDataSet;
            this.accountCatagoryBindingSource.Filter = "id > 0";
            // 
            // closedCheckBox
            // 
            this.closedCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.accountBindingSource, "closed", true));
            this.closedCheckBox.Location = new System.Drawing.Point(95, 171);
            this.closedCheckBox.Name = "closedCheckBox";
            this.closedCheckBox.Size = new System.Drawing.Size(121, 24);
            this.closedCheckBox.TabIndex = 8;
            this.closedCheckBox.UseVisualStyleBackColor = true;
            // 
            // envelopesCheckBox
            // 
            this.envelopesCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.accountBindingSource, "envelopes", true));
            this.envelopesCheckBox.Location = new System.Drawing.Point(95, 141);
            this.envelopesCheckBox.Name = "envelopesCheckBox";
            this.envelopesCheckBox.Size = new System.Drawing.Size(121, 24);
            this.envelopesCheckBox.TabIndex = 12;
            this.envelopesCheckBox.UseVisualStyleBackColor = true;
            // 
            // accountBindingNavigator
            // 
            this.accountBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.accountBindingNavigator.BindingSource = this.accountBindingSource;
            this.accountBindingNavigator.CountItem = null;
            this.accountBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.accountBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.accountBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.accountBindingNavigatorSaveItem,
            this.toolStripSeparator1,
            this.modifyAccountTypesTSB});
            this.accountBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.accountBindingNavigator.MoveFirstItem = null;
            this.accountBindingNavigator.MoveLastItem = null;
            this.accountBindingNavigator.MoveNextItem = null;
            this.accountBindingNavigator.MovePreviousItem = null;
            this.accountBindingNavigator.Name = "accountBindingNavigator";
            this.accountBindingNavigator.PositionItem = null;
            this.accountBindingNavigator.Size = new System.Drawing.Size(314, 25);
            this.accountBindingNavigator.TabIndex = 1;
            this.accountBindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Enabled = false;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            this.bindingNavigatorDeleteItem.Visible = false;
            // 
            // accountBindingNavigatorSaveItem
            // 
            this.accountBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.accountBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("accountBindingNavigatorSaveItem.Image")));
            this.accountBindingNavigatorSaveItem.Name = "accountBindingNavigatorSaveItem";
            this.accountBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.accountBindingNavigatorSaveItem.Text = "Save Data";
            this.accountBindingNavigatorSaveItem.Click += new System.EventHandler(this.accountBindingNavigatorSaveItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // modifyAccountTypesTSB
            // 
            this.modifyAccountTypesTSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.modifyAccountTypesTSB.Image = ((System.Drawing.Image)(resources.GetObject("modifyAccountTypesTSB.Image")));
            this.modifyAccountTypesTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.modifyAccountTypesTSB.Name = "modifyAccountTypesTSB";
            this.modifyAccountTypesTSB.Size = new System.Drawing.Size(131, 22);
            this.modifyAccountTypesTSB.Text = "Modify Account Types";
            this.modifyAccountTypesTSB.Click += new System.EventHandler(this.modifyAccountTypesTSB_Click);
            // 
            // accountTableAdapter
            // 
            this.accountTableAdapter.ClearBeforeFill = true;
            // 
            // accountTypeTableAdapter
            // 
            this.accountTypeTableAdapter.ClearBeforeFill = true;
            // 
            // accountCatagoryTableAdapter
            // 
            this.accountCatagoryTableAdapter.ClearBeforeFill = true;
            // 
            // EditAccountsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 376);
            this.Controls.Add(this.splitContainer);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditAccountsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Accounts";
            this.Load += new System.EventHandler(this.EditAccountsForm_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accountBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fFDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountCatagoryBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountBindingNavigator)).EndInit();
            this.accountBindingNavigator.ResumeLayout(false);
            this.accountBindingNavigator.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView accountTreeView;
        private FFDBDataSet fFDBDataSet;
        private System.Windows.Forms.BindingSource accountBindingSource;
        private FamilyFinance2.FFDBDataSetTableAdapters.AccountTableAdapter accountTableAdapter;
        private System.Windows.Forms.BindingNavigator accountBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton accountBindingNavigatorSaveItem;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.ComboBox accountTypeIDComboBox;
        private System.Windows.Forms.ComboBox catagoryComboBox;
        private System.Windows.Forms.CheckBox closedCheckBox;
        private System.Windows.Forms.CheckBox envelopesCheckBox;
        private FamilyFinance2.FFDBDataSetTableAdapters.AccountTypeTableAdapter accountTypeTableAdapter;
        private System.Windows.Forms.BindingSource accountTypeBindingSource;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton modifyAccountTypesTSB;
        private System.Windows.Forms.BindingSource accountCatagoryBindingSource;
        private FamilyFinance2.FFDBDataSetTableAdapters.AccountCatagoryTableAdapter accountCatagoryTableAdapter;
        private System.Windows.Forms.CheckBox checkBox1;

    }
}