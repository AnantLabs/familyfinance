﻿namespace FamilyFinance2.Forms.Main
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
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.editTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.accountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envelopesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editTSMI});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mainMenuStrip.Size = new System.Drawing.Size(739, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "mainMenuStrip";
            // 
            // editTSMI
            // 
            this.editTSMI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editTSMI.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountsToolStripMenuItem,
            this.envelopesToolStripMenuItem,
            this.transactionTypesToolStripMenuItem});
            this.editTSMI.Name = "editTSMI";
            this.editTSMI.Size = new System.Drawing.Size(39, 20);
            this.editTSMI.Text = "Edit";
            this.editTSMI.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            // 
            // accountsToolStripMenuItem
            // 
            this.accountsToolStripMenuItem.Name = "accountsToolStripMenuItem";
            this.accountsToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.accountsToolStripMenuItem.Text = "Accounts";
            this.accountsToolStripMenuItem.Click += new System.EventHandler(this.accountsToolStripMenuItem_Click);
            // 
            // envelopesToolStripMenuItem
            // 
            this.envelopesToolStripMenuItem.Name = "envelopesToolStripMenuItem";
            this.envelopesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.envelopesToolStripMenuItem.Text = "Envelopes";
            this.envelopesToolStripMenuItem.Click += new System.EventHandler(this.envelopesToolStripMenuItem_Click);
            // 
            // transactionTypesToolStripMenuItem
            // 
            this.transactionTypesToolStripMenuItem.Name = "transactionTypesToolStripMenuItem";
            this.transactionTypesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.transactionTypesToolStripMenuItem.Text = "Transaction Types";
            this.transactionTypesToolStripMenuItem.Click += new System.EventHandler(this.transactionTypesToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 526);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.Text = "Family Finance";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editTSMI;
        private System.Windows.Forms.ToolStripMenuItem accountsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem envelopesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transactionTypesToolStripMenuItem;
    }
}
