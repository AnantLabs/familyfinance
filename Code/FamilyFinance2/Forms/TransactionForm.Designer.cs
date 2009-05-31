namespace FamilyFinance2.Forms
{
    partial class TransactionForm
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
            this.transactionLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.subTransactionDGV1 = new FamilyFinance2.SubTransactionDGV();
            ((System.ComponentModel.ISupportInitialize)(this.subTransactionDGV1)).BeginInit();
            this.SuspendLayout();
            // 
            // transactionLayoutPanel
            // 
            this.transactionLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.transactionLayoutPanel.ColumnCount = 1;
            this.transactionLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.transactionLayoutPanel.Location = new System.Drawing.Point(13, 13);
            this.transactionLayoutPanel.Name = "transactionLayoutPanel";
            this.transactionLayoutPanel.RowCount = 2;
            this.transactionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.transactionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.transactionLayoutPanel.Size = new System.Drawing.Size(769, 291);
            this.transactionLayoutPanel.TabIndex = 0;
            // 
            // subTransactionDGV1
            // 
            this.subTransactionDGV1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.subTransactionDGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.subTransactionDGV1.Location = new System.Drawing.Point(13, 310);
            this.subTransactionDGV1.Name = "subTransactionDGV1";
            this.subTransactionDGV1.Size = new System.Drawing.Size(328, 204);
            this.subTransactionDGV1.TabIndex = 1;
            // 
            // TransactionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 526);
            this.Controls.Add(this.subTransactionDGV1);
            this.Controls.Add(this.transactionLayoutPanel);
            this.Name = "TransactionForm";
            this.Text = "TransactionForm";
            ((System.ComponentModel.ISupportInitialize)(this.subTransactionDGV1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel transactionLayoutPanel;
        private SubTransactionDGV subTransactionDGV1;

    }
}