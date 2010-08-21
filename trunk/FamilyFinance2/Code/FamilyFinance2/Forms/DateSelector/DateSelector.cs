using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2.Forms.DateSelector
{
    public partial class DateSelector : Form
    {
        public DateSelector()
        {
            InitializeComponent();
        }

        private bool wasDatePicked = false;

        public bool WasDatePicked()
        {
            return this.wasDatePicked;
        }

        public DateTime GetDataPicked()
        {
            return this.monthCalendar.SelectionStart;
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            this.wasDatePicked = true;
            this.Close();
        }

        public DateSelector(DateTime oldDate)
        {
            this.InitializeComponent();

            this.monthCalendar.BoldedDates = new System.DateTime[] { oldDate };
            this.monthCalendar.SelectionStart = oldDate;
            this.monthCalendar.SelectionEnd = oldDate;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.wasDatePicked = false;
            this.Close();
        }
    }
}
