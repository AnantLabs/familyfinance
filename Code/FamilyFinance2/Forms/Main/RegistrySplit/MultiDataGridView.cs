using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FamilyFinance2.Forms.Transaction;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main.RegistrySplit.Register
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

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private Panel panel;

        private bool inRowValidating;

        private RegistryDataSet regDataSet;

        ////////////////////////////////
        // envDGV 
        private DataGridView envDGV;
        private BindingSource envDGVBindingSource;
        
        private DataGridViewTextBoxColumn envColTransactionID;
        private DataGridViewTextBoxColumn envColLineItemID;
        private DataGridViewTextBoxColumn envColID;
        private DataGridViewTextBoxColumn envColDate;
        private DataGridViewTextBoxColumn envColLineType;
        private DataGridViewTextBoxColumn envColSource;
        private DataGridViewTextBoxColumn envColDestination;
        private DataGridViewTextBoxColumn envColLineDescription;
        private DataGridViewTextBoxColumn envColDescription;
        private DataGridViewTextBoxColumn envColDebitAmount;
        private DataGridViewTextBoxColumn envColComplete;
        private DataGridViewTextBoxColumn envColCreditAmount;
        private DataGridViewTextBoxColumn envColBalanceAmount;

        ////////////////////////////////
        //   liDGV
        private DataGridView liDGV;
        private BindingSource liDGVBindingSource;
        private BindingSource liTypeColBindingSource;
        private BindingSource liOppAccountColBindingSource;
        private BindingSource liEnvelopeColBindingSource;

        private DataGridViewTextBoxColumn liColID;
        private DataGridViewTextBoxColumn liColTransactionID;
        private CalendarColumn liColDate;
        private DataGridViewComboBoxColumn liColTypeID;
        private DataGridViewComboBoxColumn liColOppAccountID;
        private DataGridViewTextBoxColumn liColDescription;
        private DataGridViewTextBoxColumn liColConfirmationNum;
        private DataGridViewComboBoxColumn liColEnvelopeID;
        private DataGridViewTextBoxColumn liColDebitAmount;
        private DataGridViewTextBoxColumn liColComplete;
        private DataGridViewTextBoxColumn liColCreditAmount;
        private DataGridViewTextBoxColumn liColBalanceAmount;


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
        private void buildTheEnvLineDGV()
        {
            // transactionIDColumn
            this.envColTransactionID = new DataGridViewTextBoxColumn();
            this.envColTransactionID.Name = "transactionID";
            this.envColTransactionID.HeaderText = "transactionID";
            this.envColTransactionID.DataPropertyName = "transactionID";
            this.envColTransactionID.Visible = true;

            // lineItemIDColumn
            this.envColLineItemID = new DataGridViewTextBoxColumn();
            this.envColLineItemID.Name = "lineID";
            this.envColLineItemID.HeaderText = "lineItemID";
            this.envColLineItemID.DataPropertyName = "lineItemID";
            this.envColLineItemID.Visible = true;

            // eLineIDColumn
            this.envColID = new DataGridViewTextBoxColumn();
            this.envColID.Name = "eLineID";
            this.envColID.HeaderText = "eLineIDColumn";
            this.envColID.DataPropertyName = "eLineID";
            this.envColID.Visible = true;

            // dateColumn
            this.envColDate = new DataGridViewTextBoxColumn();
            this.envColDate.Name = "date";
            this.envColDate.HeaderText = "Date";
            this.envColDate.DataPropertyName = "date";
            this.envColDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envColDate.Visible = true;
            this.envColDate.Width = 85;

            // lineTypeslColumn
            this.envColLineType = new DataGridViewTextBoxColumn();
            this.envColLineType.Name = "lineType";
            this.envColLineType.HeaderText = "Type";
            this.envColLineType.DataPropertyName = "lineType";
            this.envColLineType.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envColLineType.Width = 80;
            this.envColLineType.Visible = true;

            // sourceColumn
            this.envColSource = new DataGridViewTextBoxColumn();
            this.envColSource.Name = "source";
            this.envColSource.HeaderText = "Source";
            this.envColSource.DataPropertyName = "sourceAccount";
            this.envColSource.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envColSource.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.envColSource.FillWeight = 30;
            this.envColSource.Visible = true;

            // destinationColumn
            this.envColDestination = new DataGridViewTextBoxColumn();
            this.envColDestination.Name = "destination";
            this.envColDestination.HeaderText = "Destination";
            this.envColDestination.DataPropertyName = "destinationAccount";
            this.envColDestination.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envColDestination.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.envColDestination.FillWeight = 30;
            this.envColDestination.Visible = true;

            // descriptionColumn
            this.envColLineDescription = new DataGridViewTextBoxColumn();
            this.envColLineDescription.Name = "lineDescription";
            this.envColLineDescription.HeaderText = "Description";
            this.envColLineDescription.DataPropertyName = "lineDescription";
            this.envColLineDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envColLineDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.envColLineDescription.FillWeight = 50;
            this.envColLineDescription.Visible = true;

            // subDescriptionColumn
            this.envColDescription = new DataGridViewTextBoxColumn();
            this.envColDescription.Name = "description";
            this.envColDescription.HeaderText = "Sub Description";
            this.envColDescription.DataPropertyName = "description";
            this.envColDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envColDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.envColDescription.FillWeight = 50;
            this.envColDescription.Visible = true;

            // creditAmountColumn
            this.envColCreditAmount = new DataGridViewTextBoxColumn();
            this.envColCreditAmount.Name = "creditAmount";
            this.envColCreditAmount.HeaderText = "Credit";
            this.envColCreditAmount.DataPropertyName = "creditAmount";
            this.envColCreditAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envColCreditAmount.DefaultCellStyle = new MyCellStyleMoney();
            this.envColCreditAmount.Visible = true;
            this.envColCreditAmount.Width = 65;

            // completeColumn
            this.envColComplete = new DataGridViewTextBoxColumn();
            this.envColComplete.Name = "complete";
            this.envColComplete.HeaderText = "CR";
            this.envColComplete.DataPropertyName = "complete";
            this.envColComplete.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envColComplete.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            this.envColComplete.Visible = true;
            this.envColComplete.Width = 25;

            // debitAmountColumn
            this.envColDebitAmount = new DataGridViewTextBoxColumn();
            this.envColDebitAmount.Name = "debitAmountColumn";
            this.envColDebitAmount.HeaderText = "Debit";
            this.envColDebitAmount.DataPropertyName = "debitAmount";
            this.envColDebitAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envColDebitAmount.DefaultCellStyle = new MyCellStyleMoney();
            this.envColDebitAmount.Visible = true;
            this.envColDebitAmount.Width = 65;

            // balanceAmountColumn
            this.envColBalanceAmount = new DataGridViewTextBoxColumn();
            this.envColBalanceAmount.Name = "balanceAmount";
            this.envColBalanceAmount.HeaderText = "Balance";
            this.envColBalanceAmount.DataPropertyName = "balanceAmount";
            this.envColBalanceAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.envColBalanceAmount.DefaultCellStyle = new MyCellStyleMoney();
            this.envColBalanceAmount.Visible = true;
            this.envColBalanceAmount.Width = 75;

            // envDGV
            this.envDGV.Name = "envDGV";
            this.envDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.envDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.envDGV.Dock = DockStyle.Fill;
            this.envDGV.AutoGenerateColumns = false;
            this.envDGV.AllowUserToOrderColumns = false;
            this.envDGV.AllowUserToDeleteRows = false;
            this.envDGV.AllowUserToResizeRows = false;
            this.envDGV.AllowUserToAddRows = false;
            this.envDGV.RowHeadersVisible = false;
            this.envDGV.ShowCellErrors = false;
            this.envDGV.ShowRowErrors = false;
            this.envDGV.MultiSelect = false;
            this.envDGV.ReadOnly = true;
            this.envDGV.Columns.AddRange(
                new DataGridViewColumn[] 
                {
                    this.envColDate,
                    this.envColTransactionID,
                    this.envColLineItemID,
                    this.envColID,
                    this.envColLineType,
                    this.envColSource,
                    this.envColDestination,
                    this.envColLineDescription,
                    this.envColDescription,
                    this.envColCreditAmount,
                    this.envColComplete,
                    this.envColDebitAmount,
                    this.envColBalanceAmount
                }
                );

            this.envDGV.DataError += new DataGridViewDataErrorEventHandler(dgv_DataError);
            //this.envDGV.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
            //this.envDGV.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);

            //this.envDGV.RowPrePaint += new DataGridViewRowPrePaintEventHandler(dgv_RowPrePaint);

            //this.liDGV.CellDoubleClick += new DataGridViewCellEventHandler(LineItemDGV_CellDoubleClick);
            //this.liDGV.RowValidating += new DataGridViewCellCancelEventHandler(LineItemDGV_RowValidating);
            //this.liDGV.RowValidated += new DataGridViewCellEventHandler(LineItemDGV_RowValidated);
            //this.liDGV.CellValueChanged += new DataGridViewCellEventHandler(LineItemDGV_CellValueChanged);
        }

        private void buildTheLineItemDGV()
        {
            // lineItemIDColumn
            this.liColID = new DataGridViewTextBoxColumn();
            this.liColID.Name = "ID";
            this.liColID.HeaderText = "lineID";
            this.liColID.DataPropertyName = "id";
            this.liColID.Visible = true;

            // transactionIDColumn
            this.liColTransactionID = new DataGridViewTextBoxColumn();
            this.liColTransactionID.Name = "transactionID";
            this.liColTransactionID.HeaderText = "transactionID";
            this.liColTransactionID.DataPropertyName = "transactionID";
            this.liColTransactionID.Visible = true;

            // dateColumn
            this.liColDate = new CalendarColumn();
            this.liColDate.Name = "date";
            this.liColDate.HeaderText = "Date";
            this.liColDate.DataPropertyName = "date";
            this.liColDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.liColDate.Resizable = DataGridViewTriState.True;
            this.liColDate.Width = 85;
            this.liColDate.Visible = true;

            // typeIDColumn
            this.liColTypeID = new DataGridViewComboBoxColumn();
            this.liColTypeID.Name = "typeID";
            this.liColTypeID.HeaderText = "Type";
            this.liColTypeID.DataPropertyName = "typeID";
            this.liColTypeID.DataSource = this.liTypeColBindingSource;
            this.liColTypeID.DisplayMember = "name";
            this.liColTypeID.ValueMember = "id";
            this.liColTypeID.AutoComplete = true;
            this.liColTypeID.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.liColTypeID.Resizable = DataGridViewTriState.True;
            this.liColTypeID.DisplayStyleForCurrentCellOnly = true;
            this.liColTypeID.Width = 80;
            this.liColTypeID.Visible = true;

            // oppAccountIDColumn
            this.liColOppAccountID = new DataGridViewComboBoxColumn();
            this.liColOppAccountID.Name = "oppAccountID";
            this.liColOppAccountID.HeaderText = "Source / Destination";
            this.liColOppAccountID.DataPropertyName = "oppAccountID";
            this.liColOppAccountID.DataSource = this.liOppAccountColBindingSource;
            this.liColOppAccountID.DisplayMember = "name";
            this.liColOppAccountID.ValueMember = "id";
            this.liColOppAccountID.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.liColOppAccountID.Resizable = DataGridViewTriState.True;
            this.liColOppAccountID.DisplayStyleForCurrentCellOnly = true;
            this.liColOppAccountID.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.liColOppAccountID.FillWeight = 120;
            this.liColOppAccountID.Visible = true;

            // descriptionColumn
            this.liColDescription = new DataGridViewTextBoxColumn();
            this.liColDescription.Name = "description";
            this.liColDescription.HeaderText = "Description";
            this.liColDescription.DataPropertyName = "description";
            this.liColDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.liColDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.liColDescription.FillWeight = 200;
            this.liColDescription.Visible = true;

            // confirmationNumColumn
            this.liColConfirmationNum = new DataGridViewTextBoxColumn();
            this.liColConfirmationNum.Name = "confirmationNum";
            this.liColConfirmationNum.HeaderText = "Confirmation #";
            this.liColConfirmationNum.DataPropertyName = "confirmationNumber";
            this.liColConfirmationNum.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.liColConfirmationNum.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.liColConfirmationNum.FillWeight = 100;
            this.liColConfirmationNum.Visible = true;

            // envelopeIDColumn
            this.liColEnvelopeID = new DataGridViewComboBoxColumn();
            this.liColEnvelopeID.Name = "envelopeID";
            this.liColEnvelopeID.HeaderText = "Envelope";
            this.liColEnvelopeID.DataPropertyName = "envelopeID";
            this.liColEnvelopeID.DataSource = this.liEnvelopeColBindingSource;
            this.liColEnvelopeID.DisplayMember = "name";
            this.liColEnvelopeID.ValueMember = "id";
            this.liColEnvelopeID.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.liColEnvelopeID.DisplayStyleForCurrentCellOnly = true;
            this.liColEnvelopeID.Resizable = DataGridViewTriState.True;
            this.liColEnvelopeID.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.liColEnvelopeID.FillWeight = 100;
            this.liColEnvelopeID.Visible = true;

            // debitAmountColumn
            this.liColDebitAmount = new DataGridViewTextBoxColumn();
            this.liColDebitAmount.Name = "debitAmount";
            this.liColDebitAmount.HeaderText = "Debit";
            this.liColDebitAmount.DataPropertyName = "debitAmount";
            this.liColDebitAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.liColDebitAmount.DefaultCellStyle = new MyCellStyleMoney();
            this.liColDebitAmount.Width = 65;

            // completeColumn
            this.liColComplete = new DataGridViewTextBoxColumn();
            this.liColComplete.Name = "complete";
            this.liColComplete.HeaderText = "CR";
            this.liColComplete.DataPropertyName = "complete";
            this.liColComplete.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.liColComplete.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            this.liColComplete.Width = 25;
            this.liColComplete.ReadOnly = true;

            // creditAmountColumn
            this.liColCreditAmount = new DataGridViewTextBoxColumn();
            this.liColCreditAmount.Name = "creditAmount";
            this.liColCreditAmount.HeaderText = "Credit";
            this.liColCreditAmount.DataPropertyName = "creditAmount";
            this.liColCreditAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.liColCreditAmount.DefaultCellStyle = new MyCellStyleMoney();
            this.liColCreditAmount.Width = 65;

            // balanceAmountColumn
            this.liColBalanceAmount = new DataGridViewTextBoxColumn();
            this.liColBalanceAmount.Name = "balanceAmount";
            this.liColBalanceAmount.HeaderText = "Balance";
            this.liColBalanceAmount.DataPropertyName = "balanceAmount";
            this.liColBalanceAmount.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.liColBalanceAmount.DefaultCellStyle = new MyCellStyleMoney();
            this.liColBalanceAmount.Width = 75;
            this.liColBalanceAmount.ReadOnly = true;

            // theDataGridView
            this.liDGV.DataSource = this.liDGVBindingSource;
            this.liDGV.Name = "theDataGridView";
            this.liDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.liDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //this.liDGV.AlternatingRowsDefaultCellStyle = this.MyCellStyleAlternatingRow;
            this.liDGV.Dock = DockStyle.Fill;
            this.liDGV.AutoGenerateColumns = false;
            this.liDGV.AllowUserToOrderColumns = false;
            this.liDGV.AllowUserToDeleteRows = false;
            this.liDGV.AllowUserToResizeRows = false;
            this.liDGV.AllowUserToAddRows = true;
            this.liDGV.RowHeadersVisible = false;
            this.liDGV.ShowCellErrors = false;
            this.liDGV.ShowRowErrors = false;
            this.liDGV.MultiSelect = false;

            this.liDGV.Columns.AddRange(
                new DataGridViewColumn[] 
                {
                    this.liColDate,
                    this.liColID,
                    this.liColTransactionID,
                    this.liColTypeID,
                    this.liColOppAccountID,
                    this.liColDescription,
                    this.liColConfirmationNum,
                    this.liColEnvelopeID,
                    this.liColCreditAmount,
                    this.liColComplete,
                    this.liColDebitAmount,
                    this.liColBalanceAmount
                }
                );

            this.liDGV.DataError += new DataGridViewDataErrorEventHandler(dgv_DataError);
            //this.liDGV.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
            //this.liDGV.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);

            //this.liDGV.RowPrePaint += new DataGridViewRowPrePaintEventHandler(liDGV_RowPrePaint);
            //this.liDGV.RowValidating += new DataGridViewCellCancelEventHandler(liDGV_RowValidating);
            //this.liDGV.RowValidated += new DataGridViewCellEventHandler(liDGV_RowValidated);

            //this.liDGV.CellValueChanged += new DataGridViewCellEventHandler(liDGV_CellValueChanged);
        }

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
            this.regDataSet = new RegistryDataSet();
            this.regDataSet.myInit();
            
            this.inRowValidating = false;          

            ////////////////////////////////////
            // LineItem Setup
            this.liDGV = new DataGridView();

            this.liDGVBindingSource = new BindingSource(this.regDataSet, "LineItem");
            this.liOppAccountColBindingSource = new BindingSource(this.regDataSet, "Account");
            this.liOppAccountColBindingSource.Sort = "name";
            this.liEnvelopeColBindingSource = new BindingSource(this.regDataSet, "Envelope");
            this.liEnvelopeColBindingSource.Sort = "name";
            this.liTypeColBindingSource = new BindingSource(this.regDataSet, "LineType");
            this.liTypeColBindingSource.Sort = "name";

            this.buildTheLineItemDGV();


            ////////////////////////////////////
            // EnvelopeLine Setup
            this.envDGV = new DataGridView();

            this.envDGVBindingSource = new BindingSource(this.regDataSet, "EnvelopeLineView");

            this.buildTheEnvLineDGV();


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

