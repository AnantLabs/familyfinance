using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2
{
    public partial class MultiDataGridViewControl : UserControl
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        public enum dgv {LineItem, SubLine};

        private SubLineDGV subLineDGV;
        private LineItemDGV lineItemDGV;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Properties
        ////////////////////////////////////////////////////////////////////////////////////////////
        public bool ShowTypeColumn
        {
            get
            {
                if (dgvType == dgv.LineItem)
                    return lineItemDGV.ShowTypeColumn;
                else
                    return subLineDGV.ShowTypeColumn;
            }
            set 
            { 
                this.lineItemDGV.ShowTypeColumn = value;
                this.subLineDGV.ShowTypeColumn = value;
            }
        }

        public bool ShowConfermationColumn
        {
            get { return this.lineItemDGV.ShowConfermationColumn; }
            set { this.lineItemDGV.ShowConfermationColumn = value; }
        }

        public bool ShowEnvelopeColumn
        {
            get { return this.lineItemDGV.ShowEnvelopeColumn; }
            set { this.lineItemDGV.ShowEnvelopeColumn = value; }
        }

        public int CurrentLineID
        {
            get
            {
                if (dgvType == dgv.LineItem)
                    return lineItemDGV.CurrentLineID;
                else
                    return subLineDGV.CurrentLineID;
            }
        }

        public int CurrentTransactionID
        {
            get 
            {
                if (dgvType == dgv.LineItem)
                    return lineItemDGV.CurrentTransactionID;
                else
                    return subLineDGV.CurrentTransactionID;
            }
        }

        public int CurrentSubLineID
        {
            get { return subLineDGV.CurrentSubLineID; }
        }

        private dgv dgvType;
        public dgv CurrentDGVType
        {
            get { return dgvType; }
        }
        

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        //private void Envelope_EnvelopeEndingBalanceChanged(object sender, BalanceChangedEventArgs e)
        //{
        //    int envelopeID = e.EnvelopeID;
        //    decimal newAmount = e.NewAmount;

        //    if (this.dgvType == dgv.SubLine)
        //        if (envelopeID == this.subLineDGV.CurrentEnvelopeID)
        //            if (SpclAccount.NULL == this.subLineDGV.CurrentAccountID)
        //                this.setAccountEnvelope(SpclAccount.NULL, envelopeID);
        //}

        //private void AEBalance_AEBalanceChangedEvent(object sender, BalanceChangedEventArgs e)
        //{
        //    int envelopeID = e.EnvelopeID;
        //    int accountID = e.AccountID;
        //    decimal newAmount = e.NewAmount;

        //    if (this.dgvType == dgv.SubLine)
        //        if (envelopeID == this.subLineDGV.CurrentEnvelopeID)
        //            if (accountID == this.subLineDGV.CurrentAccountID)
        //                this.setAccountEnvelope(accountID, envelopeID);
        //}

        //private void Account_AccountEndingBalanceChangedEvent(object sender, BalanceChangedEventArgs e)
        //{
        //    int accountID = e.AccountID;
        //    decimal newAmount = e.NewAmount;

        //    if (this.dgvType == dgv.LineItem)
        //        if (accountID == this.lineItemDGV.CurrentAccountID)
        //            this.setAccount(accountID);
        //}


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void myInit()
        {
            this.SuspendLayout();

            this.lineItemDGV = new LineItemDGV();
            this.lineItemDGV.Dock = DockStyle.Fill;
            this.lineItemDGV.setAccountID(SpclAccount.NULL);

            //this.subLineDGV = new SubLineDGV();
            //this.subLineDGV.Dock = DockStyle.Fill;
            //this.subLineDGV.Visible = false;
            //this.subLineDGV.setAccountEnvelopeID(SpclAccount.NULL, SpclEnvelope.NULL);

            this.dgvType = dgv.LineItem;
            this.Controls.Add(this.lineItemDGV);
            this.Controls.Add(this.subLineDGV);

            //this.globalDataSet.Account.AccountEndingBalanceChanged += new BalanceChangedEventHandler(Account_AccountEndingBalanceChangedEvent);
            //this.globalDataSet.AEBalance.AEBalanceChanged += new BalanceChangedEventHandler(AEBalance_AEBalanceChangedEvent);
            //this.globalDataSet.Envelope.EnvelopeEndingBalanceChanged += new BalanceChangedEventHandler(Envelope_EnvelopeEndingBalanceChanged);
        

            this.ResumeLayout();
        }

        private void setAccount(short accountID)
        {
            this.dgvType = dgv.LineItem;
            this.lineItemDGV.setAccountID(accountID);
            //this.subLineDGV.Visible = false;
            this.lineItemDGV.Visible = true;
        }

        private void setAccountEnvelope(int accountID, int envelopeID)
        {
            this.dgvType = dgv.SubLine;
            this.subLineDGV.setAccountEnvelopeID(accountID, envelopeID);
            this.lineItemDGV.Visible = false;
            this.subLineDGV.Visible = true;
        }



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public MultiDataGridViewControl()
        {
            myInit();
        }

        ~MultiDataGridViewControl()
        {
            //this.globalDataSet.Account.AccountEndingBalanceChanged -= new BalanceChangedEventHandler(Account_AccountEndingBalanceChangedEvent);
            //this.globalDataSet.AEBalance.AEBalanceChanged -= new BalanceChangedEventHandler(AEBalance_AEBalanceChangedEvent);
            //this.globalDataSet.Envelope.EnvelopeEndingBalanceChanged -= new BalanceChangedEventHandler(Envelope_EnvelopeEndingBalanceChanged);
        }

        public void setEnvelopeAndAccount(short accountID, short envelopeID)
        {
            if (envelopeID == SpclEnvelope.NULL)
                setAccount(accountID);

            else
                setAccountEnvelope(accountID, envelopeID);
        }



    }// END public partial class multiDataGridView
}// END namespace FamilyFinance.Custom_Controls

