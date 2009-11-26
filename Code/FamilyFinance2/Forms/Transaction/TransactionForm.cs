using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Transaction
{
    public partial class TransactionForm : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and Variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private TransactionDataSet tDataSet;
        private readonly int thisTransactionID;
        
        private int _currentLineID;
        private int CurrentLineID
        {
            get { return _currentLineID; }
            set 
            {
                _currentLineID = value;
                //this.label9.Text = value.ToString();
                this.mySetSubLineDGVFilter(value);
                this.tDataSet.mySetCurrentLineID(value);
            }
        }

        private int _currentSubLineID;
        private int CurrentSubLineID
        {
            get { return _currentSubLineID; }
            set
            {
                _currentSubLineID = value;
                //this.label11.Text = value.ToString();
            }
        }

        private CDLinesDGV creditDGV;
        private CDLinesDGV debitDGV;
        private SubTransactionDGV subTransDGV;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Data Grid View events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void cdDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            CDLinesDGV lineDGV = sender as CDLinesDGV;
            int lineID = -1;

            // Defaults. Used for new lines.
            lineDGV.flagTransactionError = false;
            lineDGV.flagLineError = false;
            lineDGV.flagAccountError = false;
            lineDGV.flagFutureDate = false;
            lineDGV.flagNegativeBalance = false;
            lineDGV.flagReadOnlyEnvelope = false;
            lineDGV.flagReadOnlyAccount = false;

            // try to get the current row lineID
            try { lineID = Convert.ToInt32(lineDGV["lineItemIDColumn", e.RowIndex].Value); }
            catch { return; }

            TransactionDataSet.LineItemRow thisLine = this.tDataSet.LineItem.FindByid(lineID);

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

        private void subTransDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int subLineID = -1;

            // Defaults. Used for new lines.
            subTransDGV.flagEnvelopeError = false;
            subTransDGV.flagNegativeAmount = false;

            // try to get the current row lineID
            try { subLineID = Convert.ToInt32(subTransDGV["subLineIDColumn", e.RowIndex].Value); }
            catch { return; }

            TransactionDataSet.SubLineItemRow thisSubLine = this.tDataSet.SubLineItem.FindByid(subLineID);

            // Set Flags
            if (thisSubLine != null)
            {
                if (thisSubLine.envelopeID == SpclEnvelope.NULL)
                    subTransDGV.flagEnvelopeError = true;

                if (thisSubLine.amount < 0.00m)
                    subTransDGV.flagNegativeAmount = true;
            }
        }

        private void creditDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                mySetCurrentLineID(LineCD.CREDIT, e.RowIndex);

            creditDGV.myHighLightOn();
            debitDGV.myHighLightOff();
            subTransDGV.myHighLightOff();

            myResetValues();
        }
        
        private void debitDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                mySetCurrentLineID(LineCD.DEBIT, e.RowIndex);

            debitDGV.myHighLightOn();
            creditDGV.myHighLightOff();
            subTransDGV.myHighLightOff();

            myResetValues();
        }

        private void subTransDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.subTransDGV.myHighLightOn();

            myResetValues();
        }

        
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Context Menu Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void creditContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            bool found = false;

            for (int row = 0; row < this.creditDGV.RowCount; row++)
            {
                for (int col = 0; col < this.creditDGV.ColumnCount; col++)
                {
                    if (this.creditDGV.GetCellDisplayRectangle(col, row, true).Contains(this.creditDGV.PointToClient(MousePosition)))
                    {
                        this.creditDGV.CurrentCell = this.creditDGV[col, row];
                        this.creditDGV.myHighLightOn();
                        this.debitDGV.myHighLightOff();

                        mySetCurrentLineID(LineCD.CREDIT, row);
                        found = true;
                        break;
                    } 
                }
                if (found)
                    break;
            }

            if (!found)
                mySetCurrentLineID(LineCD.CREDIT, -1);

            this.updateCM();
        }

        private void creditDGV_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            mySetCurrentLineID(LineCD.CREDIT, e.RowIndex);
            e.ContextMenuStrip = this.creditDGV.ContextMenuStrip;
        }


        private void debitContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            bool found = false;

            for (int row = 0; row < this.debitDGV.RowCount; row++)
            {
                for (int col = 0; col < this.debitDGV.ColumnCount; col++)
                {
                    if (this.debitDGV.GetCellDisplayRectangle(col, row, true).Contains(this.debitDGV.PointToClient(MousePosition)))
                    {
                        this.debitDGV.CurrentCell = this.debitDGV[col, row];
                        this.debitDGV.myHighLightOn();
                        this.creditDGV.myHighLightOff();

                        mySetCurrentLineID(LineCD.DEBIT, row);
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }

            if (!found)
                mySetCurrentLineID(LineCD.DEBIT, -1);

            this.updateCM();
        }

        private void debitDGV_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            mySetCurrentLineID(LineCD.DEBIT, e.RowIndex);
            e.ContextMenuStrip = this.debitDGV.ContextMenuStrip;
        }


        private void newLineMenu_Click(object sender, EventArgs e)
        {
            decimal debitSum;
            decimal creditSum;
            decimal difference;
            string menuText = (sender as ToolStripMenuItem).Text;
            TransactionDataSet.LineItemRow newLine = this.tDataSet.LineItem.NewLineItemRow();
            
            //newLine.transactionID = this.thisTransactionID;
            this.tDataSet.myCheckTransaction(out creditSum, out debitSum);

            if (menuText.Contains("Source"))
            {
                newLine.creditDebit = LineCD.CREDIT;
                difference = debitSum - creditSum;
            }
            else if (menuText.Contains("Destination"))
            {
                newLine.creditDebit = LineCD.DEBIT;
                difference = creditSum - debitSum;
            }
            else
                return;

            if (difference > 0.0m)
                newLine.amount = difference;
            else
                newLine.amount = 0.0m;

            this.tDataSet.LineItem.Rows.Add(newLine);

            myResetValues();
        }

        private void changeLineMenu_Click(object sender, EventArgs e)
        {
            TransactionDataSet.LineItemRow line = this.tDataSet.LineItem.FindByid(this.CurrentLineID);

            if (line != null)
            {
                bool cd = line.creditDebit;
                line.creditDebit = !cd;
            }

            myResetValues();
        }

        private void deleteLineMenu_Click(object sender, EventArgs e)
        {
            this.tDataSet.myDeleteLine(this.CurrentLineID);
            myResetValues();
        }

        private void showConfirmationColMenu_Click(object sender, EventArgs e)
        {
            if (this.debitDGV.ShowConfirmationColumn == true)
            {
                this.debitDGV.ShowConfirmationColumn = false;
                this.creditDGV.ShowConfirmationColumn = false;
            }
            else
            {
                this.debitDGV.ShowConfirmationColumn = true;
                this.creditDGV.ShowConfirmationColumn = true;
            }
        }


        private void subLineContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            object value;
            bool isEnabled = false;

            // Find the row that was clicked
            for (int row = 0; row < this.subTransDGV.RowCount; row++)
            {
                if (this.subTransDGV.GetRowDisplayRectangle(row, true).Contains(this.subTransDGV.PointToClient(MousePosition)))
                {
                    try
                    {
                        value = this.subTransDGV["subLineIDColumn", row].Value;
                        this.CurrentSubLineID = Convert.ToInt32(value);
                        isEnabled = true;
                    }
                    catch 
                    { 
                        this.CurrentSubLineID = -1; 
                    }
                    break;
                }
                else
                {
                    this.CurrentSubLineID = -1;
                }
            }

            // Disable or enable the delete menu item
            this.subTransDGV.ContextMenuStrip.Items[1].Enabled = isEnabled; 
        }

        private void newSubLineMenu_Click(object sender, EventArgs e)
        {

            decimal lineAmount = this.tDataSet.LineItem.FindByid(this.CurrentLineID).amount;
            decimal subSum = this.tDataSet.SubLineItem.mySubLineSum(this.CurrentLineID);
            decimal difference = lineAmount - subSum;
            TransactionDataSet.SubLineItemRow newSubLine = this.tDataSet.SubLineItem.NewSubLineItemRow();

            newSubLine.amount = difference;

            this.tDataSet.SubLineItem.Rows.Add(newSubLine);

            myResetValues();
        }

        private void deleteSubLineMenu_Click(object sender, EventArgs e)
        {
            this.tDataSet.SubLineItem.FindByid(this.CurrentSubLineID).Delete();
            myResetValues();
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Button Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            this.tDataSet.myCheckTransaction();
            this.tDataSet.myRippleBalanceChanges();
            this.tDataSet.mySaveChanges();
            this.Close();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildDGVs()
        {
            ////////////////////////////////////
            // Credit DGV (Top)
            this.creditDGV = new CDLinesDGV(LineCD.CREDIT);
            this.creditDGV.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.creditDGV, 0, 1);
            this.transactionLayoutPanel.SetColumnSpan(this.creditDGV, 6);

            this.creditDGV.BindingSourceLineItemDGV.DataSource = tDataSet;
            this.creditDGV.BindingSourceLineItemDGV.DataMember = "LineItem";
            this.creditDGV.BindingSourceLineItemDGV.Filter = "creditDebit = 0"; // LineCD.CREDIT

            this.creditDGV.BindingSourceAccountIDCol.DataSource = tDataSet;
            this.creditDGV.BindingSourceAccountIDCol.DataMember = "Account";
            this.creditDGV.BindingSourceAccountIDCol.Sort = "name";
            this.creditDGV.BindingSourceAccountIDCol.Filter = "id <> " + SpclAccount.MULTIPLE;

            this.creditDGV.BindingSourceEnvelopeIDCol.DataSource = tDataSet;
            this.creditDGV.BindingSourceEnvelopeIDCol.DataMember = "Envelope";
            this.creditDGV.BindingSourceEnvelopeIDCol.Sort = "fullName";

            this.creditDGV.BindingSourceLineTypeIDCol.DataSource = tDataSet;
            this.creditDGV.BindingSourceLineTypeIDCol.DataMember = "LineType";
            this.creditDGV.BindingSourceLineTypeIDCol.Sort = "name";

            this.creditDGV.CellClick += new DataGridViewCellEventHandler(creditDGV_CellClick);
            this.creditDGV.RowPrePaint += new DataGridViewRowPrePaintEventHandler(cdDGV_RowPrePaint);
            this.creditDGV.CellContextMenuStripNeeded += new DataGridViewCellContextMenuStripNeededEventHandler(creditDGV_CellContextMenuStripNeeded);


            ////////////////////////////////////
            // Debit DGV (Bottom)
            this.debitDGV = new CDLinesDGV(LineCD.DEBIT);
            this.debitDGV.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.debitDGV, 0, 3);
            this.transactionLayoutPanel.SetColumnSpan(this.debitDGV, 6);

            this.debitDGV.BindingSourceLineItemDGV.DataSource = tDataSet;
            this.debitDGV.BindingSourceLineItemDGV.DataMember = "LineItem";
            this.debitDGV.BindingSourceLineItemDGV.Filter = "creditDebit = 1"; // LineCD.DEBIT

            this.debitDGV.BindingSourceAccountIDCol.DataSource = tDataSet;
            this.debitDGV.BindingSourceAccountIDCol.DataMember = "Account";
            this.debitDGV.BindingSourceAccountIDCol.Sort = "name";
            this.debitDGV.BindingSourceAccountIDCol.Filter = "id <> " + SpclAccount.MULTIPLE.ToString();

            this.debitDGV.BindingSourceEnvelopeIDCol.DataSource = tDataSet;
            this.debitDGV.BindingSourceEnvelopeIDCol.DataMember = "Envelope";
            this.debitDGV.BindingSourceEnvelopeIDCol.Sort = "fullName";

            this.debitDGV.BindingSourceLineTypeIDCol.DataSource = tDataSet;
            this.debitDGV.BindingSourceLineTypeIDCol.DataMember = "LineType";
            this.debitDGV.BindingSourceLineTypeIDCol.Sort = "name";

            this.debitDGV.CellClick += new DataGridViewCellEventHandler(debitDGV_CellClick);
            this.debitDGV.RowPrePaint += new DataGridViewRowPrePaintEventHandler(cdDGV_RowPrePaint);
            this.debitDGV.CellContextMenuStripNeeded += new DataGridViewCellContextMenuStripNeededEventHandler(debitDGV_CellContextMenuStripNeeded);


            ////////////////////////////////////
            // SubTransactions DGV
            this.subTransDGV = new SubTransactionDGV();
            this.transactionLayoutPanel.Controls.Add(this.subTransDGV, 0, 5);
            this.transactionLayoutPanel.SetRowSpan(this.subTransDGV, 3);

            this.subTransDGV.BindingSourceSubLineDGV.DataSource = this.tDataSet;
            this.subTransDGV.BindingSourceSubLineDGV.DataMember = "SubLineItem";

            this.subTransDGV.BindingSourceEnvelopeCol.DataSource = this.tDataSet;
            this.subTransDGV.BindingSourceEnvelopeCol.DataMember = "Envelope";
            this.subTransDGV.BindingSourceEnvelopeCol.Filter = "id <> " + SpclEnvelope.SPLIT.ToString();

            this.subTransDGV.RowPrePaint += new DataGridViewRowPrePaintEventHandler(subTransDGV_RowPrePaint);
            this.subTransDGV.CellClick += new DataGridViewCellEventHandler(subTransDGV_CellClick);
        }


        private void buildCMs()
        {
            // Credit Context Menu
            this.creditDGV.ContextMenuStrip = new ContextMenuStrip();
            this.creditDGV.ContextMenuStrip.ShowImageMargin = false;
            this.creditDGV.ContextMenuStrip.ShowCheckMargin = true;
            this.creditDGV.ContextMenuStrip.AutoSize = true;

            this.creditDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("New Source Line", null, newLineMenu_Click));
            this.creditDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Change to Destination Line", null, changeLineMenu_Click));
            this.creditDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Delete Line", null, deleteLineMenu_Click));
            this.creditDGV.ContextMenuStrip.Items.Add(new ToolStripSeparator()); // -------------------
            this.creditDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Show Confermation Column", null, showConfirmationColMenu_Click));

            this.creditDGV.ContextMenuStrip.Opening += new CancelEventHandler(creditContextMenuStrip_Opening);

            // Debit Context Menu
            this.debitDGV.ContextMenuStrip = new ContextMenuStrip();
            this.debitDGV.ContextMenuStrip.ShowImageMargin = false;
            this.debitDGV.ContextMenuStrip.ShowCheckMargin = true;
            this.debitDGV.ContextMenuStrip.AutoSize = true;

            this.debitDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("New Destination Line", null, newLineMenu_Click));
            this.debitDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Change to Source Line", null, changeLineMenu_Click));
            this.debitDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Delete Line", null, deleteLineMenu_Click));
            this.debitDGV.ContextMenuStrip.Items.Add(new ToolStripSeparator()); // -------------------
            this.debitDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Show Confermation Column", null, showConfirmationColMenu_Click));

            this.debitDGV.ContextMenuStrip.Opening += new CancelEventHandler(debitContextMenuStrip_Opening);


            // Sub Lines Context Menu
            this.subTransDGV.ContextMenuStrip = new ContextMenuStrip();
            this.subTransDGV.ContextMenuStrip.ShowImageMargin = false;
            this.subTransDGV.ContextMenuStrip.ShowCheckMargin = false;
            this.subTransDGV.ContextMenuStrip.AutoSize = true;

            this.subTransDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("New Sub Line", null, newSubLineMenu_Click));
            this.subTransDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Delete Sub Line", null, deleteSubLineMenu_Click));

            this.subTransDGV.ContextMenuStrip.Opening += new CancelEventHandler(subLineContextMenuStrip_Opening);
        }

        private void updateCM()
        {
            if (this.CurrentLineID <= 0)
            {
                this.creditDGV.ContextMenuStrip.Items[1].Enabled = false;
                this.creditDGV.ContextMenuStrip.Items[2].Enabled = false;
                this.debitDGV.ContextMenuStrip.Items[1].Enabled = false;
                this.debitDGV.ContextMenuStrip.Items[2].Enabled = false;
            }
            else
            {
                this.creditDGV.ContextMenuStrip.Items[1].Enabled = true;
                this.creditDGV.ContextMenuStrip.Items[2].Enabled = true;
                this.debitDGV.ContextMenuStrip.Items[1].Enabled = true;
                this.debitDGV.ContextMenuStrip.Items[2].Enabled = true;
            }

            (this.creditDGV.ContextMenuStrip.Items[4] as ToolStripMenuItem).Checked = this.debitDGV.ShowConfirmationColumn;
            (this.debitDGV.ContextMenuStrip.Items[4] as ToolStripMenuItem).Checked = this.debitDGV.ShowConfirmationColumn;
        }


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
            decimal creditSum;
            decimal debitSum;
            decimal subLineSum;
            decimal lineAmount;

            /////////////////////////////////
            // Update the Source and Destination sums.
            this.tDataSet.myCheckTransaction(out creditSum, out debitSum);

            sourceSumLabel.Text = creditSum.ToString("C2");
            destinationSumLabel.Text = debitSum.ToString("C2");

            if (creditSum != debitSum)
                sourceSumLabel.ForeColor = destinationSumLabel.ForeColor = System.Drawing.Color.Red;
            else
                sourceSumLabel.ForeColor = destinationSumLabel.ForeColor = System.Drawing.SystemColors.ControlText;


            /////////////////////////////////
            // Update the subLineSum and lineAmount.
            if (this.CurrentLineID == -1)
            {
                subLineSumLabel.Enabled = false;
                lineAmountLabel.Enabled = false;
                subLineSumLabel.Text = "$0.00";
                lineAmountLabel.Text = "$0.00";
            }
            try
            {
                bool envelopes = this.tDataSet.LineItem.FindByid(this.CurrentLineID).AccountRowByFK_Line_accountID.envelopes;
                subLineSum = this.tDataSet.SubLineItem.mySubLineSum(this.CurrentLineID);
                lineAmount = this.tDataSet.LineItem.FindByid(this.CurrentLineID).amount;

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

        private void mySetCurrentLineID(bool creditDebit, int row)
        {
            object value;

            try
            {
                if (creditDebit == LineCD.CREDIT)
                    value = this.creditDGV["lineitemIDcolumn", row].Value;
                else
                    value = this.debitDGV["lineitemIDcolumn", row].Value;

                this.CurrentLineID = Convert.ToInt32(value);
            }
            catch
            {
                this.CurrentLineID = -1;
            }
        }

        private void mySetSubLineDGVFilter(int lineID)
        {
            bool lineAccountUsesEnvelopes = false;

            try
            {
                lineAccountUsesEnvelopes = this.tDataSet.LineItem.FindByid(lineID).AccountRowByFK_Line_accountID.envelopes;
            }
            catch 
            { 
                lineAccountUsesEnvelopes = false; 
            }

            if (lineAccountUsesEnvelopes)
            {
                this.subTransDGV.BindingSourceSubLineDGV.Filter = "lineItemID = " + lineID.ToString();
                this.subTransDGV.Enabled = true;
            }
            else
            {
                this.subTransDGV.BindingSourceSubLineDGV.Filter = "id = -1";
                this.subTransDGV.Enabled = false;
            }
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
            this.tDataSet.myInit();
            this.tDataSet.myFillAccountTable();
            this.tDataSet.myFillEnvelopeTable();
            this.tDataSet.myFillLineTypeTable();
            this.tDataSet.myFillLineItemAndSubLine(transactionID);

            this.buildDGVs();
            this.buildCMs();

            // Initialize to a a certain line.
            this.creditDGV.myHighLightOff();
            this.debitDGV.myHighLightOff();
            this.CurrentLineID = -1;
            this.myResetValues();
        }
       

    }
}
