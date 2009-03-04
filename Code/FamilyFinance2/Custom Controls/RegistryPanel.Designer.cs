namespace FamilyFinance2.Custom_Controls
{
    partial class RegistryPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegistryPanel));
            this.fFDBDataSet = new FamilyFinance2.FFDBDataSet();
            this.lineItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lineItemTableAdapter = new FamilyFinance2.FFDBDataSetTableAdapters.LineItemTableAdapter();
            this.tableAdapterManager = new FamilyFinance2.FFDBDataSetTableAdapters.TableAdapterManager();
            this.lineItemBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.lineItemBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.lineItemDataGridView = new System.Windows.Forms.DataGridView();
            this.lineTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lineTypeTableAdapter = new FamilyFinance2.FFDBDataSetTableAdapters.LineTypeTableAdapter();
            this.accountBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accountTableAdapter = new FamilyFinance2.FFDBDataSetTableAdapters.AccountTableAdapter();
            this.lineIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transactionIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineTypeIDColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.accountIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oppAccountIDColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.fFDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineItemBindingNavigator)).BeginInit();
            this.lineItemBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lineItemDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // fFDBDataSet
            // 
            this.fFDBDataSet.DataSetName = "FFDBDataSet";
            this.fFDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lineItemBindingSource
            // 
            this.lineItemBindingSource.DataMember = "LineItem";
            this.lineItemBindingSource.DataSource = this.fFDBDataSet;
            // 
            // lineItemTableAdapter
            // 
            this.lineItemTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AccountTableAdapter = this.accountTableAdapter;
            this.tableAdapterManager.AccountTypeTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.EnvelopeTableAdapter = null;
            this.tableAdapterManager.LineItemTableAdapter = this.lineItemTableAdapter;
            this.tableAdapterManager.LineTypeTableAdapter = this.lineTypeTableAdapter;
            this.tableAdapterManager.SubLineItemTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = FamilyFinance2.FFDBDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // lineItemBindingNavigator
            // 
            this.lineItemBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.lineItemBindingNavigator.BindingSource = this.lineItemBindingSource;
            this.lineItemBindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.lineItemBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.lineItemBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.lineItemBindingNavigatorSaveItem});
            this.lineItemBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.lineItemBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.lineItemBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.lineItemBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.lineItemBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.lineItemBindingNavigator.Name = "lineItemBindingNavigator";
            this.lineItemBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.lineItemBindingNavigator.Size = new System.Drawing.Size(487, 25);
            this.lineItemBindingNavigator.TabIndex = 0;
            this.lineItemBindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 15);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 6);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 6);
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
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // lineItemBindingNavigatorSaveItem
            // 
            this.lineItemBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lineItemBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("lineItemBindingNavigatorSaveItem.Image")));
            this.lineItemBindingNavigatorSaveItem.Name = "lineItemBindingNavigatorSaveItem";
            this.lineItemBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 23);
            this.lineItemBindingNavigatorSaveItem.Text = "Save Data";
            this.lineItemBindingNavigatorSaveItem.Click += new System.EventHandler(this.lineItemBindingNavigatorSaveItem_Click);
            // 
            // lineItemDataGridView
            // 
            this.lineItemDataGridView.AutoGenerateColumns = false;
            this.lineItemDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lineItemDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.lineIDColumn,
            this.transactionIDColumn,
            this.dateColumn,
            this.lineTypeIDColumn,
            this.accountIDColumn,
            this.oppAccountIDColumn,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewCheckBoxColumn2,
            this.dataGridViewCheckBoxColumn3});
            this.lineItemDataGridView.DataSource = this.lineItemBindingSource;
            this.lineItemDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineItemDataGridView.Location = new System.Drawing.Point(0, 25);
            this.lineItemDataGridView.Name = "lineItemDataGridView";
            this.lineItemDataGridView.Size = new System.Drawing.Size(487, 354);
            this.lineItemDataGridView.TabIndex = 1;
            // 
            // lineTypeBindingSource
            // 
            this.lineTypeBindingSource.DataMember = "LineType";
            this.lineTypeBindingSource.DataSource = this.fFDBDataSet;
            // 
            // lineTypeTableAdapter
            // 
            this.lineTypeTableAdapter.ClearBeforeFill = true;
            // 
            // accountBindingSource
            // 
            this.accountBindingSource.DataMember = "Account";
            this.accountBindingSource.DataSource = this.fFDBDataSet;
            // 
            // accountTableAdapter
            // 
            this.accountTableAdapter.ClearBeforeFill = true;
            // 
            // lineIDColumn
            // 
            this.lineIDColumn.DataPropertyName = "id";
            this.lineIDColumn.HeaderText = "id";
            this.lineIDColumn.Name = "lineIDColumn";
            // 
            // transactionIDColumn
            // 
            this.transactionIDColumn.DataPropertyName = "transactionID";
            this.transactionIDColumn.HeaderText = "transactionID";
            this.transactionIDColumn.Name = "transactionIDColumn";
            // 
            // dateColumn
            // 
            this.dateColumn.DataPropertyName = "date";
            this.dateColumn.HeaderText = "date";
            this.dateColumn.Name = "dateColumn";
            // 
            // lineTypeIDColumn
            // 
            this.lineTypeIDColumn.DataPropertyName = "lineTypeID";
            this.lineTypeIDColumn.DataSource = this.lineTypeBindingSource;
            this.lineTypeIDColumn.DisplayMember = "name";
            this.lineTypeIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.lineTypeIDColumn.HeaderText = "Type";
            this.lineTypeIDColumn.Name = "lineTypeIDColumn";
            this.lineTypeIDColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.lineTypeIDColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.lineTypeIDColumn.ToolTipText = "Transaction Type";
            this.lineTypeIDColumn.ValueMember = "id";
            // 
            // accountIDColumn
            // 
            this.accountIDColumn.DataPropertyName = "accountID";
            this.accountIDColumn.HeaderText = "accountID";
            this.accountIDColumn.Name = "accountIDColumn";
            // 
            // oppAccountIDColumn
            // 
            this.oppAccountIDColumn.DataPropertyName = "oppAccountID";
            this.oppAccountIDColumn.DataSource = this.fFDBDataSet;
            this.oppAccountIDColumn.DisplayMember = "Account.name";
            this.oppAccountIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.oppAccountIDColumn.HeaderText = "Source / Destination";
            this.oppAccountIDColumn.Name = "oppAccountIDColumn";
            this.oppAccountIDColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.oppAccountIDColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.oppAccountIDColumn.ToolTipText = "Source or Destination";
            this.oppAccountIDColumn.ValueMember = "Account.id";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "description";
            this.dataGridViewTextBoxColumn7.HeaderText = "description";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "confirmationNumber";
            this.dataGridViewTextBoxColumn8.HeaderText = "confirmationNumber";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "envelopeID";
            this.dataGridViewTextBoxColumn9.HeaderText = "envelopeID";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "complete";
            this.dataGridViewTextBoxColumn10.HeaderText = "complete";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "amount";
            this.dataGridViewTextBoxColumn11.HeaderText = "amount";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "creditDebit";
            this.dataGridViewCheckBoxColumn1.HeaderText = "creditDebit";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.DataPropertyName = "transactionError";
            this.dataGridViewCheckBoxColumn2.HeaderText = "transactionError";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            // 
            // dataGridViewCheckBoxColumn3
            // 
            this.dataGridViewCheckBoxColumn3.DataPropertyName = "lineError";
            this.dataGridViewCheckBoxColumn3.HeaderText = "lineError";
            this.dataGridViewCheckBoxColumn3.Name = "dataGridViewCheckBoxColumn3";
            // 
            // RegistryPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lineItemDataGridView);
            this.Controls.Add(this.lineItemBindingNavigator);
            this.Name = "RegistryPanel";
            this.Size = new System.Drawing.Size(487, 379);
            ((System.ComponentModel.ISupportInitialize)(this.fFDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineItemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineItemBindingNavigator)).EndInit();
            this.lineItemBindingNavigator.ResumeLayout(false);
            this.lineItemBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lineItemDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FFDBDataSet fFDBDataSet;
        private System.Windows.Forms.BindingSource lineItemBindingSource;
        private FamilyFinance2.FFDBDataSetTableAdapters.LineItemTableAdapter lineItemTableAdapter;
        private FamilyFinance2.FFDBDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator lineItemBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton lineItemBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView lineItemDataGridView;
        private FamilyFinance2.FFDBDataSetTableAdapters.AccountTableAdapter accountTableAdapter;
        private FamilyFinance2.FFDBDataSetTableAdapters.LineTypeTableAdapter lineTypeTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn transactionIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn lineTypeIDColumn;
        private System.Windows.Forms.BindingSource lineTypeBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountIDColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn oppAccountIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
        private System.Windows.Forms.BindingSource accountBindingSource;
    }
}
