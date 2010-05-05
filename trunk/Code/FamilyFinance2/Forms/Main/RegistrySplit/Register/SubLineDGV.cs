using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using FamilyFinance2.Forms.Transaction;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main.RegistrySplit.Register
{
    class SubLineDGV : MyDataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private SubLineDataSet slDataSet;
        private BindingSource subLineDGVBindingSource;

        int currentAccountID;
        int currentEnvelopeID;

        // Columns
        private DataGridViewTextBoxColumn subLineItemIDColumn;
        private DataGridViewTextBoxColumn transactionIDColumn;
        private DataGridViewTextBoxColumn dateColumn;
        private DataGridViewTextBoxColumn lineTypeColumn;
        private DataGridViewTextBoxColumn sourceColumn;
        private DataGridViewTextBoxColumn destinationColumn;
        private DataGridViewTextBoxColumn descriptionColumn;
        private DataGridViewTextBoxColumn subDescriptionColumn;
        private DataGridViewTextBoxColumn debitAmountColumn;
        private DataGridViewTextBoxColumn completeColumn;
        private DataGridViewTextBoxColumn creditAmountColumn;
        private DataGridViewTextBoxColumn balanceAmountColumn;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Properties
        ////////////////////////////////////////////////////////////////////////////////////////////



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void SubLineDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            if (row >= 0)
            {
                int transID = this.CurrentTransactionID;

                TransactionForm tf = new TransactionForm(transID);
                tf.ShowDialog();
                this.myReloadSubLineView();
            }

        }

        protected override void MyDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int subLineID = Convert.ToInt32(this[subLineItemIDColumn.Index, e.RowIndex].Value);
            SubLineDataSet.SubLineViewRow thisSubLine = this.slDataSet.SubLineView.FindByeLineID(subLineID);

            // Defaults. Used for new lines.
            this.flagTransactionError = false;
            this.flagLineError = false;
            this.flagNegativeBalance = false;
            this.flagFutureDate = false;
            this.flagAccountError = false;

            if (thisSubLine != null)
            {
                // Set row Flags
                //flagTransactionError = thisSubLine.tr;

                if (thisSubLine.amount < 0.0m)
                    this.flagNegativeBalance = true;

                if (thisSubLine.date > DateTime.Today) // future Date
                    this.flagFutureDate = true;
            }
        }

        
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildTheDataGridView()
        {
            // lineItemIDColumn
            this.subLineItemIDColumn = new DataGridViewTextBoxColumn();
            this.subLineItemIDColumn.Name = "eLineIDColumn";
            this.subLineItemIDColumn.HeaderText = "subLineItemID";
            this.subLineItemIDColumn.DataPropertyName = "subLineItemID";
            this.subLineItemIDColumn.Visible = false;

            // transactionIDColumn
            this.transactionIDColumn = new DataGridViewTextBoxColumn();
            this.transactionIDColumn.Name = "transactionIDColumn";
            this.transactionIDColumn.HeaderText = "transactionID";
            this.transactionIDColumn.DataPropertyName = "transactionID";
            this.transactionIDColumn.Visible = false;

            // dateColumn
            this.dateColumn = new DataGridViewTextBoxColumn();
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.HeaderText = "Date";
            this.dateColumn.DataPropertyName = "date";
            this.dateColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dateColumn.Visible = true;
            this.dateColumn.Width = 85;

            // lineTypeslColumn
            this.lineTypeColumn = new DataGridViewTextBoxColumn();
            this.lineTypeColumn.Name = "lineTypeColumn";
            this.lineTypeColumn.HeaderText = "Type";
            this.lineTypeColumn.DataPropertyName = "lineType";
            this.lineTypeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.lineTypeColumn.Width = 80;
            this.lineTypeColumn.Visible = true;

            // sourceColumn
            this.sourceColumn = new DataGridViewTextBoxColumn();
            this.sourceColumn.Name = "sourceColumn";
            this.sourceColumn.HeaderText = "Source";
            this.sourceColumn.DataPropertyName = "sourceAccount";
            this.sourceColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.sourceColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.sourceColumn.FillWeight = 30;
            this.sourceColumn.Visible = true;

            // destinationColumn
            this.destinationColumn = new DataGridViewTextBoxColumn();
            this.destinationColumn.Name = "destinationColumn";
            this.destinationColumn.HeaderText = "Destination";
            this.destinationColumn.DataPropertyName = "destinationAccount";
            this.destinationColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.destinationColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.destinationColumn.FillWeight = 30;
            this.destinationColumn.Visible = true;

            // descriptionColumn
            this.descriptionColumn = new DataGridViewTextBoxColumn();
            this.descriptionColumn.Name = "descriptionColumn";
            this.descriptionColumn.HeaderText = "Description";
            this.descriptionColumn.DataPropertyName = "description";
            this.descriptionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.descriptionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionColumn.FillWeight = 50;
            this.descriptionColumn.Visible = true;

            // subDescriptionColumn
            this.subDescriptionColumn = new DataGridViewTextBoxColumn();
            this.subDescriptionColumn.Name = "subDescriptionColumn";
            this.subDescriptionColumn.HeaderText = "Sub Description";
            this.subDescriptionColumn.DataPropertyName = "subDescription";
            this.subDescriptionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.subDescriptionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.subDescriptionColumn.FillWeight = 50;
            this.subDescriptionColumn.Visible = true;

            // creditAmountColumn
            this.creditAmountColumn = new DataGridViewTextBoxColumn();
            this.creditAmountColumn.Name = "creditAmountColumn";
            this.creditAmountColumn.HeaderText = "Credit";
            this.creditAmountColumn.DataPropertyName = "creditAmount";
            this.creditAmountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.creditAmountColumn.DefaultCellStyle = new MyCellStyleMoney();
            this.creditAmountColumn.Visible = true;
            this.creditAmountColumn.Width = 65;

            // completeColumn
            this.completeColumn = new DataGridViewTextBoxColumn();
            this.completeColumn.Name = "completeColumn";
            this.completeColumn.HeaderText = "CR";
            this.completeColumn.DataPropertyName = "complete";
            this.completeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.completeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            this.completeColumn.Visible = true;
            this.completeColumn.Width = 25;

            // debitAmountColumn
            this.debitAmountColumn = new DataGridViewTextBoxColumn();
            this.debitAmountColumn.Name = "debitAmountColumn";
            this.debitAmountColumn.HeaderText = "Debit";
            this.debitAmountColumn.DataPropertyName = "debitAmount";
            this.debitAmountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.debitAmountColumn.DefaultCellStyle = new MyCellStyleMoney();
            this.debitAmountColumn.Visible = true;
            this.debitAmountColumn.Width = 65;

            // balanceAmountColumn
            this.balanceAmountColumn = new DataGridViewTextBoxColumn();
            this.balanceAmountColumn.Name = "balanceAmountColumn";
            this.balanceAmountColumn.HeaderText = "Balance";
            this.balanceAmountColumn.DataPropertyName = "balanceAmount";
            this.balanceAmountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.balanceAmountColumn.DefaultCellStyle = new MyCellStyleMoney();
            this.balanceAmountColumn.Visible = true;
            this.balanceAmountColumn.Width = 75;

            // theDataGridView
            this.AllowUserToAddRows = false;
            this.ReadOnly = true;
            this.DataSource = this.subLineDGVBindingSource;
            this.Columns.AddRange(
                new DataGridViewColumn[] 
                {
                    this.dateColumn,
                    this.subLineItemIDColumn,
                    this.transactionIDColumn,
                    this.lineTypeColumn,
                    this.sourceColumn,
                    this.destinationColumn,
                    this.descriptionColumn,
                    this.subDescriptionColumn,
                    this.creditAmountColumn,
                    this.completeColumn,
                    this.debitAmountColumn,
                    this.balanceAmountColumn
                }
                );
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public SubLineDGV()
        {
            // Setup the Data Set
            this.slDataSet = new SubLineDataSet();
            this.slDataSet.myInit();

            ////////////////////////////////////
            // Setup the Bindings
            this.subLineDGVBindingSource = new BindingSource(this.slDataSet, "SubLineView");

            this.buildTheDataGridView();
            this.currentAccountID = SpclAccount.NULL;
            this.currentEnvelopeID = SpclEnvelope.NULL;

            ////////////////////////////////////
            // Subscribe to event.
            this.CellDoubleClick += new DataGridViewCellEventHandler(SubLineDGV_CellDoubleClick);
            //this.RowPrePaint += new DataGridViewRowPrePaintEventHandler(SubLineDGV_RowPrePaint);
        }

        public void setAccountEnvelopeID(int accountID, int envelopeID)
        {
            const int NULL = -1;

            if (envelopeID >= NULL && accountID >= NULL)
            {
                this.currentEnvelopeID = envelopeID;
                this.currentAccountID = accountID;
                this.slDataSet.myFill(accountID, envelopeID);    
            }

        }

        public void myReloadSubLineView()
        {
            this.setAccountEnvelopeID(this.currentAccountID, this.currentEnvelopeID);
        }

    }
}
