using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;
using FamilyFinance2.Forms.Main.RegistrySplit.TreeView;
using FamilyFinance2.Forms.Main.RegistrySplit.Register;

namespace FamilyFinance2.Forms.Main.RegistrySplit
{
    public class RegistySplitContainer : UserControl
    {
        ///////////////////////////////////////////////////////////////////////
        //   Local Variables
        ///////////////////////////////////////////////////////////////////////
        private Label temp;
        private SplitContainer splitContainer;
        private AccountTLV accountTLV;
        private MultiDataGridViewControl multiDGV;



        ///////////////////////////////////////////////////////////////////////
        //   Internal Events
        ///////////////////////////////////////////////////////////////////////
        private void accountTLV_SelectedAccountEnvelopeChanged(object sender, SelectedAccountEnvelopeChangedEventArgs e)
        {
            temp.Text = "AccountID = " + e.AccountID.ToString() + "  EnvelopeID = " + e.EnvelopeID.ToString();
            this.multiDGV.mySetEnvelopeAndAccount(e.AccountID, e.EnvelopeID);
        }
 

        ///////////////////////////////////////////////////////////////////////
        //   Functions Private
        ///////////////////////////////////////////////////////////////////////




        ///////////////////////////////////////////////////////////////////////
        //   Functions Public
        ///////////////////////////////////////////////////////////////////////
        public RegistySplitContainer()
        {
            // This RegistySplitContainer : UserControl
            this.InitializeComponent();
            this.BorderStyle = BorderStyle.None;

            // SplitContainer
            this.splitContainer.FixedPanel = FixedPanel.Panel1;


            this.temp = new Label();
            this.temp.AutoSize = true;
            this.splitContainer.Panel2.Controls.Add(temp);

            // The Account Tree List View
            this.accountTLV = new AccountTLV();
            this.accountTLV.Text = "accountTLV";
            this.accountTLV.Dock = DockStyle.Fill;
            this.accountTLV.SelectedAccountEnvelopeChanged += new SelectedAccountEnvelopeChangedEventHandler(accountTLV_SelectedAccountEnvelopeChanged);
            this.splitContainer.Panel1.Controls.Add(this.accountTLV);
            this.accountTLV.myRebuildTree();

            // the Multi Data Grid View
            this.multiDGV = new MultiDataGridViewControl();
            this.multiDGV.Dock = DockStyle.Fill;
            this.splitContainer.Panel2.Controls.Add(this.multiDGV);


        }

        public void myReloadAccount()
        {
            //multiDGV.myReloadAccounts();
            accountTLV.myRebuildAccounts();
        }

        public void myReloadAccountTypes()
        {
            this.accountTLV.myRebuildAccountType();
        }

        public void myReloadEnvelope()
        {
            //multiDGV.myReloadEnvelopes();
            this.accountTLV.myRebuildEnvelopes();
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
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Size = new System.Drawing.Size(900, 554);
            this.splitContainer.SplitterDistance = 303;
            this.splitContainer.TabIndex = 0;
            // 
            // RegistySplitContainer
            // 
            this.Controls.Add(this.splitContainer);
            this.Name = "RegistySplitContainer";
            this.Size = new System.Drawing.Size(900, 554);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

    }
}
