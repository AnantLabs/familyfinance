using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2
{
    class SubLineDGV : MyDataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////

        // Binding Sources
        private BindingSource SubLineDGVBindingSource;

        // Columns
        private DataGridViewTextBoxColumn lineItemIDColumn;
        private DataGridViewTextBoxColumn transactionIDColumn;
        private DataGridViewTextBoxColumn dateColumn;
        private DataGridViewTextBoxColumn lineTypeColumn;
        private DataGridViewTextBoxColumn sourceColumn;
        private DataGridViewTextBoxColumn destinationColumn;
        private DataGridViewTextBoxColumn descriptionColumn;
        private DataGridViewTextBoxColumn debitAmountColumn;
        private DataGridViewTextBoxColumn completeColumn;
        private DataGridViewTextBoxColumn creditAmountColumn;
        private DataGridViewTextBoxColumn balanceAmountColumn;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Properties
        ////////////////////////////////////////////////////////////////////////////////////////////
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

        
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildTheDataGridView()
        {
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
            this.lineTypeColumn.Name = "lineTypeslColumn";
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

            // creditAmountColumn
            this.creditAmountColumn = new DataGridViewTextBoxColumn();
            this.creditAmountColumn.Name = "creditAmountColumn";
            this.creditAmountColumn.HeaderText = "Credit";
            this.creditAmountColumn.DataPropertyName = "creditAmount";
            this.creditAmountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.creditAmountColumn.DefaultCellStyle = CellStyleMoney;
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
            this.debitAmountColumn.DefaultCellStyle = CellStyleMoney;
            this.debitAmountColumn.Visible = true;
            this.debitAmountColumn.Width = 65;

            // balanceAmountColumn
            this.balanceAmountColumn = new DataGridViewTextBoxColumn();
            this.balanceAmountColumn.Name = "balanceAmountColumn";
            this.balanceAmountColumn.HeaderText = "Balance";
            this.balanceAmountColumn.DataPropertyName = "balanceAmount";
            this.balanceAmountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.balanceAmountColumn.DefaultCellStyle = CellStyleMoney;
            this.balanceAmountColumn.Visible = true;
            this.balanceAmountColumn.Width = 75;

            // theDataGridView
            this.AllowUserToAddRows = false;
            this.ReadOnly = true;
            this.DataSource = this.SubLineDGVBindingSource;
            this.Columns.AddRange(
                new DataGridViewColumn[] 
                {
                    this.dateColumn,
                    this.lineItemIDColumn,
                    this.transactionIDColumn,
                    this.lineTypeColumn,
                    this.sourceColumn,
                    this.destinationColumn,
                    this.descriptionColumn,
                    this.creditAmountColumn,
                    this.completeColumn,
                    this.debitAmountColumn,
                    this.balanceAmountColumn
                }
                );

            this.CellDoubleClick += new DataGridViewCellEventHandler(SubLineDGV_CellDoubleClick);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public SubLineDGV()
        {
            myReloadTables();

            // Binding Sources
            this.SubLineDGVBindingSource = new BindingSource(this.fFDBDataSet, "SubLineView");

            //this.SuspendLayout();
            this.buildTheDataGridView();
            //this.ResumeLayout();
        }

        public void setAccountEnvelopeID(short accountID, short envelopeID)
        {
            const int INVALID = 0;

            if (envelopeID > INVALID && accountID > INVALID)
            {
                this.currentEnvelopeID = envelopeID;
                this.currentAccountID = accountID;

                this.fFDBDataSet.SubLineView.myFillTAByEnvelopeAndAccount(envelopeID, accountID);                

                //this.debitAmountColumn.HeaderText = this.globalDataSet.Envelope.FindByid(envelopeID).debitColumnName;
                //this.creditAmountColumn.HeaderText = this.globalDataSet.Envelope.FindByid(envelopeID).creditColumnName;
            }
            else if (envelopeID > INVALID)
            {
                this.currentEnvelopeID = envelopeID;
                this.currentAccountID = SpclAccount.NULL;

                this.fFDBDataSet.SubLineView.myFillTAByEnvelope(envelopeID);

                //this.debitAmountColumn.HeaderText = this.globalDataSet.Envelope.FindByid(envelopeID).debitColumnName;
                //this.creditAmountColumn.HeaderText = this.globalDataSet.Envelope.FindByid(envelopeID).creditColumnName;
            }
            else 
            {
                this.currentEnvelopeID = SpclEnvelope.NULL;
                this.currentAccountID = SpclAccount.NULL;

                this.fFDBDataSet.SubLineView.myFillTAByEnvelope(-100);

                this.debitAmountColumn.HeaderText = "Debit";
                this.creditAmountColumn.HeaderText = "Credit";
            }

        }

        public void myReloadTables()
        {
            fFDBDataSet.LineItem.myFill();
        }
    }
}
