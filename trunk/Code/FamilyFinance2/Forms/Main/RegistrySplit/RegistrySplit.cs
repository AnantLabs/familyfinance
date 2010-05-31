using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;
using FamilyFinance2.Forms.Main.RegistrySplit;

namespace FamilyFinance2.Forms.Main.RegistrySplit
{
    public class RegistySplit
    {
        ///////////////////////////////////////////////////////////////////////
        //   Local Variables
        ///////////////////////////////////////////////////////////////////////
        private Label temp;
        private SplitContainer splitContainer;
        private AccountTLV accountTLV;
        private MultiDataGridView multiDGV;



        ///////////////////////////////////////////////////////////////////////
        //   Internal Events
        ///////////////////////////////////////////////////////////////////////
        private void accountTLV_SelectedAccountEnvelopeChanged(object sender, SelectedAccountEnvelopeChangedEventArgs e)
        {
            temp.Text = "AccountID = " + e.AccountID.ToString() + "  EnvelopeID = " + e.EnvelopeID.ToString();
            this.multiDGV.setEnvelopeAndAccount(e.AccountID, e.EnvelopeID);
        }



        ///////////////////////////////////////////////////////////////////////
        //   Functions Public
        ///////////////////////////////////////////////////////////////////////
        public RegistySplit()
        {
            // SplitContainer
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.splitContainer.SuspendLayout();
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.FixedPanel = FixedPanel.Panel1;
            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.TabIndex = 1;
            this.splitContainer.ResumeLayout();

            // Temp
            this.temp = new Label();
            this.temp.AutoSize = true;
            this.splitContainer.Panel2.Controls.Add(temp);

            // The Account Tree List View
            this.accountTLV = new AccountTLV();
            this.accountTLV.SelectedAccountEnvelopeChanged += new SelectedAccountEnvelopeChangedEventHandler(accountTLV_SelectedAccountEnvelopeChanged);
            this.splitContainer.Panel1.Controls.Add(this.accountTLV.getControls());

            // the Multi Data Grid View
            this.multiDGV = new MultiDataGridView();
            this.splitContainer.Panel2.Controls.Add(this.multiDGV.getControl());
        }

        public Control getControl()
        {
            return this.splitContainer;
        }

        public void setSplit(int val)
        {
            this.splitContainer.SplitterDistance = val;
        }

        public void myReloadAccount()
        {
            multiDGV.reloadAccounts();
            accountTLV.myRebuildAccounts();
        }

        public void myReloadAccountTypes()
        {
            this.accountTLV.myRebuildAccountType();
        }

        public void myReloadEnvelope()
        {
            multiDGV.reloadEnvelopes();
            this.accountTLV.myRebuildEnvelopes();
        }

        public void myReloadLineItem()
        {
            multiDGV.reloadLines();
        }

        public void myReloadLineType()
        {
            multiDGV.reloadLineTypes();
        }

    }
}
