using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Transaction
{
    class SubTransactionDGV : DataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        public BindingSource BindingSourceSubLineDGV;
        public BindingSource BindingSourceEnvelopeCol;

        private DataGridViewComboBoxColumn envelopeIDColumn;
        private DataGridViewTextBoxColumn descriptionColumn;
        private DataGridViewTextBoxColumn amountColumn;

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////


        
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildTheDataGridView()
        {
            // envelopeIDColumn
            this.envelopeIDColumn = new DataGridViewComboBoxColumn();
            this.envelopeIDColumn.Name = "envelopeIDColumn";
            this.envelopeIDColumn.HeaderText = "Envelope";
            this.envelopeIDColumn.DataPropertyName = "envelopeID";
            this.envelopeIDColumn.DataSource = this.BindingSourceEnvelopeCol;
            this.envelopeIDColumn.DisplayMember = "fullName";
            this.envelopeIDColumn.ValueMember = "id";
            this.envelopeIDColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            this.descriptionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.descriptionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionColumn.FillWeight = 100;
            this.descriptionColumn.Visible = true;

            // amountColumn
            this.amountColumn = new DataGridViewTextBoxColumn();
            this.amountColumn.Name = "amountColumn";
            this.amountColumn.HeaderText = "Amount";
            this.amountColumn.DataPropertyName = "amount";
            this.amountColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
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

            this.DataSource = this.BindingSourceSubLineDGV;
            this.Columns.AddRange(
                new DataGridViewColumn[] 
                {
                    this.envelopeIDColumn,
                    this.descriptionColumn,
                    this.amountColumn
                }
                );
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public SubTransactionDGV()
        {
            // Initialise Binding Sources
            this.BindingSourceSubLineDGV = new BindingSource();
            this.BindingSourceEnvelopeCol = new BindingSource();
            
            this.buildTheDataGridView();
        }






    }
}
