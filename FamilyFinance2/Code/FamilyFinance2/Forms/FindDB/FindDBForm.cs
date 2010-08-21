using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2.Forms.FindDB
{
    public partial class FindDBForm : Form
    {
        public enum OpenResult { Cancel, Found, Create };
        public OpenResult Result;
        public string FileDir;

        public FindDBForm()
        {
            InitializeComponent();
            FileDir = "";
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Result = OpenResult.Cancel;
            this.Close();
        }

        private void findFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Family Finance Database File | " + Properties.Settings.Default.DBFileName;
            open.SupportMultiDottedExtensions = false;
            open.Title = "Find Exsisting Database File";
            open.CheckFileExists = true;
            open.InitialDirectory = Properties.Settings.Default.DataDirectory;

            if (open.ShowDialog() == DialogResult.Cancel)
                return;

            Result = OpenResult.Found;
            FileDir = Path.GetDirectoryName(open.FileName);
            this.Close();
        }

        private void createNewFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "Please locate the directory where the database file should go.";
            //folder.RootFolder = Directory.(Application.ExecutablePath);
            folder.ShowNewFolderButton = true;
            
            if (folder.ShowDialog() == DialogResult.Cancel)
                return;

            Result = OpenResult.Create;
            FileDir = folder.SelectedPath;
            this.Close();
        }
    }
}