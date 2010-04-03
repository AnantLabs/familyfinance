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
<<<<<<< .mine
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.editTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.accountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envelopesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registySplitContainer1 = new FamilyFinance2.Forms.Main.RegistySplitContainer();
            this.mainMenuStrip.SuspendLayout();
=======
>>>>>>> .r107
            this.SuspendLayout();
            // 
            // registySplitContainer1
            // 
            this.registySplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.registySplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registySplitContainer1.Location = new System.Drawing.Point(0, 24);
            this.registySplitContainer1.Name = "registySplitContainer1";
            this.registySplitContainer1.Size = new System.Drawing.Size(739, 502);
            this.registySplitContainer1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 526);
<<<<<<< .mine
            this.Controls.Add(this.registySplitContainer1);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
=======
>>>>>>> .r107
            this.Name = "MainForm";
            this.Text = "Family Finance";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private RegistySplitContainer registySplitContainer1;
    }
}

