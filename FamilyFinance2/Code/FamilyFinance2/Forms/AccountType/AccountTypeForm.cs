using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.AccountType
{
    public partial class AccountTypeForm : Form
    {
        ///////////////////////////////////////////////////////////////////////
        //   Local Variables
        ///////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////
        //   Internal Events 
        ///////////////////////////////////////////////////////////////////////
        private void accountTypeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.saveChanges();
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            //TODO: Finish the deleting of a Account type
            MessageBox.Show("Deleting Account Types is not supported yet.", "Not Supported Yet", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void AccountTypeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.saveChanges();
        }


        ///////////////////////////////////////////////////////////////////////
        //   Functions Private 
        ///////////////////////////////////////////////////////////////////////
        private void saveChanges()
        {
            this.accountTypeBindingSource.EndEdit();
            this.accountTypeDataSet.AccountType.myUpdateDB();
        }


        ///////////////////////////////////////////////////////////////////////
        //   Functions Public 
        ///////////////////////////////////////////////////////////////////////
        public AccountTypeForm()
        {
            InitializeComponent();

            this.accountTypeDataSet.AccountType.myFillTable();

            this.accountTypeBindingSource.Filter = "id > 0";
            this.accountTypeBindingSource.Sort = "name";
        }

    }
}
