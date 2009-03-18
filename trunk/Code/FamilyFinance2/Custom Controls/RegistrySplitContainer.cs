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

        private Label temp;

        ///////////////////////////////////////////////////////////////////////
        //   Internal Events
        ///////////////////////////////////////////////////////////////////////
        private void accountTLV_SelectedAccountEnvelopeChanged(object sender, SelectedAccountEnvelopeChangedEventArgs e)
        {
            temp.Text = "AccountID = " + e.AccountID.ToString() + "  EnvelopeID = " + e.EnvelopeID.ToString();
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
            this.accountTLV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Controls.Add(this.accountTLV);

            this.accountTLV.SelectedAccountEnvelopeChanged += new SelectedAccountEnvelopeChangedEventHandler(accountTLV_SelectedAccountEnvelopeChanged);
        }

        ~RegistySplitContainer()
        {
        }

        public void myRefresh()
        {
            accountTLV.myRefresh();
        }

    }
}
