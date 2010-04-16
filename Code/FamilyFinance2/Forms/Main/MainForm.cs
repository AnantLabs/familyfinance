using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.Forms.AccountType;
using FamilyFinance2.Forms.EditAccounts;
using FamilyFinance2.Forms.EditEnvelopes;
using FamilyFinance2.Forms.LineType;
using FamilyFinance2.Forms.Transaction;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.Main
{
    public partial class MainForm : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Avriables
        ////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void accountTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AccountTypeForm().ShowDialog();
            this.registySplitContainer1.myReloadAccountTypes();
        }

        private void transactionTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new LineTypeForm().ShowDialog();
            this.registySplitContainer1.myReloadLineType();
        }

        private void envelopesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new EditEnvelopesForm().ShowDialog();
            this.registySplitContainer1.myReloadEnvelope();
        }

        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new EditAccountsForm().ShowDialog();
            this.registySplitContainer1.myReloadAccount();
        }
        

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public MainForm()
        {



            InitializeComponent();
        }

    }
}
