using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2.Forms
{
    public partial class TransactionForm : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private FFDBDataSet fFDBDataSet;

        private CDLinesDGV creditDGV;
        private CDLinesDGV debitDGV;



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void debitDGV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int lineID = Convert.ToInt32(this.debitDGV["lineItemIDColumn", e.RowIndex].Value);
                this.subTransactionDGV1.mySetLineID(lineID);
            }
        }

        private void creditDGV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int lineID = Convert.ToInt32(this.creditDGV["lineItemIDColumn", e.RowIndex].Value);
                this.subTransactionDGV1.mySetLineID(lineID);
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void myReloadLineTypes()
        {
            fFDBDataSet.LineType.myFillTA();
        }
        
        private void myReloadAccounts()
        {
            fFDBDataSet.Account.myFillTA();
        }

        private void myReloadEnvelopes()
        {
            fFDBDataSet.Envelope.myFillTA();
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public TransactionForm(int transactionID)
        {
            InitializeComponent();

            ////////////////////////////////////
            // Initialize the dataset and fill the appropriate tables.
            this.fFDBDataSet = new FFDBDataSet();
            this.myReloadLineTypes();
            this.myReloadAccounts();
            this.myReloadEnvelopes();
            this.fFDBDataSet.LineItem.myFillByTransaction(transactionID);
            this.fFDBDataSet.SubLineItem.myFillTAByTransactionID(transactionID);

            // Debit DGV (Bottom)
            this.debitDGV = new CDLinesDGV(LineCD.DEBIT, ref this.fFDBDataSet);
            this.debitDGV.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.debitDGV, 0, 1);
            this.debitDGV.RowEnter += new DataGridViewCellEventHandler(debitDGV_RowEnter);

            // Credit DGV (Top)
            this.creditDGV = new CDLinesDGV(LineCD.CREDIT, ref this.fFDBDataSet);
            this.creditDGV.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.creditDGV, 0, 0);
            this.creditDGV.RowEnter += new DataGridViewCellEventHandler(creditDGV_RowEnter);

            // SubTransactions
            this.subTransactionDGV1.myInit(ref fFDBDataSet);
            this.debitDGV_RowEnter(this, new DataGridViewCellEventArgs(-1, -1));

        }

    }
}
