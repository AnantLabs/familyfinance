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
            }
        }

        private int _currentEnvelopeLineID;
        private int CurrentEnvelopeLineID
        {
            get { return _currentEnvelopeLineID; }
            set
            {
                _currentEnvelopeLineID = value;
                //this.label11.Text = value.ToString();
            }
        }

        private CDLinesDGV creditDGV;
        private CDLinesDGV debitDGV;
        private EnvelopeLineDGV envLinesDGV;



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Data table events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void LineItem_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            decimal debitSum;
            decimal creditSum;
            decimal difference;
            TransactionDataSet.LineItemRow line = e.Row as TransactionDataSet.LineItemRow;

            this.tDataSet.myCheckTransaction();
            this.tDataSet.myGetCDSums(out creditSum, out debitSum);

            if (this.creditDGV.Focused)
            {
                line.creditDebit = LineCD.CREDIT;
                difference = debitSum - creditSum;
            }
            else if (this.debitDGV.Focused)
            {
                line.creditDebit = LineCD.DEBIT;
                difference = creditSum - debitSum;
            }
            else
                return;

            line.transactionID = this.thisTransactionID;

            if (difference > 0.0m)
                line.amount = difference;
            else
                line.amount = 0.0m;


            myResetValues();
        }
        
        private void EnvelopeLine_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            TransactionDataSet.EnvelopeLineRow eLine = e.Row as TransactionDataSet.EnvelopeLineRow;
            decimal lineAmount = this.tDataSet.LineItem.FindByid(this.CurrentLineID).amount;
            decimal envSum = this.tDataSet.EnvelopeLine.myEnvelopeLineSum(this.CurrentLineID);
            decimal difference = lineAmount - envSum;

            eLine.lineItemID = this.CurrentLineID;
            eLine.amount = difference;

            myResetValues();
        }

        
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Data Grid View events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void cdDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            CDLinesDGV lineDGV = sender as CDLinesDGV;
            int lineID = -1;

            // Defaults. Used for new lines.
            lineDGV.flagLineError = false;
            lineDGV.flagAccountError = false;

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

                //if (thisLine.envelopeID == SpclEnvelope.SPLIT || !thisLineUsesEnvelopes)
                //    lineDGV.flagReadOnlyEnvelope = true;


            }
        }

        private void envLinesDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int subLineID = -1;

            // Defaults. Used for new lines.
            envLinesDGV.flagEnvelopeError = false;
            envLinesDGV.flagNegativeAmount = false;

            // try to get the current row lineID
            try { subLineID = Convert.ToInt32(envLinesDGV["EnvelopeLineIDColumn", e.RowIndex].Value); }
            catch { return; }

            TransactionDataSet.EnvelopeLineRow thisSubLine = this.tDataSet.EnvelopeLine.FindByid(subLineID);

            // Set Flags
            if (thisSubLine != null)
            {
                if (thisSubLine.envelopeID == SpclEnvelope.NULL)
                    envLinesDGV.flagEnvelopeError = true;

                if (thisSubLine.amount < 0.00m)
                    envLinesDGV.flagNegativeAmount = true;
            }
        }


        private void creditDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                mySetCurrentLineID(LineCD.CREDIT, e.RowIndex);

            creditDGV.myHighLightOn();
            debitDGV.myHighLightOff();
            envLinesDGV.myHighLightOff();

            myResetValues();
        }
        
        private void debitDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                mySetCurrentLineID(LineCD.DEBIT, e.RowIndex);

            debitDGV.myHighLightOn();
            creditDGV.myHighLightOff();
            envLinesDGV.myHighLightOff();

            myResetValues();
        }

        private void subTransDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.envLinesDGV.myHighLightOn();

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


        private void envLineContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            bool found = false;
            object value;
            bool isEnabled = false;

            for (int row = 0; row < this.envLinesDGV.RowCount; row++)
            {
                for (int col = 0; col < this.envLinesDGV.ColumnCount; col++)
                {
                    if (this.envLinesDGV.GetCellDisplayRectangle(col, row, true).Contains(this.envLinesDGV.PointToClient(MousePosition)))
                    {
                        this.envLinesDGV.CurrentCell = this.envLinesDGV[col, row];
                        found = true;
                        try
                        {
                            value = this.envLinesDGV["EnvelopeLineIDColumn", row].Value;
                            this.CurrentEnvelopeLineID = Convert.ToInt32(value);
                            isEnabled = true;
                        }
                        catch
                        {
                            this.CurrentEnvelopeLineID = -1;
                        }
                        break;
                    }
                    else
                    {
                        this.CurrentEnvelopeLineID = -1;
                    }
                }
                if (found)
                    break;
            }

            this.envLinesDGV.ContextMenuStrip.Items[0].Enabled = isEnabled; 
        }

        private void deleteSubLineMenu_Click(object sender, EventArgs e)
        {
            this.tDataSet.myDeleteEnvLine(this.CurrentEnvelopeLineID);
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
            this.tDataSet.mySetDependentValues();
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
            this.creditDGV.BindingSourceEnvelopeIDCol.Sort = "name";

            this.creditDGV.BindingSourceTypeIDCol.DataSource = tDataSet;
            this.creditDGV.BindingSourceTypeIDCol.DataMember = "LineType";
            this.creditDGV.BindingSourceTypeIDCol.Sort = "name";

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
            this.debitDGV.BindingSourceEnvelopeIDCol.Sort = "name";

            this.debitDGV.BindingSourceTypeIDCol.DataSource = tDataSet;
            this.debitDGV.BindingSourceTypeIDCol.DataMember = "LineType";
            this.debitDGV.BindingSourceTypeIDCol.Sort = "name";

            this.debitDGV.CellClick += new DataGridViewCellEventHandler(debitDGV_CellClick);
            this.debitDGV.RowPrePaint += new DataGridViewRowPrePaintEventHandler(cdDGV_RowPrePaint);
            this.debitDGV.CellContextMenuStripNeeded += new DataGridViewCellContextMenuStripNeededEventHandler(debitDGV_CellContextMenuStripNeeded);


            ////////////////////////////////////
            // SubTransactions DGV
            this.envLinesDGV = new EnvelopeLineDGV();
            this.transactionLayoutPanel.Controls.Add(this.envLinesDGV, 0, 5);
            this.transactionLayoutPanel.SetRowSpan(this.envLinesDGV, 3);

            this.envLinesDGV.BindingSourceEnvelopeLineDGV.DataSource = this.tDataSet;
            this.envLinesDGV.BindingSourceEnvelopeLineDGV.DataMember = "EnvelopeLine";

            this.envLinesDGV.BindingSourceEnvelopeCol.DataSource = this.tDataSet;
            this.envLinesDGV.BindingSourceEnvelopeCol.DataMember = "Envelope";
            this.envLinesDGV.BindingSourceEnvelopeCol.Sort = "name";
            this.envLinesDGV.BindingSourceEnvelopeCol.Filter = "id <> " + SpclEnvelope.SPLIT.ToString();

            this.envLinesDGV.RowPrePaint += new DataGridViewRowPrePaintEventHandler(envLinesDGV_RowPrePaint);
            this.envLinesDGV.CellClick += new DataGridViewCellEventHandler(subTransDGV_CellClick);
        }


        private void buildCMs()
        {
            // Credit Context Menu
            this.creditDGV.ContextMenuStrip = new ContextMenuStrip();
            this.creditDGV.ContextMenuStrip.ShowImageMargin = false;
            this.creditDGV.ContextMenuStrip.ShowCheckMargin = true;
            this.creditDGV.ContextMenuStrip.AutoSize = true;

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

            this.debitDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Change to Source Line", null, changeLineMenu_Click));
            this.debitDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Delete Line", null, deleteLineMenu_Click));
            this.debitDGV.ContextMenuStrip.Items.Add(new ToolStripSeparator()); // -------------------
            this.debitDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Show Confermation Column", null, showConfirmationColMenu_Click));

            this.debitDGV.ContextMenuStrip.Opening += new CancelEventHandler(debitContextMenuStrip_Opening);


            // Sub Lines Context Menu
            this.envLinesDGV.ContextMenuStrip = new ContextMenuStrip();
            this.envLinesDGV.ContextMenuStrip.ShowImageMargin = false;
            this.envLinesDGV.ContextMenuStrip.ShowCheckMargin = false;
            this.envLinesDGV.ContextMenuStrip.AutoSize = true;

            this.envLinesDGV.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Delete Sub Line", null, deleteSubLineMenu_Click));

            this.envLinesDGV.ContextMenuStrip.Opening += new CancelEventHandler(envLineContextMenuStrip_Opening);
        }

        private void updateCM()
        {
            if (this.CurrentLineID <= 0)
            {
                this.creditDGV.ContextMenuStrip.Items[0].Enabled = false;
                this.creditDGV.ContextMenuStrip.Items[1].Enabled = false;
                this.debitDGV.ContextMenuStrip.Items[0].Enabled = false;
                this.debitDGV.ContextMenuStrip.Items[1].Enabled = false;
            }
            else
            {
                this.creditDGV.ContextMenuStrip.Items[0].Enabled = true;
                this.creditDGV.ContextMenuStrip.Items[1].Enabled = true;
                this.debitDGV.ContextMenuStrip.Items[0].Enabled = true;
                this.debitDGV.ContextMenuStrip.Items[1].Enabled = true;
            }

            (this.creditDGV.ContextMenuStrip.Items[3] as ToolStripMenuItem).Checked = this.debitDGV.ShowConfirmationColumn;
            (this.debitDGV.ContextMenuStrip.Items[3] as ToolStripMenuItem).Checked = this.debitDGV.ShowConfirmationColumn;
        }


        private void myResetValues()
        {
            decimal creditSum;
            decimal debitSum;
            decimal envLineSum;
            decimal lineAmount;

            /////////////////////////////////
            // Update the Source and Destination sums.
            this.tDataSet.myCheckTransaction();
            this.tDataSet.myGetCDSums(out creditSum, out debitSum);

            sourceSumLabel.Text = creditSum.ToString("C2");
            destinationSumLabel.Text = debitSum.ToString("C2");

            if (creditSum != debitSum)
            {
                sourceSumLabel.ForeColor = destinationSumLabel.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                sourceSumLabel.ForeColor = destinationSumLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            }


            /////////////////////////////////
            // Update the subLineSum and lineAmount.
            try
            {
                bool envelopes = this.tDataSet.LineItem.FindByid(this.CurrentLineID).AccountRowByFK_Line_accountID.envelopes;
                envLineSum = this.tDataSet.EnvelopeLine.myEnvelopeLineSum(this.CurrentLineID);
                lineAmount = this.tDataSet.LineItem.FindByid(this.CurrentLineID).amount;

                if (envelopes)
                {
                    subLineSumLabel.Enabled = true;
                    lineAmountLabel.Enabled = true;
                    subLineSumLabel.Text = envLineSum.ToString("C2");
                    lineAmountLabel.Text = lineAmount.ToString("C2");

                    if (lineAmount != envLineSum)
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
            catch
            {
                subLineSumLabel.Enabled = false;
                lineAmountLabel.Enabled = false;
                subLineSumLabel.Text = "$0.00";
                lineAmountLabel.Text = "$0.00";
            }
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
                this.envLinesDGV.BindingSourceEnvelopeLineDGV.Filter = "lineItemID = " + lineID.ToString();
                this.envLinesDGV.Enabled = true;
                this.envLinesDGV.AllowUserToAddRows = true;
            }
            else
            {
                this.envLinesDGV.BindingSourceEnvelopeLineDGV.Filter = "id = -1";
                this.envLinesDGV.Enabled = false;
                this.envLinesDGV.AllowUserToAddRows = false;
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

            this.tDataSet.LineItem.TableNewRow += new DataTableNewRowEventHandler(LineItem_TableNewRow);
            this.tDataSet.EnvelopeLine.TableNewRow += new DataTableNewRowEventHandler(EnvelopeLine_TableNewRow);

            this.buildDGVs();
            this.buildCMs();

            // Initialize to a a certain line.
            this.creditDGV.myHighLightOff();
            this.debitDGV.myHighLightOff();
            this.CurrentLineID = -1;
            this.myResetValues();
        }

        public TransactionForm(int transactionID, int lineID)
            : this(transactionID)
        {

            if (this.tDataSet.LineItem.FindByid(lineID).creditDebit == LineCD.CREDIT)
            {
                creditDGV.myHighLightOn();
                debitDGV.myHighLightOff();
                envLinesDGV.myHighLightOff();
            }
            else
            {
                debitDGV.myHighLightOn();
                creditDGV.myHighLightOff();
                envLinesDGV.myHighLightOff();
            }

            this.CurrentLineID = lineID;
            myResetValues();

        }

        public TransactionForm(int transactionID, int lineID, int eLineID)
            : this(transactionID, lineID)
        {

            //if (this.tDataSet.LineItem.FindByid(lineID).creditDebit == LineCD.CREDIT)
            //{
            //    creditDGV.myHighLightOn();
            //    debitDGV.myHighLightOff();
            //    envLinesDGV.myHighLightOff();
            //}
            //else
            //{
            //    debitDGV.myHighLightOn();
            //    creditDGV.myHighLightOff();
            //    envLinesDGV.myHighLightOff();
            //}

            //this.CurrentLineID = lineID;
            //myResetValues();

        }


        public List<AEPair> myGetChanges()
        {
            return this.tDataSet.myGetChanges();
        }
    }
}
