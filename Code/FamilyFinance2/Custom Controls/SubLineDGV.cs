using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2
{
    class SubLineDGV : DataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        public const string SORT_SUBin = "date, creditDebit DESC, subLineID";
        public const string SORT_SUBdb = "date, creditDebit DESC, SubLineItem.id";

        private FFDBDataSet fFDBDataSet;

        // row valuse used in painting cells
        private bool rowLineError;
        private bool rowNegativeBalance;
        private string rowErrorText;

        // Binding Sources
        private BindingSource theDGVBindingSource;

        // Columns
        private DataGridViewTextBoxColumn subLineIDColumn;
        private DataGridViewTextBoxColumn lineItemIDColumn;
        private DataGridViewTextBoxColumn transactionIDColumn;
        private DataGridViewTextBoxColumn envelopeIDColumn;
        private DataGridViewTextBoxColumn dateColumn;
        private DataGridViewTextBoxColumn lineTypeColumn;
        private DataGridViewTextBoxColumn accountIDColumn;
        private DataGridViewTextBoxColumn descriptionColumn;
        private DataGridViewTextBoxColumn completeColumn;
        private DataGridViewTextBoxColumn debitAmountColumn;
        private DataGridViewTextBoxColumn creditAmountColumn;
        private DataGridViewTextBoxColumn balanceAmountColumn;
        private DataGridViewTextBoxColumn sourceColumn;
        private DataGridViewTextBoxColumn destinationColumn;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Properties
        ////////////////////////////////////////////////////////////////////////////////////////////
        public bool ShowTypeColumn
        {
            get { return this.lineTypeColumn.Visible; }
            set { this.lineTypeColumn.Visible = value; }
        }

        public int CurrentSubLineID
        {
            get
            {
                int subLineID = -1;

                try
                {
                    subLineID = Convert.ToInt32(this.CurrentRow.Cells[subLineIDColumn.Index].Value);
                }
                catch
                {
                }

                return subLineID;
            }
        }

        public int CurrentLineID
        {
            get
            {
                int lineID = -1;

                try
                {
                    lineID = Convert.ToInt32(this.CurrentRow.Cells[lineItemIDColumn.Index].Value);
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
                    transID = Convert.ToInt32(this.CurrentRow.Cells[transactionIDColumn.Index].Value);
                }
                catch
                {
                }

                return transID;
            }
        }

        private int currentAccountID;
        public int CurrentAccountID
        {
            get { return currentAccountID; }
        }
        
        private int currentEnvelopeID;
        public int CurrentEnvelopeID
        {
            get { return currentEnvelopeID; }
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void SubLineDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            //if (col == debitAmountColumn.Index && row == -1 && currentEnvelopeID != -1)
            //{
            //    string oldName = this.globalDataSet.Account.FindByid(currentEnvelopeID).debitColumnName;
            //    ChangeColumnNameForm ccnf = new ChangeColumnNameForm(LineCD.DEBIT, oldName);

            //    ccnf.ShowDialog();

            //    this.debitAmountColumn.HeaderText = ccnf.NewColumnName;
            //    this.globalDataSet.myNewColumnName("Envelope", currentEnvelopeID, LineCD.DEBIT, ccnf.NewColumnName);
            //}

            //else if (col == creditAmountColumn.Index && row == -1 && currentEnvelopeID != -1)
            //{
            //    string oldName = this.globalDataSet.Account.FindByid(currentEnvelopeID).creditColumnName;
            //    ChangeColumnNameForm ccnf = new ChangeColumnNameForm(LineCD.CREDIT, oldName);

            //    ccnf.ShowDialog();

            //    this.creditAmountColumn.HeaderText = ccnf.NewColumnName;
            //    this.globalDataSet.myNewColumnName("Envelope", currentEnvelopeID, LineCD.CREDIT, ccnf.NewColumnName);
            //}
        }

        private void SubLineDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            //bool rowTransactionError;
            //decimal subLineBalance = Convert.ToDecimal(this["balanceAmountColumn", e.RowIndex].Value);
            //int subLineID = Convert.ToInt32(this["subLineIDColumn", e.RowIndex].Value);
            //fFDBDataSet.SubLineItemRow subLineRow = this.globalDataSet.SubLineItem.FindByid(subLineID);
            //System.Drawing.Color backColor;
            //System.Drawing.Color foreColor;

            //// Defaults. Usually for new line
            //this.rowNegativeBalance = false;
            //this.rowLineError = false;
            //backColor = this.globalDataSet.MyColors.DGVBack;
            //foreColor = this.globalDataSet.MyColors.DGVFore;

            //if (subLineRow != null)
            //{
            //    // Set error flags
            //    this.rowLineError = subLineRow.LineItemRow.lineError;
            //    rowTransactionError = subLineRow.LineItemRow.transactionError;

            //    if (subLineBalance < 0.0m)
            //        this.rowNegativeBalance = true;


            //    // Set default row back and fore colors
            //    if (rowTransactionError)
            //    {
            //        backColor = this.globalDataSet.MyColors.DGVErrorBack;
            //        foreColor = this.globalDataSet.MyColors.DGVErrorFore;
            //        this.rowErrorText = "This transaction needs attention. ";
            //    }
            //    else if (subLineRow.LineItemRow.date > DateTime.Today) // future Date
            //    {
            //        backColor = this.globalDataSet.MyColors.DGVFutureBack;
            //        foreColor = this.globalDataSet.MyColors.DGVFutureFore;
            //    }
            //    else // else this is a regular transaction in the past
            //    {
            //        if (e.RowIndex % 2 == 0) // if this is an even number set normal
            //        {
            //            backColor = this.globalDataSet.MyColors.DGVBack;
            //            foreColor = this.globalDataSet.MyColors.DGVFore;
            //        }
            //        else // else set alternating color
            //        {
            //            backColor = this.globalDataSet.MyColors.DGVAlternateBack;
            //            foreColor = this.globalDataSet.MyColors.DGVAlternateFore;
            //        }
            //    }
            //}

            //this.Rows[e.RowIndex].DefaultCellStyle.BackColor = backColor;
            //this.Rows[e.RowIndex].DefaultCellStyle.ForeColor = foreColor;
        }

        private void SubLineDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //int col = e.ColumnIndex;
            //int row = e.RowIndex;

            //// If negative balance
            //if (col == balanceAmountColumn.Index && this.rowNegativeBalance)
            //{
            //    e.CellStyle.ForeColor = this.globalDataSet.MyColors.DGVNegativeBalanceFore;
            //}
            
            //// If Line  Error
            //if (col == this.envelopeIDColumn.Index && this.rowLineError)
            //{
            //    e.CellStyle.BackColor = this.globalDataSet.MyColors.DGVErrorBack;
            //    e.CellStyle.ForeColor = this.globalDataSet.MyColors.DGVErrorFore;
            //    this.rowErrorText += "The Sub-Lines need attention. ";
            //}

            //this[col, row].ToolTipText = this.rowErrorText;
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

            // Binding Sources
            this.theDGVBindingSource = new BindingSource(this.fFDBDataSet, "SubLineView");
            this.theDGVBindingSource.Sort = SORT_SUBin;

         
            // subLineIDColumn
            this.subLineIDColumn = new DataGridViewTextBoxColumn();
            this.subLineIDColumn.Name = "subLineIDColumn";
            this.subLineIDColumn.HeaderText = "subLineID";
            this.subLineIDColumn.DataPropertyName = "subLineID";
            this.subLineIDColumn.Visible = false;

            // lineItemIDColumn
            this.lineItemIDColumn = new DataGridViewTextBoxColumn();
            this.lineItemIDColumn.Name = "lineItemIDColumn";
            this.lineItemIDColumn.HeaderText = "lineItemID";
            this.lineItemIDColumn.DataPropertyName = "lineItemID";
            this.lineItemIDColumn.Visible = false;

            // transactionIDColumn
            this.transactionIDColumn = new DataGridViewTextBoxColumn();
            this.transactionIDColumn.Name = "transactionIDColumn";
            this.transactionIDColumn.HeaderText = "transactionID";
            this.transactionIDColumn.DataPropertyName = "transactionID";
            this.transactionIDColumn.Visible = false;

            // envelopeIDColumn
            this.envelopeIDColumn = new DataGridViewTextBoxColumn();
            this.envelopeIDColumn.Name = "envelopeIDColumn";
            this.envelopeIDColumn.HeaderText = "envelopeID";
            this.envelopeIDColumn.DataPropertyName = "envelopID";
            this.envelopeIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envelopeIDColumn.Visible = false;

            // accountIDColumn
            this.accountIDColumn = new DataGridViewTextBoxColumn();
            this.accountIDColumn.Name = "accountIDColumn";
            this.accountIDColumn.HeaderText = "accountID";
            this.accountIDColumn.DataPropertyName = "accountID";
            this.accountIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.accountIDColumn.Visible = false;

            // dateColumn
            this.dateColumn = new DataGridViewTextBoxColumn();
            this.dateColumn.Name = "dateColumn";
            this.dateColumn.HeaderText = "Date";
            this.dateColumn.DataPropertyName = "date";
            this.dateColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dateColumn.Visible = true;
            this.dateColumn.Width = 70;

            // lineTypeslColumn
            this.lineTypeColumn = new DataGridViewTextBoxColumn();
            this.lineTypeColumn.Name = "lineTypeslColumn";
            this.lineTypeColumn.HeaderText = "Type";
            this.lineTypeColumn.DataPropertyName = "lineType";
            this.lineTypeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.lineTypeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.lineTypeColumn.FillWeight = 25;
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

            // creditAmountColumn
            this.creditAmountColumn = new DataGridViewTextBoxColumn();
            this.creditAmountColumn.Name = "creditAmountColumn";
            this.creditAmountColumn.HeaderText = "Credit Amount";
            this.creditAmountColumn.DataPropertyName = "creditAmount";
            this.creditAmountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            //this.creditAmountColumn.DefaultCellStyle = new CellStyles().Money;
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
            this.debitAmountColumn.HeaderText = "Debit Amount";
            this.debitAmountColumn.DataPropertyName = "debitAmount";
            this.debitAmountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            //this.debitAmountColumn.DefaultCellStyle = new CellStyles().Money;
            this.debitAmountColumn.Visible = true;
            this.debitAmountColumn.Width = 65;

            // balanceAmountColumn
            this.balanceAmountColumn = new DataGridViewTextBoxColumn();
            this.balanceAmountColumn.Name = "balanceAmountColumn";
            this.balanceAmountColumn.HeaderText = "Balance";
            this.balanceAmountColumn.DataPropertyName = "balanceAmount";
            this.balanceAmountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            //this.balanceAmountColumn.DefaultCellStyle = new CellStyles().Money;
            this.balanceAmountColumn.Visible = true;
            this.balanceAmountColumn.Width = 75;

            // theDataGridView
            this.Name = "theDataGridView";
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.Dock = DockStyle.Fill;
            this.AllowUserToAddRows = false;
            this.AutoGenerateColumns = false;
            this.AllowUserToOrderColumns = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.RowHeadersVisible = false;
            this.ShowCellErrors = false;
            this.ReadOnly = true;
            this.DataSource = this.theDGVBindingSource;
            //this.AlternatingRowsDefaultCellStyle = new CellStyles().AlternatingRow;
            this.Columns.AddRange(
                new DataGridViewColumn[] 
                {
                    this.dateColumn,
                    this.subLineIDColumn,
                    this.lineItemIDColumn,
                    this.transactionIDColumn,
                    this.envelopeIDColumn,
                    this.lineTypeColumn,
                    this.accountIDColumn,
                    this.sourceColumn,
                    this.destinationColumn,
                    this.descriptionColumn,
                    this.creditAmountColumn,
                    this.completeColumn,
                    this.debitAmountColumn,
                    this.balanceAmountColumn
                }
                );

            //this.CellDoubleClick += new DataGridViewCellEventHandler(SubLineDGV_CellDoubleClick);
            //this.CellFormatting += new DataGridViewCellFormattingEventHandler(SubLineDGV_CellFormatting);
            //this.RowPrePaint += new DataGridViewRowPrePaintEventHandler(SubLineDGV_RowPrePaint);

        }

        

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public SubLineDGV()
        {
            fFDBDataSet = new FFDBDataSet();

            fFDBDataSet.Account.myFillTA();
            fFDBDataSet.Envelope.myFillTA();
            fFDBDataSet.LineType.myFillTA();
            //fFDBDataSet.LineItem.myFillTA();

            myInit();
        }

        public void setAccountEnvelopeID(int accountID, int envelopeID)
        {
            const int INVALID = 0;

            string filter;

            //this.fFDBDataSet.myFillSubLineView();

            if (envelopeID > INVALID && accountID > INVALID)
            {
                this.currentEnvelopeID = envelopeID;
                this.currentAccountID = accountID;

                filter = "envelopeID = " + envelopeID.ToString();
                filter += " AND accountID = " + accountID.ToString();
                //this.fFDBDataSet.myFillSubLineViewBalance(currentAccountID, currentEnvelopeID, SORT_SUBdb);
                this.theDGVBindingSource.Filter = filter;
                this.theDGVBindingSource.Sort = SORT_SUBin;

                //this.debitAmountColumn.HeaderText = this.globalDataSet.Envelope.FindByid(envelopeID).debitColumnName;
                //this.creditAmountColumn.HeaderText = this.globalDataSet.Envelope.FindByid(envelopeID).creditColumnName;
                this.debitAmountColumn.HeaderText = "Debit";
                this.creditAmountColumn.HeaderText = "Credit";
            }
            else if (envelopeID > INVALID)
            {
                this.currentEnvelopeID = envelopeID;
                this.currentAccountID = SpclAccount.NULL;

                filter = "envelopeID = " + envelopeID.ToString();
                //this.fFDBDataSet.myFillSubLineViewBalance(currentAccountID, currentEnvelopeID, SORT_SUBdb);
                this.theDGVBindingSource.Filter = filter;
                this.theDGVBindingSource.Sort = SORT_SUBin;

                //this.debitAmountColumn.HeaderText = this.globalDataSet.Envelope.FindByid(envelopeID).debitColumnName;
                //this.creditAmountColumn.HeaderText = this.globalDataSet.Envelope.FindByid(envelopeID).creditColumnName;
                this.debitAmountColumn.HeaderText = "Debit";
                this.creditAmountColumn.HeaderText = "Credit";
            }
            else 
            {
                this.currentEnvelopeID = SpclEnvelope.NULL;
                this.currentAccountID = SpclAccount.NULL;

                filter = "subLineID = -100";
                this.theDGVBindingSource.Filter = filter;

                this.debitAmountColumn.HeaderText = "Debit";
                this.creditAmountColumn.HeaderText = "Credit";
            }
        }


    }
}
