using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FamilyFinance2.SharedElements;
using FamilyFinance2.Forms.EditGroup;

namespace FamilyFinance2.Forms.EditEnvelopes
{
    public partial class EditEnvelopesForm : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local  Variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private MyTreeNode ClosedEnvelopesNode;

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void EditEnvelopesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // End and save 
            this.envelopeBindingSource.EndEdit();
            this.eEDataSet.myUpdateEnvelopeDB();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            this.parentEnvelopeComboBox.Enabled = true;
            this.nameTextBox.Enabled = true;
            this.closedCheckBox.Enabled = true;
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            //TODO: Finish the deleting of an Envelope
            MessageBox.Show("Deleting Envelopes is not supported yet.", "Not Supported Yet", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void envelopeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            // End, save and rebuild tree.
            this.envelopeBindingSource.EndEdit();
            this.eEDataSet.myUpdateEnvelopeDB();
            this.buildEnvelopeTree();
        }

        private void envelopeTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MyTreeNode node = e.Node as MyTreeNode;
            int envelopeID = node.ID;

            // End, save 
            this.envelopeBindingSource.EndEdit();
            this.eEDataSet.myUpdateEnvelopeDB();
            
            // Enable or disable the combo box
            EEDataSet.EnvelopeRow eRow = this.eEDataSet.Envelope.FindByid(envelopeID);
            if (eRow == null || envelopeID == SpclEnvelope.NULL)
            {
                this.parentEnvelopeComboBox.Enabled = false;
                this.nameTextBox.Enabled = false;
                this.closedCheckBox.Enabled = false;
            }
            else if (eRow.closed)
            {
                this.parentEnvelopeComboBox.Enabled = false;
                this.nameTextBox.Enabled = true;
                this.closedCheckBox.Enabled = true;
            }
            else
            {
                this.parentEnvelopeComboBox.Enabled = true;
                this.nameTextBox.Enabled = true;
                this.closedCheckBox.Enabled = true;
            }
            

            // Set Selected Envelope to the selected envelope
            this.envelopeBindingSource.Filter = "id = " + envelopeID.ToString();
        }

        private void editEnvelopeGroupTSB_Click(object sender, EventArgs e)
        {
            EditGroupForm egf = new EditGroupForm();
            egf.ShowDialog();

            //this.Changes.Copy(atf.Changes);

            this.eEDataSet.myFillGroupTable();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildEnvelopeTree()
        {
            Font closedFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Color closedForeColor = System.Drawing.Color.DarkSlateGray;

            this.envelopeTreeView.Nodes.Clear();
            this.ClosedEnvelopesNode.Nodes.Clear();

            this.ClosedEnvelopesNode.NodeFont = closedFont;
            this.ClosedEnvelopesNode.ForeColor = closedForeColor;

            // Add all envelopes to the tree
            foreach (EEDataSet.EnvelopeRow eRow in eEDataSet.Envelope)
            {
                if (eRow.id <= SpclEnvelope.NOENVELOPE)
                    continue;

                if (eRow.closed)
                {
                    bool found = false;
                    MyTreeNode newEnv = new MyTreeNode(eRow.name, eRow.id);
                    newEnv.NodeFont = closedFont;
                    newEnv.ForeColor = closedForeColor;

                    // Go throu each node in the closed node and add the new Envelope to the corrolating Envelope Group
                    foreach (MyTreeNode groupNode in this.ClosedEnvelopesNode.Nodes)
                    {
                        if (groupNode.Text == eRow.EnvelopeGroupRow.name)
                        {
                            // We have found the appropriate node: add the Envelope node, there is nothing else to do.
                            groupNode.Nodes.Add(newEnv);
                            found = true;
                        }
                    }

                    // If we get here there was no group node matching the new Envelope.   
                    if (!found)
                    {
                        MyTreeNode newGroupNode = new MyTreeNode(eRow.EnvelopeGroupRow.name, SpclEnvelopeGroup.NULL);
                        newGroupNode.NodeFont = closedFont;
                        newGroupNode.ForeColor = closedForeColor;
                        newGroupNode.Nodes.Add(newEnv);
                        this.ClosedEnvelopesNode.Nodes.Add(newGroupNode);
                    }
                }
                else
                {
                    bool found = false;
                    MyTreeNode newEnv = new MyTreeNode(eRow.name, eRow.id);

                    // Go throu each node in the closed node and add the new Envelope to the corrolating Envelope Group
                    foreach (MyTreeNode groupNode in this.envelopeTreeView.Nodes)
                    {
                        if (groupNode.Text == eRow.EnvelopeGroupRow.name)
                        {
                            // We have found the appropriate node: add the Envelope node, there is nothing else to do.
                            groupNode.Nodes.Add(newEnv);
                            found = true;
                        }
                    }

                    // If we get here there was no type node matching the new account.  
                    if (!found)
                    {
                        MyTreeNode newGroupNode = new MyTreeNode(eRow.EnvelopeGroupRow.name, SpclEnvelopeGroup.NULL);
                        newGroupNode.Nodes.Add(newEnv);
                        this.envelopeTreeView.Nodes.Add(newGroupNode);
                    }
                }

            }

            this.envelopeTreeView.Nodes.Add(this.ClosedEnvelopesNode);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public EditEnvelopesForm()
        {
            InitializeComponent();

            // Initialize dataset
            this.eEDataSet.myInit();
            this.eEDataSet.myFillEnvelopTable();
            this.eEDataSet.myFillGroupTable();

            this.ClosedEnvelopesNode = new MyTreeNode("Closed Envelopes", -10);
            buildEnvelopeTree();

            // set the max length on the name text box.
            this.nameTextBox.MaxLength = this.eEDataSet.Envelope.nameColumn.MaxLength;


            // Select the first node if it is there
            if (this.envelopeTreeView.Nodes.Count > 0)
                this.envelopeTreeView.SelectedNode = this.envelopeTreeView.Nodes[0];
            else
            {
                // else select nothing
                this.envelopeBindingSource.AddNew();
                this.envelopeGroupBindingSource.Filter = "id = -10";
            }
        }

    }
}
