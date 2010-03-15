
namespace FamilyFinance2.Forms.Transaction
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
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.subLineSumLabel = new System.Windows.Forms.Label();
            this.lineAmountLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.DoneButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.destinationSumLabel = new System.Windows.Forms.Label();
            this.sourceSumLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.transactionLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // transactionLayoutPanel
            // 
            this.transactionLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.transactionLayoutPanel.ColumnCount = 6;
            this.transactionLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57F));
            this.transactionLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.transactionLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.transactionLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.transactionLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.transactionLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.transactionLayoutPanel.Controls.Add(this.label11, 0, 4);
            this.transactionLayoutPanel.Controls.Add(this.label10, 0, 2);
            this.transactionLayoutPanel.Controls.Add(this.subLineSumLabel, 2, 6);
            this.transactionLayoutPanel.Controls.Add(this.lineAmountLabel, 2, 5);
            this.transactionLayoutPanel.Controls.Add(this.label4, 1, 6);
            this.transactionLayoutPanel.Controls.Add(this.label1, 1, 5);
            this.transactionLayoutPanel.Controls.Add(this.label9, 0, 0);
            this.transactionLayoutPanel.Controls.Add(this.DoneButton, 5, 7);
            this.transactionLayoutPanel.Controls.Add(this.cancelButton, 4, 7);
            this.transactionLayoutPanel.Controls.Add(this.label5, 4, 2);
            this.transactionLayoutPanel.Controls.Add(this.label8, 4, 4);
            this.transactionLayoutPanel.Controls.Add(this.sourceSumLabel, 5, 2);
            this.transactionLayoutPanel.Controls.Add(this.destinationSumLabel, 5, 4);
            this.transactionLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.transactionLayoutPanel.Name = "transactionLayoutPanel";
            this.transactionLayoutPanel.RowCount = 8;
            this.transactionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.transactionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.transactionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.transactionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.transactionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.transactionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.transactionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.transactionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.transactionLayoutPanel.Size = new System.Drawing.Size(838, 424);
            this.transactionLayoutPanel.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label11.Location = new System.Drawing.Point(3, 255);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 17);
            this.label11.TabIndex = 11;
            this.label11.Text = "Envelope Lines";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label10.Location = new System.Drawing.Point(3, 127);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(117, 17);
            this.label10.TabIndex = 10;
            this.label10.Text = "Destination Lines";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // subLineSumLabel
            // 
            this.subLineSumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.subLineSumLabel.AutoSize = true;
            this.subLineSumLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.subLineSumLabel.Location = new System.Drawing.Point(469, 295);
            this.subLineSumLabel.Name = "subLineSumLabel";
            this.subLineSumLabel.Size = new System.Drawing.Size(66, 15);
            this.subLineSumLabel.TabIndex = 2;
            this.subLineSumLabel.Text = "$99999.99";
            this.subLineSumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lineAmountLabel
            // 
            this.lineAmountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lineAmountLabel.AutoSize = true;
            this.lineAmountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lineAmountLabel.Location = new System.Drawing.Point(469, 275);
            this.lineAmountLabel.Name = "lineAmountLabel";
            this.lineAmountLabel.Size = new System.Drawing.Size(66, 15);
            this.lineAmountLabel.TabIndex = 5;
            this.lineAmountLabel.Text = "$99999.99";
            this.lineAmountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label4.Location = new System.Drawing.Point(364, 295);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Envelope Sum:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label1.Location = new System.Drawing.Point(375, 275);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Line Amount:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 17);
            this.label9.TabIndex = 9;
            this.label9.Text = "Source Lines";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DoneButton
            // 
            this.DoneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DoneButton.Location = new System.Drawing.Point(760, 398);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(75, 23);
            this.DoneButton.TabIndex = 12;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(676, 398);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label8.Location = new System.Drawing.Point(650, 255);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 15);
            this.label8.TabIndex = 8;
            this.label8.Text = "Destination Sum:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // destinationSumLabel
            // 
            this.destinationSumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.destinationSumLabel.AutoSize = true;
            this.destinationSumLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.destinationSumLabel.Location = new System.Drawing.Point(769, 255);
            this.destinationSumLabel.Name = "destinationSumLabel";
            this.destinationSumLabel.Size = new System.Drawing.Size(66, 15);
            this.destinationSumLabel.TabIndex = 7;
            this.destinationSumLabel.Text = "$99999.99";
            this.destinationSumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sourceSumLabel
            // 
            this.sourceSumLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceSumLabel.AutoSize = true;
            this.sourceSumLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.sourceSumLabel.Location = new System.Drawing.Point(769, 127);
            this.sourceSumLabel.Name = "sourceSumLabel";
            this.sourceSumLabel.Size = new System.Drawing.Size(66, 15);
            this.sourceSumLabel.TabIndex = 6;
            this.sourceSumLabel.Text = "$99999.99";
            this.sourceSumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label5.Location = new System.Drawing.Point(673, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Source Sum:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TransactionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 448);
            this.Controls.Add(this.transactionLayoutPanel);
            this.Name = "TransactionForm";
            this.Text = "TransactionForm";
            this.transactionLayoutPanel.ResumeLayout(false);
            this.transactionLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel transactionLayoutPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label subLineSumLabel;
        private System.Windows.Forms.Label lineAmountLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label sourceSumLabel;
        private System.Windows.Forms.Label destinationSumLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button DoneButton;
        private System.Windows.Forms.Button cancelButton;

    }
}