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
        private EnvelopeGroupTableAdapter GroupTA;


        /////////////////////////
        //   Functions Public 
        public void myInit()
        {
            this.EnvelopeTA = new EnvelopeTableAdapter();
            this.EnvelopeTA.ClearBeforeFill = true;

            this.GroupTA = new EnvelopeGroupTableAdapter();
            this.GroupTA.ClearBeforeFill = true;
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

        public void myFillGroupTable()
        {
            this.GroupTA.Fill(this.EnvelopeGroup);
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
                envelopeRow.groupID = SpclEnvelope.NULL;
                envelopeRow.closed = false;

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

                if (e.Column.ColumnName == "name")
                {
                    tmp = e.ProposedValue as string;
                    maxLen = this.nameColumn.MaxLength;

                    if (tmp.Length > maxLen)
                        thisRow.name = tmp.Substring(0, maxLen);
                }

                stayOut = false;
            }



            /////////////////////////
            //   Functions Public 
            public void myResetID()
            {
                this.newID = DBquery.getNewID("id", "Envelope");
            }
        }
    }
}
