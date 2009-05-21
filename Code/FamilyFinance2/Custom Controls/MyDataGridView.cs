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
        protected bool rowError;
        protected bool rowEnvelopeError;
        protected bool rowNegativeBalance;
        protected bool rowSplitEnvelope;
        protected bool rowMultipleAccounts;
        protected bool rowFutureDate;

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
        private void MyDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;

            if (col < 0 || row < 0)
                return;

            string colName = this.Columns[col].Name;
            bool readOnlyCell = false;
            string toolTipText = "";

            // Set the back ground and the tool tip.
            if (this.rowError)
            {
                e.CellStyle.BackColor = CellStyleError.BackColor;
                toolTipText = "This transaction needs attention.";
            }
            else if (this.rowEnvelopeError && colName == "envelopeIDColumn")
            {
                e.CellStyle.BackColor = CellStyleError.BackColor;
                toolTipText = "The sub transactions need attention.";
            }
            else if (this.rowFutureDate)
                e.CellStyle.BackColor = CellStyleFuture.BackColor;

            // rowNegativeBalance
            if (this.rowNegativeBalance && colName == "balanceAmountColumn")
                e.CellStyle.ForeColor = CellStyleMoney.ForeColor;

            // rowMultipleAccounts
            if (this.rowMultipleAccounts && colName == "oppAccountIDColumn")
                readOnlyCell = true;

            // rowSplitEnvelope
            if (this.rowSplitEnvelope && colName == "envelopeIDColumn")
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

            this.CellFormatting += new DataGridViewCellFormattingEventHandler(MyDataGridView_CellFormatting);
            this.DataError += new DataGridViewDataErrorEventHandler(MyDataGridView_DataError);
        }





    }
}
