using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Transaction
{
    class CDLinesDGV : DataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////

        public BindingSource BindingSourceLineItemDGV;
        public BindingSource BindingSourceTypeIDCol;
        public BindingSource BindingSourceAccountIDCol;
        public BindingSource BindingSourceEnvelopeIDCol;

        private DataGridViewTextBoxColumn lineItemIDColumn;
        private DataGridViewTextBoxColumn transactionIDColumn;
        private CalendarColumn dateColumn;
        private DataGridViewComboBoxColumn typeIDColumn;
        private DataGridViewComboBoxColumn accountIDColumn;
        private DataGridViewTextBoxColumn descriptionColumn;
        private DataGridViewTextBoxColumn confirmationNumColumn;
        private DataGridViewComboBoxColumn envelopeIDColumn;
        private DataGridViewTextBoxColumn completeColumn;
        private DataGridViewTextBoxColumn creditDebitColumn;
        private DataGridViewTextBoxColumn amountColumn;

        // row flags used in painting cells
        public bool flagLineError;
        public bool flagAccountError;

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Properties
        ////////////////////////////////////////////////////////////////////////////////////////////
        public bool ShowTypeColumn
        {
            get { return this.Columns["typeIDColumn"].Visible; }
            set { this.Columns["typeIDColumn"].Visible = value; }
        }

        public bool ShowConfirmationColumn
        {
            get { return this.Columns["confirmationNumColumn"].Visible; }
            set { this.Columns["confirmationNumColumn"].Visible = value; }
        }

        public int CurrentLineID
        {
            get
            {
                int lineID = -1;

                try
                {
                    lineID = Convert.ToInt32(this.CurrentRow.Cells["lineItemIDColumn"].Value);
                }
                catch
                {
                }

                return lineID;
            }
        }

        public int CurrentTransactionID
        {
            get
            {
                int transID = -1;

                try
                {
                    transID = Convert.ToInt32(this.CurrentRow.Cells["transactionIDColumn"].Value);
                }
                catch
                {
                }

                return transID;
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void MyDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;

            if (col < 0 || row < 0)
                return;

            string colName = this.Columns[col].Name;
            string toolTipText = this[col, row].ToolTipText;

            // Set the back ground and the tool tip.
            if (this.flagLineError && (colName == "amountColumn"))
            {
                e.CellStyle.BackColor = System.Drawing.Color.Red;
                toolTipText = "This line amount and its envelope sum need to match.";
            }
            else if (this.flagAccountError && colName == "accountIDColumn")
            {
                e.CellStyle.BackColor = System.Drawing.Color.Red;
                toolTipText = "Please choose an account.";
            }

            this[col, row].ToolTipText = toolTipText;
        }

        private void LineItemDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            string cellValue = this[col, row].Value.ToString();

            if (col == completeColumn.Index && row >= 0)
            {
                if (cellValue == LineState.PENDING)
                    this[col, row].Value = LineState.CLEARED;

                else if (cellValue == LineState.CLEARED)
                    this[col, row].Value = LineState.RECONSILED;

                else
                    this[col, row].Value = LineState.PENDING;
            }
        }

        private void MyDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string temp = "stop";
            temp = temp + temp;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildTheDataGridView(bool thisCD)
        {

            // lineItemIDColumn
            this.lineItemIDColumn = new DataGridViewTextBoxColumn();
            this.lineItemIDColumn.Name = "lineItemIDColumn";
            this.lineItemIDColumn.HeaderText = "lineItemID";
            this.lineItemIDColumn.DataPropertyName = "id";
            this.lineItemIDColumn.Width = 40;
            this.lineItemIDColumn.Visible = false;

            // transactionIDColumn
            this.transactionIDColumn = new DataGridViewTextBoxColumn();
            this.transactionIDColumn.Name = "transactionIDColumn";
            this.transactionIDColumn.HeaderText = "transactionID";
            this.transactionIDColumn.DataPropertyName = "transactionID";
            this.transactionIDColumn.Width = 40;
            this.transactionIDColumn.Visible = false;

            // dateColumn
            this.dateColumn = new CalendarColumn();
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.HeaderText = "Date";
            this.dateColumn.DataPropertyName = "date";
            this.dateColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dateColumn.Resizable = DataGridViewTriState.True;
            this.dateColumn.Width = 85;
            this.dateColumn.Visible = true;

            // typeIDColumn
            this.typeIDColumn = new DataGridViewComboBoxColumn();
            this.typeIDColumn.Name = "typeIDColumn";
            this.typeIDColumn.HeaderText = "Type";
            this.typeIDColumn.DataSource = this.BindingSourceTypeIDCol;
            this.typeIDColumn.DataPropertyName = "typeID";
            this.typeIDColumn.DisplayMember = "name";
            this.typeIDColumn.ValueMember = "id";
            this.typeIDColumn.AutoComplete = true;
            this.typeIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.typeIDColumn.Resizable = DataGridViewTriState.True;
            this.typeIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.typeIDColumn.Width = 80;
            this.typeIDColumn.Visible = true;

            // accountIDColumn
            this.accountIDColumn = new DataGridViewComboBoxColumn();
            this.accountIDColumn.Name = "accountIDColumn";
            this.accountIDColumn.HeaderText = "Source / Destination";
            this.accountIDColumn.DataSource = this.BindingSourceAccountIDCol;
            this.accountIDColumn.DataPropertyName = "accountID";
            this.accountIDColumn.DisplayMember = "name";
            this.accountIDColumn.ValueMember = "id";
            this.accountIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.accountIDColumn.Resizable = DataGridViewTriState.True;
            this.accountIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.accountIDColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.accountIDColumn.FillWeight = 120;
            this.accountIDColumn.Visible = true;

            // descriptionColumn
            this.descriptionColumn = new DataGridViewTextBoxColumn();
            this.descriptionColumn.Name = "descriptionColumn";
            this.descriptionColumn.HeaderText = "Description";
            this.descriptionColumn.DataPropertyName = "description";
            this.descriptionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.descriptionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionColumn.FillWeight = 200;
            this.descriptionColumn.Visible = true;

            // confirmationNumColumn
            this.confirmationNumColumn = new DataGridViewTextBoxColumn();
            this.confirmationNumColumn.Name = "confirmationNumColumn";
            this.confirmationNumColumn.HeaderText = "Confirmation #";
            this.confirmationNumColumn.DataPropertyName = "confirmationNumber";
            this.confirmationNumColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.confirmationNumColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.confirmationNumColumn.FillWeight = 100;
            this.confirmationNumColumn.Visible = true;

            // envelopeIDColumn
            this.envelopeIDColumn = new DataGridViewComboBoxColumn();
            this.envelopeIDColumn.Name = "envelopeIDColumn";
            this.envelopeIDColumn.HeaderText = "Envelope";
            this.envelopeIDColumn.DataSource = this.BindingSourceEnvelopeIDCol;
            this.envelopeIDColumn.DataPropertyName = "envelopeID";
            this.envelopeIDColumn.DisplayMember = "name";
            this.envelopeIDColumn.ValueMember = "id";
            this.envelopeIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envelopeIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.envelopeIDColumn.Resizable = DataGridViewTriState.True;
            this.envelopeIDColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.envelopeIDColumn.FillWeight = 100;
            this.envelopeIDColumn.Visible = false;
            this.envelopeIDColumn.ReadOnly = true;
            
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
            this.creditDebitColumn = new DataGridViewTextBoxColumn();
            this.creditDebitColumn.Name = "creditDebitColumn";
            this.creditDebitColumn.HeaderText = "CD";
            this.creditDebitColumn.DataPropertyName = "creditDebit";
            this.creditDebitColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.creditDebitColumn.Width = 20;
            this.creditDebitColumn.Visible = false;

            // amountColumn
            this.amountColumn = new DataGridViewTextBoxColumn();
            this.amountColumn.Name = "amountColumn";
            this.amountColumn.HeaderText = "Amount";
            this.amountColumn.DataPropertyName = "amount";
            this.amountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.amountColumn.DefaultCellStyle = new MyCellStyleMoney();
            this.amountColumn.Width = 65;

            // This Data Grid View
            this.Name = "theDataGridView";
            this.DataSource = this.BindingSourceLineItemDGV;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.Dock = DockStyle.Fill;
            this.AutoGenerateColumns = false;
            this.AllowUserToOrderColumns = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.AllowUserToAddRows = true;
            this.RowHeadersVisible = false;
            this.ShowCellErrors = false;
            this.ShowRowErrors = false;
            this.MultiSelect = false;

            this.Columns.AddRange(
                new DataGridViewColumn[] 
                {
                    this.dateColumn,
                    this.lineItemIDColumn,
                    this.transactionIDColumn,
                    this.typeIDColumn,
                    this.accountIDColumn,
                    this.descriptionColumn,
                    this.confirmationNumColumn,
                    this.envelopeIDColumn,
                    this.completeColumn,
                    this.creditDebitColumn,
                    this.amountColumn
                }
                );

            if (thisCD)
                this.accountIDColumn.HeaderText = "Destination Account";
            else
                this.accountIDColumn.HeaderText = "Source Account";
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public CDLinesDGV(bool creditDebit)
        {
            ///////////////////////////////////////////////////////////////////
            // Initial Flag values
            this.flagLineError = false;
            this.flagAccountError = false;

            ///////////////////////////////////////////////////////////////////
            // Build theDataGridView


            // Initialize the Binding Sources
            this.BindingSourceLineItemDGV = new BindingSource();
            this.BindingSourceAccountIDCol = new BindingSource();
            this.BindingSourceEnvelopeIDCol = new BindingSource();
            this.BindingSourceTypeIDCol = new BindingSource();

            this.buildTheDataGridView(creditDebit);

            ////////////////////////////////////
            // Subscribe to events.
            this.CellDoubleClick += new DataGridViewCellEventHandler(LineItemDGV_CellDoubleClick);
            this.CellFormatting += new DataGridViewCellFormattingEventHandler(MyDataGridView_CellFormatting);
            this.DataError += new DataGridViewDataErrorEventHandler(MyDataGridView_DataError);
        }

        public void myHighLightOn()
        {
            if (this.CurrentCell != null)
            {
                this.CurrentCell.Style.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                this.CurrentCell.Style.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            }

            if (this.DefaultCellStyle != null)
            {
                this.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                this.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            }
        }

        public void myHighLightOff()
        {
            if (this.CurrentCell != null)
            {
                this.CurrentCell.Style.SelectionBackColor = System.Drawing.SystemColors.Window;
                this.CurrentCell.Style.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            }

            if (this.DefaultCellStyle != null)
            {
                this.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Window;
                this.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            }
        }

    }
}
