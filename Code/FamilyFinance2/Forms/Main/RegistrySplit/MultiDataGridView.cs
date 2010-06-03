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

            //public static int TransactionID;
            //public static int LineID;
            //public static int EnvLineID;

            public static bool AccountUsesEnvelopes;
            public static bool AccountIsCredit;

            public static DataGridView DGV;
        }

        private static class EnvLine
        {
            public const string TRANSACTION_ID_NAME = "transactionID";
            public const string LINE_ID_NAME = "lineItemID";
            public const string E_LINE_ID_NAME = "eLineID";
            public const string DATE_NAME = "date";

            private static DataGridView dgv;
            private static BindingSource dgvBindingSource;

            private static DataGridViewTextBoxColumn transactionIDCol;
            private static DataGridViewTextBoxColumn lineItemIDCol;
            private static DataGridViewTextBoxColumn idCol;
            private static DataGridViewTextBoxColumn dateCol;
            private static DataGridViewTextBoxColumn lineTypeCol;
            private static DataGridViewTextBoxColumn sourceCol;
            private static DataGridViewTextBoxColumn destinationCol;
            private static DataGridViewTextBoxColumn lineDescriptionCol;
            private static DataGridViewTextBoxColumn descriptionCol;
            private static DataGridViewTextBoxColumn creditAmountCol;
            private static DataGridViewTextBoxColumn completeCol;
            private static DataGridViewTextBoxColumn debitAmountCol;
            private static DataGridViewTextBoxColumn balanceAmountCol;

            private static void buildTheColumns()
            {
                // transactionIDColumn
                transactionIDCol = new DataGridViewTextBoxColumn();
                transactionIDCol.Name = TRANSACTION_ID_NAME;
                transactionIDCol.HeaderText = "transactionID";
                transactionIDCol.DataPropertyName = "transactionID";
                transactionIDCol.Visible = true;

                // lineItemIDColumn
                lineItemIDCol = new DataGridViewTextBoxColumn();
                lineItemIDCol.Name = LINE_ID_NAME;
                lineItemIDCol.HeaderText = "lineItemID";
                lineItemIDCol.DataPropertyName = "lineItemID";
                lineItemIDCol.Visible = true;

                // idColumn
                idCol = new DataGridViewTextBoxColumn();
                idCol.Name = E_LINE_ID_NAME;
                idCol.HeaderText = "eLineID";
                idCol.DataPropertyName = "eLineID";
                idCol.Visible = true;

                // dateColumn
                dateCol = new DataGridViewTextBoxColumn();
                dateCol.Name = DATE_NAME;
                dateCol.HeaderText = "Date";
                dateCol.DataPropertyName = "date";
                dateCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                dateCol.Visible = true;
                dateCol.Width = 85;

                // descriptionColumn
                lineDescriptionCol = new DataGridViewTextBoxColumn();
                lineDescriptionCol.Name = "lineDescription";
                lineDescriptionCol.HeaderText = "Description";
                lineDescriptionCol.DataPropertyName = "lineDescription";
                lineDescriptionCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                lineDescriptionCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                lineDescriptionCol.FillWeight = 50;
                lineDescriptionCol.Visible = true;

                // subDescriptionColumn
                descriptionCol = new DataGridViewTextBoxColumn();
                descriptionCol.Name = "description";
                descriptionCol.HeaderText = "Sub Description";
                descriptionCol.DataPropertyName = "description";
                descriptionCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                descriptionCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                descriptionCol.FillWeight = 50;
                descriptionCol.Visible = true;

                // lineTypeslColumn
                lineTypeCol = new DataGridViewTextBoxColumn();
                lineTypeCol.Name = "lineType";
                lineTypeCol.HeaderText = "Type";
                lineTypeCol.DataPropertyName = "lineType";
                lineTypeCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                lineTypeCol.Width = 80;
                lineTypeCol.Visible = true;

                // sourceColumn
                sourceCol = new DataGridViewTextBoxColumn();
                sourceCol.Name = "source";
                sourceCol.HeaderText = "Source";
                sourceCol.DataPropertyName = "sourceAccount";
                sourceCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                sourceCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                sourceCol.FillWeight = 30;
                sourceCol.Visible = true;

                // destinationColumn
                destinationCol = new DataGridViewTextBoxColumn();
                destinationCol.Name = "destination";
                destinationCol.HeaderText = "Destination";
                destinationCol.DataPropertyName = "destinationAccount";
                destinationCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                destinationCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                destinationCol.FillWeight = 30;
                destinationCol.Visible = true;

                // creditAmountColumn
                creditAmountCol = new DataGridViewTextBoxColumn();
                creditAmountCol.Name = "creditAmount";
                creditAmountCol.HeaderText = "Credit";
                creditAmountCol.DataPropertyName = "creditAmount";
                creditAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                creditAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                creditAmountCol.Visible = true;
                creditAmountCol.Width = 65;

                // completeColumn
                completeCol = new DataGridViewTextBoxColumn();
                completeCol.Name = "complete";
                completeCol.HeaderText = "CR";
                completeCol.DataPropertyName = "complete";
                completeCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                completeCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
                completeCol.Visible = true;
                completeCol.Width = 25;

                // debitAmountColumn
                debitAmountCol = new DataGridViewTextBoxColumn();
                debitAmountCol.Name = "debitAmountColumn";
                debitAmountCol.HeaderText = "Debit";
                debitAmountCol.DataPropertyName = "debitAmount";
                debitAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                debitAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                debitAmountCol.Visible = true;
                debitAmountCol.Width = 65;

                // balanceAmountColumn
                balanceAmountCol = new DataGridViewTextBoxColumn();
                balanceAmountCol.Name = "balanceAmount";
                balanceAmountCol.HeaderText = "Balance";
                balanceAmountCol.DataPropertyName = "balanceAmount";
                balanceAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                balanceAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                balanceAmountCol.Visible = true;
                balanceAmountCol.Width = 75;
            }

            public static DataGridView getDGV(ref RegistryDataSet dataSource)
            {
                dgvBindingSource = new BindingSource(dataSource, "EnvelopeLineView");

                buildTheColumns();


                // envDGV
                dgv = new DataGridView();
                dgv.Name = "theDataGridView";
                dgv.DataSource = dgvBindingSource;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgv.AlternatingRowsDefaultCellStyle = new MyCellStyleAlternatingRow();
                dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dgv.Dock = DockStyle.Fill;
                dgv.AutoGenerateColumns = false;
                dgv.AllowUserToOrderColumns = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToResizeRows = false;
                dgv.AllowUserToAddRows = false;
                dgv.RowHeadersVisible = false;
                dgv.ShowCellErrors = false;
                dgv.ShowRowErrors = false;
                dgv.MultiSelect = false;
                dgv.ReadOnly = true;

                dgv.Columns.AddRange(
                    new DataGridViewColumn[] 
                {
                    dateCol,
                    transactionIDCol,
                    lineItemIDCol,
                    idCol,
                    lineTypeCol,
                    sourceCol,
                    destinationCol,
                    lineDescriptionCol,
                    descriptionCol,
                    creditAmountCol,
                    completeCol,
                    debitAmountCol,
                    balanceAmountCol
                }
                    );

                return dgv; 
            }

        }

        private static class LineItem
        {
            public const string TRANSACTION_ID_NAME = "transactionID";
            public const string LINE_ID_NAME = "lineItemID";
            public const string DATE_NAME = "date";

            private static DataGridView dgv;

            private static BindingSource dgvBindingSource;
            private static BindingSource typeColBindingSource;
            private static BindingSource oppAccountColBindingSource;
            private static BindingSource envelopeColBindingSource;

            private static DataGridViewTextBoxColumn idCol;
            private static DataGridViewTextBoxColumn transactionIDCol;
            private static CalendarColumn dateCol;
            private static DataGridViewComboBoxColumn lineTypeCol;
            private static DataGridViewComboBoxColumn oppAccountIDCol;
            private static DataGridViewTextBoxColumn descriptionCol;
            private static DataGridViewTextBoxColumn confirmationNumCol;
            private static DataGridViewComboBoxColumn envelopeIDCol;
            private static DataGridViewTextBoxColumn creditAmountCol;
            private static DataGridViewTextBoxColumn completeCol;
            private static DataGridViewTextBoxColumn debitAmountCol;
            private static DataGridViewTextBoxColumn balanceAmountCol;

            private static void buildTheColumns()
            {
                // idColumn
                idCol = new DataGridViewTextBoxColumn();
                idCol.Name = LINE_ID_NAME;
                idCol.HeaderText = "id";
                idCol.DataPropertyName = "id";
                idCol.Visible = true;

                // transactionIDColumn
                transactionIDCol = new DataGridViewTextBoxColumn();
                transactionIDCol.Name = TRANSACTION_ID_NAME;
                transactionIDCol.HeaderText = "transactionID";
                transactionIDCol.DataPropertyName = "transactionID";
                transactionIDCol.Visible = true;

                // dateColumn
                dateCol = new CalendarColumn();
                dateCol.Name = DATE_NAME;
                dateCol.HeaderText = "Date";
                dateCol.DataPropertyName = "date";
                dateCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                dateCol.Resizable = DataGridViewTriState.True;
                dateCol.Visible = true;
                dateCol.Width = 85;

                // typeIDColumn
                lineTypeCol = new DataGridViewComboBoxColumn();
                lineTypeCol.Name = "typeID";
                lineTypeCol.HeaderText = "Type";
                lineTypeCol.DataPropertyName = "typeID";
                lineTypeCol.DataSource = typeColBindingSource;
                lineTypeCol.DisplayMember = "name";
                lineTypeCol.ValueMember = "id";
                lineTypeCol.AutoComplete = true;
                lineTypeCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                lineTypeCol.Resizable = DataGridViewTriState.True;
                lineTypeCol.DisplayStyleForCurrentCellOnly = true;
                lineTypeCol.Width = 80;
                lineTypeCol.Visible = true;

                // oppAccountIDColumn
                oppAccountIDCol = new DataGridViewComboBoxColumn();
                oppAccountIDCol.Name = "oppAccountID";
                oppAccountIDCol.HeaderText = "Source / Destination";
                oppAccountIDCol.DataPropertyName = "oppAccountID";
                oppAccountIDCol.DataSource = oppAccountColBindingSource;
                oppAccountIDCol.DisplayMember = "name";
                oppAccountIDCol.ValueMember = "id";
                oppAccountIDCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                oppAccountIDCol.Resizable = DataGridViewTriState.True;
                oppAccountIDCol.DisplayStyleForCurrentCellOnly = true;
                oppAccountIDCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                oppAccountIDCol.FillWeight = 120;
                oppAccountIDCol.Visible = true;

                // descriptionColumn
                descriptionCol = new DataGridViewTextBoxColumn();
                descriptionCol.Name = "description";
                descriptionCol.HeaderText = "Description";
                descriptionCol.DataPropertyName = "description";
                descriptionCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                descriptionCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                descriptionCol.FillWeight = 200;
                descriptionCol.Visible = true;

                // confirmationNumColumn
                confirmationNumCol = new DataGridViewTextBoxColumn();
                confirmationNumCol.Name = "confirmationNum";
                confirmationNumCol.HeaderText = "Confirmation #";
                confirmationNumCol.DataPropertyName = "confirmationNumber";
                confirmationNumCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                confirmationNumCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                confirmationNumCol.FillWeight = 100;
                confirmationNumCol.Visible = true;

                // envelopeIDColumn
                envelopeIDCol = new DataGridViewComboBoxColumn();
                envelopeIDCol.Name = "envelopeID";
                envelopeIDCol.HeaderText = "Envelope";
                envelopeIDCol.DataPropertyName = "envelopeID";
                envelopeIDCol.DataSource = envelopeColBindingSource;
                envelopeIDCol.DisplayMember = "name";
                envelopeIDCol.ValueMember = "id";
                envelopeIDCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                envelopeIDCol.DisplayStyleForCurrentCellOnly = true;
                envelopeIDCol.Resizable = DataGridViewTriState.True;
                envelopeIDCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                envelopeIDCol.FillWeight = 100;
                envelopeIDCol.Visible = true;

                // creditAmountColumn
                creditAmountCol = new DataGridViewTextBoxColumn();
                creditAmountCol.Name = "creditAmount";
                creditAmountCol.HeaderText = "Credit";
                creditAmountCol.DataPropertyName = "creditAmount";
                creditAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                creditAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                creditAmountCol.Width = 65;


                // completeColumn
                completeCol = new DataGridViewTextBoxColumn();
                completeCol.Name = "complete";
                completeCol.HeaderText = "CR";
                completeCol.DataPropertyName = "complete";
                completeCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                completeCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
                completeCol.Width = 25;
                completeCol.ReadOnly = true;

                // debitAmountColumn
                debitAmountCol = new DataGridViewTextBoxColumn();
                debitAmountCol.Name = "debitAmountColumn";
                debitAmountCol.HeaderText = "Debit";
                debitAmountCol.DataPropertyName = "debitAmount";
                debitAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                debitAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                debitAmountCol.Visible = true;
                debitAmountCol.Width = 65;

                // balanceAmountColumn
                balanceAmountCol = new DataGridViewTextBoxColumn();
                balanceAmountCol.Name = "balanceAmount";
                balanceAmountCol.HeaderText = "Balance";
                balanceAmountCol.DataPropertyName = "balanceAmount";
                balanceAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                balanceAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                balanceAmountCol.Visible = true;
                balanceAmountCol.Width = 75;
            }

            public static DataGridView getDGV(ref RegistryDataSet dataSource)
            {
                dgvBindingSource = new BindingSource(dataSource, "LineItem");

                typeColBindingSource = new BindingSource(dataSource, "LineType");
                typeColBindingSource.Sort = "name";
                oppAccountColBindingSource = new BindingSource(dataSource, "Account");
                oppAccountColBindingSource.Sort = "name";
                envelopeColBindingSource = new BindingSource(dataSource, "Envelope");
                envelopeColBindingSource.Sort = "name";

                buildTheColumns();

                // Line Item DGV
                dgv = new DataGridView();
                dgv.Name = "theDataGridView";
                dgv.DataSource = dgvBindingSource;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgv.AlternatingRowsDefaultCellStyle = new MyCellStyleAlternatingRow();
                dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dgv.Dock = DockStyle.Fill;
                dgv.AutoGenerateColumns = false;
                dgv.AllowUserToOrderColumns = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToResizeRows = false;
                dgv.AllowUserToAddRows = false;
                dgv.RowHeadersVisible = false;
                dgv.ShowCellErrors = false;
                dgv.ShowRowErrors = false;
                dgv.MultiSelect = false;

                dgv.Columns.AddRange(
                    new DataGridViewColumn[] 
                    {
                        dateCol,
                        transactionIDCol,
                        idCol,
                        lineTypeCol,
                        oppAccountIDCol,
                        descriptionCol,
                        confirmationNumCol,
                        envelopeIDCol,
                        creditAmountCol,
                        completeCol,
                        debitAmountCol,
                        balanceAmountCol
                    }
                    );

                return dgv;
            }

            public static void setNegativeBalanceFormat(bool isAccountCredit)
            {
                if (isAccountCredit)
                    balanceAmountCol.DefaultCellStyle.Format = "$#0.00;$#0.00;$0.00";
                else
                    balanceAmountCol.DefaultCellStyle.Format = "$#0.00;($#0.00);$0.00";
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private Panel panel;
        private DataGridView liDGV;
        private DataGridView envDGV;

        //private bool inRowValidating;

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
            int col = e.ColumnIndex;
            int row = e.RowIndex;

            if (col < 0 || row < 0)
                return;

            string colName = Current.DGV.Columns[col].Name;
            bool readOnlyCell = Current.DGV[col, row].ReadOnly;
            string toolTipText = Current.DGV[col, row].ToolTipText;

            // Set the back ground and the tool tip.
            if (this.flagTransactionError)
            {
                e.CellStyle.BackColor = System.Drawing.Color.Red;
                toolTipText = "This transaction needs attention.";
            }
            else if (this.flagLineError && colName == "envelopeID")
            {
                e.CellStyle.BackColor = System.Drawing.Color.Red;
                toolTipText = "This line amount and its envelope sum need to match.";
            }
            else if (this.flagAccountError && colName == "oppAccountID")
            {
                e.CellStyle.BackColor = System.Drawing.Color.Red;
                toolTipText = "Please choose an account.";
            }
            else if (this.flagFutureDate)
                e.CellStyle.BackColor = System.Drawing.Color.LightGray;

            // rowNegativeBalance
            if (this.flagNegativeBalance && colName == "balanceAmount")
                e.CellStyle.ForeColor = System.Drawing.Color.Red;

            // rowMultipleAccounts
            if (this.flagReadOnlyAccount && colName == "oppAccountID")
                readOnlyCell = true;

            // rowSplitEnvelope
            if (this.flagReadOnlyEnvelope && colName == "envelopeID")
                readOnlyCell = true;

            Current.DGV[col, row].ToolTipText = toolTipText;
            Current.DGV[col, row].ReadOnly = readOnlyCell;
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            if (col < 0 || row < 0)
                return;

            string colName = Current.DGV.Columns[col].Name;

            if (colName == "complete" && Current.DGV == this.liDGV)
            {
                string cellValue = Current.DGV[col, row].Value.ToString();

                if (cellValue == LineState.PENDING)
                    Current.DGV[col, row].Value = LineState.CLEARED;

                else if (cellValue == LineState.CLEARED)
                    Current.DGV[col, row].Value = LineState.RECONSILED;

                else
                    Current.DGV[col, row].Value = LineState.PENDING;
            }
            else if (Current.DGV == this.liDGV)
            {
                int transID = Convert.ToInt32(Current.DGV[LineItem.TRANSACTION_ID_NAME, row].Value);
                int lineID = Convert.ToInt32(Current.DGV[LineItem.LINE_ID_NAME, row].Value);

                TransactionForm tf = new TransactionForm(transID, lineID);
                tf.ShowDialog();
                //this.myReloadLineItems(); // <- remove this line
            }
            else if (Current.DGV == this.envDGV)
            {
                int transID = Convert.ToInt32(Current.DGV[EnvLine.TRANSACTION_ID_NAME, row].Value);
                int lineID = Convert.ToInt32(Current.DGV[EnvLine.LINE_ID_NAME, row].Value);
                int eLineID = Convert.ToInt32(Current.DGV[EnvLine.E_LINE_ID_NAME, e.RowIndex].Value);

                TransactionForm tf = new TransactionForm(transID, lineID);
                tf.ShowDialog();
                //this.myReloadLineItems(); // <- remove this line
            }

        }

        
        private void envDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int eLineID = Convert.ToInt32(Current.DGV[EnvLine.E_LINE_ID_NAME, e.RowIndex].Value);
            RegistryDataSet.EnvelopeLineViewRow thisELine = this.regDataSet.EnvelopeLineView.FindByeLineID(eLineID);

            // Defaults. Used for new lines.
            this.flagTransactionError = false;
            this.flagLineError = false;
            this.flagNegativeBalance = false;
            this.flagReadOnlyEnvelope = false;
            this.flagReadOnlyAccount = false;
            this.flagFutureDate = false;
            this.flagAccountError = false;

            if (thisELine != null)
            {
                // Set row Flags
                //flagTransactionError = thisSubLine.tr;

                if (thisELine.amount < 0.0m)
                    this.flagNegativeBalance = true;

                if (thisELine.date > DateTime.Today) // future Date
                    this.flagFutureDate = true;
            }
        }


        private void liDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int lineID = Convert.ToInt32(Current.DGV[LineItem.LINE_ID_NAME, e.RowIndex].Value);
            RegistryDataSet.LineItemRow thisLine = this.regDataSet.LineItem.FindByid(lineID);

            // Defaults. Used for new lines.
            this.flagTransactionError = false;
            this.flagLineError = false;
            this.flagNegativeBalance = false;
            this.flagReadOnlyEnvelope = false;
            this.flagReadOnlyAccount = false;
            this.flagFutureDate = false;
            this.flagAccountError = false;

            if (thisLine != null)
            {
                // Set row Flags
                flagTransactionError = thisLine.transactionError;
                flagLineError = thisLine.lineError;

                if (!thisLine.IsbalanceAmountNull() && thisLine.balanceAmount < 0.0m && !Current.AccountIsCredit)
                    this.flagNegativeBalance = true;

                if (!thisLine.IsbalanceAmountNull() && thisLine.balanceAmount > 0.0m && Current.AccountIsCredit)
                    this.flagNegativeBalance = true;

                if (thisLine.oppAccountID == SpclAccount.MULTIPLE)
                    this.flagReadOnlyAccount = true;

                if (thisLine.envelopeID == SpclEnvelope.SPLIT)
                    this.flagReadOnlyEnvelope = true;

                if (thisLine.date > DateTime.Today) // future Date
                    this.flagFutureDate = true;

                if (thisLine.oppAccountID == SpclAccount.NULL)
                    this.flagAccountError = true;
            }
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
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public MultiDataGridView()
        {
            //this.inRowValidating = false; 

            this.regDataSet = new RegistryDataSet();
            this.regDataSet.myInit();

            ////////////////////////////////////
            // The DataGridViews
            this.liDGV = LineItem.getDGV(ref this.regDataSet);
            this.liDGV.DataError +=new DataGridViewDataErrorEventHandler(dgv_DataError);
            this.liDGV.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
            this.liDGV.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);
            this.liDGV.RowPrePaint +=new DataGridViewRowPrePaintEventHandler(liDGV_RowPrePaint);

            this.envDGV = EnvLine.getDGV(ref this.regDataSet);
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
                Current.AccountIsCredit = false;
                Current.DGV = null;

                this.liDGV.Visible = false;
                this.envDGV.Visible = false;
            }
            else if (accountID > SpclAccount.NULL && envelopeID == SpclEnvelope.NULL)
            {
                Current.AccountID = accountID;
                Current.EnvelopeID = envelopeID;
                Current.AccountUsesEnvelopes = this.regDataSet.Account.FindByid(accountID).envelopes;
                Current.AccountIsCredit = !this.regDataSet.Account.FindByid(accountID).creditDebit;
                LineItem.setNegativeBalanceFormat(Current.AccountIsCredit);
                Current.DGV = this.liDGV;

                this.regDataSet.myFillLines(accountID);

                this.liDGV.Visible = true;
                this.envDGV.Visible = false;
            }
            else
            {
                Current.AccountID = accountID;
                Current.EnvelopeID = envelopeID;
                Current.AccountUsesEnvelopes = true;
                Current.AccountIsCredit = false;
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

