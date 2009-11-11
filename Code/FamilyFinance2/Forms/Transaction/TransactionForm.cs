﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.Forms.Transaction;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms
{
    public partial class TransactionForm : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private TransactionDataSet tDataSet;
        private readonly int thisTransactionID;

        private CDLinesDGV creditDGV;
        private CDLinesDGV debitDGV;
        private SubTransactionDGV subTransDGV;



        // Menu Items
        private ToolStripMenuItem showConfirmationColToolStripMenuItem;
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
        private void cdDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            CDLinesDGV lineDGV = sender as CDLinesDGV;
            int lineID = lineDGV.CurrentLineID;
            TransactionDataSet.LineItemRow thisLine = this.tDataSet.LineItem.FindByid(lineID);

            // Defaults. Used for new lines.
            lineDGV.flagTransactionError = false;
            lineDGV.flagLineError = false;
            lineDGV.flagAccountError = false;
            lineDGV.flagNegativeBalance = false;
            lineDGV.flagReadOnlyEnvelope = false;
            lineDGV.flagReadOnlyAccount = false;
            lineDGV.flagFutureDate = false;

            // Set Flags
            if (thisLine != null)
            {
                bool thisLineUsesEnvelopes = thisLine.AccountRowByFK_Line_accountID.envelopes;

                lineDGV.flagLineError = thisLine.lineError;

                if (thisLine.accountID == SpclAccount.NULL)
                    lineDGV.flagAccountError = true;

                if (thisLine.envelopeID == SpclEnvelope.SPLIT || !thisLineUsesEnvelopes)
                    lineDGV.flagReadOnlyEnvelope = true;

                if (thisLine.date > DateTime.Today) // future Date
                    lineDGV.flagFutureDate = true;
            }
        }

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



        private void creditDebitDGVContextMenu_Opening(object sender, CancelEventArgs e)
        {
            string text;
            TransactionDataSet.LineItemRow line = this.tDataSet.LineItem.FindByid(this.currentLineID);

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

        private void showConfirmationColMenu_Click(object sender, EventArgs e)
        {
            if (this.showConfirmationColToolStripMenuItem.Checked == true)
            {
                this.debitDGV.ShowConfirmationColumn = false;
                this.creditDGV.ShowConfirmationColumn = false;
                this.showConfirmationColToolStripMenuItem.Checked = false;
            }
            else
            {
                this.debitDGV.ShowConfirmationColumn = true;
                this.creditDGV.ShowConfirmationColumn = true;
                this.showConfirmationColToolStripMenuItem.Checked = true;
            }
        }

        private void newCreditLineMenu_Click(object sender, EventArgs e)
        {
            decimal difference = this.debitSum - this.creditSum;
            TransactionDataSet.LineItemRow newLine = this.tDataSet.LineItem.NewLineItemRow();

            newLine.transactionID = this.thisTransactionID;
            newLine.creditDebit = LineCD.CREDIT;

            if (difference > 0.0m)
                newLine.amount = difference;
            else
                newLine.amount = 0.0m;

            this.tDataSet.LineItem.Rows.Add(newLine);

            myResetValues();
        }

        private void newDebitLineMenu_Click(object sender, EventArgs e)
        {
            decimal difference = this.creditSum - this.debitSum;
            TransactionDataSet.LineItemRow newLine = this.tDataSet.LineItem.NewLineItemRow();

            newLine.transactionID = this.thisTransactionID;
            newLine.creditDebit = LineCD.DEBIT;

            if (difference > 0.0m)
                newLine.amount = difference;
            else
                newLine.amount = 0.0m;

            this.tDataSet.LineItem.Rows.Add(newLine);

            myResetValues();
        }

        private void deleteLineMenu_Click(object sender, EventArgs e)
        {
            this.tDataSet.LineItem.FindByid(this.currentLineID).Delete();

            myResetValues();
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            //this.fFDBDataSet.mySaveAndCheckTransaction(this.thisTransactionID);
            this.Close();
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void myReloadLineTypes()
        {
            tDataSet.myFillLineTypeTable();
        }
        
        private void myReloadAccounts()
        {
            tDataSet.myFillAccountTable();
        }

        private void myReloadEnvelopes()
        {
            tDataSet.myFillEnvelopeTable();
        }

        private void myResetValues()
        {
            /////////////////////////////////
            // Update the Source and Destination sums.
            //this.tDataSet.LineItem.myGetTransCDSum(thisTransactionID, out creditSum , out debitSum);
            sourceSumLabel.Text = creditSum.ToString("C2");
            destinationSumLabel.Text = debitSum.ToString("C2");

            if (creditSum != debitSum)
                sourceSumLabel.ForeColor = destinationSumLabel.ForeColor = System.Drawing.Color.Red;
            else
                sourceSumLabel.ForeColor = destinationSumLabel.ForeColor = System.Drawing.SystemColors.ControlText;

            //this.fFDBDataSet.mySaveAndCheckTransaction(thisTransactionID);
            //this.fFDBDataSet.LineItem.myFillByTransaction(thisTransactionID);

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
                bool envelopes = this.tDataSet.LineItem.FindByid(this.currentLineID).AccountRowByFK_Line_accountID.envelopes;
                subLineSum = this.tDataSet.SubLineItem.mySubLineSum(this.currentLineID);
                lineAmount = this.tDataSet.LineItem.FindByid(this.currentLineID).amount;

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
            this.tDataSet = new TransactionDataSet();
            this.tDataSet.myFillAccountTable();
            this.tDataSet.myFillEnvelopeTable();
            this.tDataSet.myFillLineTypeTable();
            this.tDataSet.myFillLineItemAndSubLine(transactionID);

            // Credit DGV (Top)
            this.creditDGV = new CDLinesDGV(ref tDataSet, transactionID, LineCD.CREDIT);
            this.creditDGV.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.creditDGV, 0, 1);
            this.transactionLayoutPanel.SetColumnSpan(this.creditDGV, 6);
            this.creditDGV.RowEnter += new DataGridViewCellEventHandler(creditDGV_RowEnter);
            this.creditDGV.RowPrePaint += new DataGridViewRowPrePaintEventHandler(cdDGV_RowPrePaint);

            // Debit DGV (Bottom)
            this.debitDGV = new CDLinesDGV(ref tDataSet, transactionID, LineCD.DEBIT);
            this.debitDGV.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.debitDGV, 0, 3);
            this.transactionLayoutPanel.SetColumnSpan(this.debitDGV, 6);
            this.debitDGV.RowEnter += new DataGridViewCellEventHandler(debitDGV_RowEnter);
            this.debitDGV.RowPrePaint += new DataGridViewRowPrePaintEventHandler(cdDGV_RowPrePaint);

            // SubTransactions
            this.subTransDGV = new SubTransactionDGV(ref tDataSet);
            this.subTransDGV.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.subTransDGV, 0, 5);
            this.transactionLayoutPanel.SetRowSpan(this.subTransDGV, 3);
           // this.subTransDGV.myInit(ref tDataSet);

            
            ////////////////////////////////////
            // Build the creditDGV/debitDGV context menus
            this.creditDGV.ContextMenuStrip = new ContextMenuStrip();
            this.creditDGV.ContextMenuStrip.ShowImageMargin = false;
            this.creditDGV.ContextMenuStrip.ShowCheckMargin = true;
            this.creditDGV.ContextMenuStrip.AutoSize = true;
            this.creditDGV.ContextMenuStrip.Opening += new CancelEventHandler(creditDebitDGVContextMenu_Opening);

            this.showConfirmationColToolStripMenuItem = new ToolStripMenuItem("Show Confermation Column", null, showConfirmationColMenu_Click);
            this.showConfirmationColToolStripMenuItem.Checked = true;
            this.creditDGV.ContextMenuStrip.Items.Add(this.showConfirmationColToolStripMenuItem);

            this.creditDGV.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            this.newSourceLineToolStripMenuItem = new ToolStripMenuItem("New Source Line", null, newCreditLineMenu_Click);
            this.creditDGV.ContextMenuStrip.Items.Add(this.newSourceLineToolStripMenuItem);

            this.newDestinationLineToolStripMenuItem = new ToolStripMenuItem("New Destination Line", null, newDebitLineMenu_Click);
            this.creditDGV.ContextMenuStrip.Items.Add(this.newDestinationLineToolStripMenuItem);

            this.creditDGV.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            this.deleteLineToolStripMenuItem = new ToolStripMenuItem("Delete Line", null, deleteLineMenu_Click);
            this.creditDGV.ContextMenuStrip.Items.Add(this.deleteLineToolStripMenuItem);

            this.debitDGV.ContextMenuStrip = this.creditDGV.ContextMenuStrip;


            this.CurrentLineID = this.debitDGV.CurrentLineID;

        }

        

    }
}
