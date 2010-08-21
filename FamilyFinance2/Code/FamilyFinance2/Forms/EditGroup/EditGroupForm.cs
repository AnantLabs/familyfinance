using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2.Forms.EditGroup
{
    public partial class EditGroupForm : Form
    {
        ///////////////////////////////////////////////////////////////////////
        //   Local Variables
        ///////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////
        //   Internal Events 
        ///////////////////////////////////////////////////////////////////////
        private void envelopeGroupBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.saveChanges();
        }
        
        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            //TODO: Finish the deleting of a Account type
            MessageBox.Show("Deleting Account Types is not supported yet.", "Not Supported Yet", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void EditGroupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.saveChanges();
        }


        ///////////////////////////////////////////////////////////////////////
        //   Functions Private 
        ///////////////////////////////////////////////////////////////////////
        private void saveChanges()
        {
            this.Validate();
            this.envelopeGroupBindingSource.EndEdit();
            this.groupDataSet.EnvelopeGroup.myUpdateDB();
        }


        ///////////////////////////////////////////////////////////////////////
        //   Functions Public 
        ///////////////////////////////////////////////////////////////////////
        public EditGroupForm()
        {
            InitializeComponent();

            this.groupDataSet.EnvelopeGroup.myFillTable();

            this.envelopeGroupBindingSource.Filter = "id > 0";
            this.envelopeGroupBindingSource.Sort = "name";
        }




    }
}
