using System.Data;
using FamilyFinance2.Forms.EditGroup.GroupDataSetTableAdapters;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.EditGroup 
{
    public partial class GroupDataSet 
    {
        partial class EnvelopeGroupDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private EnvelopeGroupTableAdapter groupTA;
            private int newID;
            private bool stayOut;


            ///////////////////////////////////////////////////////////////////////
            //   Overriden Functions 
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow +=new DataTableNewRowEventHandler(EnvelopeGroupDataTable_TableNewRow);
                this.ColumnChanged +=new DataColumnChangeEventHandler(EnvelopeGroupDataTable_ColumnChanged);

                this.groupTA = new EnvelopeGroupTableAdapter();
                this.groupTA.ClearBeforeFill = true;

                this.newID = 1;
                this.stayOut = false;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void EnvelopeGroupDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                stayOut = true;
                EnvelopeGroupRow newRow = e.Row as EnvelopeGroupRow;

                newRow.id = this.newID++;
                newRow.name = "";

                stayOut = false;
            }

            private void EnvelopeGroupDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                if (stayOut)
                    return;

                stayOut = true;

                EnvelopeGroupRow row;
                string tmp;
                int maxLen;

                row = e.Row as EnvelopeGroupRow;

                if (e.Column.ColumnName == "name")
                {
                    tmp = e.ProposedValue as string;
                    maxLen = this.nameColumn.MaxLength;

                    if (tmp.Length > maxLen)
                        row.name = tmp.Substring(0, maxLen);
                }

                stayOut = false;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Functions Private 
            ///////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////
            //   Functions Public 
            ///////////////////////////////////////////////////////////////////////
            public void myFillTable()
            {
                this.groupTA.Fill(this);
                this.newID = DBquery.getNewID("id", "EnvelopeGroup");
            }

            public void myUpdateDB()
            {
                this.groupTA.Update(this);
            }

        }// END partial class AccountTypeDataTable
    }
}
