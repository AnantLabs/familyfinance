using System.Data;
using System.Collections.Generic;
using FamilyFinance2.SharedElements;
using FamilyFinance2.Forms.EditEnvelopes.EEDataSetTableAdapters;

namespace FamilyFinance2.Forms.EditEnvelopes 
{

    public partial class EEDataSet 
    {
        //////////////////////////
        //   Local Variables
        private EnvelopeTableAdapter EnvelopeTA;


        /////////////////////////
        //   Functions Public 
        public void myInit()
        {
            this.EnvelopeTA = new EnvelopeTableAdapter();
            this.EnvelopeTA.ClearBeforeFill = true;
        }

        public void myUpdateEnvelopeDB()
        {
            this.EnvelopeTA.Update(this.Envelope);
        }

        public void myFillEnvelopTable()
        {
            this.EnvelopeTA.Fill(this.Envelope);
            this.Envelope.myResetID();
        }


        ///////////////////////////////////////////////////////////////////////
        //   Envelope Data Table 
        ///////////////////////////////////////////////////////////////////////
        public partial class EnvelopeDataTable
        {
            //////////////////////////
            //   Local Variables
            private int newID;
            private bool stayOut;


            //////////////////////////
            //   Overriden Functions 
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(EnvelopeDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(EnvelopeDataTable_ColumnChanged);

                this.newID = 1;
                this.stayOut = false;
            }


            /////////////////////////
            //   Internal Events
            private void EnvelopeDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                stayOut = true;
                EnvelopeRow envelopeRow = e.Row as EnvelopeRow;

                envelopeRow.id = this.newID++;
                envelopeRow.name = "New Envelope";
                envelopeRow.fullName = "New Envelope";
                envelopeRow.parentEnvelope = SpclEnvelope.NULL;
                envelopeRow.closed = false;
                envelopeRow.endingBalance = 0.0m;

                stayOut = false;
            }

            private void EnvelopeDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                if (stayOut)
                    return;

                stayOut = true;
                EnvelopeRow thisRow = e.Row as EnvelopeRow;
                string tmp;
                int maxLen;

                switch (e.Column.ColumnName)
                {
                    case "name":
                        tmp = e.ProposedValue as string;
                        maxLen = this.nameColumn.MaxLength;

                        if (tmp.Length > maxLen)
                            thisRow.name = tmp.Substring(0, maxLen);

                        mySetFullName(ref thisRow);
                        break;

                    case "fullName":
                        tmp = e.ProposedValue as string;
                        maxLen = this.fullNameColumn.MaxLength;

                        if (tmp.Length > maxLen)
                            thisRow.name = tmp.Substring(0, maxLen);
                        break;

                    case "parentEnvelope":
                        mySetFullName(ref thisRow);
                        break;

                    case "closed":
                        if ((e.ProposedValue as bool?) == true)
                        {
                            List<int> idList = this.myGetChildEnvelopeIDList(thisRow.id);

                            foreach (int childID in idList)
                            {
                                EnvelopeRow childRow = this.FindByid(childID);
                                childRow.parentEnvelope = thisRow.parentEnvelope;
                                this.mySetFullName(ref childRow);
                            }

                            thisRow.parentEnvelope = SpclEnvelope.NULL;
                        }
                        break;
                }

                stayOut = false;
            }


            /////////////////////////
            //   Functions Private
            private void mySetFullName(ref EnvelopeRow thisEnvelope)
            {
                if (thisEnvelope == null)
                    return;

                // Get this envelope Full Name and set it
                thisEnvelope.fullName = this.myGetFullName(ref thisEnvelope);

                // Find all the child envelopes and update their full names too.
                List<int> childIDList = this.myGetChildEnvelopeIDList(thisEnvelope.id);

                foreach (int id in childIDList)
                {
                    EnvelopeRow row = this.FindByid(id);
                    mySetFullName(ref row);
                }
            }

            private string myGetFullName(ref EnvelopeRow thisEnvelope)
            {
                if (thisEnvelope.parentEnvelope == SpclEnvelope.NULL)
                    return thisEnvelope.name;

                else
                {
                    EnvelopeRow parent = this.FindByid(thisEnvelope.parentEnvelope);
                    return this.myGetFullName(ref parent) + ": " + thisEnvelope.name;
                }
            }


            /////////////////////////
            //   Functions Public 
            public void myResetID()
            {
                this.newID = FFDataBase.myDBGetNewID("id", "Envelope");
            }

            public List<int> myGetChildEnvelopeIDList(int envelopeID)
            {
                List<int> idList = new List<int>();

                foreach (EnvelopeRow row in this)
                {
                    if (row.id > 0 && row.parentEnvelope == envelopeID)
                        idList.Add(row.id);
                }

                idList.Sort();
                return idList;
            }

            public List<int> myGetAllChildEnvelopeIDList(int envelopeID)
            {
                List<int> idList = myGetChildEnvelopeIDList(envelopeID);
                List<int> temp;

                for (int i = 0; i < idList.Count; i++)
                {
                    temp = myGetAllChildEnvelopeIDList(idList[i]);

                    foreach (int id in temp)
                        idList.Add(id);
                }

                return idList;
            }
        }
    }
}
