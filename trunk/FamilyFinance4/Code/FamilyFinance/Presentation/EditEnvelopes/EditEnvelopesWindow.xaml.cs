using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FamilyFinance.Presentation.EditEnvelopes
{
    /// <summary>
    /// Interaction logic for EditEnvelopesWindow.xaml
    /// </summary>
    public partial class EditEnvelopesWindow : Window
    {
        public EditEnvelopesWindow()
        {
            InitializeComponent();
        }

        private void EditEnvelopeGroups_Click(object sender, RoutedEventArgs e)
        {
            EditTypes.EditTypesWindow etWin = new EditTypes.EditTypesWindow(EditTypes.EditTypesVM.Table.EnvelopeGroup);
            etWin.ShowInTaskbar = false;
            etWin.ShowDialog();
        }
    }
}
