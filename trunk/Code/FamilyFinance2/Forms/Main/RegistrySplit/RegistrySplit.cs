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
        private SplitContainer splitContainer;
        private AccountTLV accountTLV;
        private MultiDataGridView multiDGV;



        ///////////////////////////////////////////////////////////////////////
        //   Internal Events
        ///////////////////////////////////////////////////////////////////////
        private void accountTLV_SelectedAccountEnvelopeChanged(object sender, SelectedAccountEnvelopeChangedEventArgs e)
        {
            this.multiDGV.setEnvelopeAndAccount(e.AccountID, e.EnvelopeID);
        }

        private void multiDGV_BalanceChanges(object sender, BalanceChangesEventArgs e)
        {
            accountTLV.updateBalance(e.AEChanges);
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

            // The Account Tree List View
            this.accountTLV = new AccountTLV();
            this.splitContainer.Panel1.Controls.Add(this.accountTLV.getControls());
            this.accountTLV.SelectedAccountEnvelopeChanged += new SelectedAccountEnvelopeChangedEventHandler(accountTLV_SelectedAccountEnvelopeChanged);

            // the Multi Data Grid View
            this.multiDGV = new MultiDataGridView();
            this.splitContainer.Panel2.Controls.Add(this.multiDGV.getControl());
            this.multiDGV.BalanceChanges += new BalanceChangesEventHandler(multiDGV_BalanceChanges);
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
            accountTLV.rebuildAccounts();
        }

        public void myReloadAccountTypes()
        {
            this.accountTLV.rebuildAccountType();
        }

        public void myReloadEnvelope()
        {
            multiDGV.reloadEnvelopes();
            this.accountTLV.rebuildEnvelopes();
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
