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
        private void LineItemDGV_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            //int lineID = Convert.ToInt32(this["lineItemIDColumn", e.RowIndex].Value);
            //FamilyFinanceDBDataSet.LineItemRow thisLine = this.globalDataSet.LineItem.FindByid(lineID);
            //System.Drawing.Color backColor;
            //System.Drawing.Color foreColor;

            //// Defaults. Used for new lines.
            //this.rowNegativeBalance = false;
            //this.rowLineError = false;
            //this.rowMultipleAccounts = false;
            //this.rowSplitEnvelope = false;
            //bool rowTransactionError = false;
            //this.rowErrorText = "";
            //backColor = this.globalDataSet.MyColors.DGVBack;
            //foreColor = this.globalDataSet.MyColors.DGVFore;

            //if (thisLine != null)
            //{
            //    // Set row Flags
            //    rowTransactionError = thisLine.transactionError;
            //    this.rowLineError = thisLine.lineError;

            //    if (thisLine.balanceAmount < 0.0m)
            //        this.rowNegativeBalance = true;

            //    if (thisLine.oppAccountID == thisLine.accountID)
            //        this.rowMultipleAccounts = true;

            //    if (thisLine.oppAccountID == SpclAccount.MULTIPLE)
            //        this.rowMultipleAccounts = true;

            //    if (thisLine.envelopeID == SpclEnvelope.SPLIT)
            //        this.rowSplitEnvelope = true;

            //    // Set default row back and fore colors
            //    if (rowTransactionError)
            //    {
            //        backColor = this.globalDataSet.MyColors.DGVErrorBack;
            //        foreColor = this.globalDataSet.MyColors.DGVErrorFore;
            //        this.rowErrorText = "This transaction needs attention. ";
            //    }
            //    else if (thisLine.date > DateTime.Today) // future Date
            //    {
            //        backColor = this.globalDataSet.MyColors.DGVFutureBack;
            //        foreColor = this.globalDataSet.MyColors.DGVFutureFore;
            //    }
            //    else // else this is a past date
            //    {
            //        if (e.RowIndex % 2 == 0) // if this is an even number set normal
            //        {
            //            backColor = this.globalDataSet.MyColors.DGVBack;
            //            foreColor = this.globalDataSet.MyColors.DGVFore;
            //        }
            //        else // else set alternating color
            //        {
            //            backColor = this.globalDataSet.MyColors.DGVAlternateBack;
            //            foreColor = this.globalDataSet.MyColors.DGVAlternateFore;
            //        }
            //    }

            //} // END if (lineRow != null)


            //this.Rows[e.RowIndex].DefaultCellStyle.BackColor = backColor;
            //this.Rows[e.RowIndex].DefaultCellStyle.ForeColor = foreColor;
        }

        private void LineItemDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //int col = e.ColumnIndex;
            //int row = e.RowIndex;
            //bool readOnlyCell = false;

            //// rowLineError
            //if (col == this.envelopeIDColumn.Index && this.rowLineError)
            //{
            //    e.CellStyle.BackColor = this.fFDBDataSet.MyColors.DGVErrorBack;
            //    e.CellStyle.ForeColor = this.fFDBDataSet.MyColors.DGVErrorFore;
            //    this.rowErrorText += "The sub transactions need attention. ";
            //}
            
            //// rowNegativeBalance
            //if (col == balanceAmountColumn.Index && this.rowNegativeBalance)
            //{
            //    e.CellStyle.ForeColor = this.fFDBDataSet.MyColors.DGVNegativeBalanceFore;
            //}

            //// rowMultipleAccounts
            //if (col == this.oppAccountIDColumn.Index && this.rowMultipleAccounts)
            //    readOnlyCell = true;

            //// rowSplitEnvelope
            //if (col == this.envelopeIDColumn.Index && this.rowSplitEnvelope)
            //    readOnlyCell = true;

            //this[col, row].ToolTipText = this.rowErrorText;
            //this[col, row].ReadOnly = readOnlyCell;
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

            // Setup the Cell Styles
            CellStyleNormal = new DataGridViewCellStyle();

            CellStyleMoney = new DataGridViewCellStyle();
            CellStyleMoney.Alignment = DataGridViewContentAlignment.TopRight;
            CellStyleMoney.Format = "C2";
            CellStyleMoney.NullValue = null;

            CellStyleAlternatingRow = new DataGridViewCellStyle();
            //ButtonFace / Control is a nise soft greay color.
            //GradientInactiveCaption is a baby blue color
            //InactiveBorder nice very soft blue color.
            CellStyleAlternatingRow.BackColor = System.Drawing.SystemColors.InactiveBorder;

            CellStyleFuture = new DataGridViewCellStyle();
            CellStyleFuture.BackColor = System.Drawing.Color.LightGray;

            CellStyleError = new DataGridViewCellStyle();
            CellStyleError.BackColor = System.Drawing.Color.Red;



            // Build theDataGridView
            this.Name = "theDataGridView";
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
            this.Dock = DockStyle.Fill;
            this.AutoGenerateColumns = false;
            this.AllowUserToOrderColumns = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.AllowUserToAddRows = true;
            this.RowHeadersVisible = false;
            this.ShowCellErrors = true;
            this.ShowRowErrors = true;
            this.MultiSelect = false;
            this.AlternatingRowsDefaultCellStyle = this.CellStyleNormal;
            //this.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Blue;
        }



    }
}
