namespace FamilyFinance2.Forms.EditGroup
{
    partial class EditGroupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditGroupForm));
            this.groupDataSet = new FamilyFinance2.Forms.EditGroup.GroupDataSet();
            this.envelopeGroupBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.envelopeGroupBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.envelopeGroupBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.envelopeGroupDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.groupDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.envelopeGroupBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.envelopeGroupBindingNavigator)).BeginInit();
            this.envelopeGroupBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.envelopeGroupDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupDataSet
            // 
            this.groupDataSet.DataSetName = "GroupDataSet";
            this.groupDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // envelopeGroupBindingSource
            // 
            this.envelopeGroupBindingSource.DataMember = "EnvelopeGroup";
            this.envelopeGroupBindingSource.DataSource = this.groupDataSet;
            // 
            // envelopeGroupBindingNavigator
            // 
            this.envelopeGroupBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.envelopeGroupBindingNavigator.BindingSource = this.envelopeGroupBindingSource;
            this.envelopeGroupBindingNavigator.CountItem = null;
            this.envelopeGroupBindingNavigator.DeleteItem = null;
            this.envelopeGroupBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.envelopeGroupBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.envelopeGroupBindingNavigatorSaveItem});
            this.envelopeGroupBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.envelopeGroupBindingNavigator.MoveFirstItem = null;
            this.envelopeGroupBindingNavigator.MoveLastItem = null;
            this.envelopeGroupBindingNavigator.MoveNextItem = null;
            this.envelopeGroupBindingNavigator.MovePreviousItem = null;
            this.envelopeGroupBindingNavigator.Name = "envelopeGroupBindingNavigator";
            this.envelopeGroupBindingNavigator.PositionItem = null;
            this.envelopeGroupBindingNavigator.Size = new System.Drawing.Size(257, 25);
            this.envelopeGroupBindingNavigator.TabIndex = 0;
            this.envelopeGroupBindingNavigator.Text = "bindingNavigator1";
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
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            this.bindingNavigatorDeleteItem.Click += new System.EventHandler(this.bindingNavigatorDeleteItem_Click);
            // 
            // envelopeGroupBindingNavigatorSaveItem
            // 
            this.envelopeGroupBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.envelopeGroupBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("envelopeGroupBindingNavigatorSaveItem.Image")));
            this.envelopeGroupBindingNavigatorSaveItem.Name = "envelopeGroupBindingNavigatorSaveItem";
            this.envelopeGroupBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.envelopeGroupBindingNavigatorSaveItem.Text = "Save Data";
            this.envelopeGroupBindingNavigatorSaveItem.Click += new System.EventHandler(this.envelopeGroupBindingNavigatorSaveItem_Click);
            // 
            // envelopeGroupDataGridView
            // 
            this.envelopeGroupDataGridView.AllowUserToAddRows = false;
            this.envelopeGroupDataGridView.AllowUserToDeleteRows = false;
            this.envelopeGroupDataGridView.AllowUserToResizeColumns = false;
            this.envelopeGroupDataGridView.AllowUserToResizeRows = false;
            this.envelopeGroupDataGridView.AutoGenerateColumns = false;
            this.envelopeGroupDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.envelopeGroupDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            this.envelopeGroupDataGridView.DataSource = this.envelopeGroupBindingSource;
            this.envelopeGroupDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.envelopeGroupDataGridView.Location = new System.Drawing.Point(0, 25);
            this.envelopeGroupDataGridView.MultiSelect = false;
            this.envelopeGroupDataGridView.Name = "envelopeGroupDataGridView";
            this.envelopeGroupDataGridView.RowHeadersVisible = false;
            this.envelopeGroupDataGridView.ShowEditingIcon = false;
            this.envelopeGroupDataGridView.ShowRowErrors = false;
            this.envelopeGroupDataGridView.Size = new System.Drawing.Size(257, 293);
            this.envelopeGroupDataGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Envelope Group";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // EditGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 318);
            this.Controls.Add(this.envelopeGroupDataGridView);
            this.Controls.Add(this.envelopeGroupBindingNavigator);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditGroupForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Envelope Groups";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditGroupForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.groupDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.envelopeGroupBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.envelopeGroupBindingNavigator)).EndInit();
            this.envelopeGroupBindingNavigator.ResumeLayout(false);
            this.envelopeGroupBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.envelopeGroupDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupDataSet groupDataSet;
        private System.Windows.Forms.BindingSource envelopeGroupBindingSource;
        private System.Windows.Forms.BindingNavigator envelopeGroupBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton envelopeGroupBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView envelopeGroupDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}