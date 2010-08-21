using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FamilyFinance2.SharedElements;
using FamilyFinance2.Forms.AccountType;
using FamilyFinance2.Forms.EditAccounts;
using FamilyFinance2.Forms.EditEnvelopes;
using FamilyFinance2.Forms.LineType;
using FamilyFinance2.Forms.Transaction;
using FamilyFinance2.Forms.Import.Qif;
using FamilyFinance2.Forms.Main.RegistrySplit;

namespace FamilyFinance2.Forms.Main
{
    public partial class MainForm : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Avriables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private RegistySplit regSplit;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void accountTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AccountTypeForm().ShowDialog();
            this.regSplit.myReloadAccountTypes();
        }

        private void transactionTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new LineTypeForm().ShowDialog();
            this.regSplit.myReloadLineType();
        }

        private void envelopesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new EditEnvelopesForm().ShowDialog();
            this.regSplit.myReloadEnvelope();
        }

        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new EditAccountsForm().ShowDialog();
            this.regSplit.myReloadAccount();
        }

        private void importQIFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ImportQIFForm().ShowDialog();
            this.regSplit.myReloadAccount();
            this.regSplit.myReloadAccountTypes();
            this.regSplit.myReloadEnvelope();
            this.regSplit.myReloadLineItem();
            this.regSplit.myReloadLineType();
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
            this.regSplit = new RegistySplit();

            // Repack the controls
            this.Controls.Clear();
            this.Controls.Add(this.regSplit.getControl());
            this.Controls.Add(this.mainToolStrip);

            this.regSplit.setSplit(330);
        }

    }
}
