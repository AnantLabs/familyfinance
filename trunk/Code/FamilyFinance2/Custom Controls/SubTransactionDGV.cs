using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2
{
    class SubTransactionDGV : MyDataGridView
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private int currentLineID;

        // Binding Sources
        private BindingSource subLineDGVBindingSource;
        private BindingSource envelopeColBindingSource;

        // Columns
        public DataGridViewTextBoxColumn lineItemIDColumn;
        public DataGridViewComboBoxColumn envelopeIDColumn;
        public DataGridViewTextBoxColumn descriptionColumn;
        public DataGridViewTextBoxColumn amountColumn;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Properties
        ////////////////////////////////////////////////////////////////////////////////////////////



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////

        
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
            this.lineItemIDColumn.Width = 40;
            this.lineItemIDColumn.Visible = false;

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
            this.amountColumn.DefaultCellStyle = this.MyCellStyleMoney;
            this.amountColumn.Width = 65;

            // theDataGridView
            this.AllowUserToAddRows = true;
            this.DataSource = this.subLineDGVBindingSource;
            this.Columns.AddRange(
                new DataGridViewColumn[] 
                {
                    this.envelopeIDColumn,
                    this.lineItemIDColumn,
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
        }

        public void myInit(ref FFDBDataSet ffDataSet)
        {
            fFDBDataSet = ffDataSet;

            // Binding Sources
            this.subLineDGVBindingSource = new BindingSource(this.fFDBDataSet, "SubLineItem");
            this.envelopeColBindingSource = new BindingSource(this.fFDBDataSet, "Envelope");
            this.envelopeColBindingSource.Filter = "id <> " + SpclEnvelope.SPLIT.ToString();

            this.mySetLineID(-1);   // Empty set

            this.buildTheDataGridView();

        }

        public void mySetLineID(int lineID)
        {
            bool lineAccountUsesEnvelopes;
            this.currentLineID = lineID;

            try
            {
                lineAccountUsesEnvelopes = this.fFDBDataSet.LineItem.FindByid(lineID).AccountRowByFK_Line_accountID.envelopes;

                if (lineAccountUsesEnvelopes)
                {
                    this.subLineDGVBindingSource.Filter = "lineItemID = " + lineID.ToString();
                    this.AllowUserToAddRows = true;
                    this.Enabled = true;
                }
            }
            catch { lineAccountUsesEnvelopes = false; }

            if (!lineAccountUsesEnvelopes)
            {
                this.subLineDGVBindingSource.Filter = "id = -1";
                this.AllowUserToAddRows = false;
                this.Enabled = false;
            }

        }



    }
}
