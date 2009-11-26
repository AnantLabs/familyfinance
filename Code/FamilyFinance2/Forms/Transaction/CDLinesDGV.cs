using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Transaction
{
    class CDLinesDGV : MyDataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////

        public BindingSource BindingSourceLineItemDGV;
        public BindingSource BindingSourceLineTypeIDCol;
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

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
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
            this.lineItemIDColumn.Visible = true;

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
            this.typeIDColumn.DataSource = this.BindingSourceLineTypeIDCol;
            this.typeIDColumn.DataPropertyName = "lineTypeID";
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
            this.envelopeIDColumn.DisplayMember = "fullName";
            this.envelopeIDColumn.ValueMember = "id";
            this.envelopeIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envelopeIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.envelopeIDColumn.Resizable = DataGridViewTriState.True;
            this.envelopeIDColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.envelopeIDColumn.FillWeight = 100;
            this.envelopeIDColumn.Visible = true;
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
            this.AllowUserToAddRows = true;
            this.DataSource = this.BindingSourceLineItemDGV;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
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
            // Initialize the Binding Sources
            this.BindingSourceLineItemDGV = new BindingSource();
            this.BindingSourceAccountIDCol = new BindingSource();
            this.BindingSourceEnvelopeIDCol = new BindingSource();
            this.BindingSourceLineTypeIDCol = new BindingSource();

            this.buildTheDataGridView(creditDebit);

            ////////////////////////////////////
            // Subscribe to events.
            this.CellDoubleClick += new DataGridViewCellEventHandler(LineItemDGV_CellDoubleClick);
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
