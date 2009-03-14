﻿namespace FamilyFinance2.Forms
{
    partial class EditEnvelopesForm
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
            System.Windows.Forms.Label parentEnvelopeLabel;
            System.Windows.Forms.Label closedLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditEnvelopesForm));
            this.envelopeTreeView = new System.Windows.Forms.TreeView();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.envelopeBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.envelopeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.fFDBDataSet = new FamilyFinance2.FFDBDataSet();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.envelopeBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.parentEnvelopeComboBox = new System.Windows.Forms.ComboBox();
            this.parentEnvelopeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.closedCheckBox = new System.Windows.Forms.CheckBox();
            this.envelopeTableAdapter = new FamilyFinance2.FFDBDataSetTableAdapters.EnvelopeTableAdapter();
            nameLabel = new System.Windows.Forms.Label();
            parentEnvelopeLabel = new System.Windows.Forms.Label();
            closedLabel = new System.Windows.Forms.Label();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.envelopeBindingNavigator)).BeginInit();
            this.envelopeBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.envelopeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fFDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parentEnvelopeBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(31, 31);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(86, 13);
            nameLabel.TabIndex = 0;
            nameLabel.Text = "Envelope Name:";
            // 
            // parentEnvelopeLabel
            // 
            parentEnvelopeLabel.AutoSize = true;
            parentEnvelopeLabel.Location = new System.Drawing.Point(28, 57);
            parentEnvelopeLabel.Name = "parentEnvelopeLabel";
            parentEnvelopeLabel.Size = new System.Drawing.Size(89, 13);
            parentEnvelopeLabel.TabIndex = 2;
            parentEnvelopeLabel.Text = "Parent Envelope:";
            // 
            // closedLabel
            // 
            closedLabel.AutoSize = true;
            closedLabel.Location = new System.Drawing.Point(75, 86);
            closedLabel.Name = "closedLabel";
            closedLabel.Size = new System.Drawing.Size(42, 13);
            closedLabel.TabIndex = 4;
            closedLabel.Text = "Closed:";
            // 
            // envelopeTreeView
            // 
            this.envelopeTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.envelopeTreeView.FullRowSelect = true;
            this.envelopeTreeView.HideSelection = false;
            this.envelopeTreeView.Location = new System.Drawing.Point(0, 0);
            this.envelopeTreeView.Name = "envelopeTreeView";
            this.envelopeTreeView.Size = new System.Drawing.Size(267, 369);
            this.envelopeTreeView.TabIndex = 0;
            this.envelopeTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.envelopeTreeView_AfterSelect);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.envelopeTreeView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.envelopeBindingNavigator);
            this.splitContainer.Panel2.Controls.Add(nameLabel);
            this.splitContainer.Panel2.Controls.Add(this.nameTextBox);
            this.splitContainer.Panel2.Controls.Add(parentEnvelopeLabel);
            this.splitContainer.Panel2.Controls.Add(this.parentEnvelopeComboBox);
            this.splitContainer.Panel2.Controls.Add(closedLabel);
            this.splitContainer.Panel2.Controls.Add(this.closedCheckBox);
            this.splitContainer.Size = new System.Drawing.Size(615, 369);
            this.splitContainer.SplitterDistance = 267;
            this.splitContainer.TabIndex = 1;
            // 
            // envelopeBindingNavigator
            // 
            this.envelopeBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.envelopeBindingNavigator.BindingSource = this.envelopeBindingSource;
            this.envelopeBindingNavigator.CountItem = null;
            this.envelopeBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.envelopeBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.envelopeBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.envelopeBindingNavigatorSaveItem});
            this.envelopeBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.envelopeBindingNavigator.MoveFirstItem = null;
            this.envelopeBindingNavigator.MoveLastItem = null;
            this.envelopeBindingNavigator.MoveNextItem = null;
            this.envelopeBindingNavigator.MovePreviousItem = null;
            this.envelopeBindingNavigator.Name = "envelopeBindingNavigator";
            this.envelopeBindingNavigator.PositionItem = null;
            this.envelopeBindingNavigator.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.envelopeBindingNavigator.Size = new System.Drawing.Size(344, 25);
            this.envelopeBindingNavigator.TabIndex = 2;
            this.envelopeBindingNavigator.Text = "bindingNavigator1";
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
            // envelopeBindingSource
            // 
            this.envelopeBindingSource.DataMember = "Envelope";
            this.envelopeBindingSource.DataSource = this.fFDBDataSet;
            this.envelopeBindingSource.Filter = "";
            // 
            // fFDBDataSet
            // 
            this.fFDBDataSet.DataSetName = "FFDBDataSet";
            this.fFDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            this.bindingNavigatorDeleteItem.ToolTipText = "Delete This Envelope";
            this.bindingNavigatorDeleteItem.Visible = false;
            // 
            // envelopeBindingNavigatorSaveItem
            // 
            this.envelopeBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.envelopeBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("envelopeBindingNavigatorSaveItem.Image")));
            this.envelopeBindingNavigatorSaveItem.Name = "envelopeBindingNavigatorSaveItem";
            this.envelopeBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.envelopeBindingNavigatorSaveItem.Text = "Save Data";
            this.envelopeBindingNavigatorSaveItem.Click += new System.EventHandler(this.envelopeBindingNavigatorSaveItem_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.envelopeBindingSource, "name", true));
            this.nameTextBox.Location = new System.Drawing.Point(123, 28);
            this.nameTextBox.MaxLength = 30;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(209, 20);
            this.nameTextBox.TabIndex = 1;
            // 
            // parentEnvelopeComboBox
            // 
            this.parentEnvelopeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.parentEnvelopeComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.envelopeBindingSource, "parentEnvelope", true));
            this.parentEnvelopeComboBox.DataSource = this.parentEnvelopeBindingSource;
            this.parentEnvelopeComboBox.DisplayMember = "fullName";
            this.parentEnvelopeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parentEnvelopeComboBox.FormattingEnabled = true;
            this.parentEnvelopeComboBox.Location = new System.Drawing.Point(123, 54);
            this.parentEnvelopeComboBox.Name = "parentEnvelopeComboBox";
            this.parentEnvelopeComboBox.Size = new System.Drawing.Size(209, 21);
            this.parentEnvelopeComboBox.TabIndex = 3;
            this.parentEnvelopeComboBox.ValueMember = "id";
            // 
            // parentEnvelopeBindingSource
            // 
            this.parentEnvelopeBindingSource.DataMember = "Envelope";
            this.parentEnvelopeBindingSource.DataSource = this.fFDBDataSet;
            this.parentEnvelopeBindingSource.Filter = "";
            this.parentEnvelopeBindingSource.Sort = "fullName";
            // 
            // closedCheckBox
            // 
            this.closedCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.envelopeBindingSource, "closed", true));
            this.closedCheckBox.Location = new System.Drawing.Point(123, 81);
            this.closedCheckBox.Name = "closedCheckBox";
            this.closedCheckBox.Size = new System.Drawing.Size(121, 24);
            this.closedCheckBox.TabIndex = 5;
            this.closedCheckBox.UseVisualStyleBackColor = true;
            // 
            // envelopeTableAdapter
            // 
            this.envelopeTableAdapter.ClearBeforeFill = true;
            // 
            // EditEnvelopesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 369);
            this.Controls.Add(this.splitContainer);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditEnvelopesForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Envelopes";
            this.Load += new System.EventHandler(this.EditEnvelopesForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditEnvelopesForm_FormClosing);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.envelopeBindingNavigator)).EndInit();
            this.envelopeBindingNavigator.ResumeLayout(false);
            this.envelopeBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.envelopeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fFDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parentEnvelopeBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView envelopeTreeView;
        private System.Windows.Forms.SplitContainer splitContainer;
        private FFDBDataSet fFDBDataSet;
        private System.Windows.Forms.BindingSource envelopeBindingSource;
        private FamilyFinance2.FFDBDataSetTableAdapters.EnvelopeTableAdapter envelopeTableAdapter;
        private System.Windows.Forms.BindingNavigator envelopeBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton envelopeBindingNavigatorSaveItem;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.ComboBox parentEnvelopeComboBox;
        private System.Windows.Forms.CheckBox closedCheckBox;
        private System.Windows.Forms.BindingSource parentEnvelopeBindingSource;
    }
}