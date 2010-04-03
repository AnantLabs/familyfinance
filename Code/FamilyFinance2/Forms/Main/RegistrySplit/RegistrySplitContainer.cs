using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main
{
    public partial class RegistySplitContainer : UserControl
    {
        private SplitContainer splitContainer1;
        ///////////////////////////////////////////////////////////////////////
        //   Local Variables
        ///////////////////////////////////////////////////////////////////////

        private Label temp;

        ///////////////////////////////////////////////////////////////////////
        //   Internal Events
        ///////////////////////////////////////////////////////////////////////
        private void accountTLV_SelectedAccountEnvelopeChanged(object sender, SelectedAccountEnvelopeChangedEventArgs e)
        {
            temp.Text = "AccountID = " + e.AccountID.ToString() + "  EnvelopeID = " + e.EnvelopeID.ToString();
            //this.multiDGV.setEnvelopeAndAccount(e.AccountID, e.EnvelopeID);
        }
 

        ///////////////////////////////////////////////////////////////////////
        //   Functions Private
        ///////////////////////////////////////////////////////////////////////




        ///////////////////////////////////////////////////////////////////////
        //   Functions Public
        ///////////////////////////////////////////////////////////////////////
        public RegistySplitContainer()
        {
            this.temp = new Label();
            this.temp.AutoSize = true;
            //this.Panel2.Controls.Add(temp);

            // SplitContainer
            this.BorderStyle = BorderStyle.Fixed3D;

            // The Account Tree List View
            //this.accountTLV = new AccountTLV();
            //this.accountTLV.Text = "accountTLV";
            //this.accountTLV.Dock = DockStyle.Fill;
            //this.accountTLV.SelectedAccountEnvelopeChanged += new SelectedAccountEnvelopeChangedEventHandler(accountTLV_SelectedAccountEnvelopeChanged);
            //this.Panel1.Controls.Add(this.accountTLV);

            // the Multi Data Grid View
            //this.multiDGV = new MultiDataGridViewControl();
            //this.multiDGV.Dock = DockStyle.Fill;
            //this.Panel2.Controls.Add(this.multiDGV);


        }

        public void myReloadAccount()
        {
            //multiDGV.myReloadAccounts();
            //accountTLV.myRebuildTree();
        }

        public void myReloadAccountTypes()
        {
            //accountTLV.myRebuildTree();
        }

        public void myReloadEnvelope()
        {
            //multiDGV.myReloadEnvelopes();
            //accountTLV.myRebuildTree();
        }

        public void myReloadLineItem()
        {
            //multiDGV.myReloadLineItems();
        }

        public void myReloadLineType()
        {
            //multiDGV.myReloadLineTypes();
        }

        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(890, 360);
            this.splitContainer1.SplitterDistance = 656;
            this.splitContainer1.TabIndex = 0;
            // 
            // RegistySplitContainer
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "RegistySplitContainer";
            this.Size = new System.Drawing.Size(890, 360);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

    }
}
