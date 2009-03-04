using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2.Custom_Controls
{
    public partial class RegistryPanel : UserControl
    {
        public RegistryPanel()
        {
            InitializeComponent();
        }

        private void lineItemBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.lineItemBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.fFDBDataSet);

        }
    }
}
