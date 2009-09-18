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

        protected readonly DataGridViewCellStyle MyCellStyleNormal;
        protected readonly DataGridViewCellStyle MyCellStyleMoney;
        protected readonly DataGridViewCellStyle MyCellStyleAlternatingRow;
        protected readonly DataGridViewCellStyle MyCellStyleError;
        protected readonly DataGridViewCellStyle MyCellStyleFuture;


        // row flags used in painting cells
        protected bool flagTransactionError;
        protected bool flagLineError;
        protected bool flagAccountError;
        protected bool flagNegativeBalance;
        protected bool flagReadOnlyEnvelope;
        protected bool flagReadOnlyAccounts;
        protected bool flagFutureDate;



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Properties
        ////////////////////////////////////////////////////////////////////////////////////////////
        public bool ShowTypeColumn
        {
            get 
            {
                return this.Columns["typeIDColumn"].Visible;
            }
            set 
            { 
                this.Columns["typeIDColumn"].Visible = value; 
            }
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
                e.CellStyle.BackColor = MyCellStyleError.BackColor;
                toolTipText = "This transaction needs attention.";
            }
            else if (this.flagLineError && colName == "envelopeIDColumn")
            {
                e.CellStyle.BackColor = MyCellStyleError.BackColor;
                toolTipText = "The sub lines need attention.";
            }
            else if (this.flagAccountError && colName == "accountIDColumn")
            {
                e.CellStyle.BackColor = MyCellStyleError.BackColor;
                toolTipText = "Please choose an account.";
            }
            else if (this.flagFutureDate)
                e.CellStyle.BackColor = MyCellStyleFuture.BackColor;

            // rowNegativeBalance
            if (this.flagNegativeBalance && colName == "balanceAmountColumn")
                e.CellStyle.ForeColor = MyCellStyleMoney.ForeColor;

            // rowMultipleAccounts
            if (this.flagReadOnlyAccounts && colName == "oppAccountIDColumn")
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
            // Setup the Cell Styles
            MyCellStyleNormal = new DataGridViewCellStyle();
            MyCellStyleNormal.BackColor = System.Drawing.SystemColors.Window;
            MyCellStyleNormal.ForeColor = System.Drawing.SystemColors.ControlText;
            MyCellStyleNormal.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            MyCellStyleNormal.SelectionForeColor = System.Drawing.SystemColors.HighlightText;

            MyCellStyleMoney = new DataGridViewCellStyle();
            MyCellStyleMoney.Alignment = DataGridViewContentAlignment.TopRight;
            MyCellStyleMoney.Format = "C2";

            MyCellStyleAlternatingRow = new DataGridViewCellStyle();
            //ButtonFace / Control is a nise soft greay color.
            //GradientInactiveCaption is a baby blue color
            //InactiveBorder nice very soft blue color.
            MyCellStyleAlternatingRow.BackColor = System.Drawing.SystemColors.InactiveBorder;

            MyCellStyleFuture = new DataGridViewCellStyle();
            MyCellStyleFuture.BackColor = System.Drawing.Color.LightGray;

            MyCellStyleError = new DataGridViewCellStyle();
            MyCellStyleError.BackColor = System.Drawing.Color.Red;

            ///////////////////////////////////////////////////////////////////
            // Initial Flag values
            this.flagTransactionError = false;
            this.flagLineError = false;
            this.flagAccountError = false;
            this.flagNegativeBalance = false;
            this.flagReadOnlyEnvelope = false;
            this.flagReadOnlyAccounts = false;
            this.flagFutureDate = false;

            ///////////////////////////////////////////////////////////////////
            // Build theDataGridView
            this.Name = "theDataGridView";
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.AlternatingRowsDefaultCellStyle = this.MyCellStyleAlternatingRow;
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

            this.CellFormatting += new DataGridViewCellFormattingEventHandler(MyDataGridView_CellFormatting);
            this.DataError += new DataGridViewDataErrorEventHandler(MyDataGridView_DataError);



        }

    }
}
