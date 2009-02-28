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
    public partial class LineTypeForm : Form
    {
        public LineTypeForm()
        {
            InitializeComponent();
        }

        private void lineTypeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.lineTypeBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.fFDBDataSet);

        }

        private void LineTypeForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'fFDBDataSet.LineType' table. You can move, or remove it, as needed.
            this.lineTypeTableAdapter.Fill(this.fFDBDataSet.LineType);

        }
    }
}
