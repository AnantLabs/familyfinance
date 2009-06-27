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
        private readonly int thisTransactionID;

        private CDLinesDGV creditDGV;
        private CDLinesDGV debitDGV;
        private SubTransactionDGV subTransactionDGV1;
        
        private int currentLineID;
        private int CurrentLineID
        {
            get { return currentLineID; }
            set 
            {
                currentLineID = value;
                this.subTransactionDGV1.mySetLineID(value);
            }
        }
        
        private decimal creditSum;
        private decimal debitSum;
        private decimal subLineSum;
        private decimal lineAmount;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void creditDGV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.CurrentLineID = Convert.ToInt32(this.creditDGV["lineItemIDColumn", e.RowIndex].Value);
            }

            creditDGV.myHighlightOn();
            debitDGV.myHighlightOff();

            myResetValues();
        }

        private void debitDGV_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.CurrentLineID = Convert.ToInt32(this.debitDGV["lineItemIDColumn", e.RowIndex].Value);
            }

            debitDGV.myHighlightOn();
            creditDGV.myHighlightOff();

            myResetValues();
        }


        private void creditDGV_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {              
            int transIDcol = this.creditDGV.transactionIDColumn.Index;
            int cdCol = this.creditDGV.creditDebitColumn.Index;
            int amountCol = this.creditDGV.amountColumn.Index;
            int row = this.creditDGV.Rows.Count - 2;
            decimal difference = this.debitSum - this.creditSum;

            myResetValues();

            if (Convert.ToInt32(this.creditDGV[transIDcol, row].Value) != this.thisTransactionID)
                this.creditDGV[transIDcol, row].Value = this.thisTransactionID;

            if (Convert.ToBoolean(this.creditDGV[cdCol, row].Value) != LineCD.CREDIT)
                this.creditDGV[cdCol, row].Value = LineCD.CREDIT;

            if (difference > 0.0m)
                this.creditDGV[amountCol, row].Value = difference;
            else
                this.creditDGV[amountCol, row].Value = 0.0m;
        }

        private void debitDGV_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            int transIDcol = this.debitDGV.transactionIDColumn.Index;
            int cdCol = this.debitDGV.creditDebitColumn.Index;
            int amountCol = this.debitDGV.amountColumn.Index;
            int row = this.creditDGV.Rows.Count - 2;
            decimal difference = this.creditSum - this.debitSum;

            myResetValues();

            if (Convert.ToInt32(this.debitDGV[transIDcol, row].Value) != this.thisTransactionID)
                this.debitDGV[transIDcol, row].Value = this.thisTransactionID;

            if (Convert.ToBoolean(this.debitDGV[cdCol, row].Value) != LineCD.DEBIT)
                this.debitDGV[cdCol, row].Value = LineCD.DEBIT;

            if (difference > 0.0m)
                this.debitDGV[amountCol, row].Value = difference;
            else
                this.debitDGV[amountCol, row].Value = 0.0m;
        }

        private void subTransactionDGV_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            for (int row = 0; row < this.subTransactionDGV1.Rows.Count - 1; row++)
            {
                if (this.subTransactionDGV1[this.subTransactionDGV1.lineItemIDColumn.Index, row].Value == DBNull.Value)
                    this.subTransactionDGV1[this.subTransactionDGV1.lineItemIDColumn.Index, row].Value = this.currentLineID;
            }

            myResetValues();
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            this.fFDBDataSet.mySaveTransaction();
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

        private void myResetValues()
        {
            /////////////////////////////////
            // Update the Source and Destination sums.
            creditSum = this.fFDBDataSet.LineItem.myGetTransCDSum(thisTransactionID, LineCD.CREDIT);
            debitSum = this.fFDBDataSet.LineItem.myGetTransCDSum(thisTransactionID, LineCD.DEBIT);
            sourceSumLabel.Text = creditSum.ToString("C2");
            destinationSumLabel.Text = debitSum.ToString("C2");

            if (creditSum != debitSum)
                sourceSumLabel.ForeColor = destinationSumLabel.ForeColor = System.Drawing.Color.Red;
            else
                sourceSumLabel.ForeColor = destinationSumLabel.ForeColor = System.Drawing.SystemColors.ControlText;

            this.fFDBDataSet.myCheckTransaction(thisTransactionID);

            /////////////////////////////////
            // Update the subLineSum and lineAmount.
            if (this.currentLineID == 0)
            {
                subLineSumLabel.Enabled = false;
                lineAmountLabel.Enabled = false;
                subLineSumLabel.Text = "$0.00";
                lineAmountLabel.Text = "$0.00";
            }
            try
            {
                bool envelopes = this.fFDBDataSet.LineItem.FindByid(this.currentLineID).AccountRowByFK_Line_accountID.envelopes;
                subLineSum = this.fFDBDataSet.SubLineItem.mySubLineSum(this.currentLineID);
                lineAmount = this.fFDBDataSet.LineItem.FindByid(this.currentLineID).amount;

                if (envelopes)
                {
                    subLineSumLabel.Enabled = true;
                    lineAmountLabel.Enabled = true;
                    subLineSumLabel.Text = subLineSum.ToString("C2");
                    lineAmountLabel.Text = lineAmount.ToString("C2");

                    if (lineAmount != subLineSum)
                        lineAmountLabel.ForeColor = subLineSumLabel.ForeColor = System.Drawing.Color.Red;
                    else
                        lineAmountLabel.ForeColor = subLineSumLabel.ForeColor = System.Drawing.SystemColors.ControlText;
                }
                else
                {
                    subLineSumLabel.Enabled = false;
                    lineAmountLabel.Enabled = false;
                    subLineSumLabel.Text = "$0.00";
                    lineAmountLabel.Text = "$0.00";
                }
            }
            catch { return; }
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public TransactionForm(int transactionID)
        {
            InitializeComponent();
            this.thisTransactionID = transactionID;

            ////////////////////////////////////
            // Initialize the dataset and fill the appropriate tables.
            this.fFDBDataSet = new FFDBDataSet();
            this.myReloadLineTypes();
            this.myReloadAccounts();
            this.myReloadEnvelopes();
            this.fFDBDataSet.LineItem.myFillByTransaction(transactionID);
            this.fFDBDataSet.SubLineItem.myFillTAByTransactionID(transactionID);

            // Credit DGV (Top)
            this.creditDGV = new CDLinesDGV(transactionID, LineCD.CREDIT, ref this.fFDBDataSet);
            this.creditDGV.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.creditDGV, 0, 1);
            this.transactionLayoutPanel.SetColumnSpan(this.creditDGV, 6);
            this.creditDGV.RowEnter += new DataGridViewCellEventHandler(creditDGV_RowEnter);
            this.creditDGV.UserAddedRow += new DataGridViewRowEventHandler(creditDGV_UserAddedRow);

            // Debit DGV (Bottom)
            this.debitDGV = new CDLinesDGV(transactionID, LineCD.DEBIT, ref this.fFDBDataSet);
            this.debitDGV.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.debitDGV, 0, 3);
            this.transactionLayoutPanel.SetColumnSpan(this.debitDGV, 6);
            this.debitDGV.RowEnter += new DataGridViewCellEventHandler(debitDGV_RowEnter);
            this.debitDGV.UserAddedRow += new DataGridViewRowEventHandler(debitDGV_UserAddedRow);

            // SubTransactions
            this.subTransactionDGV1 = new SubTransactionDGV();
            this.subTransactionDGV1.Dock = DockStyle.Fill;
            this.transactionLayoutPanel.Controls.Add(this.subTransactionDGV1, 0, 5);
            this.transactionLayoutPanel.SetRowSpan(this.subTransactionDGV1, 3);
            this.subTransactionDGV1.myInit(ref fFDBDataSet);
            this.subTransactionDGV1.UserAddedRow += new DataGridViewRowEventHandler(subTransactionDGV_UserAddedRow);


            //this.CurrentLineID = this.debitDGV.CurrentLineID;
        }

        

    }
}
