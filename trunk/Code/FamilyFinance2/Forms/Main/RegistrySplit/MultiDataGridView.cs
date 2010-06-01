using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FamilyFinance2.Forms.Transaction;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main.RegistrySplit
{
    public class MultiDataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal class
        ////////////////////////////////////////////////////////////////////////////////////////////
        private static class Current
        {
            public static int AccountID = SpclAccount.NULL;
            public static int EnvelopeID = SpclEnvelope.NULL;

            public static int TransactionID;
            public static int LineID;
            public static int EnvLineID;

            public static bool AccountUsesEnvelopes;

            public static DataGridView DGV;
        }

        private class EnvLine
        {
            private DataGridView dgv;
            private BindingSource dgvBindingSource;

            private DataGridViewTextBoxColumn transactionIDCol;
            private DataGridViewTextBoxColumn lineItemIDCol;
            private DataGridViewTextBoxColumn idCol;
            private DataGridViewTextBoxColumn dateCol;
            private DataGridViewTextBoxColumn lineTypeCol;
            private DataGridViewTextBoxColumn sourceCol;
            private DataGridViewTextBoxColumn destinationCol;
            private DataGridViewTextBoxColumn lineDescriptionCol;
            private DataGridViewTextBoxColumn descriptionCol;
            private DataGridViewTextBoxColumn creditAmountCol;
            private DataGridViewTextBoxColumn completeCol;
            private DataGridViewTextBoxColumn debitAmountCol;
            private DataGridViewTextBoxColumn balanceAmountCol;

            private void buildTheColumns()
            {
                // transactionIDColumn
                this.transactionIDCol = new DataGridViewTextBoxColumn();
                this.transactionIDCol.Name = "transactionID";
                this.transactionIDCol.HeaderText = "transactionID";
                this.transactionIDCol.DataPropertyName = "transactionID";
                this.transactionIDCol.Visible = true;

                // lineItemIDColumn
                this.lineItemIDCol = new DataGridViewTextBoxColumn();
                this.lineItemIDCol.Name = "lineItemID";
                this.lineItemIDCol.HeaderText = "lineItemID";
                this.lineItemIDCol.DataPropertyName = "lineItemID";
                this.lineItemIDCol.Visible = true;

                // idColumn
                this.idCol = new DataGridViewTextBoxColumn();
                this.idCol.Name = "eLineID";
                this.idCol.HeaderText = "eLineID";
                this.idCol.DataPropertyName = "eLineID";
                this.idCol.Visible = true;

                // dateColumn
                this.dateCol = new DataGridViewTextBoxColumn();
                this.dateCol.Name = "date";
                this.dateCol.HeaderText = "Date";
                this.dateCol.DataPropertyName = "date";
                this.dateCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dateCol.Visible = true;
                this.dateCol.Width = 85;

                // descriptionColumn
                this.lineDescriptionCol = new DataGridViewTextBoxColumn();
                this.lineDescriptionCol.Name = "lineDescription";
                this.lineDescriptionCol.HeaderText = "Description";
                this.lineDescriptionCol.DataPropertyName = "lineDescription";
                this.lineDescriptionCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.lineDescriptionCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.lineDescriptionCol.FillWeight = 50;
                this.lineDescriptionCol.Visible = true;

                // subDescriptionColumn
                this.descriptionCol = new DataGridViewTextBoxColumn();
                this.descriptionCol.Name = "description";
                this.descriptionCol.HeaderText = "Sub Description";
                this.descriptionCol.DataPropertyName = "description";
                this.descriptionCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.descriptionCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.descriptionCol.FillWeight = 50;
                this.descriptionCol.Visible = true;

                // lineTypeslColumn
                this.lineTypeCol = new DataGridViewTextBoxColumn();
                this.lineTypeCol.Name = "lineType";
                this.lineTypeCol.HeaderText = "Type";
                this.lineTypeCol.DataPropertyName = "lineType";
                this.lineTypeCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.lineTypeCol.Width = 80;
                this.lineTypeCol.Visible = true;

                // sourceColumn
                this.sourceCol = new DataGridViewTextBoxColumn();
                this.sourceCol.Name = "source";
                this.sourceCol.HeaderText = "Source";
                this.sourceCol.DataPropertyName = "sourceAccount";
                this.sourceCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.sourceCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.sourceCol.FillWeight = 30;
                this.sourceCol.Visible = true;

                // destinationColumn
                this.destinationCol = new DataGridViewTextBoxColumn();
                this.destinationCol.Name = "destination";
                this.destinationCol.HeaderText = "Destination";
                this.destinationCol.DataPropertyName = "destinationAccount";
                this.destinationCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.destinationCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.destinationCol.FillWeight = 30;
                this.destinationCol.Visible = true;

                // creditAmountColumn
                this.creditAmountCol = new DataGridViewTextBoxColumn();
                this.creditAmountCol.Name = "creditAmount";
                this.creditAmountCol.HeaderText = "Credit";
                this.creditAmountCol.DataPropertyName = "creditAmount";
                this.creditAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.creditAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                this.creditAmountCol.Visible = true;
                this.creditAmountCol.Width = 65;

                // completeColumn
                this.completeCol = new DataGridViewTextBoxColumn();
                this.completeCol.Name = "complete";
                this.completeCol.HeaderText = "CR";
                this.completeCol.DataPropertyName = "complete";
                this.completeCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.completeCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
                this.completeCol.Visible = true;
                this.completeCol.Width = 25;

                // debitAmountColumn
                this.debitAmountCol = new DataGridViewTextBoxColumn();
                this.debitAmountCol.Name = "debitAmountColumn";
                this.debitAmountCol.HeaderText = "Debit";
                this.debitAmountCol.DataPropertyName = "debitAmount";
                this.debitAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.debitAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                this.debitAmountCol.Visible = true;
                this.debitAmountCol.Width = 65;

                // balanceAmountColumn
                this.balanceAmountCol = new DataGridViewTextBoxColumn();
                this.balanceAmountCol.Name = "balanceAmount";
                this.balanceAmountCol.HeaderText = "Balance";
                this.balanceAmountCol.DataPropertyName = "balanceAmount";
                this.balanceAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.balanceAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                this.balanceAmountCol.Visible = true;
                this.balanceAmountCol.Width = 75;
            }

            public EnvLine(ref RegistryDataSet dataSource)
            {
                this.dgvBindingSource = new BindingSource(dataSource , "EnvelopeLineView");

                this.buildTheColumns();


                // envDGV
                this.dgv = new DataGridView();
                this.dgv.Name = "theDataGridView";
                this.dgv.DataSource = this.dgvBindingSource;
                this.dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                this.dgv.AlternatingRowsDefaultCellStyle = new MyCellStyleAlternatingRow();
                this.dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
                this.dgv.Dock = DockStyle.Fill;
                this.dgv.AutoGenerateColumns = false;
                this.dgv.AllowUserToOrderColumns = false;
                this.dgv.AllowUserToDeleteRows = false;
                this.dgv.AllowUserToResizeRows = false;
                this.dgv.AllowUserToAddRows = false;
                this.dgv.RowHeadersVisible = false;
                this.dgv.ShowCellErrors = false;
                this.dgv.ShowRowErrors = false;
                this.dgv.MultiSelect = false;
                this.dgv.ReadOnly = true;

                this.dgv.Columns.AddRange(
                    new DataGridViewColumn[] 
                {
                    this.dateCol,
                    this.transactionIDCol,
                    this.lineItemIDCol,
                    this.idCol,
                    this.lineTypeCol,
                    this.sourceCol,
                    this.destinationCol,
                    this.lineDescriptionCol,
                    this.descriptionCol,
                    this.creditAmountCol,
                    this.completeCol,
                    this.debitAmountCol,
                    this.balanceAmountCol
                }
                    );

            }

            public DataGridView getDGV()
            {
                return this.dgv; 
            }

        }

        private class LineItem
        {
            private DataGridView dgv;

            private BindingSource dgvBindingSource;
            private BindingSource typeColBindingSource;
            private BindingSource oppAccountColBindingSource;
            private BindingSource envelopeColBindingSource;

            private DataGridViewTextBoxColumn idCol;
            private DataGridViewTextBoxColumn transactionIDCol;
            private CalendarColumn dateCol;
            private DataGridViewComboBoxColumn lineTypeCol;
            private DataGridViewComboBoxColumn oppAccountIDCol;
            private DataGridViewTextBoxColumn descriptionCol;
            private DataGridViewTextBoxColumn confirmationNumCol;
            private DataGridViewComboBoxColumn envelopeIDCol;
            private DataGridViewTextBoxColumn creditAmountCol;
            private DataGridViewTextBoxColumn completeCol;
            private DataGridViewTextBoxColumn debitAmountCol;
            private DataGridViewTextBoxColumn balanceAmountCol;

            private void buildTheColumns()
            {
                // idColumn
                this.idCol = new DataGridViewTextBoxColumn();
                this.idCol.Name = "id";
                this.idCol.HeaderText = "id";
                this.idCol.DataPropertyName = "id";
                this.idCol.Visible = true;

                // transactionIDColumn
                this.transactionIDCol = new DataGridViewTextBoxColumn();
                this.transactionIDCol.Name = "transactionID";
                this.transactionIDCol.HeaderText = "transactionID";
                this.transactionIDCol.DataPropertyName = "transactionID";
                this.transactionIDCol.Visible = true;

                // dateColumn
                this.dateCol = new CalendarColumn();
                this.dateCol.Name = "date";
                this.dateCol.HeaderText = "Date";
                this.dateCol.DataPropertyName = "date";
                this.dateCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dateCol.Resizable = DataGridViewTriState.True;
                this.dateCol.Visible = true;
                this.dateCol.Width = 85;

                // typeIDColumn
                this.lineTypeCol = new DataGridViewComboBoxColumn();
                this.lineTypeCol.Name = "typeID";
                this.lineTypeCol.HeaderText = "Type";
                this.lineTypeCol.DataPropertyName = "typeID";
                this.lineTypeCol.DataSource = this.typeColBindingSource;
                this.lineTypeCol.DisplayMember = "name";
                this.lineTypeCol.ValueMember = "id";
                this.lineTypeCol.AutoComplete = true;
                this.lineTypeCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.lineTypeCol.Resizable = DataGridViewTriState.True;
                this.lineTypeCol.DisplayStyleForCurrentCellOnly = true;
                this.lineTypeCol.Width = 80;
                this.lineTypeCol.Visible = true;

                // oppAccountIDColumn
                this.oppAccountIDCol = new DataGridViewComboBoxColumn();
                this.oppAccountIDCol.Name = "oppAccountID";
                this.oppAccountIDCol.HeaderText = "Source / Destination";
                this.oppAccountIDCol.DataPropertyName = "oppAccountID";
                this.oppAccountIDCol.DataSource = this.oppAccountColBindingSource;
                this.oppAccountIDCol.DisplayMember = "name";
                this.oppAccountIDCol.ValueMember = "id";
                this.oppAccountIDCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.oppAccountIDCol.Resizable = DataGridViewTriState.True;
                this.oppAccountIDCol.DisplayStyleForCurrentCellOnly = true;
                this.oppAccountIDCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.oppAccountIDCol.FillWeight = 120;
                this.oppAccountIDCol.Visible = true;

                // descriptionColumn
                this.descriptionCol = new DataGridViewTextBoxColumn();
                this.descriptionCol.Name = "description";
                this.descriptionCol.HeaderText = "Description";
                this.descriptionCol.DataPropertyName = "description";
                this.descriptionCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.descriptionCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.descriptionCol.FillWeight = 200;
                this.descriptionCol.Visible = true;

                // confirmationNumColumn
                this.confirmationNumCol = new DataGridViewTextBoxColumn();
                this.confirmationNumCol.Name = "confirmationNum";
                this.confirmationNumCol.HeaderText = "Confirmation #";
                this.confirmationNumCol.DataPropertyName = "confirmationNumber";
                this.confirmationNumCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.confirmationNumCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.confirmationNumCol.FillWeight = 100;
                this.confirmationNumCol.Visible = true;

                // envelopeIDColumn
                this.envelopeIDCol = new DataGridViewComboBoxColumn();
                this.envelopeIDCol.Name = "envelopeID";
                this.envelopeIDCol.HeaderText = "Envelope";
                this.envelopeIDCol.DataPropertyName = "envelopeID";
                this.envelopeIDCol.DataSource = this.envelopeColBindingSource;
                this.envelopeIDCol.DisplayMember = "name";
                this.envelopeIDCol.ValueMember = "id";
                this.envelopeIDCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.envelopeIDCol.DisplayStyleForCurrentCellOnly = true;
                this.envelopeIDCol.Resizable = DataGridViewTriState.True;
                this.envelopeIDCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.envelopeIDCol.FillWeight = 100;
                this.envelopeIDCol.Visible = true;

                // creditAmountColumn
                this.creditAmountCol = new DataGridViewTextBoxColumn();
                this.creditAmountCol.Name = "creditAmount";
                this.creditAmountCol.HeaderText = "Credit";
                this.creditAmountCol.DataPropertyName = "creditAmount";
                this.creditAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.creditAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                this.creditAmountCol.Width = 65;


                // completeColumn
                this.completeCol = new DataGridViewTextBoxColumn();
                this.completeCol.Name = "complete";
                this.completeCol.HeaderText = "CR";
                this.completeCol.DataPropertyName = "complete";
                this.completeCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.completeCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
                this.completeCol.Width = 25;
                this.completeCol.ReadOnly = true;

                // debitAmountColumn
                this.debitAmountCol = new DataGridViewTextBoxColumn();
                this.debitAmountCol.Name = "debitAmountColumn";
                this.debitAmountCol.HeaderText = "Debit";
                this.debitAmountCol.DataPropertyName = "debitAmount";
                this.debitAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.debitAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                this.debitAmountCol.Visible = true;
                this.debitAmountCol.Width = 65;

                // balanceAmountColumn
                this.balanceAmountCol = new DataGridViewTextBoxColumn();
                this.balanceAmountCol.Name = "balanceAmount";
                this.balanceAmountCol.HeaderText = "Balance";
                this.balanceAmountCol.DataPropertyName = "balanceAmount";
                this.balanceAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                this.balanceAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                this.balanceAmountCol.Visible = true;
                this.balanceAmountCol.Width = 75;
            }

            public LineItem(ref RegistryDataSet dataSource)
            {
                this.dgvBindingSource = new BindingSource(dataSource, "LineItem");

                this.typeColBindingSource = new BindingSource(dataSource, "LineType");
                this.typeColBindingSource.Sort = "name";
                this.oppAccountColBindingSource = new BindingSource(dataSource, "Account");
                this.oppAccountColBindingSource.Sort = "name";
                this.envelopeColBindingSource = new BindingSource(dataSource, "Envelope");
                this.envelopeColBindingSource.Sort = "name";

                this.buildTheColumns();

                // Line Item DGV
                this.dgv = new DataGridView();
                this.dgv.Name = "theDataGridView";
                this.dgv.DataSource = this.dgvBindingSource;
                this.dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                this.dgv.AlternatingRowsDefaultCellStyle = new MyCellStyleAlternatingRow();
                this.dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
                this.dgv.Dock = DockStyle.Fill;
                this.dgv.AutoGenerateColumns = false;
                this.dgv.AllowUserToOrderColumns = false;
                this.dgv.AllowUserToDeleteRows = false;
                this.dgv.AllowUserToResizeRows = false;
                this.dgv.AllowUserToAddRows = false;
                this.dgv.RowHeadersVisible = false;
                this.dgv.ShowCellErrors = false;
                this.dgv.ShowRowErrors = false;
                this.dgv.MultiSelect = false;

                this.dgv.Columns.AddRange(
                    new DataGridViewColumn[] 
                    {
                        this.dateCol,
                        this.transactionIDCol,
                        this.idCol,
                        this.lineTypeCol,
                        this.oppAccountIDCol,
                        this.descriptionCol,
                        this.confirmationNumCol,
                        this.envelopeIDCol,
                        this.creditAmountCol,
                        this.completeCol,
                        this.debitAmountCol,
                        this.balanceAmountCol
                    }
                    );
            }

            public DataGridView getDGV()
            {
                return this.dgv;
            }

        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private Panel panel;
        private DataGridView liDGV;
        private DataGridView envDGV;

        private bool inRowValidating;

        private RegistryDataSet regDataSet;

        // row flags used in painting cells
        private bool flagTransactionError;
        private bool flagLineError;
        private bool flagAccountError;
        private bool flagFutureDate;
        private bool flagNegativeBalance;
        private bool flagReadOnlyAccount;
        private bool flagReadOnlyEnvelope;


        
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string temp = "stop";
            temp = temp + temp;
        }

        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //int col = e.ColumnIndex;
            //int row = e.RowIndex;

            //if (col < 0 || row < 0)
            //    return;

            //string colName = Current.DGV.Columns[col].Name;
            //bool readOnlyCell = Current.DGV[col, row].ReadOnly;
            //string toolTipText = Current.DGV[col, row].ToolTipText;

            //// Set the back ground and the tool tip.
            //if (this.flagTransactionError)
            //{
            //    e.CellStyle.BackColor = System.Drawing.Color.Red;
            //    toolTipText = "This transaction needs attention.";
            //}
            //else if (this.flagLineError && (colName == "envelopeID" || colName == "amount"))
            //{
            //    e.CellStyle.BackColor = System.Drawing.Color.Red;
            //    toolTipText = "This line amount and its envelope sum need to match.";
            //}
            //else if (this.flagAccountError && (colName == "accountID" || colName == "oppAccountID"))
            //{
            //    e.CellStyle.BackColor = System.Drawing.Color.Red;
            //    toolTipText = "Please choose an account.";
            //}
            //else if (this.flagFutureDate)
            //    e.CellStyle.BackColor = System.Drawing.Color.LightGray;

            //// rowNegativeBalance
            //if (this.flagNegativeBalance && colName == "balanceAmount")
            //    e.CellStyle.ForeColor = System.Drawing.Color.Red;

            //// rowMultipleAccounts
            //if (this.flagReadOnlyAccount && colName == "oppAccountID")
            //    readOnlyCell = true;

            //// rowSplitEnvelope
            //if (this.flagReadOnlyEnvelope && colName == "envelopeID")
            //    readOnlyCell = true;

            //Current.DGV[col, row].ToolTipText = toolTipText;
            //Current.DGV[col, row].ReadOnly = readOnlyCell;
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //int row = e.RowIndex;
            //int col = e.ColumnIndex;

            //if (col < 0 || row < 0)
            //    return;

            //string colName = Current.DGV.Columns[col].Name;

            //if (colName == "complete" && Current.DGV == this.liDGV)
            //{
            //    string cellValue = Current.DGV[col, row].Value.ToString();

            //    if (cellValue == LineState.PENDING)
            //        Current.DGV[col, row].Value = LineState.CLEARED;

            //    else if (cellValue == LineState.CLEARED)
            //        Current.DGV[col, row].Value = LineState.RECONSILED;

            //    else
            //        Current.DGV[col, row].Value = LineState.PENDING;
            //}
            //else
            //{
            //    int transID = Convert.ToInt32(Current.DGV["transactionID", row].Value);
            //    int lineID = Convert.ToInt32(Current.DGV["lineID", row].Value);

            //    TransactionForm tf = new TransactionForm(transID, lineID);
            //    tf.ShowDialog();
            //    //this.myReloadLineItems(); // <- remove this line
            //}

        }

        
        private void envDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
        //    int subLineID = Convert.ToInt32(this[envLineItemIDColumn.Index, e.RowIndex].Value);
        //    SubLineDataSet.SubLineViewRow thisSubLine = this.slDataSet.SubLineView.FindByeLineID(subLineID);

        //    // Defaults. Used for new lines.
        //    this.flagTransactionError = false;
        //    this.flagLineError = false;
        //    this.flagNegativeBalance = false;
        //    this.flagFutureDate = false;
        //    this.flagAccountError = false;

        //    if (thisSubLine != null)
        //    {
        //        // Set row Flags
        //        //flagTransactionError = thisSubLine.tr;

        //        if (thisSubLine.amount < 0.0m)
        //            this.flagNegativeBalance = true;

        //        if (thisSubLine.date > DateTime.Today) // future Date
        //            this.flagFutureDate = true;
        //    }
        }


        private void liDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
        //    int lineID = Convert.ToInt32(this[lineItemIDColumn.Index, e.RowIndex].Value);
        //    RegistryDataSet.LineItemRow thisLine = this.regDataSet.LineItem.FindByid(lineID);

        //    // Defaults. Used for new lines.
        //    this.flagTransactionError = false;
        //    this.flagLineError = false;
        //    this.flagNegativeBalance = false;
        //    this.flagReadOnlyEnvelope = false;
        //    this.flagReadOnlyAccount = false;
        //    this.flagFutureDate = false;
        //    this.flagAccountError = false;

        //    if (thisLine != null)
        //    {
        //        // Set row Flags
        //        flagTransactionError = thisLine.transactionError;
        //        flagLineError = thisLine.lineError;

        //        if (!thisLine.IsbalanceAmountNull() && thisLine.balanceAmount < 0.0m)
        //            this.flagNegativeBalance = true;

        //        if (thisLine.oppAccountID == SpclAccount.MULTIPLE)
        //            this.flagReadOnlyAccount = true;

        //        if (thisLine.envelopeID == SpclEnvelope.SPLIT)
        //            this.flagReadOnlyEnvelope = true;

        //        if (thisLine.date > DateTime.Today) // future Date
        //            this.flagFutureDate = true;

        //        if (thisLine.oppAccountID == SpclAccount.NULL)
        //            this.flagAccountError = true;
        //    }
        }

        private void liDGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        //    this.regDataSet.myEditLine(this.CurrentLineID);
        }

        private void liDGV_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            //if (inRowValidating)
            //    return;

            //inRowValidating = true;

            //int row = e.RowIndex;
            //int typeID = Convert.ToInt32(this.liDGV[liColTypeID.Index, row].Value);
            //int oppAccount = Convert.ToInt32(this.liDGV[liColOppAccountID.Index, row].Value);
            //int envelope = Convert.ToInt32(this.liDGV[liColEnvelopeID.Index, row].Value);
            //string description = Convert.ToString(this.liDGV[liColDescription.Index, row].Value);
            //string confNum = Convert.ToString(this.liDGV[liColConfirmationNum.Index, row].Value);

            //bool allNull = (
            //                typeID == SpclLineType.NULL
            //             && envelope == SpclEnvelope.NULL
            //             && oppAccount == SpclAccount.NULL
            //             && description == ""
            //             && confNum == ""
            //             );

            //if (allNull) // Means the user didn't enter any values. - Cancel the edit -
            //    this.liDGVBindingSource.CancelEdit();

            //else if (oppAccount == SpclAccount.NULL) // Means the user didn't enter the required feild. Ask user what to do.
            //{
            //    if (DialogResult.Yes == MessageBox.Show("You have not entered the required Source or Destination.\n\nDo you want to discard this entry?", "Discard Entry?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error))
            //    {   // The user said to discard the changes.
            //        this.liDGVBindingSource.CancelEdit();
            //    }
            //    else
            //    {   // The user answerd No or Cancel. Cancel leaving the row
            //        e.Cancel = true;
            //    }
            //}

            //inRowValidating = false;
        }

        private void liDGV_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            //int transID = Convert.ToInt32(this[transactionIDColumn.Index, e.RowIndex].Value);

            //if (currentAccountID != SpclAccount.NULL)  // Save the changes
            //{
            //    lineItemDGVBindingSource.EndEdit();
            //    //this.fFDBDataSet.myCommitSingleLineChanges(lineID, currentAccountID);
            //    //this.fFDBDataSet.mySaveAndCheckTransaction(transID);
            //    //this.fFDBDataSet.LineItem.myFillByAccount(this.currentAccountID);
            //    //this.fFDBDataSet.LineItem.myFillBalance();
            //}
        }

        

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////

        private int CurrentLineID()
        {
            int lineID = -1;

            //try
            //{
            //    lineID = Convert.ToInt32(this.CurrentRow.Cells["lineItemIDColumn"].Value);
            //}
            //catch
            //{
            //}

            return lineID;
        }

        private int CurrentTransactionID()
        {
            int transID = -1;

            //try
            //{
            //    transID = Convert.ToInt32(this.CurrentRow.Cells["transactionIDColumn"].Value);
            //}
            //catch
            //{
            //}

            return transID;
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public MultiDataGridView()
        {
            this.inRowValidating = false; 

            this.regDataSet = new RegistryDataSet();
            this.regDataSet.myInit();

            ////////////////////////////////////
            // The DataGridViews
            LineItem lineItem = new LineItem(ref this.regDataSet);
            EnvLine envLine = new EnvLine(ref this.regDataSet);

            this.liDGV = lineItem.getDGV();
            this.liDGV.DataError +=new DataGridViewDataErrorEventHandler(dgv_DataError);
            this.liDGV.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
            this.liDGV.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);

            this.envDGV = envLine.getDGV();
            this.envDGV.DataError += new DataGridViewDataErrorEventHandler(dgv_DataError);
            this.envDGV.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
            this.envDGV.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);


            ////////////////////////////////////
            // Panel Setup
            this.panel = new Panel();
            this.panel.Controls.Add(this.liDGV);
            this.panel.Controls.Add(this.envDGV);
            this.panel.Dock = DockStyle.Fill;

            this.panel.ResumeLayout();

            ////////////////////////////////////
            // Panel Setup
            //this.buildTheContextMenues();

            ////////////////////////////////////
            // Fill the tables
            this.reloadLineTypes();
            this.reloadAccounts();
            this.reloadEnvelopes();
        }

        public Control getControl()
        {
            return this.panel;
        }

        public void setEnvelopeAndAccount(int accountID, int envelopeID)
        {
            if (accountID == Current.AccountID && envelopeID == Current.EnvelopeID)
                return;

            else if (accountID == SpclAccount.NULL && envelopeID == SpclEnvelope.NULL)
            {
                Current.AccountID = accountID;
                Current.EnvelopeID = envelopeID;
                Current.AccountUsesEnvelopes = false;
                Current.DGV = null;

                this.liDGV.Visible = false;
                this.envDGV.Visible = false;
            }
            else if (accountID > SpclAccount.NULL && envelopeID == SpclEnvelope.NULL)
            {
                Current.AccountID = accountID;
                Current.EnvelopeID = envelopeID;
                Current.AccountUsesEnvelopes = this.regDataSet.Account.FindByid(accountID).envelopes;
                Current.DGV = this.liDGV;

                this.regDataSet.myFillLines(accountID);

                this.liDGV.Visible = true;
                this.envDGV.Visible = false;
            }
            else
            {
                Current.AccountID = accountID;
                Current.EnvelopeID = envelopeID;
                Current.DGV = this.envDGV;

                this.regDataSet.myFillLines(accountID, envelopeID);

                this.envDGV.Visible = true;
                this.liDGV.Visible = false;
            }
        }

        public void reloadLines()
        {
            this.setEnvelopeAndAccount(Current.AccountID, Current.EnvelopeID);
        }

        public void reloadAccounts()
        {
            this.regDataSet.myFillAccountTable();
        }

        public void reloadEnvelopes()
        {
            this.regDataSet.myFillEnvelopeTable();
        }

        public void reloadLineTypes()
        {
            this.regDataSet.myFillLineTypeTable();
        }


    }// END public partial class multiDataGridView
}// END namespace FamilyFinance.Custom_Controls

