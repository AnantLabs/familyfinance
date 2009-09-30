namespace FamilyFinance2.Forms
{
    partial class LineTypeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineTypeForm));
            this.fFDBDataSet = new FamilyFinance2.FFDBDataSet();
            this.lineTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lineTypeBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.lineTypeBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.lineTypeDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.fFDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineTypeBindingNavigator)).BeginInit();
            this.lineTypeBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lineTypeDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // fFDBDataSet
            // 
            this.fFDBDataSet.DataSetName = "FFDBDataSet";
            this.fFDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lineTypeBindingSource
            // 
            this.lineTypeBindingSource.DataMember = "LineType";
            this.lineTypeBindingSource.DataSource = this.fFDBDataSet;
            this.lineTypeBindingSource.Filter = "id > 0";
            this.lineTypeBindingSource.Sort = "name";
            // 
            // lineTypeBindingNavigator
            // 
            this.lineTypeBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.lineTypeBindingNavigator.BindingSource = this.lineTypeBindingSource;
            this.lineTypeBindingNavigator.CountItem = null;
            this.lineTypeBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.lineTypeBindingNavigator.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.lineTypeBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.lineTypeBindingNavigatorSaveItem});
            this.lineTypeBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.lineTypeBindingNavigator.MoveFirstItem = null;
            this.lineTypeBindingNavigator.MoveLastItem = null;
            this.lineTypeBindingNavigator.MoveNextItem = null;
            this.lineTypeBindingNavigator.MovePreviousItem = null;
            this.lineTypeBindingNavigator.Name = "lineTypeBindingNavigator";
            this.lineTypeBindingNavigator.PositionItem = null;
            this.lineTypeBindingNavigator.Size = new System.Drawing.Size(153, 25);
            this.lineTypeBindingNavigator.TabIndex = 0;
            this.lineTypeBindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add a new Transaction Type";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            this.bindingNavigatorDeleteItem.Visible = false;
            // 
            // lineTypeBindingNavigatorSaveItem
            // 
            this.lineTypeBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lineTypeBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("lineTypeBindingNavigatorSaveItem.Image")));
            this.lineTypeBindingNavigatorSaveItem.Name = "lineTypeBindingNavigatorSaveItem";
            this.lineTypeBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.lineTypeBindingNavigatorSaveItem.Text = "Save Changes";
            this.lineTypeBindingNavigatorSaveItem.Click += new System.EventHandler(this.lineTypeBindingNavigatorSaveItem_Click);
            // 
            // lineTypeDataGridView
            // 
            this.lineTypeDataGridView.AllowUserToAddRows = false;
            this.lineTypeDataGridView.AllowUserToDeleteRows = false;
            this.lineTypeDataGridView.AllowUserToResizeColumns = false;
            this.lineTypeDataGridView.AllowUserToResizeRows = false;
            this.lineTypeDataGridView.AutoGenerateColumns = false;
            this.lineTypeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lineTypeDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            this.lineTypeDataGridView.DataSource = this.lineTypeBindingSource;
            this.lineTypeDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineTypeDataGridView.Location = new System.Drawing.Point(0, 25);
            this.lineTypeDataGridView.MultiSelect = false;
            this.lineTypeDataGridView.Name = "lineTypeDataGridView";
            this.lineTypeDataGridView.RowHeadersVisible = false;
            this.lineTypeDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.lineTypeDataGridView.ShowCellErrors = false;
            this.lineTypeDataGridView.ShowCellToolTips = false;
            this.lineTypeDataGridView.ShowEditingIcon = false;
            this.lineTypeDataGridView.ShowRowErrors = false;
            this.lineTypeDataGridView.Size = new System.Drawing.Size(153, 203);
            this.lineTypeDataGridView.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Type";
            this.dataGridViewTextBoxColumn2.MaxInputLength = 10;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LineTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(153, 228);
            this.Controls.Add(this.lineTypeDataGridView);
            this.Controls.Add(this.lineTypeBindingNavigator);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LineTypeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction Type";
            this.Load += new System.EventHandler(this.LineTypeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fFDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lineTypeBindingNavigator)).EndInit();
            this.lineTypeBindingNavigator.ResumeLayout(false);
            this.lineTypeBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lineTypeDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FFDBDataSet fFDBDataSet;
        private System.Windows.Forms.BindingSource lineTypeBindingSource;
        private System.Windows.Forms.BindingNavigator lineTypeBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton lineTypeBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView lineTypeDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}