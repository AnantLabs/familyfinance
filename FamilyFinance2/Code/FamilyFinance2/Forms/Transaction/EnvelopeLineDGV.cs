using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Transaction
{
    class EnvelopeLineDGV : DataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        public BindingSource BindingSourceEnvelopeLineDGV;
        public BindingSource BindingSourceEnvelopeCol;

        private DataGridViewTextBoxColumn envelopeLineIDColumn;
        private DataGridViewComboBoxColumn envelopeIDColumn;
        private DataGridViewTextBoxColumn descriptionColumn;
        private DataGridViewTextBoxColumn amountColumn;

        // Row flags used in painting cells
        public bool flagEnvelopeError;
        public bool flagNegativeAmount;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void EnvelopeLineDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;

            if (col < 0 || row < 0)
                return;

            string colName = this.Columns[col].Name;

            // Set the back ground and the tool tip.
            if (this.flagEnvelopeError && colName == "envelopeIDColumn")
            {
                e.CellStyle.BackColor = System.Drawing.Color.Red;
                this[col, row].ToolTipText = "Please choose an envelope.";
            }

            // rowNegativeBalance
            if (this.flagNegativeAmount && colName == "amountColumn")
                e.CellStyle.ForeColor = System.Drawing.Color.Red;
        }

        
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildTheDataGridView()
        {
            // lineItemIDColumn
            this.envelopeLineIDColumn = new DataGridViewTextBoxColumn();
            this.envelopeLineIDColumn.Name = "EnvelopeLineIDColumn";
            this.envelopeLineIDColumn.HeaderText = "EnvelopeLineID";
            this.envelopeLineIDColumn.DataPropertyName = "id";
            this.envelopeLineIDColumn.Width = 40;
            this.envelopeLineIDColumn.Visible = false;

            // envelopeIDColumn
            this.envelopeIDColumn = new DataGridViewComboBoxColumn();
            this.envelopeIDColumn.Name = "envelopeIDColumn";
            this.envelopeIDColumn.HeaderText = "Envelope";
            this.envelopeIDColumn.DataPropertyName = "envelopeID";
            this.envelopeIDColumn.DataSource = this.BindingSourceEnvelopeCol;
            this.envelopeIDColumn.DisplayMember = "name";
            this.envelopeIDColumn.ValueMember = "id";
            this.envelopeIDColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            this.envelopeIDColumn.DisplayStyleForCurrentCellOnly = true;
            this.envelopeIDColumn.Resizable = DataGridViewTriState.True;
            this.envelopeIDColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.envelopeIDColumn.FillWeight = 50;
            this.envelopeIDColumn.Visible = true;

            // descriptionColumn
            this.descriptionColumn = new DataGridViewTextBoxColumn();
            this.descriptionColumn.Name = "descriptionColumn";
            this.descriptionColumn.HeaderText = "Description";
            this.descriptionColumn.DataPropertyName = "description";
            this.descriptionColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            this.descriptionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionColumn.FillWeight = 100;
            this.descriptionColumn.Visible = true;

            // amountColumn
            this.amountColumn = new DataGridViewTextBoxColumn();
            this.amountColumn.Name = "amountColumn";
            this.amountColumn.HeaderText = "Amount";
            this.amountColumn.DataPropertyName = "amount";
            this.amountColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            this.amountColumn.DefaultCellStyle = new MyCellStyleMoney();
            this.amountColumn.Width = 65;

            // theDataGridView
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.SelectionMode = DataGridViewSelectionMode.CellSelect;
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

            this.DataSource = this.BindingSourceEnvelopeLineDGV;
            this.Columns.AddRange(
                new DataGridViewColumn[] 
                {
                    this.envelopeLineIDColumn,
                    this.envelopeIDColumn,
                    this.descriptionColumn,
                    this.amountColumn
                }
                );
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public EnvelopeLineDGV()
        {
            // Initialise Binding Sources
            this.BindingSourceEnvelopeLineDGV = new BindingSource();
            this.BindingSourceEnvelopeCol = new BindingSource();
            
            this.buildTheDataGridView();

            this.CellFormatting += new DataGridViewCellFormattingEventHandler(EnvelopeLineDGV_CellFormatting);
        }

        public void myHighLightOn()
        {
            if (this.CurrentCell != null)
            {
                this.CurrentCell.Style.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                this.CurrentCell.Style.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            }

            if (this.DefaultCellStyle != null)
            {
                this.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                this.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            }
        }

        public void myHighLightOff()
        {
            if (this.CurrentCell != null)
            {
                this.CurrentCell.Style.SelectionBackColor = System.Drawing.SystemColors.Window;
                this.CurrentCell.Style.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            }

            if (this.DefaultCellStyle != null)
            {
                this.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Window;
                this.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            }
        }


    }
}
