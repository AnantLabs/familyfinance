using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2
{
    class MyDataGridView : DataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        protected FFDBDataSet fFDBDataSet;

        protected readonly DataGridViewCellStyle CellStyleNormal;
        protected readonly DataGridViewCellStyle CellStyleMoney;
        protected readonly DataGridViewCellStyle CellStyleAlternatingRow;
        protected readonly DataGridViewCellStyle CellStyleError;
        protected readonly DataGridViewCellStyle CellStyleFuture;


        // row flags used in painting cells
        protected bool rowTransactionError;
        protected bool rowLineError;
        protected bool rowNegativeBalance;
        protected bool rowSplitEnvelope;
        protected bool rowMultipleAccounts;

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Properties
        ////////////////////////////////////////////////////////////////////////////////////////////
        public bool ShowTypeColumn
        {
            get { return this.Columns["typeIDColumn"].Visible; }
            set { this.Columns["typeIDColumn"].Visible = value; }
        }

        public bool ShowConfermationColumn
        {
            get { return this.Columns["confermationNumColumn"].Visible; }
            set { this.Columns["confermationNumColumn"].Visible = value; }
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
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void MyDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int lineID = Convert.ToInt32(this["lineItemIDColumn", e.RowIndex].Value);
            FFDBDataSet.LineItemRow thisLine = this.fFDBDataSet.LineItem.FindByid(lineID);
            System.Drawing.Color backColor = this.Rows[e.RowIndex].DefaultCellStyle.BackColor;
            System.Drawing.Color foreColor = this.Rows[e.RowIndex].DefaultCellStyle.ForeColor;

            // Defaults. Used for new lines.
            this.rowTransactionError = false;
            this.rowLineError = false;
            this.rowNegativeBalance = false;
            this.rowSplitEnvelope = false;
            this.rowMultipleAccounts = false;


            if (thisLine != null)
            {
                // Set row Flags
                rowTransactionError = thisLine.transactionError;
                rowLineError = thisLine.lineError;

                if (thisLine.balanceAmount < 0.0m)
                    this.rowNegativeBalance = true;

                if (thisLine.oppAccountID == SpclAccount.MULTIPLE)
                    this.rowMultipleAccounts = true;

                if (thisLine.envelopeID == SpclEnvelope.SPLIT)
                    this.rowSplitEnvelope = true;

                // Set default row back and fore colors
                if (rowTransactionError)
                {
                    backColor = this.CellStyleError.BackColor;
                    foreColor = this.CellStyleError.ForeColor;
                }
                else if (thisLine.date > DateTime.Today) // future Date
                {
                    backColor = this.CellStyleFuture.BackColor;
                    foreColor = this.CellStyleFuture.ForeColor;
                }
            }

            this.Rows[e.RowIndex].DefaultCellStyle.BackColor = backColor;
            this.Rows[e.RowIndex].DefaultCellStyle.ForeColor = foreColor;
        }

        private void MyDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;

            if (col < 0 || row < 0)
                return;
            
            bool readOnlyCell = false;
            string colName = this.Columns[col].Name;
            string toolTipText = "";

            // Row Errors
            if (this.rowTransactionError)
                toolTipText = "This transaction needs attention.";

            else if (colName == "envelopeIDColumn" && this.rowLineError)
            {
                e.CellStyle.BackColor = CellStyleError.BackColor;
                object temp = this[col, row].Value;
                toolTipText = "The sub transactions need attention. ";
            }

            // rowNegativeBalance
            if (colName == "balanceAmountColumn" && this.rowNegativeBalance)
                e.CellStyle.ForeColor = CellStyleMoney.ForeColor;

            // rowMultipleAccounts
            if (colName == "oppAccountIDColumn" && this.rowMultipleAccounts)
                readOnlyCell = true;

            // rowSplitEnvelope
            if (colName == "envelopeIDColumn" && this.rowSplitEnvelope)
                readOnlyCell = true;

            this[col, row].ToolTipText = toolTipText;
            this[col, row].ReadOnly = readOnlyCell;
        }

        private void MyDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string temp = "stop";
        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public MyDataGridView()
        {
            fFDBDataSet = new FFDBDataSet();

            ///////////////////////////////////////////////////////////////////
            // Setup the Cell Styles
            CellStyleNormal = new DataGridViewCellStyle();

            CellStyleMoney = new DataGridViewCellStyle();
            CellStyleMoney.Alignment = DataGridViewContentAlignment.TopRight;
            CellStyleMoney.Format = "C2";

            CellStyleAlternatingRow = new DataGridViewCellStyle();
            //ButtonFace / Control is a nise soft greay color.
            //GradientInactiveCaption is a baby blue color
            //InactiveBorder nice very soft blue color.
            CellStyleAlternatingRow.BackColor = System.Drawing.SystemColors.InactiveBorder;

            CellStyleFuture = new DataGridViewCellStyle();
            CellStyleFuture.BackColor = System.Drawing.Color.LightGray;

            CellStyleError = new DataGridViewCellStyle();
            CellStyleError.BackColor = System.Drawing.Color.Red;


            ///////////////////////////////////////////////////////////////////
            // Build theDataGridView
            this.Name = "theDataGridView";
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.AlternatingRowsDefaultCellStyle = this.CellStyleAlternatingRow;
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

            this.RowPrePaint += new DataGridViewRowPrePaintEventHandler(MyDataGridView_RowPrePaint);
            this.CellFormatting += new DataGridViewCellFormattingEventHandler(MyDataGridView_CellFormatting);
            this.DataError += new DataGridViewDataErrorEventHandler(MyDataGridView_DataError);
        }





    }
}
