using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2.Forms
{
    public partial class AccountTypeForm : Form
    {
        public AccountTypeForm()
        {
            InitializeComponent();
        }

        private void TypeForm_Load(object sender, EventArgs e)
        {
            this.accountTypeTableAdapter.Fill(this.fFDBDataSet.AccountType);

        }

        private void accountTypeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.accountTypeBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.fFDBDataSet);

        }
    }
}
