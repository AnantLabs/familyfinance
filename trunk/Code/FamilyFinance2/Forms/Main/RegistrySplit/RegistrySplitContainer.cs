using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;
using Aga.Controls;

namespace FamilyFinance2.Forms.Main
{
    public partial class RegistySplitContainer : UserControl
    {
        private SplitContainer splitContainer;
        private FamilyFinance2.Forms.Main.RegistrySplit.TreeView.AccountBrowser accountBrowser1;
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.accountBrowser1 = new Forms.Main.RegistrySplit.TreeView.AccountBrowser();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.accountBrowser1);
            this.splitContainer.Size = new System.Drawing.Size(924, 554);
            this.splitContainer.SplitterDistance = 305;
            this.splitContainer.TabIndex = 0;
            // 
            // accountBrowser1
            // 
            this.accountBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountBrowser1.Location = new System.Drawing.Point(0, 0);
            this.accountBrowser1.Name = "accountBrowser1";
            this.accountBrowser1.Size = new System.Drawing.Size(305, 554);
            this.accountBrowser1.TabIndex = 0;
            // 
            // RegistySplitContainer
            // 
            this.Controls.Add(this.splitContainer);
            this.Name = "RegistySplitContainer";
            this.Size = new System.Drawing.Size(924, 554);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

    }
}
