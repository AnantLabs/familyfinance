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
    public partial class EditEnvelopesForm : Form
    {

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void EditEnvelopesForm_Load(object sender, EventArgs e)
        {
            this.fFDBDataSet.Envelope.myFill();
            buildEnvelopeTree();

            if (this.envelopeTreeView.Nodes.Count > 0)
                this.envelopeTreeView.SelectedNode = this.envelopeTreeView.Nodes[0];

            else
            {
                this.envelopeBindingSource.AddNew();
                this.parentEnvelopeBindingSource.Filter = "id = -1";
            }
        }

        private void EditEnvelopesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveChanges();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            this.parentEnvelopeBindingSource.Filter = "id <> -2 AND id <> 0";
        }

        private void envelopeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            saveChanges();
        }

        private void envelopeTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string filter;
            List<short> idList;
            MyTreeNode node = e.Node as MyTreeNode;
            short envelopeID = node.ID;

            filter = "id <> -2 AND id <> 0 AND id <> " + envelopeID.ToString();
            idList = this.fFDBDataSet.Envelope.myGetAllChildEnvelopeIDList(envelopeID);

            foreach (short id in idList)
                filter += " AND id <> " + id.ToString();

            this.parentEnvelopeBindingSource.Filter = filter;
            this.envelopeBindingSource.Filter = "id = " + envelopeID.ToString();
                        
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Private
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void buildEnvelopeTree()
        {
            // Add all the root envelopes
            List<short> idList = fFDBDataSet.Envelope.myGetChildEnvelopeIDList(SpclEnvelope.NULL);

            this.envelopeTreeView.Nodes.Clear();

            foreach (short id in idList)
            {
                MyTreeNode node = new MyTreeNode();
                node.Text = this.fFDBDataSet.Envelope.FindByid(id).name;
                node.ID = id;
                this.addBranch(id, ref node);
                this.envelopeTreeView.Nodes.Add(node);
            }

            if (this.envelopeTreeView.Nodes.Count > 0)
                this.envelopeTreeView.ExpandAll();

            this.envelopeTreeView.Sort();
        }

        private void addBranch(short envelopeID, ref MyTreeNode branch)
        {
            // Add all the branch nodes
            List<short> idList = fFDBDataSet.Envelope.myGetChildEnvelopeIDList(envelopeID);

            foreach(short id in idList)
            {
                MyTreeNode node = new MyTreeNode();
                node.Text = this.fFDBDataSet.Envelope.FindByid(id).name;
                node.ID = id;
                branch.Nodes.Add(node);

                this.addBranch(id, ref node);
            }
        }

        private void saveChanges()
        {
            this.Validate();
            this.envelopeBindingSource.EndEdit();
            this.fFDBDataSet.Envelope.mySaveChanges();

            this.buildEnvelopeTree();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Functions Public
        ////////////////////////////////////////////////////////////////////////////////////////////
        public EditEnvelopesForm()
        {
            InitializeComponent();
        }

    }
}
