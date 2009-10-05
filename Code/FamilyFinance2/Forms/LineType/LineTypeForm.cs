using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2.Forms.LineType
{
    public partial class LineTypeForm : Form
    {
        ///////////////////////////////////////////////////////////////////////
        //   Local Variables
        ///////////////////////////////////////////////////////////////////////
        public Changes Changes;

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
            this.Changes.AddTable(DBTables.LineType);
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

            this.Changes = new Changes();

            this.lineTypeBindingSource.Filter = "id > " + SpclLineType.NULL.ToString();
            this.lineTypeBindingSource.Sort = "name";
        }

    }
}
