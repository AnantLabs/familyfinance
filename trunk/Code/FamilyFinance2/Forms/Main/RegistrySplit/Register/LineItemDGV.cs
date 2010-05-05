using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.Forms.Transaction;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main.RegistrySplit.Register
{
    class LineItemDGV : MyDataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private RegistryDataSet regDataSet;

        private bool accountUsesEnvelopes;
        private bool inRowValidating;

        // Binding Sources
        private BindingSource lineItemDGVBindingSource;
        private BindingSource lineTypeColBindingSource;
        private BindingSource oppAccountColBindingSource;
        private BindingSource envelopeColBindingSource;

        // Columns
        private DataGridViewTextBoxColumn lineItemIDColumn;
        private DataGridViewTextBoxColumn transactionIDColumn;
        private CalendarColumn dateColumn;
        private DataGridViewComboBoxColumn typeIDColumn;
        private DataGridViewComboBoxColumn oppAccountIDColumn;
        private DataGridViewTextBoxColumn descriptionColumn;
        private DataGridViewTextBoxColumn confirmationNumColumn;
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

        private int currentAccountID;
        public int CurrentAccountID
        {
            get { return currentAccountID; }
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void LineItemDGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.regDataSet.myEditLine(this.CurrentLineID);
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
            else if (row >= 0)
            {
                int transID = Convert.ToInt32(this[transactionIDColumn.Index, row].Value);
                int lineID = Convert.ToInt32(this[lineItemIDColumn.Index, row].Value);

                TransactionForm tf = new TransactionForm(transID, lineID);
                tf.ShowDialog();
                this.myReloadLineItems(); // <- remove this line
            }

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
            string confNum = Convert.ToString(this[confirmationNumColumn.Index, row].Value);

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

            inRowValidating = false;
        }

        private void LineItemDGV_RowValidated(object sender, DataGridViewCellEventArgs e)
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

        protected override void MyDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int lineID = Convert.ToInt32(this[lineItemIDColumn.Index, e.RowIndex].Value);
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

                if (!thisLine.IsbalanceAmountNull() && thisLine.balanceAmount < 0.0m)
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
            this.lineItemIDColumn.Visible = true;

            // transactionIDColumn
            this.transactionIDColumn = new DataGridViewTextBoxColumn();
            this.transactionIDColumn.Name = "transactionIDColumn";
            this.transactionIDColumn.HeaderText = "transactionID";
            this.transactionIDColumn.DataPropertyName = "transactionID";
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
            this.typeIDColumn.DataPropertyName = "typeID";
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
            this.oppAccountIDColumn.DataSource = this.oppAccountColBindingSource;
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
            this.envelopeIDColumn.DataPropertyName = "envelopeID";
            this.envelopeIDColumn.DataSource = this.envelopeColBindingSource;
            this.envelopeIDColumn.DisplayMember = "name";
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
            this.debitAmountColumn.DefaultCellStyle = new MyCellStyleMoney();
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
            this.creditAmountColumn.DefaultCellStyle = new MyCellStyleMoney();
            this.creditAmountColumn.Width = 65;

            // balanceAmountColumn
            this.balanceAmountColumn = new DataGridViewTextBoxColumn();
            this.balanceAmountColumn.Name = "balanceAmountColumn";
            this.balanceAmountColumn.HeaderText = "Balance";
            this.balanceAmountColumn.DataPropertyName = "balanceAmount";
            this.balanceAmountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.balanceAmountColumn.DefaultCellStyle = new MyCellStyleMoney();
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
                    this.confirmationNumColumn,
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
            this.regDataSet = new RegistryDataSet();
            this.regDataSet.myInit();

            this.inRowValidating = false;
            this.showEnvelopeColumn = true;

            ////////////////////////////////////
            // Fill the tables
            this.myReloadLineTypes();
            this.myReloadAccounts();
            this.myReloadEnvelopes();

            ////////////////////////////////////
            // Setup the Bindings
            this.lineItemDGVBindingSource = new BindingSource(this.regDataSet, "LineItem");

            this.oppAccountColBindingSource = new BindingSource(this.regDataSet, "Account");
            this.oppAccountColBindingSource.Sort = "name";

            this.envelopeColBindingSource = new BindingSource(this.regDataSet, "Envelope");
            this.envelopeColBindingSource.Sort = "name";

            this.lineTypeColBindingSource = new BindingSource(this.regDataSet, "LineType");
            this.lineTypeColBindingSource.Sort = "name";

            this.buildTheDataGridView();

            ////////////////////////////////////
            // Subscribe to event.
            this.CellDoubleClick += new DataGridViewCellEventHandler(LineItemDGV_CellDoubleClick);
            this.RowValidating += new DataGridViewCellCancelEventHandler(LineItemDGV_RowValidating);
            this.RowValidated += new DataGridViewCellEventHandler(LineItemDGV_RowValidated);
            this.CellValueChanged += new DataGridViewCellEventHandler(LineItemDGV_CellValueChanged);
        }

        public void setAccountID(int accountID)
        {
            const int INVALID = 0;

            if (accountID > INVALID)
            {
                this.currentAccountID = accountID;
                this.accountUsesEnvelopes = this.regDataSet.Account.FindByid(accountID).envelopes;
                this.regDataSet.myFillLineItemTablebyAccount(accountID);
                this.ShowEnvelopeColumn = showEnvelopeColumn;
                this.AllowUserToAddRows = true;

            }
            else
            {
                this.accountUsesEnvelopes = false;
                this.regDataSet.myFillLineItemTablebyAccount(SpclAccount.NULL);
                this.currentAccountID = SpclAccount.NULL;
                this.ShowEnvelopeColumn = showEnvelopeColumn;
                this.AllowUserToAddRows = false;
            }
        }

        public void myReloadLineItems()
        {
            this.regDataSet.myFillLineItemTablebyAccount(this.currentAccountID);
        }

        public void myReloadAccounts()
        {
            this.regDataSet.myFillAccountTable();
        }

        public void myReloadEnvelopes()
        {
            this.regDataSet.myFillEnvelopeTable();
        }

        public void myReloadLineTypes()
        {
            this.regDataSet.myFillLineTypeTable();
        }

    }
}
