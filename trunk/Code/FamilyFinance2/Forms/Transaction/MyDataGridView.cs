using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2.Forms.Transaction 
{
    abstract class MyDataGridView : DataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////

        // row flags used in painting cells
        protected bool flagTransactionError;
        protected bool flagLineError;
        protected bool flagAccountError;
        protected bool flagFutureDate;
        protected bool flagNegativeBalance;
        protected bool flagReadOnlyAccount;
        protected bool flagReadOnlyEnvelope;



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Properties
        ////////////////////////////////////////////////////////////////////////////////////////////
        public bool ShowTypeColumn
        {
            get { return this.Columns["typeIDColumn"].Visible; }
            set  {  this.Columns["typeIDColumn"].Visible = value; }
        }

        public bool ShowConfirmationColumn
        {
            get { return this.Columns["confirmationNumColumn"].Visible; }
            set { this.Columns["confirmationNumColumn"].Visible = value; }
        }

        public int CurrentLineID
        {
            get
            {
                int lineID = -1;

                try
                {
                    lineID = Convert.ToInt32(this.CurrentRow.Cells["lineItemIDColumn"].Value);
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
                    transID = Convert.ToInt32(this.CurrentRow.Cells["transactionIDColumn"].Value);
                }
                catch
                {
                }

                return transID;
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Abstract functions
        ////////////////////////////////////////////////////////////////////////////////////////////
        protected abstract void MyDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e);


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void MyDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;

            if (col < 0 || row < 0)
                return;

            string colName = this.Columns[col].Name;
            bool readOnlyCell = this[col, row].ReadOnly;
            string toolTipText = this[col, row].ToolTipText;

            // Set the back ground and the tool tip.
            if (this.flagTransactionError)
            {
                e.CellStyle.BackColor = System.Drawing.Color.Red;
                toolTipText = "This transaction needs attention.";
            }
            else if (this.flagLineError && (colName == "envelopeIDColumn" || colName == "amountColumn"))
            {
                e.CellStyle.BackColor = System.Drawing.Color.Red;
                toolTipText = "This line amount and its envelope sum need to match.";
            }
            else if (this.flagAccountError && (colName == "accountIDColumn" || colName == "oppAccountIDColumn"))
            {
                e.CellStyle.BackColor = System.Drawing.Color.Red;
                toolTipText = "Please choose an account.";
            }
            else if (this.flagFutureDate)
                e.CellStyle.BackColor = System.Drawing.Color.LightGray;

            // rowNegativeBalance
            if (this.flagNegativeBalance && colName == "balanceAmountColumn")
                e.CellStyle.ForeColor = System.Drawing.Color.Red;

            // rowMultipleAccounts
            if (this.flagReadOnlyAccount && colName == "oppAccountIDColumn")
                readOnlyCell = true;

            // rowSplitEnvelope
            if (this.flagReadOnlyEnvelope && colName == "envelopeIDColumn")
                readOnlyCell = true;

            this[col, row].ToolTipText = toolTipText;
            this[col, row].ReadOnly = readOnlyCell;
        }

        private void MyDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string temp = "stop";
            temp = temp + temp;
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public MyDataGridView()
        {

            ///////////////////////////////////////////////////////////////////
            // Initial Flag values
            this.flagTransactionError = false;
            this.flagLineError = false;
            this.flagAccountError = false;
            this.flagNegativeBalance = false;
            this.flagReadOnlyEnvelope = false;
            this.flagReadOnlyAccount = false;
            this.flagFutureDate = false;

            ///////////////////////////////////////////////////////////////////
            // Build theDataGridView
            this.Name = "theDataGridView";
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //this.AlternatingRowsDefaultCellStyle = this.MyCellStyleAlternatingRow;
            this.Dock = DockStyle.Fill;
            this.AutoGenerateColumns = false;
            this.AllowUserToOrderColumns = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.AllowUserToAddRows = true;
            this.RowHeadersVisible = false;
            this.ShowCellErrors = false;
            this.ShowRowErrors = false;
            this.MultiSelect = false;

            this.DataError += new DataGridViewDataErrorEventHandler(MyDataGridView_DataError);
            this.CellFormatting += new DataGridViewCellFormattingEventHandler(MyDataGridView_CellFormatting);
            this.RowPrePaint += new DataGridViewRowPrePaintEventHandler(MyDataGridView_RowPrePaint);

        }


    }
}
