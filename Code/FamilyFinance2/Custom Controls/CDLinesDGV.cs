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
        private readonly int thisTransactionID;

        // Binding Sources
        private BindingSource lineItemDGVBindingSource;
        private BindingSource lineTypeColBindingSource;
        private BindingSource accountColBindingSource;
        private BindingSource envelopeColBindingSource;

        // Columns
        public DataGridViewTextBoxColumn lineItemIDColumn;
        public DataGridViewTextBoxColumn transactionIDColumn;
        public CalendarColumn dateColumn;
        public DataGridViewComboBoxColumn typeIDColumn;
        public DataGridViewComboBoxColumn accountIDColumn;
        public DataGridViewTextBoxColumn descriptionColumn;
        public DataGridViewTextBoxColumn confermationNumColumn;
        public DataGridViewComboBoxColumn envelopeIDColumn;
        public DataGridViewTextBoxColumn completeColumn;
        public DataGridViewTextBoxColumn creditDebitColumn;
        public DataGridViewTextBoxColumn amountColumn;



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
            this.flagAccountError = false;
            this.flagNegativeBalance = false;
            this.flagReadOnlyEnvelope = false;
            this.flagReadOnlyAccounts = false;
            this.flagFutureDate = false;

            // Set Flags
            if (thisLine != null)
            {
                bool thisLineUsesEnvelopes = thisLine.AccountRowByFK_Line_accountID.envelopes;

                this.flagLineError = thisLine.lineError;

                if (thisLine.accountID == SpclAccount.NULL)
                    this.flagAccountError = true;

                if (thisLine.envelopeID == SpclEnvelope.SPLIT || !thisLineUsesEnvelopes)
                    this.flagReadOnlyEnvelope = true;

                if (thisLine.date > DateTime.Today) // future Date
                    this.flagFutureDate = true;
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

            // accountIDColumn
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
            this.amountColumn.DefaultCellStyle = this.MyCellStyleMoney;
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
                    this.creditDebitColumn,
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
        public CDLinesDGV(int transID, bool creditDebit, ref FFDBDataSet ffDataSet)
        {
            this.fFDBDataSet = ffDataSet;
            this.thisCreditDebit = creditDebit;
            this.thisTransactionID = transID;
            this.AllowUserToAddRows = false;

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
            // Subscribe to events.
            this.CellDoubleClick += new DataGridViewCellEventHandler(LineItemDGV_CellDoubleClick);
            this.RowPrePaint += new DataGridViewRowPrePaintEventHandler(LineItemDGV_RowPrePaint);
        }

        public void myHighlightOn()
        {
            if (this.CurrentCell != null)
            {
                this.CurrentCell.Style.SelectionBackColor = this.MyCellStyleNormal.SelectionBackColor;
                this.CurrentCell.Style.SelectionForeColor = this.MyCellStyleNormal.SelectionForeColor;
            }

            if (this.DefaultCellStyle != null)
            {
                this.DefaultCellStyle.SelectionBackColor = this.MyCellStyleNormal.SelectionBackColor;
                this.DefaultCellStyle.SelectionForeColor = this.MyCellStyleNormal.SelectionForeColor;
            }
        }

        public void myHighlightOff()
        {
            if (this.CurrentCell != null)
            {
                this.CurrentCell.Style.SelectionBackColor = this.MyCellStyleNormal.BackColor;
                this.CurrentCell.Style.SelectionForeColor = this.MyCellStyleNormal.ForeColor;
            }

            if (this.DefaultCellStyle != null)
            {
                this.DefaultCellStyle.SelectionBackColor = this.MyCellStyleNormal.BackColor;
                this.DefaultCellStyle.SelectionForeColor = this.MyCellStyleNormal.ForeColor;
            }
        }

    }
}
