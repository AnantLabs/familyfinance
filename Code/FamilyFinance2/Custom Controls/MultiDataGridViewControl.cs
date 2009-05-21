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
        private dgv dgvType;
        public dgv CurrentDGVType
        {
            get { return dgvType; }
        }        
        
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


        

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////



        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void setAccount(short accountID)
        {
            this.dgvType = dgv.LineItem;
            this.lineItemDGV.setAccountID(accountID);
            this.lineItemDGV.Visible = true;
            this.subLineDGV.Visible = false;
        }

        private void setAccountEnvelope(short accountID, short envelopeID)
        {
            this.dgvType = dgv.SubLine;
            this.subLineDGV.setAccountEnvelopeID(accountID, envelopeID);
            this.subLineDGV.Visible = true;
            this.lineItemDGV.Visible = false;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public MultiDataGridViewControl()
        {
            this.SuspendLayout();

            this.lineItemDGV = new LineItemDGV();
            this.lineItemDGV.Dock = DockStyle.Fill;
            this.lineItemDGV.setAccountID(SpclAccount.NULL);

            this.subLineDGV = new SubLineDGV();
            this.subLineDGV.Dock = DockStyle.Fill;
            this.subLineDGV.Visible = false;
            this.subLineDGV.setAccountEnvelopeID(SpclAccount.NULL, SpclEnvelope.NULL);

            this.dgvType = dgv.LineItem;
            this.Controls.Add(this.lineItemDGV);
            this.Controls.Add(this.subLineDGV);

            this.ResumeLayout();
        }

        ~MultiDataGridViewControl()
        {
        }

        public void setEnvelopeAndAccount(short accountID, short envelopeID)
        {
            if (envelopeID == SpclEnvelope.NULL)
                setAccount(accountID);

            else
                setAccountEnvelope(accountID, envelopeID);
        }

        public void myReloadAccounts()
        {
            this.lineItemDGV.myReloadAccounts();
        }

        public void myReloadEnvelopes()
        {
            this.lineItemDGV.myReloadEnvelopes();
        }

        public void myReloadLineTypes()
        {
            this.lineItemDGV.myReloadLineTypes();
        }

        public void myReloadLineItems()
        {
            this.lineItemDGV.myReloadLineItems();
            this.subLineDGV.myReloadSubLineView();
        }

    }// END public partial class multiDataGridView
}// END namespace FamilyFinance.Custom_Controls

