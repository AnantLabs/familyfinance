using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2
{
    class CDLinesDGV : DataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private FFDBDataSet fFDBDataSet;
        public readonly int thisTransactionID;
        public readonly int thisViewID;
        private readonly bool thisCD;


        // Row flags used between RowPrePaint and  CellFormatting
        private string rowErrorText;
        private bool rowLineError;
        private bool rowSplitEnvelope;
        private bool rowNoEnvelope;
        private bool rowAccountNull;


        // Binding Sources
        private BindingSource theDGVBindingSource;
        private BindingSource lineTypeBindingSource;
        private BindingSource accountBindingSource;
        private BindingSource envelopeBindingSource;

        // Columns
        private DataGridViewTextBoxColumn lineItemIDColumn;
        private DataGridViewTextBoxColumn transactionIDColumn;
        private CalendarColumn dateColumn;
        private DataGridViewComboBoxColumn typeIDColumn;
        private DataGridViewComboBoxColumn accountIDColumn;
        private DataGridViewTextBoxColumn oppAccountIDColumn;
        private DataGridViewTextBoxColumn descriptionColumn;
        private DataGridViewTextBoxColumn confermationNumColumn;
        private DataGridViewComboBoxColumn envelopeIDColumn;
        private DataGridViewTextBoxColumn completeColumn;
        private DataGridViewTextBoxColumn amountColumn;
        private DataGridViewCheckBoxColumn creditDebitColumn;



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Properties
        ////////////////////////////////////////////////////////////////////////////////////////////
        public int CurrentLineID
        {
            get
            {
                int lineID = -1;

                try
                {
                    lineID = Convert.ToInt32(this.CurrentRow.Cells[this.lineItemIDColumn.Index].Value);
                }
                catch
                {
                }

                return lineID;
            }
        }

        public int CurrentAccountID
        {
            get
            {
                int accountID = SpclAccount.NULL;

                try
                {
                    accountID = Convert.ToInt32(this.CurrentRow.Cells[this.accountIDColumn.Index].Value);
                }
                catch
                {
                }

                return accountID;
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void CDLinesDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            if (col == completeColumn.Index && row >= 0)
            {
                if (this[col, row].Value.ToString() == LineState.PENDING)
                    this[col, row].Value = LineState.CLEARED;

                else if (this[col, row].Value.ToString() == LineState.CLEARED)
                    this[col, row].Value = LineState.RECONSILED;

                else
                    this[col, row].Value = LineState.PENDING;
            }
        }

        private void CDLinesDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            //short accountID = Convert.ToInt16(this["accountIDColumn", e.RowIndex].Value);
            //int lineID = Convert.ToInt32(this["lineItemIDColumn", e.RowIndex].Value);
            //FFDBDataSet.LineItemRow lineRow = this.fFDBDataSet.LineItem.FindByid(lineID);
            //System.Drawing.Color backColor;
            //System.Drawing.Color foreColor;
            //bool usesEnvelopes;

            //// Defaults
            //this.rowLineError = false;
            //this.rowSplitEnvelope = false;
            //this.rowNoEnvelope = false;
            //this.rowAccountNull = false;
            //this.rowErrorText = "";
            ////backColor = this.fFDBDataSet.MyColors.DGVBack;
            ////foreColor = this.fFDBDataSet.MyColors.DGVFore;
            //usesEnvelopes = false;

            //// Set error flags
            //if (lineRow != null)
            //{
            //    this.rowLineError = lineRow.lineError;

            //    // Set usesEnvelopes
            //    if (accountID == SpclAccount.NULL)
            //    {
            //        usesEnvelopes = false;
            //        this.rowAccountNull = true;
            //        this.rowErrorText += "Select an account. ";
            //    }
            //    else
            //        usesEnvelopes = this.fFDBDataSet.Account.FindByid(accountID).envelopes;
                

            //    // Set the flags
            //    if (usesEnvelopes && lineRow.envelopeID == SpclEnvelope.NULL)
            //    {
            //        this.rowLineError = true;
            //        this.rowErrorText += "Select an envelope. ";
            //    }
            //    else if (rowLineError) // Either the previous error or this next one.
            //    {
            //        this.rowLineError = true;
            //        this.rowErrorText += "The sub transactions need attention. ";
            //    }

            //    if (lineRow.envelopeID == SpclEnvelope.SPLIT)
            //        this.rowSplitEnvelope = true;

            //    if (usesEnvelopes == false)
            //        this.rowNoEnvelope = true;


            //} // END if (lineRow != null)

            //this.Rows[e.RowIndex].DefaultCellStyle.BackColor = backColor;
            //this.Rows[e.RowIndex].DefaultCellStyle.ForeColor = foreColor;
        }

        private void CDLinesDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //System.Drawing.Color backColor;
            //System.Drawing.Color foreColor;
            //bool cellReadOnly;
            //int col = e.ColumnIndex;
            //int row = e.RowIndex;

            //backColor = e.CellStyle.BackColor;
            //foreColor = e.CellStyle.ForeColor;
            //cellReadOnly = false;


            //// Disable or enable the envelopeID cell.
            //if (col == this.envelopeIDColumn.Index)
            //{
            //    if (this.rowNoEnvelope)
            //    {
            //        cellReadOnly = true;
            //        backColor = this.fFDBDataSet.MyColors.DGVDisabledBack;
            //        this.rowErrorText = "This account does not use Envelopes. ";
            //    }
            //    else if (this.rowLineError)
            //    {
            //        backColor = this.fFDBDataSet.MyColors.DGVErrorBack;
            //        foreColor = this.fFDBDataSet.MyColors.DGVErrorFore;
            //        this.rowErrorText = "This account does not use Envelopes. ";
            //    }

            //    if (this.rowSplitEnvelope)
            //    {
            //        cellReadOnly = true;
            //    }
            //}

            //if (col == this.accountIDColumn.Index && this.rowAccountNull)
            //{
            //    this.rowErrorText = "Select an account.";
            //}

            //this[col, row].ReadOnly = cellReadOnly;
            //this[col, row].ToolTipText = this.rowErrorText;
            //e.CellStyle.BackColor = backColor;
            //e.CellStyle.ForeColor = foreColor;
        }

        private void CDLinesDGV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //int col = e.ColumnIndex;
            //int row = e.RowIndex;

            //if (col < 0 || row < 0)
            //    return;

            //int accountID = Convert.ToInt32(this[this.accountIDColumn.Index, row].Value);

            //// if this is a new row
            //if (accountID == SpclAccount.NULL)
            //{
            //    decimal debitAmount;
            //    decimal creditAmount;
            //    decimal newAmount;

            //    this.fFDBDataSet.myGetTransactionBalance(thisTransactionID, out creditAmount, out debitAmount);

            //    this[this.transactionIDColumn.Index, row].Value = thisTransactionID;
            //    this[this.creditDebitColumn.Index, row].Value = thisCD;
            //    this[this.oppAccountIDColumn.Index, row].Value = SpclAccount.NULL;

            //    newAmount = creditAmount - debitAmount;

            //    if (newAmount > 0.0m)
            //        this[this.amountColumn.Index, row].Value = newAmount;
            //}
        }

        private void CDLinesDGV_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;

            int lineID = Convert.ToInt32(this[lineItemIDColumn.Index, row].Value);
            int typeID = Convert.ToInt32(this[typeIDColumn.Index, row].Value);
            int envelopeID = Convert.ToInt32(this[envelopeIDColumn.Index, row].Value);
            int accountID = Convert.ToInt32(this[this.accountIDColumn.Index, row].Value);
            decimal amount = Convert.ToDecimal(this[amountColumn.Index, row].Value);
            string description = Convert.ToString(this[descriptionColumn.Index, row].Value);
            string confNum = Convert.ToString(this[confermationNumColumn.Index, row].Value);

            // Auto: id, date, typeID, envelopeID, complete, amount
            // Assigned programatiaclly: transactionID, creditDebit, oppAccountID
            // Assigned by User: accountID, description, confermation#

            bool allNull = (
                 typeID == SpclLineType.NULL &&
                 envelopeID == SpclEnvelope.NULL &&
                 accountID == SpclAccount.NULL &&
                 description == "" &&
                 confNum == "");

            bool allRequired = (accountID != SpclAccount.NULL);

            // Set the OppAccount
            this[this.oppAccountIDColumn.Index, row].Value = SpclAccount.NULL;

            if (allNull) // Means the user didn't enter any values. - Cancel the edit -
                this.theDGVBindingSource.CancelEdit();

            else if (!allRequired) // Ask what to do.
            {
                string message;

                message = "You have not entered the required Destination.\n\nDo you want to discard this entry?";

                // If the user said to discard the changes cancel them.
                if (DialogResult.Yes == MessageBox.Show(message, "Discard Entry?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error))
                    this.theDGVBindingSource.CancelEdit();

                else// The user answerd No or Cancel. Cancel leaving the row
                    e.Cancel = true;
            }
            else// Commit the changes to the table
                this.theDGVBindingSource.EndEdit();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void myInit()
        {
            this.SuspendLayout();

            this.buildTheDataGridView();

            this.ResumeLayout();
        }

        private void buildTheDataGridView()
        {
            string filter;

            // theDGVBindingSource
            this.theDGVBindingSource = new BindingSource(this.fFDBDataSet, "LineItem");
            filter = "transactionID = " + thisTransactionID.ToString();
            filter += " AND creditDebit = " + Convert.ToInt32(thisCD).ToString();
            this.theDGVBindingSource.Filter = filter;
            this.theDGVBindingSource.Sort = "id";

            // lineTypeBindingSource
            this.lineTypeBindingSource = new BindingSource(this.fFDBDataSet, "LineType");
            filter = "id = " + SpclLineType.NULL.ToString();
            filter += " OR viewID = " + thisViewID.ToString();
            this.lineTypeBindingSource.Filter = filter;
            this.lineTypeBindingSource.Sort = "viewID, name";

            // accountBindingSource for the accountID Column.
            this.accountBindingSource = new BindingSource(this.fFDBDataSet, "Account");
            filter = "id = " + SpclAccount.NULL.ToString();
            filter += " OR viewID = " + thisViewID.ToString();
            this.accountBindingSource.Filter = filter;
            this.accountBindingSource.Sort = "viewID, name";

            // envelopeBindingSource for the Envelope Column.
            this.envelopeBindingSource = new BindingSource(this.fFDBDataSet, "Envelope");
            //filter = "viewID = " + SpclView.ADMIN.ToString();
            //filter += " OR viewID = " + thisViewID.ToString();
            //this.envelopeBindingSource.Filter = filter;
            this.envelopeBindingSource.Sort = "viewID, fullName";

            // lineItemIDColumn
            this.lineItemIDColumn = new DataGridViewTextBoxColumn();
            this.lineItemIDColumn.Name = "lineItemIDColumn";
            this.lineItemIDColumn.HeaderText = "id";
            this.lineItemIDColumn.DataPropertyName = "id";
            this.lineItemIDColumn.Visible = false;

            // transactionIDColumn
            this.transactionIDColumn = new DataGridViewTextBoxColumn();
            this.transactionIDColumn.Name = "transactionIDColumn";
            this.transactionIDColumn.HeaderText = "transactionID";
            this.transactionIDColumn.DataPropertyName = "transactionID";
            this.transactionIDColumn.Visible = false;

            // dateColumn
            this.dateColumn = new CalendarColumn();
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.HeaderText = "Date";
            this.dateColumn.DataPropertyName = "date";
            this.dateColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dateColumn.Width = 85;

            // typeIDColumn
            this.typeIDColumn = new DataGridViewComboBoxColumn();
            this.typeIDColumn.Name = "typeIDColumn";
            this.typeIDColumn.HeaderText = "Type";
            this.typeIDColumn.DataPropertyName = "typeID";
            this.typeIDColumn.DataSource = this.lineTypeBindingSource;
            this.typeIDColumn.DisplayMember = "name";
            this.typeIDColumn.ValueMember = "id";
            this.typeIDColumn.Width = 80;
            this.typeIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.typeIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.typeIDColumn.Visible = true;

            // accountIDColumn
            this.accountIDColumn = new DataGridViewComboBoxColumn();
            this.accountIDColumn.Name = "accountIDColumn";
            this.accountIDColumn.HeaderText = "Destination";
            this.accountIDColumn.DataPropertyName = "accountID";
            this.accountIDColumn.DataSource = this.accountBindingSource;
            this.accountIDColumn.DisplayMember = "name";
            this.accountIDColumn.ValueMember = "id";
            this.accountIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.accountIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.accountIDColumn.HeaderCell.Style.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            // oppAccountIDColumn
            this.oppAccountIDColumn = new DataGridViewTextBoxColumn();
            this.oppAccountIDColumn.Name = "oppAccountIDColumn";
            this.oppAccountIDColumn.HeaderText = "oppAccountID";
            this.oppAccountIDColumn.DataPropertyName = "oppAccountID";
            this.oppAccountIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.oppAccountIDColumn.Visible = false;

            // descriptionColumn
            this.descriptionColumn = new DataGridViewTextBoxColumn();
            this.descriptionColumn.Name = "descriptionColumn";
            this.descriptionColumn.HeaderText = "Description";
            this.descriptionColumn.DataPropertyName = "description";
            this.descriptionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

            // confermationNumliColumn
            this.confermationNumColumn = new DataGridViewTextBoxColumn();
            this.confermationNumColumn.Name = "confermationNumliColumn";
            this.confermationNumColumn.HeaderText = "Confermation #";
            this.confermationNumColumn.DataPropertyName = "confirmationNumber";
            this.confermationNumColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.confermationNumColumn.Visible = true;

            // envelopeIDColumn
            this.envelopeIDColumn = new DataGridViewComboBoxColumn();
            this.envelopeIDColumn.Name = "envelopeIDColumn";
            this.envelopeIDColumn.HeaderText = "Envelope";
            this.envelopeIDColumn.DataPropertyName = "envelopeID";
            this.envelopeIDColumn.DataSource = this.envelopeBindingSource;
            this.envelopeIDColumn.DisplayMember = "fullName";
            this.envelopeIDColumn.ValueMember = "id";
            this.envelopeIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envelopeIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.envelopeIDColumn.Visible = true;

            // amountColumn
            this.amountColumn = new DataGridViewTextBoxColumn();
            this.amountColumn.Name = "amountColumn";
            this.amountColumn.HeaderText = "Amount";
            this.amountColumn.DataPropertyName = "amount";
            this.amountColumn.Width = 65;
            //this.amountColumn.DefaultCellStyle = new CellStyles().Money;
            this.amountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.amountColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.amountColumn.Visible = true;

            // completeColumn
            this.completeColumn = new DataGridViewTextBoxColumn();
            this.completeColumn.Name = "completeColumn";
            this.completeColumn.HeaderText = "CR";
            this.completeColumn.DataPropertyName = "complete";
            this.completeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.completeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            this.completeColumn.Width = 25;
            this.completeColumn.ReadOnly = true;

            // creditDebitColumn
            this.creditDebitColumn = new DataGridViewCheckBoxColumn();
            this.creditDebitColumn.Name = "creditDebitColumn";
            this.creditDebitColumn.HeaderText = "creditDebitColumn";
            this.creditDebitColumn.DataPropertyName = "creditDebit";
            this.creditDebitColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.creditDebitColumn.Width = 20;
            this.creditDebitColumn.Visible = false;
            this.creditDebitColumn.ReadOnly = true;

            // theDataGridView
            this.Name = "theDataGridView";
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.AutoGenerateColumns = false;
            this.AllowUserToAddRows = false;  // By default.
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.AllowUserToOrderColumns = false;
            this.RowHeadersVisible = false;
            this.ScrollBars = ScrollBars.Vertical;
            this.MultiSelect = false;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.DataSource = this.theDGVBindingSource;
            this.Columns.AddRange(
                new DataGridViewColumn[] 
                {
                    this.dateColumn,
                    this.lineItemIDColumn,
                    this.transactionIDColumn,
                    this.typeIDColumn,
                    this.accountIDColumn,
                    this.oppAccountIDColumn,
                    this.descriptionColumn,
                    this.confermationNumColumn,
                    this.envelopeIDColumn,
                    this.amountColumn,
                    this.completeColumn,
                    this.creditDebitColumn
                }
                );



            if (thisCD == LineCD.CREDIT)
            {
                this.accountIDColumn.HeaderText = "Source";

                this.AllowUserToResizeColumns = true;
                this.completeColumn.Resizable = DataGridViewTriState.True;
                this.amountColumn.Resizable = DataGridViewTriState.True;
                this.envelopeIDColumn.Resizable = DataGridViewTriState.True;
                this.confermationNumColumn.Resizable = DataGridViewTriState.True;
                this.descriptionColumn.Resizable = DataGridViewTriState.True;
                this.accountIDColumn.Resizable = DataGridViewTriState.True;
                this.typeIDColumn.Resizable = DataGridViewTriState.True;
                this.dateColumn.Resizable = DataGridViewTriState.True;


                this.accountIDColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.accountIDColumn.FillWeight = 120;
                this.descriptionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.descriptionColumn.FillWeight = 200;
                this.confermationNumColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.confermationNumColumn.FillWeight = 100;
                this.envelopeIDColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.envelopeIDColumn.FillWeight = 100;
            }
            else
            {
                this.accountIDColumn.HeaderText = "Destination";

                this.AllowUserToResizeColumns = false;
                this.completeColumn.Resizable = DataGridViewTriState.False;
                this.amountColumn.Resizable = DataGridViewTriState.False;
                this.envelopeIDColumn.Resizable = DataGridViewTriState.False;
                this.confermationNumColumn.Resizable = DataGridViewTriState.False;
                this.descriptionColumn.Resizable = DataGridViewTriState.False;
                this.accountIDColumn.Resizable = DataGridViewTriState.False;
                this.typeIDColumn.Resizable = DataGridViewTriState.False;
                this.dateColumn.Resizable = DataGridViewTriState.False;
            }


            // theDataGridView EVENTS declarations
            if (!ReadOnly)
            {
                //this.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Lavender;
                this.AllowUserToAddRows = true;
                this.CellDoubleClick += new DataGridViewCellEventHandler(CDLinesDGV_CellDoubleClick);
                this.RowValidating += new DataGridViewCellCancelEventHandler(CDLinesDGV_RowValidating);
                this.RowEnter += new DataGridViewCellEventHandler(CDLinesDGV_RowEnter);
            }

            this.CellFormatting += new DataGridViewCellFormattingEventHandler(CDLinesDGV_CellFormatting);
            this.RowPrePaint += new DataGridViewRowPrePaintEventHandler(CDLinesDGV_RowPrePaint);
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public CDLinesDGV(ref FFDBDataSet dataSet, int viewID, int transID, bool creditDebit)
        {
            fFDBDataSet = dataSet;
            thisViewID = viewID;
            thisTransactionID = transID;
            thisCD = creditDebit;

            myInit();
        }

        public void myEndEdit()
        {
            this.theDGVBindingSource.EndEdit();
        }

        public void myDeleteCurrent()
        {
            this.theDGVBindingSource.RemoveCurrent();
        }

        public void myShowConfermationColumn(bool value)
        {
            this.confermationNumColumn.Visible = value;
        }

        public void myShowTypeColumn(bool value)
        {
            this.typeIDColumn.Visible = value;
        }

    }
}
