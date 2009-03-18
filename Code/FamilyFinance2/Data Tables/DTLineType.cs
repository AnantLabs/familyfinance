using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class LineTypeDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private FFDBDataSetTableAdapters.LineTypeTableAdapter thisTableAdapter;
            private bool autoChange;


            ///////////////////////////////////////////////////////////////////////
            //   Properties
            ///////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////
            //   Function Overrides
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.thisTableAdapter = new FFDBDataSetTableAdapters.LineTypeTableAdapter();
                this.thisTableAdapter.ClearBeforeFill = true;

                this.TableNewRow += new DataTableNewRowEventHandler(LineTypeDataTable_TableNewRow);
                this.TableNewRow +=new DataTableNewRowEventHandler(LineTypeDataTable_TableNewRow);

                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void LineTypeDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                LineTypeRow lineTypeRow = e.Row as LineTypeRow;
                int newID = -1;

                if (this.Count > 0)
                    newID = this[this.Count - 1].id + 1;

                if (newID > 0)
                    lineTypeRow.id = (short)newID;
                else
                    lineTypeRow.id = 1;

                lineTypeRow.name = "";
            }

            private void LineTypeDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                LineTypeRow row;
                string tmp;
                int maxLen;

                if (autoChange == false)
                    return;

                autoChange = false;

                row = e.Row as LineTypeRow;

                if (e.Column.ColumnName == "name")
                {
                    tmp = e.ProposedValue as string;
                    maxLen = this.nameColumn.MaxLength;

                    if (tmp.Length > maxLen)
                        row.name = tmp.Substring(0, maxLen);
                }

                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Function Private
            ///////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////
            //   Function Public
            ///////////////////////////////////////////////////////////////////////
            public void myFillTA()
            { this.thisTableAdapter.Fill(this); }

            public void myUpdateTA()
            { this.thisTableAdapter.Update(this); }

            //public int myAddType(string name)
            //{
            //    foreach (LineTypeRow row in this)
            //        if (row.name == name)
            //            return row.id;

            //    LineTypeRow newType = this.NewLineTypeRow();
            //    newType.name = name;

            //    this.Rows.Add(newType);
            //    this.thisTableAdapter.Update(newType);

            //    return newType.id;
            //}



        }// END partial class LineTypeDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance
