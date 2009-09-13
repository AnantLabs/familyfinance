using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2.Forms
{
    public partial class TransactionForm : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private FFDBDataSet fFDBDataSet;
        private readonly int thisTransactionID;

        private CDLinesDGV creditDGV;
        private CDLinesDGV debitDGV;
        private SubTransactionDGV subTransDGV;

        // Menu Items
        private ToolStripMenuItem showConfermationColToolStripMenuItem;
        private ToolStripMenuItem newSourceLineToolStripMenuItem;
        private ToolStripMenuItem newDestinationLineToolStripMenuItem;
        private ToolStripMenuItem deleteLineToolStripMenuItem;
        
        private int currentLineID;
        private int CurrentLineID
        {
            get { return currentLineID; }
            set 
            {
                currentLineID = value;
                this.subTransDGV.mySetLineID(value);
            }
        }
        
        private decimal creditSum;
        private decimal debitSum;
        private decimal subLineSum;
        private decimal lineAmount;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void creditDGV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.CurrentLineID = Convert.ToInt32(this.creditDGV["lineItemIDColumn", e.RowIndex].Value);
            }

            creditDGV.myHighlightOn();
            debitDGV.myHighlightOff();

            myResetValues();
        }

        private void debitDGV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.CurrentLineID = Convert.ToInt32(this.debitDGV["lineItemIDColumn", e.RowIndex].Value);
            }

            debitDGV.myHighlightOn();
            creditDGV.myHighlightOff();

            myResetValues();
        }



        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            string text;
            FFDBDataSet.LineItemRow line = this.fFDBDataSet.LineItem.FindByid(this.currentLineID);

            if (line.creditDebit == LineCD.CREDIT)
                text = "Delete source line ";
            else
                text = "Delete destination line ";

            text += "( " + line.LineTypeRow.name;
            text += ", [" + line.AccountRowByFK_Line_accountID.name + "]";
            text += ", " + line.amount.ToString("C2");
            text += ", \"" + line.description + "\")";

            this.deleteLineToolStripMenuItem.Text = text;

        }

        private void showConfermationColMenu_Click(object sender, EventArgs e)
        {
            if (this.showConfermationColToolStripMenuItem.Checked == true)
            {
                this.debitDGV.ShowConfermationColumn = false;
                this.creditDGV.ShowConfermationColumn = false;
                this.showConfermationColToolStripMenuItem.Checked = false;
            }
            else
            {
                this.debitDGV.ShowConfermationColumn = true;
                this.creditDGV.ShowConfermationColumn = true;
                this.showConfermationColToolStripMenuItem.Checked = true;
            }
        }

        private void newCreditLineMenu_Click(object sender, EventArgs e)
        {
            decimal difference = this.debitSum - this.creditSum;
            FFDBDataSet.LineItemRow newLine = this.fFDBDataSet.LineItem.NewLineItemRow();

            newLine.transactionID = this.thisTransactionID;
            newLine.creditDebit = LineCD.CREDIT;

            if (difference > 0.0m)
                newLine.amount = difference;
            else
                newLine.amount = 0.0m;

            this.fFDBDataSet.LineItem.Rows.Add(newLine);

            myResetValues();
        }

        private void newDebitLineMenu_Click(object sender, EventArgs e)
        {
            decimal difference = this.creditSum - this.debitSum;
            FFDBDataSet.LineItemRow newLine = this.fFDBDataSet.LineItem.NewLineItemRow();

            newLine.transactionID = this.thisTransactionID;
            newLine.creditDebit = LineCD.DEBIT;

            if (difference > 0.0m)
                newLine.amount = difference;
            else
                newLine.amount = 0.0m;

            this.fFDBDataSet.LineItem.Rows.Add(newLine);

            myResetValues();
        }

        private void deleteLineMenu_Click(object sender, EventArgs e)
        {
            this.fFDBDataSet.LineItem.FindByid(this.currentLineID).Delete();

            myResetValues();
        }


        private void subTransactionDGV_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            for (int row = 0; row < this.subTransDGV.Rows.Count - 1; row++)
            {
                if (this.subTransDGV[this.subTransDGV.lineItemIDColumn.Index, row].Value == DBNull.Value)
                    this.subTransDGV[this.subTransDGV.lineItemIDColumn.Index, row].Value = this.currentLineID;
            }

            myResetValues();
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            this.fFDBDataSet.mySaveTransaction();
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void myReloadLineTypes()
        {
            fFDBDataSet.LineType.myFill();
        }
        
        private void myReloadAccounts()
        {
            fFDBDataSet.Account.myFill();
        }

        private void myReloadEnvelopes()
        {
            fFDBDataSet.Envelope.myFill();
        }

        private void myResetValues()
        {
            /////////////////////////////////
            // Update the Source and Destination sums.
            this.fFDBDataSet.LineItem.myGetTransCDSum(thisTransactionID, out creditSum , out debitSum);
            sourceSumLabel.Text = creditSum.ToString("C2");
            destinationSumLabel.Text = debitSum.ToString("C2");

            if (creditSum != debitSum)
                sourceSumLabel.ForeColor = destinationSumLabel.ForeColor = System.Drawing.Color.Red;
            else
                sourceSumLabel.ForeColor = destinationSumLabel.ForeColor = System.Drawing.SystemColors.ControlText;

            this.fFDBDataSet.myCheckTransaction(thisTransactionID);

            /////////////////////////////////
            // Update the subLineSum and lineAmount.
            if (this.currentLineID == 0)
            {
                subLineSumLabel.Enabled = false;
                lineAmountLabel.Enabled = false;
                subLineSumLabel.Text = "$0.00";
                lineAmountLabel.Text = "$0.00";
            }
            try
            {
                bool envelopes = this.fFDBDataSet.LineItem.FindByid(this.currentLineID).AccountRowByFK_Line_accountID.envelopes;
                subLineSum = this.fFDBDataSet.SubLineItem.mySubLineSum(this.currentLineID);
                lineAmount = this.fFDBDataSet.LineItem.FindByid(this.currentLineID).amount;

                if (envelopes)
                {
                    subLineSumLabel.Enabled = true;
                    lineAmountLabel.Enabled = true;
                    subLineSumLabel.Text = subLineSum.ToString("C2");
                    lineAmountLabel.Text = lineAmount.ToString("C2");

                    if (lineAmount != subLineSum)
                        lineAmountLabel.ForeColor = subLineSumLabel.ForeColor = System.Drawing.Color.Red;
                    else
                        lineAmountLabel.ForeColor = subLineSumLabel.ForeColor = System.Drawing.SystemColors.ControlText;
                }
                else
                {
                    subLineSumLabel.Enabled = false;
                    lineAmountLabel.Enabled = false;
                    subLineSumLabel.Text = "$0.00";
                    lineAmountLabel.Text = "$0.00";
                }
            }
            catch { return; }
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public TransactionForm(int transactionID)
        {
            InitializeComponent();
            this.thisTransactionID = transactionID;

            ////////////////////////////////////
            // Initialize the dataset and fill the appropriate tables.
            this.fFDBDataSet = new FFDBDataSet();
            this.myReloadLineTypes();
            this.myReloadAccounts();
            this.myReloadEnvelopes();
            this.fFDBDataSet.LineItem.myFillByTransaction(transactionID);
            this.fFDBDataSet.SubLineItem.myFillByTransaction(transactionID);

            // Credit DGV (Top)
            this.creditDGV = new CDLinesDGV(transactionID, LineCD.CREDIT, ref this.fFDBDataSet);
            this.creditDGV.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.creditDGV, 0, 1);
            this.transactionLayoutPanel.SetColumnSpan(this.creditDGV, 6);
            this.creditDGV.RowEnter += new DataGridViewCellEventHandler(creditDGV_RowEnter);

            // Debit DGV (Bottom)
            this.debitDGV = new CDLinesDGV(transactionID, LineCD.DEBIT, ref this.fFDBDataSet);
            this.debitDGV.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.debitDGV, 0, 3);
            this.transactionLayoutPanel.SetColumnSpan(this.debitDGV, 6);
            this.debitDGV.RowEnter += new DataGridViewCellEventHandler(debitDGV_RowEnter);

            // SubTransactions
            this.subTransDGV = new SubTransactionDGV();
            this.subTransDGV.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.subTransDGV, 0, 5);
            this.transactionLayoutPanel.SetRowSpan(this.subTransDGV, 3);
            this.subTransDGV.myInit(ref fFDBDataSet);
            this.subTransDGV.UserAddedRow += new DataGridViewRowEventHandler(subTransactionDGV_UserAddedRow);

            
            ////////////////////////////////////
            // Build the context menu
            this.ContextMenuStrip = new ContextMenuStrip();
            this.ContextMenuStrip.ShowImageMargin = false;
            this.ContextMenuStrip.ShowCheckMargin = true;
            this.ContextMenuStrip.AutoSize = true;
            this.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            this.showConfermationColToolStripMenuItem = new ToolStripMenuItem("Show Confermation Column", null, showConfermationColMenu_Click);
            this.showConfermationColToolStripMenuItem.Checked = true;
            this.ContextMenuStrip.Items.Add(this.showConfermationColToolStripMenuItem);

            this.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            this.newSourceLineToolStripMenuItem = new ToolStripMenuItem("New Source Line", null, newCreditLineMenu_Click);
            this.ContextMenuStrip.Items.Add(this.newSourceLineToolStripMenuItem);

            this.newDestinationLineToolStripMenuItem = new ToolStripMenuItem("New Destination Line", null, newDebitLineMenu_Click);
            this.ContextMenuStrip.Items.Add(this.newDestinationLineToolStripMenuItem);

            this.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            this.deleteLineToolStripMenuItem = new ToolStripMenuItem("Delete Line", null, deleteLineMenu_Click);
            this.ContextMenuStrip.Items.Add(this.deleteLineToolStripMenuItem);

            this.CurrentLineID = this.debitDGV.CurrentLineID;

        }

        

    }
}
