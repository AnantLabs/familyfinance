using System.Data;
using FamilyFinance2.Forms.LineType.LineTypeDataSetTableAdapters;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Forms.LineType 
{
    public partial class LineTypeDataSet
    {
        partial class LineTypeDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private LineTypeTableAdapter LTTableAdapter;
            private int newID;
            private bool stayOut;


            ///////////////////////////////////////////////////////////////////////
            //   Function Overrides
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(LineTypeDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(LineTypeDataTable_ColumnChanged);


                this.LTTableAdapter = new LineTypeTableAdapter();
                this.LTTableAdapter.ClearBeforeFill = true;

                this.newID = 1;
                this.stayOut = false;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void LineTypeDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                stayOut = true;
                LineTypeRow lineTypeRow = e.Row as LineTypeRow;

                lineTypeRow.id = newID++;
                lineTypeRow.name = "";

                stayOut = false;
            }

            private void LineTypeDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                if (stayOut)
                    return;

                stayOut = true;
                LineTypeRow row = e.Row as LineTypeRow;
                string tmp;
                int maxLen;

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
            //   Function Private
            ///////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////
            //   Functions Public 
            ///////////////////////////////////////////////////////////////////////
            public void myFillTable()
            {
                this.LTTableAdapter.Fill(this);
                this.newID = DBquery.getNewID("id", "LineType");
            }

            public void myUpdateDB()
            {
                this.LTTableAdapter.Update(this);
            }
        }
    }
}
