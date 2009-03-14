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
            this.lineTypeTableAdapter.Update(this.fFDBDataSet.LineType);
        }

        private void LineTypeForm_Load(object sender, EventArgs e)
        {
            this.lineTypeTableAdapter.Fill(this.fFDBDataSet.LineType);

        }
    }
}
