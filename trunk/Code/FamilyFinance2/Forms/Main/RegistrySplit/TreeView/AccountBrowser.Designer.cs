namespace FamilyFinance2.Forms.Main.RegistrySplit.TreeView
{
    partial class AccountBrowser
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
            this.theTreeView = new Aga.Controls.Tree.TreeViewAdv();
            this.nameColumn = new Aga.Controls.Tree.TreeColumn();
            this.balanceColumn = new Aga.Controls.Tree.TreeColumn();
            this.iconNodeCon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.nameNodeCon = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.balanceNodeCon = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.SuspendLayout();
            // 
            // theTreeView
            // 
            this.theTreeView.AllowColumnReorder = true;
            this.theTreeView.AutoRowHeight = true;
            this.theTreeView.BackColor = System.Drawing.SystemColors.Window;
            this.theTreeView.Columns.Add(this.nameColumn);
            this.theTreeView.Columns.Add(this.balanceColumn);
            this.theTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            this.theTreeView.DefaultToolTipProvider = null;
            this.theTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.theTreeView.DragDropMarkColor = System.Drawing.Color.Black;
            this.theTreeView.FullRowSelect = true;
            this.theTreeView.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.theTreeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.theTreeView.LoadOnDemand = true;
            this.theTreeView.Location = new System.Drawing.Point(0, 0);
            this.theTreeView.Model = null;
            this.theTreeView.Name = "theTreeView";
            this.theTreeView.NodeControls.Add(this.iconNodeCon);
            this.theTreeView.NodeControls.Add(this.nameNodeCon);
            this.theTreeView.NodeControls.Add(this.balanceNodeCon);
            this.theTreeView.SelectedNode = null;
            this.theTreeView.ShowNodeToolTips = true;
            this.theTreeView.Size = new System.Drawing.Size(533, 327);
            this.theTreeView.TabIndex = 0;
            this.theTreeView.UseColumns = true;
            // 
            // nameColumn
            // 
            this.nameColumn.Header = "Account";
            this.nameColumn.SortOrder = System.Windows.Forms.SortOrder.None;
            this.nameColumn.TooltipText = "Account name";
            this.nameColumn.Width = 150;
            // 
            // balanceColumn
            // 
            this.balanceColumn.Header = "Balance";
            this.balanceColumn.SortOrder = System.Windows.Forms.SortOrder.None;
            this.balanceColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.balanceColumn.TooltipText = "Account Balance";
            this.balanceColumn.Width = 100;
            // 
            // iconNodeCon
            // 
            this.iconNodeCon.DataPropertyName = "Icon";
            this.iconNodeCon.LeftMargin = 1;
            this.iconNodeCon.ParentColumn = this.nameColumn;
            this.iconNodeCon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // nameNodeCon
            // 
            this.nameNodeCon.DataPropertyName = "Name";
            this.nameNodeCon.IncrementalSearchEnabled = true;
            this.nameNodeCon.LeftMargin = 3;
            this.nameNodeCon.ParentColumn = this.nameColumn;
            this.nameNodeCon.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.nameNodeCon.UseCompatibleTextRendering = true;
            // 
            // balanceNodeCon
            // 
            this.balanceNodeCon.DataPropertyName = "Balance";
            this.balanceNodeCon.IncrementalSearchEnabled = true;
            this.balanceNodeCon.LeftMargin = 3;
            this.balanceNodeCon.ParentColumn = this.balanceColumn;
            this.balanceNodeCon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // AccountBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.theTreeView);
            this.Name = "AccountBrowser";
            this.Size = new System.Drawing.Size(533, 327);
            this.ResumeLayout(false);

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv theTreeView;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon iconNodeCon;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nameNodeCon;
        private Aga.Controls.Tree.NodeControls.NodeTextBox balanceNodeCon;
        private Aga.Controls.Tree.TreeColumn nameColumn;
        private Aga.Controls.Tree.TreeColumn balanceColumn;
    }
}
