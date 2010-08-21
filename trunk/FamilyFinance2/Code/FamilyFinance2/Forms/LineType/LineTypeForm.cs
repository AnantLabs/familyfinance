using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.LineType
{
    public partial class LineTypeForm : Form
    {
        ///////////////////////////////////////////////////////////////////////
        //   Local Variables
        ///////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////
        //   Internal Events
        ///////////////////////////////////////////////////////////////////////
        private void lineTypeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.SaveChanges();
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            //TODO: Finish the deleting of a line type
            MessageBox.Show("Deleting Line Types is not supported yet.", "Not Supported Yet", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void LineTypeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveChanges();
        }


        ///////////////////////////////////////////////////////////////////////
        //   Function Private
        ///////////////////////////////////////////////////////////////////////
        private void SaveChanges()
        {
            this.lineTypeBindingSource.EndEdit();
            this.lineTypeDataSet.LineType.myUpdateDB();
        }


        ///////////////////////////////////////////////////////////////////////
        //   Functions Public 
        ///////////////////////////////////////////////////////////////////////
        public LineTypeForm()
        {
            InitializeComponent();
            this.lineTypeDataSet.LineType.myFillTable();

            this.lineTypeBindingSource.Filter = "id > " + SpclLineType.NULL.ToString();
            this.lineTypeBindingSource.Sort = "name";
        }

    }
}
