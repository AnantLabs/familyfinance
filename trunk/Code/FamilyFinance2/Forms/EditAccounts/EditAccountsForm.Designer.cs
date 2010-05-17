namespace FamilyFinance2.Forms.EditAccounts
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
            System.Windows.Forms.Label catagoryIDLabel;
            System.Windows.Forms.Label closedLabel;
            System.Windows.Forms.Label creditDebitLabel;
            System.Windows.Forms.Label envelopesLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditAccountsForm));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.accountTreeView = new System.Windows.Forms.TreeView();
            this.accountNormalComboBox = new System.Windows.Forms.ComboBox();
            this.accountBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.eADataSet = new FamilyFinance2.Forms.EditAccounts.EADataSet();
            this.accountBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.accountBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.editAccountTypeTSB = new System.Windows.Forms.ToolStripButton();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.accountTypeIDComboBox = new System.Windows.Forms.ComboBox();
            this.accountTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.catagoryIDComboBox = new System.Windows.Forms.ComboBox();
            this.closedCheckBox = new System.Windows.Forms.CheckBox();
            this.envelopesCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            nameLabel = new System.Windows.Forms.Label();
            accountTypeIDLabel = new System.Windows.Forms.Label();
            catagoryIDLabel = new System.Windows.Forms.Label();
            closedLabel = new System.Windows.Forms.Label();
            creditDebitLabel = new System.Windows.Forms.Label();
            envelopesLabel = new System.Windows.Forms.Label();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accountBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eADataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountBindingNavigator)).BeginInit();
            this.accountBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accountTypeBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(52, 36);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(38, 13);
            nameLabel.TabIndex = 0;
            nameLabel.Text = "Name:";
            // 
            // accountTypeIDLabel
            // 
            accountTypeIDLabel.AutoSize = true;
            accountTypeIDLabel.Location = new System.Drawing.Point(56, 64);
            accountTypeIDLabel.Name = "accountTypeIDLabel";
            accountTypeIDLabel.Size = new System.Drawing.Size(34, 13);
            accountTypeIDLabel.TabIndex = 2;
            accountTypeIDLabel.Text = "Type:";
            // 
            // catagoryIDLabel
            // 
            catagoryIDLabel.AutoSize = true;
            catagoryIDLabel.Location = new System.Drawing.Point(38, 92);
            catagoryIDLabel.Name = "catagoryIDLabel";
            catagoryIDLabel.Size = new System.Drawing.Size(52, 13);
            catagoryIDLabel.TabIndex = 4;
            catagoryIDLabel.Text = "Catagory:";
            // 
            // closedLabel
            // 
            closedLabel.AutoSize = true;
            closedLabel.Location = new System.Drawing.Point(51, 178);
            closedLabel.Name = "closedLabel";
            closedLabel.Size = new System.Drawing.Size(42, 13);
            closedLabel.TabIndex = 6;
            closedLabel.Text = "Closed:";
            // 
            // creditDebitLabel
            // 
            creditDebitLabel.AutoSize = true;
            creditDebitLabel.Location = new System.Drawing.Point(4, 120);
            creditDebitLabel.Name = "creditDebitLabel";
            creditDebitLabel.Size = new System.Drawing.Size(86, 13);
            creditDebitLabel.TabIndex = 8;
            creditDebitLabel.Text = "Account Normal:";
            // 
            // envelopesLabel
            // 
            envelopesLabel.AutoSize = true;
            envelopesLabel.Location = new System.Drawing.Point(30, 148);
            envelopesLabel.Name = "envelopesLabel";
            envelopesLabel.Size = new System.Drawing.Size(60, 13);
            envelopesLabel.TabIndex = 10;
            envelopesLabel.Text = "Envelopes:";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.IsSplitterFixed = true;
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
            this.splitContainer.Panel2.Controls.Add(this.accountNormalComboBox);
            this.splitContainer.Panel2.Controls.Add(this.accountBindingNavigator);
            this.splitContainer.Panel2.Controls.Add(nameLabel);
            this.splitContainer.Panel2.Controls.Add(this.nameTextBox);
            this.splitContainer.Panel2.Controls.Add(accountTypeIDLabel);
            this.splitContainer.Panel2.Controls.Add(this.accountTypeIDComboBox);
            this.splitContainer.Panel2.Controls.Add(catagoryIDLabel);
            this.splitContainer.Panel2.Controls.Add(this.catagoryIDComboBox);
            this.splitContainer.Panel2.Controls.Add(closedLabel);
            this.splitContainer.Panel2.Controls.Add(this.closedCheckBox);
            this.splitContainer.Panel2.Controls.Add(creditDebitLabel);
            this.splitContainer.Panel2.Controls.Add(envelopesLabel);
            this.splitContainer.Panel2.Controls.Add(this.envelopesCheckBox);
            this.splitContainer.Size = new System.Drawing.Size(490, 296);
            this.splitContainer.SplitterDistance = 257;
            this.splitContainer.TabIndex = 0;
            // 
            // accountTreeView
            // 
            this.accountTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountTreeView.FullRowSelect = true;
            this.accountTreeView.HideSelection = false;
            this.accountTreeView.Indent = 10;
            this.accountTreeView.Location = new System.Drawing.Point(0, 0);
            this.accountTreeView.Name = "accountTreeView";
            this.accountTreeView.Size = new System.Drawing.Size(257, 296);
            this.accountTreeView.TabIndex = 0;
            this.accountTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.accountTreeView_AfterSelect);
            // 
            // accountNormalComboBox
            // 
            this.accountNormalComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.accountBindingSource, "creditDebit", true));
            this.accountNormalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.accountNormalComboBox.FormattingEnabled = true;
            this.accountNormalComboBox.Location = new System.Drawing.Point(96, 116);
            this.accountNormalComboBox.Name = "accountNormalComboBox";
            this.accountNormalComboBox.Size = new System.Drawing.Size(121, 21);
            this.accountNormalComboBox.TabIndex = 12;
            this.toolTip1.SetToolTip(this.accountNormalComboBox, "Use Debit for account like checking and savings.\r\nUse Credit for accounts like cr" +
                    "edit cards and loans .");
            // 
            // accountBindingSource
            // 
            this.accountBindingSource.DataMember = "Account";
            this.accountBindingSource.DataSource = this.eADataSet;
            // 
            // eADataSet
            // 
            this.eADataSet.DataSetName = "EADataSet";
            this.eADataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // accountBindingNavigator
            // 
            this.accountBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.accountBindingNavigator.BindingSource = this.accountBindingSource;
            this.accountBindingNavigator.CountItem = null;
            this.accountBindingNavigator.DeleteItem = null;
            this.accountBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.accountBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.accountBindingNavigatorSaveItem,
            this.editAccountTypeTSB});
            this.accountBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.accountBindingNavigator.MoveFirstItem = null;
            this.accountBindingNavigator.MoveLastItem = null;
            this.accountBindingNavigator.MoveNextItem = null;
            this.accountBindingNavigator.MovePreviousItem = null;
            this.accountBindingNavigator.Name = "accountBindingNavigator";
            this.accountBindingNavigator.PositionItem = null;
            this.accountBindingNavigator.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.accountBindingNavigator.Size = new System.Drawing.Size(229, 25);
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
            this.bindingNavigatorAddNewItem.Click += new System.EventHandler(this.bindingNavigatorAddNewItem_Click);
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
            // editAccountTypeTSB
            // 
            this.editAccountTypeTSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editAccountTypeTSB.Image = ((System.Drawing.Image)(resources.GetObject("editAccountTypeTSB.Image")));
            this.editAccountTypeTSB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editAccountTypeTSB.Name = "editAccountTypeTSB";
            this.editAccountTypeTSB.Size = new System.Drawing.Size(113, 22);
            this.editAccountTypeTSB.Text = "Edit Account Types";
            this.editAccountTypeTSB.Click += new System.EventHandler(this.modifyAccountTypesTSB_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.accountBindingSource, "name", true));
            this.nameTextBox.Location = new System.Drawing.Point(96, 33);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(121, 20);
            this.nameTextBox.TabIndex = 1;
            this.toolTip1.SetToolTip(this.nameTextBox, "The name of the account");
            // 
            // accountTypeIDComboBox
            // 
            this.accountTypeIDComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.accountBindingSource, "typeID", true));
            this.accountTypeIDComboBox.DataSource = this.accountTypeBindingSource;
            this.accountTypeIDComboBox.DisplayMember = "name";
            this.accountTypeIDComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.accountTypeIDComboBox.FormattingEnabled = true;
            this.accountTypeIDComboBox.Location = new System.Drawing.Point(96, 61);
            this.accountTypeIDComboBox.Name = "accountTypeIDComboBox";
            this.accountTypeIDComboBox.Size = new System.Drawing.Size(121, 21);
            this.accountTypeIDComboBox.TabIndex = 3;
            this.toolTip1.SetToolTip(this.accountTypeIDComboBox, "The accounts type");
            this.accountTypeIDComboBox.ValueMember = "id";
            // 
            // accountTypeBindingSource
            // 
            this.accountTypeBindingSource.DataMember = "AccountType";
            this.accountTypeBindingSource.DataSource = this.eADataSet;
            // 
            // catagoryIDComboBox
            // 
            this.catagoryIDComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.accountBindingSource, "catagory", true));
            this.catagoryIDComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.catagoryIDComboBox.FormattingEnabled = true;
            this.catagoryIDComboBox.Location = new System.Drawing.Point(96, 89);
            this.catagoryIDComboBox.Name = "catagoryIDComboBox";
            this.catagoryIDComboBox.Size = new System.Drawing.Size(121, 21);
            this.catagoryIDComboBox.TabIndex = 5;
            this.toolTip1.SetToolTip(this.catagoryIDComboBox, "The accounts catagory");
            // 
            // closedCheckBox
            // 
            this.closedCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.accountBindingSource, "closed", true));
            this.closedCheckBox.Location = new System.Drawing.Point(96, 173);
            this.closedCheckBox.Name = "closedCheckBox";
            this.closedCheckBox.Size = new System.Drawing.Size(121, 24);
            this.closedCheckBox.TabIndex = 7;
            this.toolTip1.SetToolTip(this.closedCheckBox, "Check this if the account is closed");
            this.closedCheckBox.UseVisualStyleBackColor = true;
            this.closedCheckBox.Click += new System.EventHandler(this.closedCheckBox_Click);
            // 
            // envelopesCheckBox
            // 
            this.envelopesCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.accountBindingSource, "envelopes", true));
            this.envelopesCheckBox.Location = new System.Drawing.Point(96, 143);
            this.envelopesCheckBox.Name = "envelopesCheckBox";
            this.envelopesCheckBox.Size = new System.Drawing.Size(121, 24);
            this.envelopesCheckBox.TabIndex = 11;
            this.toolTip1.SetToolTip(this.envelopesCheckBox, "Check this if the account will use envelopes");
            this.envelopesCheckBox.UseVisualStyleBackColor = true;
            this.envelopesCheckBox.Click += new System.EventHandler(this.envelopesCheckBox_Click);
            // 
            // EditAccountsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 296);
            this.Controls.Add(this.splitContainer);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(402, 265);
            this.Name = "EditAccountsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Accounts";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditAccountsForm_FormClosing);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accountBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eADataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountBindingNavigator)).EndInit();
            this.accountBindingNavigator.ResumeLayout(false);
            this.accountBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accountTypeBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView accountTreeView;
        private FamilyFinance2.Forms.EditAccounts.EADataSet eADataSet;
        private System.Windows.Forms.BindingSource accountBindingSource;
        private System.Windows.Forms.BindingNavigator accountBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton accountBindingNavigatorSaveItem;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.ComboBox accountTypeIDComboBox;
        private System.Windows.Forms.ComboBox catagoryIDComboBox;
        private System.Windows.Forms.CheckBox closedCheckBox;
        private System.Windows.Forms.CheckBox envelopesCheckBox;
        private System.Windows.Forms.BindingSource accountTypeBindingSource;
        private System.Windows.Forms.ToolStripButton editAccountTypeTSB;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox accountNormalComboBox;

    }
}