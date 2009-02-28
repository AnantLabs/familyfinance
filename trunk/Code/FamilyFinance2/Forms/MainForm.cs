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
        public MainForm()
        {
            InitializeComponent();
        }

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
    }
}
