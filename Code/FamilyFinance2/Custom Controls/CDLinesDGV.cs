using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2
{
    class CDLinesDGV : MyDataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private readonly bool thisCreditDebit;

        // Binding Sources
        public BindingSource lineItemDGVBindingSource;
        public BindingSource lineTypeColBindingSource;
        public BindingSource accountColBindingSource;
        public BindingSource envelopeColBindingSource;

        // Columns
        private DataGridViewTextBoxColumn lineItemIDColumn;
        private DataGridViewTextBoxColumn transactionIDColumn;
        private CalendarColumn dateColumn;
        private DataGridViewComboBoxColumn typeIDColumn;
        private DataGridViewComboBoxColumn accountIDColumn;
        private DataGridViewTextBoxColumn descriptionColumn;
        private DataGridViewTextBoxColumn confermationNumColumn;
        private DataGridViewComboBoxColumn envelopeIDColumn;
        private DataGridViewTextBoxColumn completeColumn;
        private DataGridViewTextBoxColumn amountColumn;



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Properties
        ////////////////////////////////////////////////////////////////////////////////////////////


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

        private void LineItemDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int lineID = Convert.ToInt32(this[lineItemIDColumn.Index, e.RowIndex].Value);
            FFDBDataSet.LineItemRow thisLine = this.fFDBDataSet.LineItem.FindByid(lineID);

            // Defaults. Used for new lines.
            this.flagTransactionError = false;
            this.flagLineError = false;
            this.flagNegativeBalance = false;
            this.flagReadOnlyEnvelope = false;
            this.flagReadOnlyAccounts = false;
            this.flagFutureDate = false;

            // Set Flags
            if (thisLine != null)
            {
                bool thisLineUsesEnvelopes = thisLine.AccountRowByFK_Line_accountID.envelopes;

                this.flagLineError = thisLine.lineError;

                if (thisLine.envelopeID == SpclEnvelope.SPLIT || !thisLineUsesEnvelopes)
                    this.flagReadOnlyEnvelope = true;

                if (thisLine.date > DateTime.Today) // future Date
                    this.flagFutureDate = true;
            }
        }

        void CDLinesDGV_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            for (int row = 0; row < this.Rows.Count; row++)
            {
                if (this[lineItemIDColumn.Index, row].Value == DBNull.Value)
                    this[lineItemIDColumn.Index, row].Value = this.currentLineID;
            }
        }

        
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildTheDataGridView()
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
            this.transactionIDColumn.Visible = true;

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
            this.typeIDColumn.DataPropertyName = "lineTypeID";
            this.typeIDColumn.DataSource = this.lineTypeColBindingSource;
            this.typeIDColumn.DisplayMember = "name";
            this.typeIDColumn.ValueMember = "id";
            this.typeIDColumn.AutoComplete = true;
            this.typeIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.typeIDColumn.Resizable = DataGridViewTriState.True;
            this.typeIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.typeIDColumn.Width = 80;
            this.typeIDColumn.Visible = true;

            // oppAccountIDColumn
            this.accountIDColumn = new DataGridViewComboBoxColumn();
            this.accountIDColumn.Name = "accountIDColumn";
            this.accountIDColumn.HeaderText = "Source / Destination";
            this.accountIDColumn.DataPropertyName = "accountID";
            this.accountIDColumn.DataSource = this.accountColBindingSource;
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

            // confermationNumColumn
            this.confermationNumColumn = new DataGridViewTextBoxColumn();
            this.confermationNumColumn.Name = "confermationNumColumn";
            this.confermationNumColumn.HeaderText = "Confermation #";
            this.confermationNumColumn.DataPropertyName = "confirmationNumber";
            this.confermationNumColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.confermationNumColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.confermationNumColumn.FillWeight = 100;
            this.confermationNumColumn.Visible = true;

            // envelopeIDColumn
            this.envelopeIDColumn = new DataGridViewComboBoxColumn();
            this.envelopeIDColumn.Name = "envelopeIDColumn";
            this.envelopeIDColumn.HeaderText = "Envelope";
            this.envelopeIDColumn.DataPropertyName = "envelopeID";
            this.envelopeIDColumn.DataSource = this.envelopeColBindingSource;
            this.envelopeIDColumn.DisplayMember = "fullName";
            this.envelopeIDColumn.ValueMember = "id";
            this.envelopeIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envelopeIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.envelopeIDColumn.Resizable = DataGridViewTriState.True;
            this.envelopeIDColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.envelopeIDColumn.FillWeight = 100;
            this.envelopeIDColumn.Visible = true;
            
            // completeColumn
            this.completeColumn = new DataGridViewTextBoxColumn();
            this.completeColumn.Name = "completeColumn";
            this.completeColumn.HeaderText = "CR";
            this.completeColumn.DataPropertyName = "complete";
            this.completeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.completeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            this.completeColumn.Width = 25;
            this.completeColumn.ReadOnly = true;

            // debitAmountColumn
            this.amountColumn = new DataGridViewTextBoxColumn();
            this.amountColumn.Name = "amountColumn";
            this.amountColumn.HeaderText = "Amount";
            this.amountColumn.DataPropertyName = "amount";
            this.amountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.amountColumn.DefaultCellStyle = this.CellStyleMoney;
            this.amountColumn.Width = 65;

            // theDataGridView
            this.DataSource = this.lineItemDGVBindingSource;
            this.Columns.AddRange(
                new DataGridViewColumn[] 
                {
                    this.dateColumn,
                    this.lineItemIDColumn,
                    this.transactionIDColumn,
                    this.typeIDColumn,
                    this.accountIDColumn,
                    this.descriptionColumn,
                    this.confermationNumColumn,
                    this.envelopeIDColumn,
                    this.completeColumn,
                    this.amountColumn
                }
                );

            if(this.thisCreditDebit)
                this.accountIDColumn.HeaderText = "Destination Account";
            else
                this.accountIDColumn.HeaderText = "Source Account";

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public CDLinesDGV(bool creditDebit, ref FFDBDataSet ffDataSet)
        {
            this.fFDBDataSet = ffDataSet;
            this.thisCreditDebit = creditDebit;

            ////////////////////////////////////
            // Setup the Bindings
            this.lineItemDGVBindingSource = new BindingSource(this.fFDBDataSet, "LineItem");
            this.lineItemDGVBindingSource.Filter = "creditDebit = " + Convert.ToInt16(creditDebit).ToString();

            this.accountColBindingSource = new BindingSource(this.fFDBDataSet, "Account");
            this.accountColBindingSource.Sort = "catagoryID, name";
            this.accountColBindingSource.Filter = "id <> " + SpclAccount.MULTIPLE;

            this.envelopeColBindingSource = new BindingSource(this.fFDBDataSet, "Envelope");

            this.lineTypeColBindingSource = new BindingSource(this.fFDBDataSet, "LineType");
            this.lineTypeColBindingSource.Sort = "name";

            this.buildTheDataGridView();

            ////////////////////////////////////
            // Subscribe to event.
            this.CellDoubleClick += new DataGridViewCellEventHandler(LineItemDGV_CellDoubleClick);
            this.RowPrePaint += new DataGridViewRowPrePaintEventHandler(LineItemDGV_RowPrePaint);
            this.UserAddedRow += new DataGridViewRowEventHandler(CDLinesDGV_UserAddedRow);
        }



    }
}
