using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using FamilyFinance2.Forms.Transaction;
using FamilyFinance2.SharedElements;
using FamilyFinance2.Forms.DateSelector;

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

            public static bool AccountUsesEnvelopes = false;
            public static bool AccountIsCredit = false;

            public static DataGridView DGV;
        }

        private static class EnvLine
        {
            public const string TRANSACTION_ID_NAME = "transactionID";
            public const string LINE_ID_NAME = "lineItemID";
            public const string E_LINE_ID_NAME = "eLineID";
            public const string DATE_NAME = "date";
            public const string LINE_TYPE_NAME = "lineType";
            public const string SOURCE_NAME = "source";
            public const string DESTINATION_NAME = "destination";
            public const string LINE_DESCRIPTION_NAME = "lineDesctription";
            public const string DESCRIPTION_NAME = "desctription";
            public const string CREDIT_AMOUNT_NAME = "creditAmount";
            public const string COMPLETE_NAME = "complete";
            public const string DEBIT_AMOUNT_NAME = "debitAmount";
            public const string BALANCE_AMOUNT_NAME = "balanceAmount";

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
                transactionIDCol.Visible = false;

                // lineItemIDColumn
                lineItemIDCol = new DataGridViewTextBoxColumn();
                lineItemIDCol.Name = LINE_ID_NAME;
                lineItemIDCol.HeaderText = "lineItemID";
                lineItemIDCol.DataPropertyName = "lineItemID";
                lineItemIDCol.Visible = false;

                // idColumn
                idCol = new DataGridViewTextBoxColumn();
                idCol.Name = E_LINE_ID_NAME;
                idCol.HeaderText = "eLineID";
                idCol.DataPropertyName = "eLineID";
                idCol.Visible = false;

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
                lineDescriptionCol.Name = LINE_DESCRIPTION_NAME;
                lineDescriptionCol.HeaderText = "Description";
                lineDescriptionCol.DataPropertyName = "lineDescription";
                lineDescriptionCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                lineDescriptionCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                lineDescriptionCol.FillWeight = 50;
                lineDescriptionCol.Visible = true;

                // subDescriptionColumn
                descriptionCol = new DataGridViewTextBoxColumn();
                descriptionCol.Name = DESCRIPTION_NAME;
                descriptionCol.HeaderText = "Sub Description";
                descriptionCol.DataPropertyName = "description";
                descriptionCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                descriptionCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                descriptionCol.FillWeight = 50;
                descriptionCol.Visible = true;

                // lineTypeslColumn
                lineTypeCol = new DataGridViewTextBoxColumn();
                lineTypeCol.Name = LINE_TYPE_NAME;
                lineTypeCol.HeaderText = "Type";
                lineTypeCol.DataPropertyName = "lineType";
                lineTypeCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                lineTypeCol.Width = 80;
                lineTypeCol.Visible = true;

                // sourceColumn
                sourceCol = new DataGridViewTextBoxColumn();
                sourceCol.Name = SOURCE_NAME;
                sourceCol.HeaderText = "Source";
                sourceCol.DataPropertyName = "sourceAccount";
                sourceCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                sourceCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                sourceCol.FillWeight = 30;
                sourceCol.Visible = true;

                // destinationColumn
                destinationCol = new DataGridViewTextBoxColumn();
                destinationCol.Name = DESTINATION_NAME;
                destinationCol.HeaderText = "Destination";
                destinationCol.DataPropertyName = "destinationAccount";
                destinationCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                destinationCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                destinationCol.FillWeight = 30;
                destinationCol.Visible = true;

                // creditAmountColumn
                creditAmountCol = new DataGridViewTextBoxColumn();
                creditAmountCol.Name = CREDIT_AMOUNT_NAME;
                creditAmountCol.HeaderText = "Credit";
                creditAmountCol.DataPropertyName = "creditAmount";
                creditAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                creditAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                creditAmountCol.Visible = true;
                creditAmountCol.Width = 65;

                // completeColumn
                completeCol = new DataGridViewTextBoxColumn();
                completeCol.Name = COMPLETE_NAME;
                completeCol.HeaderText = "CR";
                completeCol.DataPropertyName = "complete";
                completeCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                completeCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
                completeCol.Visible = true;
                completeCol.Width = 25;

                // debitAmountColumn
                debitAmountCol = new DataGridViewTextBoxColumn();
                debitAmountCol.Name = DEBIT_AMOUNT_NAME;
                debitAmountCol.HeaderText = "Debit";
                debitAmountCol.DataPropertyName = "debitAmount";
                debitAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                debitAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                debitAmountCol.Visible = true;
                debitAmountCol.Width = 65;

                // balanceAmountColumn
                balanceAmountCol = new DataGridViewTextBoxColumn();
                balanceAmountCol.Name = BALANCE_AMOUNT_NAME;
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
            public const string LINE_ID_NAME = "lineID";
            public const string TRANSACTION_ID_NAME = "transactionID";
            public const string DATE_NAME = "date";
            public const string LINE_TYPE_ID_NAME = "lineTypeID";
            public const string OPP_ACCOUNT_ID_NAME = "oppAccountID";
            public const string DESCRIPTION_NAME = "desctription";
            public const string CONFIRMATION_NUM_NAME = "confirmationNum";
            public const string ENVELOPE_ID_NAME = "envelopeID";
            public const string CREDIT_AMOUNT_NAME = "creditAmount";
            public const string COMPLETE_NAME = "complete";
            public const string DEBIT_AMOUNT_NAME = "debitAmount";
            public const string BALANCE_AMOUNT_NAME = "balanceAmount";

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
                idCol.Visible = false;

                // transactionIDColumn
                transactionIDCol = new DataGridViewTextBoxColumn();
                transactionIDCol.Name = TRANSACTION_ID_NAME;
                transactionIDCol.HeaderText = "transactionID";
                transactionIDCol.DataPropertyName = "transactionID";
                transactionIDCol.Visible = false;

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
                lineTypeCol.Name = LINE_TYPE_ID_NAME;
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
                oppAccountIDCol.Name = OPP_ACCOUNT_ID_NAME;
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
                descriptionCol.Name = DESCRIPTION_NAME;
                descriptionCol.HeaderText = "Description";
                descriptionCol.DataPropertyName = "description";
                descriptionCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                descriptionCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                descriptionCol.FillWeight = 200;
                descriptionCol.Visible = true;

                // confirmationNumColumn
                confirmationNumCol = new DataGridViewTextBoxColumn();
                confirmationNumCol.Name = CONFIRMATION_NUM_NAME;
                confirmationNumCol.HeaderText = "Confirmation #";
                confirmationNumCol.DataPropertyName = "confirmationNumber";
                confirmationNumCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                confirmationNumCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                confirmationNumCol.FillWeight = 100;
                confirmationNumCol.Visible = true;

                // envelopeIDColumn
                envelopeIDCol = new DataGridViewComboBoxColumn();
                envelopeIDCol.Name = ENVELOPE_ID_NAME;
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
                creditAmountCol.Name = CREDIT_AMOUNT_NAME;
                creditAmountCol.HeaderText = "Credit";
                creditAmountCol.DataPropertyName = "creditAmount";
                creditAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                creditAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                creditAmountCol.Width = 65;


                // completeColumn
                completeCol = new DataGridViewTextBoxColumn();
                completeCol.Name = COMPLETE_NAME;
                completeCol.HeaderText = "CR";
                completeCol.DataPropertyName = "complete";
                completeCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                completeCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
                completeCol.Width = 25;
                completeCol.ReadOnly = true;

                // debitAmountColumn
                debitAmountCol = new DataGridViewTextBoxColumn();
                debitAmountCol.Name = DEBIT_AMOUNT_NAME;
                debitAmountCol.HeaderText = "Debit";
                debitAmountCol.DataPropertyName = "debitAmount";
                debitAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                debitAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                debitAmountCol.Visible = true;
                debitAmountCol.Width = 65;

                // balanceAmountColumn
                balanceAmountCol = new DataGridViewTextBoxColumn();
                balanceAmountCol.Name = BALANCE_AMOUNT_NAME;
                balanceAmountCol.HeaderText = "Balance";
                balanceAmountCol.DataPropertyName = "balanceAmount";
                balanceAmountCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                balanceAmountCol.DefaultCellStyle = new MyCellStyleMoney();
                balanceAmountCol.ReadOnly = true;
                balanceAmountCol.Visible = true;
                balanceAmountCol.Width = 75;
            }

            public static DataGridView getDGV(ref RegistryDataSet dataSource)
            {
                dgvBindingSource = new BindingSource(dataSource, "LineItem");
                dgvBindingSource.Sort = "date, creditDebit DESC, amount";

                typeColBindingSource = new BindingSource(dataSource.LineType, "");
                typeColBindingSource.Sort = "name";
                oppAccountColBindingSource = new BindingSource(dataSource.Account, "");
                oppAccountColBindingSource.Sort = "name";
                envelopeColBindingSource = new BindingSource(dataSource.Envelope, "");
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
                dgv.AllowUserToAddRows = false; // Turned on else where
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

            public static void setEnvelopeColumnVisible(bool visible)
            {
                envelopeIDCol.Visible = visible;
            }

            public static void setConfermationNumColumnVisible(bool visible)
            {
                confirmationNumCol.Visible = visible;
            }

            public static void setAccountFilter(int accID)
            {
                dgvBindingSource.Filter = "accountID = " + accID.ToString();
            }

            public static void cancelEdit()
            {
                dgvBindingSource.CancelEdit();
            }

            public static void endEdit()
            {
                try
                {
                    dgvBindingSource.EndEdit();
                }
                catch
                {
                }
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private Panel panel;
        private DataGridView liDGV;
        private DataGridView envDGV;
        private RegistryDataSet regDataSet;

        // row flags used in painting cells
        private bool flagTransactionError;
        private bool flagLineError;
        private bool flagAccountError;
        private bool flagFutureDate;
        private bool flagNegativeBalance;
        private bool flagReadOnlyAccount;
        private bool flagReadOnlyEnvelope;

        // Variables to keep track of when a change has been made to a line so that changes can
        // be forwarded to the correct place and saved to the database.
        private bool inRowValidating;
        private const int NO_DIRTY_LINE = -1;
        private int dirtyLineID = NO_DIRTY_LINE;


        ///////////////////////////////////////////////////////////////////////
        //   External Events
        ///////////////////////////////////////////////////////////////////////   
        public event BalanceChangesEventHandler BalanceChanges;
        private void OnBalanceChanges(BalanceChangesEventArgs e)
        {
            if (BalanceChanges != null)
                BalanceChanges(this, e);
        }

       
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void regDataSet_ErrorsFound(object sender, EventArgs e)
        {
            Current.DGV.Refresh();
        }


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

            if (Current.DGV == null)
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
            else if (this.flagLineError && colName == LineItem.ENVELOPE_ID_NAME)
            {
                e.CellStyle.BackColor = System.Drawing.Color.Red;
                toolTipText = "This line amount and its envelope sum need to match.";
            }
            else if (this.flagAccountError && colName == LineItem.OPP_ACCOUNT_ID_NAME)
            {
                e.CellStyle.BackColor = System.Drawing.Color.Red;
                toolTipText = "Please choose an account.";
            }
            else if (this.flagFutureDate)
                e.CellStyle.BackColor = System.Drawing.Color.LightGray;

            // rowNegativeBalance
            if (this.flagNegativeBalance && (colName == LineItem.BALANCE_AMOUNT_NAME || colName == EnvLine.BALANCE_AMOUNT_NAME))
                e.CellStyle.ForeColor = System.Drawing.Color.Red;

            // rowMultipleAccounts
            if (this.flagReadOnlyAccount && colName == LineItem.OPP_ACCOUNT_ID_NAME)
                readOnlyCell = true;

            // rowSplitEnvelope
            if (this.flagReadOnlyEnvelope && colName == LineItem.ENVELOPE_ID_NAME)
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

            if (colName == LineItem.COMPLETE_NAME && Current.DGV == this.liDGV)
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
                this.saveLineEdits();

                int transID = Convert.ToInt32(Current.DGV[LineItem.TRANSACTION_ID_NAME, row].Value);
                int lineID = Convert.ToInt32(Current.DGV[LineItem.LINE_ID_NAME, row].Value);

                TransactionForm tf = new TransactionForm(transID, lineID);
                tf.ShowDialog();
                this.regDataSet.myGetTransactionEdits(transID);

                BalanceChangesEventArgs arg = new BalanceChangesEventArgs(tf.myGetChanges());
                this.OnBalanceChanges(arg);
            }
            else if (Current.DGV == this.envDGV)
            {
                this.saveLineEdits();

                int transID = Convert.ToInt32(Current.DGV[EnvLine.TRANSACTION_ID_NAME, row].Value);
                int lineID = Convert.ToInt32(Current.DGV[EnvLine.LINE_ID_NAME, row].Value);
                int eLineID = Convert.ToInt32(Current.DGV[EnvLine.E_LINE_ID_NAME, e.RowIndex].Value);

                TransactionForm tf = new TransactionForm(transID, lineID, eLineID);
                tf.ShowDialog();
                this.reloadLines(); // <-- get rid of this.

                BalanceChangesEventArgs arg = new BalanceChangesEventArgs(tf.myGetChanges());
                this.OnBalanceChanges(arg);
            }

        }


        private void liDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (Current.DGV == null)
                return;

            // Defaults. Used for new lines.
            this.flagTransactionError = false;
            this.flagLineError = false;
            this.flagNegativeBalance = false;
            this.flagReadOnlyEnvelope = false;
            this.flagReadOnlyAccount = false;
            this.flagFutureDate = false;
            this.flagAccountError = false;

            object value = Current.DGV[LineItem.LINE_ID_NAME, e.RowIndex].Value;
            int lineID = (value == DBNull.Value) ? -1 : Convert.ToInt32(value);
            RegistryDataSet.LineItemRow thisLine = this.regDataSet.LineItem.FindByid(lineID);

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
            int newDirtyLine = Convert.ToInt32(this.liDGV[LineItem.LINE_ID_NAME, e.RowIndex].Value);

            if (this.dirtyLineID == NO_DIRTY_LINE)
                this.dirtyLineID = newDirtyLine;
            
            else if (this.dirtyLineID == newDirtyLine)
                return;

            else
                throw new Exception("This should not happen");
        }

        private void liDGV_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (this.dirtyLineID == NO_DIRTY_LINE) // Means the user didn't change any values. - Nothing to do. -
                return;

            LineItem.endEdit();
            int row = e.RowIndex;
            int oppAccount = Convert.ToInt32(this.liDGV[LineItem.OPP_ACCOUNT_ID_NAME, row].Value);

            if (oppAccount == SpclAccount.NULL)
            { 
                // Means the user didn't enter the required oppAccount. Ask user what to do.
                if (DialogResult.Yes == MessageBox.Show("You have not selected the required Source or Destination.\n\nDo you want to discard this entry?", 
                    "Discard Entry?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error))
                {   
                    // The user said to discard the changes.
                    LineItem.cancelEdit();
                    this.dirtyLineID = NO_DIRTY_LINE;
                }
                else
                {   // The user answerd No or Cancel. Cancel leaving the row
                    e.Cancel = true;
                }
            }
            else if (this.regDataSet.Account.FindByid(oppAccount).closed)
            {
                // Means the user tried to select a closed account
                MessageBox.Show("You have selected a closed account.\n\nPlease open the account first?",
                    "Closed Account", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                // cancel the changes.
                LineItem.cancelEdit();
                this.dirtyLineID = NO_DIRTY_LINE;
                e.Cancel = true;
            }
        }

        private void liDGV_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (this.inRowValidating)
                return;

            this.inRowValidating = true;

            this.saveLineEdits();

            this.inRowValidating = false;
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
                {
                    this.flagFutureDate = true;

                    // Current.DGV.Rows[0].DividerHeight
                }
            }
        }



        ////////////////////////////
        // Context menu events
        private void menu_Opening(object sender, CancelEventArgs e)
        {
            int row = 0;
            int col = 0;
            bool found = false;
            Point clientPt = this.liDGV.PointToClient(Cursor.Position);

            for (row = 0; row < this.liDGV.Rows.Count && !found; row++)
            {
                for (col = 0; col < this.liDGV.Columns.Count && !found; col++)
                {
                    Rectangle rec = this.liDGV.GetCellDisplayRectangle(col, row, true);

                    if (rec.Contains(clientPt))
                    {
                        this.liDGV[col, row].Selected = true;
                        found = true;
                    }
                }
            }

        }

        private void move_Click(object sender, EventArgs e)
        {
        }

        private void delete_Click(object sender, EventArgs e)
        {
            int row = this.liDGV.CurrentRow.Index;
            int transID = Convert.ToInt32(this.liDGV[LineItem.TRANSACTION_ID_NAME, row].Value);

            this.regDataSet.myDeleteTransaction(transID);

            BalanceChangesEventArgs arg = new BalanceChangesEventArgs(this.regDataSet.myGetChanges());
            this.OnBalanceChanges(arg);
        }

        private void duplicate_Click(object sender, EventArgs e)
        {
            int row = this.liDGV.CurrentRow.Index;
            int transID = Convert.ToInt32(this.liDGV[LineItem.TRANSACTION_ID_NAME, row].Value);
            DateTime oldDate = Convert.ToDateTime(this.liDGV[LineItem.DATE_NAME, row].Value);

            DateSelector.DateSelector date = new FamilyFinance2.Forms.DateSelector.DateSelector(oldDate);
            date.ShowDialog();

            if (date.WasDatePicked())
            {
                this.regDataSet.myDuplicateTransaction(transID, date.GetDataPicked());

                BalanceChangesEventArgs arg = new BalanceChangesEventArgs(this.regDataSet.myGetChanges());
                this.OnBalanceChanges(arg);
            }
        }

        private void autoFill_Click(object sender, EventArgs e)
        {
            AutoDistribute.Distribute(Current.AccountID);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void saveLineEdits()
        {
            // Save any changes that might be happening
            if (this.dirtyLineID != NO_DIRTY_LINE)
            {
                LineItem.endEdit();
                this.regDataSet.mySaveSingleLineEdits(this.dirtyLineID);

                BalanceChangesEventArgs arg = new BalanceChangesEventArgs(this.regDataSet.myGetChanges());
                this.OnBalanceChanges(arg);

                this.dirtyLineID = NO_DIRTY_LINE;
            }
        }

        private void buildTheContextMenues()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            menu.Opening += new CancelEventHandler(menu_Opening);

            menu.Items.Add(new ToolStripButton("Move", null, move_Click));
            menu.Items.Add(new ToolStripButton("Delete", null, delete_Click));
            menu.Items.Add(new ToolStripButton("Duplicate", null, duplicate_Click));
            menu.Items.Add(new ToolStripButton("Auto Fill Envelopes", null, autoFill_Click));

            this.liDGV.ContextMenuStrip = menu;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public MultiDataGridView()
        {

            this.inRowValidating = false; 

            this.regDataSet = new RegistryDataSet();
            this.regDataSet.myInit();
            this.regDataSet.ErrorsFound += new EventHandler(regDataSet_ErrorsFound);

            ////////////////////////////////////
            // The DataGridViews
            this.liDGV = LineItem.getDGV(ref this.regDataSet);
            this.envDGV = EnvLine.getDGV(ref this.regDataSet);


            this.liDGV.DataError +=new DataGridViewDataErrorEventHandler(dgv_DataError);
            this.liDGV.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
            this.liDGV.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);
            this.liDGV.CellValueChanged += new DataGridViewCellEventHandler(liDGV_CellValueChanged);
            this.liDGV.RowPrePaint +=new DataGridViewRowPrePaintEventHandler(liDGV_RowPrePaint);
            this.liDGV.RowValidating += new DataGridViewCellCancelEventHandler(liDGV_RowValidating);
            this.liDGV.RowValidated += new DataGridViewCellEventHandler(liDGV_RowValidated);

            this.envDGV.DataError += new DataGridViewDataErrorEventHandler(dgv_DataError);
            this.envDGV.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
            this.envDGV.CellDoubleClick += new DataGridViewCellEventHandler(dgv_CellDoubleClick);
            this.envDGV.RowPrePaint += new DataGridViewRowPrePaintEventHandler(envDGV_RowPrePaint);

            ////////////////////////////////////
            // Panel Setup
            this.panel = new Panel();
            this.panel.Controls.Add(this.liDGV);
            this.panel.Controls.Add(this.envDGV);
            this.panel.Dock = DockStyle.Fill;

            this.panel.ResumeLayout();

            ////////////////////////////////////
            // Panel Setup
            this.buildTheContextMenues();

            ////////////////////////////////////
            // Fill the tables
            this.reloadLineTypes();
            this.reloadAccounts();
            this.reloadEnvelopes();

            //this.setEnvelopeAndAccount(SpclAccount.NULL, SpclEnvelope.NULL);
        }


        public Control getControl()
        {
            return this.panel;
        }

        public void setEnvelopeAndAccount(int accountID, int envelopeID)
        {
            // If no change
            if (accountID == Current.AccountID && envelopeID == Current.EnvelopeID)
                return;

            Current.AccountID = accountID;
            Current.EnvelopeID = envelopeID;

            // If both are null
            if (accountID == SpclAccount.NULL && envelopeID == SpclEnvelope.NULL)
            {
                Current.AccountUsesEnvelopes = false;
                Current.AccountIsCredit = false;
                Current.DGV = null;

                this.liDGV.Visible = false;
                this.liDGV.AllowUserToAddRows = false;
                this.envDGV.Visible = false;
            }
            // If an account
            else if (accountID > SpclAccount.NULL && envelopeID == SpclEnvelope.NULL)
            {
                Current.AccountUsesEnvelopes = this.regDataSet.Account.FindByid(accountID).envelopes;
                Current.AccountIsCredit = !this.regDataSet.Account.FindByid(accountID).creditDebit;
                Current.DGV = this.liDGV;

                LineItem.setNegativeBalanceFormat(Current.AccountIsCredit);
                LineItem.setEnvelopeColumnVisible(Current.AccountUsesEnvelopes);
                LineItem.setAccountFilter(accountID);

                this.regDataSet.myFillLines(accountID);

                this.liDGV.Visible = true;
                this.liDGV.AllowUserToAddRows = true;

                if (this.regDataSet.LineItem.Count > 3)
                    this.liDGV.CurrentCell = this.liDGV[LineItem.DATE_NAME, this.liDGV.RowCount - 2];

                this.envDGV.Visible = false;
            }
            // else and envelope with or with out an account.
            else
            {
                Current.AccountUsesEnvelopes = true;
                Current.AccountIsCredit = false;
                Current.DGV = this.envDGV;

                this.regDataSet.myFillLines(accountID, envelopeID);

                this.liDGV.Visible = false;
                this.liDGV.AllowUserToAddRows = false;

                this.envDGV.Visible = true;
                
                if (this.regDataSet.EnvelopeLineView.Count > 2)
                    this.envDGV.CurrentCell = this.envDGV[EnvLine.DATE_NAME, this.envDGV.RowCount - 1];
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

