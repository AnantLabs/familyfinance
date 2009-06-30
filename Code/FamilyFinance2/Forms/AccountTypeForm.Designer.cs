namespace FamilyFinance2.Forms
{
    partial class AccountTypeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountTypeForm));
            this.button1 = new System.Windows.Forms.Button();
            this.accountTypeBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.accountTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.fFDBDataSet = new FamilyFinance2.FFDBDataSet();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.accountTypeBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.accountTypeDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.accountTypeBindingNavigator)).BeginInit();
            this.accountTypeBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accountTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fFDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountTypeDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // accountTypeBindingNavigator
            // 
            this.accountTypeBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.accountTypeBindingNavigator.BindingSource = this.accountTypeBindingSource;
            this.accountTypeBindingNavigator.CountItem = null;
            this.accountTypeBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.accountTypeBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.accountTypeBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.accountTypeBindingNavigatorSaveItem});
            resources.ApplyResources(this.accountTypeBindingNavigator, "accountTypeBindingNavigator");
            this.accountTypeBindingNavigator.MoveFirstItem = null;
            this.accountTypeBindingNavigator.MoveLastItem = null;
            this.accountTypeBindingNavigator.MoveNextItem = null;
            this.accountTypeBindingNavigator.MovePreviousItem = null;
            this.accountTypeBindingNavigator.Name = "accountTypeBindingNavigator";
            this.accountTypeBindingNavigator.PositionItem = null;
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorAddNewItem, "bindingNavigatorAddNewItem");
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            // 
            // accountTypeBindingSource
            // 
            this.accountTypeBindingSource.DataMember = "AccountType";
            this.accountTypeBindingSource.DataSource = this.fFDBDataSet;
            this.accountTypeBindingSource.Filter = "id > 0";
            this.accountTypeBindingSource.Sort = "name";
            // 
            // fFDBDataSet
            // 
            this.fFDBDataSet.DataSetName = "FFDBDataSet";
            this.fFDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorDeleteItem, "bindingNavigatorDeleteItem");
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            // 
            // accountTypeBindingNavigatorSaveItem
            // 
            this.accountTypeBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.accountTypeBindingNavigatorSaveItem, "accountTypeBindingNavigatorSaveItem");
            this.accountTypeBindingNavigatorSaveItem.Name = "accountTypeBindingNavigatorSaveItem";
            this.accountTypeBindingNavigatorSaveItem.Click += new System.EventHandler(this.accountTypeBindingNavigatorSaveItem_Click);
            // 
            // accountTypeDataGridView
            // 
            this.accountTypeDataGridView.AllowUserToAddRows = false;
            this.accountTypeDataGridView.AllowUserToDeleteRows = false;
            this.accountTypeDataGridView.AllowUserToResizeColumns = false;
            this.accountTypeDataGridView.AllowUserToResizeRows = false;
            this.accountTypeDataGridView.AutoGenerateColumns = false;
            this.accountTypeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.accountTypeDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            this.accountTypeDataGridView.DataSource = this.accountTypeBindingSource;
            resources.ApplyResources(this.accountTypeDataGridView, "accountTypeDataGridView");
            this.accountTypeDataGridView.MultiSelect = false;
            this.accountTypeDataGridView.Name = "accountTypeDataGridView";
            this.accountTypeDataGridView.RowHeadersVisible = false;
            this.accountTypeDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.accountTypeDataGridView.ShowCellErrors = false;
            this.accountTypeDataGridView.ShowCellToolTips = false;
            this.accountTypeDataGridView.ShowEditingIcon = false;
            this.accountTypeDataGridView.ShowRowErrors = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "name";
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.MaxInputLength = 30;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "name";
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.MaxInputLength = 30;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AccountTypeForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accountTypeDataGridView);
            this.Controls.Add(this.accountTypeBindingNavigator);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AccountTypeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.TypeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.accountTypeBindingNavigator)).EndInit();
            this.accountTypeBindingNavigator.ResumeLayout(false);
            this.accountTypeBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accountTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fFDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountTypeDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private FFDBDataSet fFDBDataSet;
        private System.Windows.Forms.BindingSource accountTypeBindingSource;
        private System.Windows.Forms.BindingNavigator accountTypeBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton accountTypeBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView accountTypeDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

    }
}