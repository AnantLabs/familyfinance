using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.Forms;

namespace FamilyFinance2
{
    public partial class MainForm : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Avriables
        ////////////////////////////////////////////////////////////////////////////////////////////
        RegistySplitContainer registrySplitCont;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void accountTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountTypeForm atf = new AccountTypeForm();
            atf.ShowDialog();
        }

        private void transactionTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LineTypeForm ltf = new LineTypeForm();
            ltf.ShowDialog();
        }

        private void envelopesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditEnvelopesForm eef = new EditEnvelopesForm();
            eef.ShowDialog();
            this.registrySplitCont.myRefreshTree();
        }

        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditAccountsForm eaf = new EditAccountsForm();
            eaf.ShowDialog();
            this.registrySplitCont.myRefreshTree();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }
        

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public MainForm()
        {
            FFDBDataSet.myResetAccountBalances();
            FFDBDataSet.myResetEnvelopeBalances();
            FFDBDataSet.myResetAEBalance();

            
            registrySplitCont = new RegistySplitContainer();
            registrySplitCont.Dock = DockStyle.Fill;
            this.Controls.Add(registrySplitCont);

            InitializeComponent();
        }
    }
}
