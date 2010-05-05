using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2.Forms.Import.Qif
{
    public partial class ImportQIFForm : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private ImportQIF importer;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events 
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void ImportQIFForm_Load(object sender, EventArgs e)
        {
            // Close down the form if the worker is null.
            // This means the user pressed cancel when asked for the QIF file.
            if (this.importer == null)
                this.Close();
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            this.doneButton.Enabled = false;
            this.importButton.Enabled = false;

            this.importer.RunAsync();
        }

        private void importer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.taskLabel.Text = e.UserState.ToString();
            this.theProgressBar.Value = e.ProgressPercentage;
        }

        private void importer_WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Exception ex = e.Error;
            if (ex == null) // no error
                this.taskLabel.Text = "Import complete.";
            else
                this.taskLabel.Text = "Error reading the file.";

            this.doneButton.Text = "Done";
            this.doneButton.Enabled = true;
        }
        

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Private Functions
        ////////////////////////////////////////////////////////////////////////////////////////////
        private string myGetFilePath()
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;
            openFile.DefaultExt = ".QIF";
            openFile.Filter = "Quicken Interchange Format (*.QIF)|*.QIF|All files (*.*)|*.*";
            openFile.Multiselect = false;
            openFile.Title = "Import QIF File";

            openFile.ShowDialog();

            return openFile.FileName;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Public Functions
        ////////////////////////////////////////////////////////////////////////////////////////////
        public ImportQIFForm()
        {
            InitializeComponent();

            // Get File, if cancel or invalid this will close on form load because the worker is null
            string file = myGetFilePath();

            if (string.IsNullOrEmpty(file))
                return;

            // Set up the progress bar
            this.theProgressBar.Maximum = 100;
            this.theProgressBar.Minimum = 0;
            this.taskLabel.Text = "Ready to import " + file;


            // Set up the importer
            this.importer = new ImportQIF(file);
            this.importer.ProgressChanged += new ProgressChangedEventHandler(importer_ProgressChanged);
            this.importer.WorkCompleted += new RunWorkerCompletedEventHandler(importer_WorkCompleted);
        }

    }
}