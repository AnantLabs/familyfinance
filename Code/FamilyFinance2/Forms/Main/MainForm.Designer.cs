namespace FamilyFinance2.Forms.Main
{
    partial class MainForm
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
            System.Windows.Forms.ToolStripDropDownButton editTSDropDown;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.accoutsTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.envelopesTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionTypesTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.importQifTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            editTSDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // editTSDropDown
            // 
            editTSDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            editTSDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accoutsTSMI,
            this.envelopesTSMI,
            this.transactionTypesTSMI,
            this.importQifTSMI});
            editTSDropDown.Image = ((System.Drawing.Image)(resources.GetObject("editTSDropDown.Image")));
            editTSDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            editTSDropDown.Name = "editTSDropDown";
            editTSDropDown.Size = new System.Drawing.Size(40, 22);
            editTSDropDown.Text = "Edit";
            // 
            // accoutsTSMI
            // 
            this.accoutsTSMI.Name = "accoutsTSMI";
            this.accoutsTSMI.Size = new System.Drawing.Size(170, 22);
            this.accoutsTSMI.Text = "Accouts";
            this.accoutsTSMI.Click += new System.EventHandler(this.accountsToolStripMenuItem_Click);
            // 
            // envelopesTSMI
            // 
            this.envelopesTSMI.Name = "envelopesTSMI";
            this.envelopesTSMI.Size = new System.Drawing.Size(170, 22);
            this.envelopesTSMI.Text = "Envelopes";
            this.envelopesTSMI.Click += new System.EventHandler(this.envelopesToolStripMenuItem_Click);
            // 
            // transactionTypesTSMI
            // 
            this.transactionTypesTSMI.Name = "transactionTypesTSMI";
            this.transactionTypesTSMI.Size = new System.Drawing.Size(170, 22);
            this.transactionTypesTSMI.Text = "Transaction Types";
            this.transactionTypesTSMI.Click += new System.EventHandler(this.transactionTypesToolStripMenuItem_Click);
            // 
            // importQifTSMI
            // 
            this.importQifTSMI.Name = "importQifTSMI";
            this.importQifTSMI.Size = new System.Drawing.Size(170, 22);
            this.importQifTSMI.Text = "Import QIF";
            this.importQifTSMI.Click += new System.EventHandler(this.importQIFToolStripMenuItem_Click);
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.BackColor = System.Drawing.SystemColors.Control;
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            editTSDropDown});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mainToolStrip.Size = new System.Drawing.Size(739, 25);
            this.mainToolStrip.TabIndex = 0;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 526);
            this.Controls.Add(this.mainToolStrip);
            this.Name = "MainForm";
            this.Text = "Family Finance";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripMenuItem accoutsTSMI;
        private System.Windows.Forms.ToolStripMenuItem envelopesTSMI;
        private System.Windows.Forms.ToolStripMenuItem transactionTypesTSMI;
        private System.Windows.Forms.ToolStripMenuItem importQifTSMI;

    }
}

