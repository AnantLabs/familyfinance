using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FamilyFinance2.Forms.EditEnvelopes
{
    public partial class EditEnvelopesForm : Form
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local  Variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        MyTreeNode ClosedEnvelopesNode;

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
            // End, save and rebuild filter.
            this.envelopeBindingSource.EndEdit();
            this.eEDataSet.myUpdateEnvelopeDB();
            this.buildEnvelopeTree();
            this.parentEnvelopeBindingSource.Filter = "closed = 0 AND id <> " + SpclEnvelope.SPLIT.ToString() + " AND id <> " + SpclEnvelope.NOENVELOPE.ToString();

            // Find the newest node (Largest ID) and select it
            int largestID = SpclEnvelope.NULL;
            foreach (MyTreeNode node in this.envelopeTreeView.Nodes)
            {
                int temp = node.ID;
                if (temp > largestID)
                    largestID = temp;
            }
            this.envelopeBindingSource.Filter = "id = " + largestID.ToString();
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
            string filter;
            List<int> idList;
            MyTreeNode node = e.Node as MyTreeNode;
            int envelopeID = node.ID;

            // End, save 
            this.envelopeBindingSource.EndEdit();
            this.eEDataSet.myUpdateEnvelopeDB();

            // Rebuild Parent Envelope filter
            idList = this.eEDataSet.Envelope.myGetAllChildEnvelopeIDList(envelopeID);

            filter = "closed = 0";
            filter += " AND id <> " + SpclEnvelope.SPLIT.ToString();
            filter += " AND id <> " + SpclEnvelope.NOENVELOPE.ToString();
            filter += " AND id <> " + envelopeID.ToString();

            foreach (int id in idList)
                filter += " AND id <> " + id.ToString();

            this.parentEnvelopeBindingSource.Filter = filter;
            
            // Enable or disable the combo box
            EEDataSet.EnvelopeRow eRow = this.eEDataSet.Envelope.FindByid(envelopeID);
            if (eRow == null || eRow.closed)
            {
                this.parentEnvelopeComboBox.Enabled = false;
            }
            else
            {
                this.parentEnvelopeComboBox.Enabled = true;
            }
            

            // Set Selected Envelope to the selected envelope
            this.envelopeBindingSource.Filter = "id = " + envelopeID.ToString();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildEnvelopeTree()
        {
            // Add all the root envelopes
            this.envelopeTreeView.Nodes.Clear();
            this.ClosedEnvelopesNode.Nodes.Clear();
            List<int> idList = eEDataSet.Envelope.myGetChildEnvelopeIDList(SpclEnvelope.NULL);

            // Add all the root nodes to the tree
            foreach (int id in idList)
            {
                MyTreeNode newEnvNode = new MyTreeNode(this.eEDataSet.Envelope.FindByid(id).name, id);

                if (this.eEDataSet.Envelope.FindByid(id).closed == false)
                {
                    newEnvNode.NodeFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    newEnvNode.ForeColor = System.Drawing.Color.Black;
                    this.envelopeTreeView.Nodes.Add(newEnvNode);
                    this.addChildEnvelopes(id, ref newEnvNode);
                }
                else
                {
                    newEnvNode.NodeFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    newEnvNode.ForeColor = System.Drawing.Color.DarkSlateGray;
                    this.ClosedEnvelopesNode.Nodes.Add(newEnvNode);
                }
            }

            if (this.envelopeTreeView.Nodes.Count > 0)
                this.envelopeTreeView.ExpandAll();

            this.ClosedEnvelopesNode.NodeFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClosedEnvelopesNode.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.envelopeTreeView.Nodes.Add(this.ClosedEnvelopesNode);

            //this.envelopeTreeView.Sort();
        }

        private void addChildEnvelopes(int parentEnvelopeID, ref MyTreeNode branch)
        {
            // Add all the branch nodes
            List<int> idList = eEDataSet.Envelope.myGetChildEnvelopeIDList(parentEnvelopeID);

            foreach(int id in idList)
            {
                MyTreeNode node = new MyTreeNode(this.eEDataSet.Envelope.FindByid(id).name, id);
                node.NodeFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                node.ForeColor = System.Drawing.Color.Black;
                branch.Nodes.Add(node);
                this.addChildEnvelopes(id, ref node);
            }
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

            this.ClosedEnvelopesNode = new MyTreeNode("Closed Envelopes", -100);
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
                this.parentEnvelopeBindingSource.Filter = "id = -100";
            }
        }

    }
}
