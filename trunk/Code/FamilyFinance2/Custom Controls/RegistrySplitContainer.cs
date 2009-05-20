using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2
{
    public partial class RegistySplitContainer : SplitContainer
    {    
        ///////////////////////////////////////////////////////////////////////
        //   Local Variables
        ///////////////////////////////////////////////////////////////////////
        private AccountTLV accountTLV;
        private MultiDataGridViewControl multiDGV;

        private Label temp;

        ///////////////////////////////////////////////////////////////////////
        //   Internal Events
        ///////////////////////////////////////////////////////////////////////
        private void accountTLV_SelectedAccountEnvelopeChanged(object sender, SelectedAccountEnvelopeChangedEventArgs e)
        {
            temp.Text = "AccountID = " + e.AccountID.ToString() + "  EnvelopeID = " + e.EnvelopeID.ToString();
            this.multiDGV.setEnvelopeAndAccount(e.AccountID, e.EnvelopeID);
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
            this.Panel2.Controls.Add(temp);

            this.accountTLV = new AccountTLV();
            this.accountTLV.Text = "accountTLV";
            this.accountTLV.Dock = DockStyle.Fill;
            this.Panel1.Controls.Add(this.accountTLV);

            this.multiDGV = new MultiDataGridViewControl();
            this.multiDGV.Dock = DockStyle.Fill;
            this.Panel2.Controls.Add(this.multiDGV);

            this.accountTLV.SelectedAccountEnvelopeChanged += new SelectedAccountEnvelopeChangedEventHandler(accountTLV_SelectedAccountEnvelopeChanged);
            this.SplitterMoved += new SplitterEventHandler(RegistySplitContainer_SplitterMoved);
        }

        void RegistySplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            temp.Text = "Splitter value = " + this.SplitterDistance.ToString();
        }

        ~RegistySplitContainer()
        {
        }

        public void myReloadTables()
        {
            accountTLV.myRebuildTree();
            multiDGV.myReloadTables();
        }
    }
}
