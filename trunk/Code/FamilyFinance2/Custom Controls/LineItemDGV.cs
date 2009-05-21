using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2
{
    class LineItemDGV : MyDataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private bool accountUsesEnvelopes;
        private bool inRowValidating;

        // Binding Sources
        private BindingSource lineItemDGVBindingSource;
        private BindingSource lineTypeColBindingSource;
        private BindingSource accountColBindingSource;
        private BindingSource envelopeColBindingSource;

        // Columns
        private DataGridViewTextBoxColumn lineItemIDColumn;
        private DataGridViewTextBoxColumn transactionIDColumn;
        private CalendarColumn dateColumn;
        private DataGridViewComboBoxColumn typeIDColumn;
        private DataGridViewComboBoxColumn oppAccountIDColumn;
        private DataGridViewTextBoxColumn descriptionColumn;
        private DataGridViewTextBoxColumn confermationNumColumn;
        private DataGridViewComboBoxColumn envelopeIDColumn;
        private DataGridViewTextBoxColumn debitAmountColumn;
        private DataGridViewTextBoxColumn completeColumn;
        private DataGridViewTextBoxColumn creditAmountColumn;
        private DataGridViewTextBoxColumn balanceAmountColumn;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Properties
        ////////////////////////////////////////////////////////////////////////////////////////////
        private bool showEnvelopeColumn;
        public bool ShowEnvelopeColumn
        {
            get { return this.envelopeIDColumn.Visible; }
            set 
            {   
                showEnvelopeColumn = value;

                // Does this account use Envelopes? 
                if (this.accountUsesEnvelopes)
                    this.envelopeIDColumn.Visible = value;
                else
                    this.envelopeIDColumn.Visible = false;
            }
        }

        private short currentAccountID;
        public short CurrentAccountID
        {
            get { return currentAccountID; }
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void LineItemDGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //int col = e.ColumnIndex;
            //int row = e.RowIndex;

            //if (row < 0 || col < 0)
            //    return;

            //object cellValue = this[col, row].Value;

            // Check if this is a MULTIPLE value in the OppAccountColumn
            //if (col == oppAccountIDColumn.Index && row >= 0)
            //{
            //    if (Convert.ToInt32(cellValue) == SpclAccount.MULTIPLE)
            //    {
            //        int transID = this.CurrentTransactionID;

            //        theDGVBindingSource.EndEdit();

            //        EditTransactionForm ef = new EditTransactionForm(ref globalDataSet, thisViewID, transID);
            //        ef.ShowDialog();
            //    }
            //}

            //// Check if this is a SPLIT value in the EnvelopeIDColumn
            //else if (col == envelopeIDColumn.Index && row >= 0)
            //{
            //    if (Convert.ToInt32(cellValue) == SpclEnvelope.SPLIT)
            //    {
            //        int lineID = CurrentLineID;

            //        theDGVBindingSource.EndEdit();

            //        EditSubTransactionForm esf = new EditSubTransactionForm(ref globalDataSet, lineID, thisViewID);
            //        esf.ShowDialog();
            //    }
            //}
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

            //else if (col == debitAmountColumn.Index && row == -1 && currentAccountID != -1)
            //{
            //    string oldName = this.globalDataSet.Account.FindByid(currentAccountID).debitColumnName;
            //    ChangeColumnNameForm ccnf = new ChangeColumnNameForm(LineCD.DEBIT, oldName);

            //    ccnf.ShowDialog();

            //    this.debitAmountColumn.HeaderText = ccnf.NewColumnName;
            //    this.globalDataSet.myNewColumnName("Account", currentAccountID, LineCD.DEBIT, ccnf.NewColumnName);  
            //}

            //else if (col == creditAmountColumn.Index && row == -1 && currentAccountID != -1)
            //{
            //    string oldName = this.globalDataSet.Account.FindByid(currentAccountID).creditColumnName;
            //    ChangeColumnNameForm ccnf = new ChangeColumnNameForm(LineCD.CREDIT, oldName);

            //    ccnf.ShowDialog();

            //    this.creditAmountColumn.HeaderText = ccnf.NewColumnName;
            //    this.globalDataSet.myNewColumnName("Account", currentAccountID, LineCD.CREDIT, ccnf.NewColumnName); 
            //}
        }

        private void LineItemDGV_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if(inRowValidating)
                return;

            inRowValidating = true;

            int row = e.RowIndex;
            int typeID = Convert.ToInt32(this[typeIDColumn.Index, row].Value);
            int oppAccount = Convert.ToInt32(this[oppAccountIDColumn.Index, row].Value);
            int envelope = Convert.ToInt32(this[envelopeIDColumn.Index, row].Value);
            string description = Convert.ToString(this[descriptionColumn.Index, row].Value);
            string confNum = Convert.ToString(this[confermationNumColumn.Index, row].Value);

            bool allNull = (
                            typeID == SpclLineType.NULL
                         && envelope == SpclEnvelope.NULL
                         && oppAccount == SpclAccount.NULL
                         && description == ""
                         && confNum == "" 
                         );

            if (allNull) // Means the user didn't enter any values. - Cancel the edit -
                this.lineItemDGVBindingSource.CancelEdit();

            else if (oppAccount == SpclAccount.NULL) // Means the user didn't enter the required feild. Ask user what to do.
            {
                if (DialogResult.Yes == MessageBox.Show("You have not entered the required Source or Destination.\n\nDo you want to discard this entry?", "Discard Entry?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error))
                {   // The user said to discard the changes.
                    this.lineItemDGVBindingSource.CancelEdit();
                }
                else
                {   // The user answerd No or Cancel. Cancel leaving the row
                    e.Cancel = true;
                }
            }
            else if(currentAccountID != SpclAccount.NULL)  // Save the changes
            {
                lineItemDGVBindingSource.EndEdit();
                //this.fFDBDataSet.myCommitSingleLineChanges(lineID, currentAccountID);
                this.fFDBDataSet.LineItem.myFillBalance();
            }

            inRowValidating = false;
        }

        private void LineItemDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int lineID = Convert.ToInt32(this[lineItemIDColumn.Index, e.RowIndex].Value);
            FFDBDataSet.LineItemRow thisLine = this.fFDBDataSet.LineItem.FindByid(lineID);

            // Defaults. Used for new lines.
            this.rowError = false;
            this.rowEnvelopeError = false;
            this.rowNegativeBalance = false;
            this.rowSplitEnvelope = false;
            this.rowMultipleAccounts = false;
            this.rowFutureDate = false;

            if (thisLine != null)
            {
                // Set row Flags
                rowError = thisLine.transactionError;
                rowEnvelopeError = thisLine.lineError;

                if (thisLine.balanceAmount < 0.0m)
                    this.rowNegativeBalance = true;

                if (thisLine.oppAccountID == SpclAccount.MULTIPLE)
                    this.rowMultipleAccounts = true;

                if (thisLine.envelopeID == SpclEnvelope.SPLIT)
                    this.rowSplitEnvelope = true;

                if (thisLine.date > DateTime.Today) // future Date
                    this.rowFutureDate = true;
            }
        }

        
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildTheDataGridView()
        {

            ///////////////////////////////////////////////////////////////////////////////
            // COLUMNS

            // lineItemIDColumn
            this.lineItemIDColumn = new DataGridViewTextBoxColumn();
            this.lineItemIDColumn.Name = "lineItemIDColumn";
            this.lineItemIDColumn.HeaderText = "lineItemID";
            this.lineItemIDColumn.DataPropertyName = "id";
            this.lineItemIDColumn.Visible = false;

            // transactionIDColumn
            this.transactionIDColumn = new DataGridViewTextBoxColumn();
            this.transactionIDColumn.Name = "transactionIDColumn";
            this.transactionIDColumn.HeaderText = "transactionID";
            this.transactionIDColumn.DataPropertyName = "transactionID";
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

            // oppAccountIDColumn
            this.oppAccountIDColumn = new DataGridViewComboBoxColumn();
            this.oppAccountIDColumn.Name = "oppAccountIDColumn";
            this.oppAccountIDColumn.HeaderText = "Source / Destination";
            this.oppAccountIDColumn.DataPropertyName = "oppAccountID";
            this.oppAccountIDColumn.DataSource = this.accountColBindingSource;
            this.oppAccountIDColumn.DisplayMember = "name";
            this.oppAccountIDColumn.ValueMember = "id";
            this.oppAccountIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.oppAccountIDColumn.Resizable = DataGridViewTriState.True;
            this.oppAccountIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.oppAccountIDColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.oppAccountIDColumn.FillWeight = 120;
            this.oppAccountIDColumn.Visible = true;

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

            // debitAmountColumn
            this.debitAmountColumn = new DataGridViewTextBoxColumn();
            this.debitAmountColumn.Name = "debitAmountColumn";
            this.debitAmountColumn.HeaderText = "Debit";
            this.debitAmountColumn.DataPropertyName = "debitAmount";
            this.debitAmountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.debitAmountColumn.DefaultCellStyle = this.CellStyleMoney;
            this.debitAmountColumn.Width = 65;
            
            // completeColumn
            this.completeColumn = new DataGridViewTextBoxColumn();
            this.completeColumn.Name = "completeColumn";
            this.completeColumn.HeaderText = "CR";
            this.completeColumn.DataPropertyName = "complete";
            this.completeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.completeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter;
            this.completeColumn.Width = 25;
            this.completeColumn.ReadOnly = true;

            // creditAmountColumn
            this.creditAmountColumn = new DataGridViewTextBoxColumn();
            this.creditAmountColumn.Name = "creditAmountColumn";
            this.creditAmountColumn.HeaderText = "Credit";
            this.creditAmountColumn.DataPropertyName = "creditAmount";
            this.creditAmountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.creditAmountColumn.DefaultCellStyle = this.CellStyleMoney;
            this.creditAmountColumn.Width = 65;

            // balanceAmountColumn
            this.balanceAmountColumn = new DataGridViewTextBoxColumn();
            this.balanceAmountColumn.Name = "balanceAmountColumn";
            this.balanceAmountColumn.HeaderText = "Balance";
            this.balanceAmountColumn.DataPropertyName = "balanceAmount";
            this.balanceAmountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.balanceAmountColumn.DefaultCellStyle = this.CellStyleMoney;
            this.balanceAmountColumn.Width = 75;
            this.balanceAmountColumn.ReadOnly = true;

            // theDataGridView
            this.DataSource = this.lineItemDGVBindingSource;
            this.Columns.AddRange(
                new DataGridViewColumn[] 
                {
                    this.dateColumn,
                    this.lineItemIDColumn,
                    this.transactionIDColumn,
                    this.typeIDColumn,
                    this.oppAccountIDColumn,
                    this.descriptionColumn,
                    this.confermationNumColumn,
                    this.envelopeIDColumn,
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
        public LineItemDGV()
        {
            this.inRowValidating = false;
            this.showEnvelopeColumn = true;

            ////////////////////////////////////
            // Fill the tables
            this.myReloadLineTypes();
            this.myReloadAccounts();
            this.myReloadEnvelopes();

            ////////////////////////////////////
            // Setup the Bindings
            this.lineItemDGVBindingSource = new BindingSource(this.fFDBDataSet, "LineItem");

            this.accountColBindingSource = new BindingSource(this.fFDBDataSet, "Account");
            this.accountColBindingSource.Sort = "catagoryID, name";

            this.envelopeColBindingSource = new BindingSource(this.fFDBDataSet, "Envelope");

            this.lineTypeColBindingSource = new BindingSource(this.fFDBDataSet, "LineType");
            this.lineTypeColBindingSource.Sort = "name";

            this.buildTheDataGridView();

            ////////////////////////////////////
            // Subscribe to event.
            this.CellDoubleClick += new DataGridViewCellEventHandler(LineItemDGV_CellDoubleClick);
            this.RowValidating += new DataGridViewCellCancelEventHandler(LineItemDGV_RowValidating);
            this.RowPrePaint += new DataGridViewRowPrePaintEventHandler(LineItemDGV_RowPrePaint);
            //this.CellValueChanged += new DataGridViewCellEventHandler(LineItemDGV_CellValueChanged);
        }

        public void setAccountID(short accountID)
        {
            const int INVALID = 0;

            if (accountID > INVALID)
            {
                this.currentAccountID = accountID;
                this.accountUsesEnvelopes = this.fFDBDataSet.Account.FindByid(accountID).envelopes;
                this.fFDBDataSet.LineItem.myFillTAByAccount(accountID);
                this.ShowEnvelopeColumn = showEnvelopeColumn;
                this.AllowUserToAddRows = true;

            }
            else
            {
                this.accountUsesEnvelopes = false;
                this.fFDBDataSet.LineItem.myFillTAByAccount(SpclAccount.NULL);
                this.currentAccountID = SpclAccount.NULL;
                this.ShowEnvelopeColumn = showEnvelopeColumn;
                this.AllowUserToAddRows = false;
            }
        }

        public void myReloadLineItems()
        {
            fFDBDataSet.LineItem.myFillTAByAccount(this.currentAccountID); // an empty set.
        }

        public void myReloadAccounts()
        {
            fFDBDataSet.Account.myFillTA();
        }

        public void myReloadEnvelopes()
        {
            fFDBDataSet.Envelope.myFillTA();
        }

        public void myReloadLineTypes()
        {
            fFDBDataSet.LineType.myFillTA();
        }
    }
}
