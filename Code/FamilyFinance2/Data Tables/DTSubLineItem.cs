using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class SubLineItemDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private FFDBDataSetTableAdapters.SubLineItemTableAdapter thisTableAdapter;
            private bool autoChange;


            ///////////////////////////////////////////////////////////////////////
            //   Properties
            ///////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////////////////////////////
            //   Function Overridden
            ///////////////////////////////////////////////////////////////////////
            public override void EndInit()
            {
                base.EndInit();

                this.thisTableAdapter = new FFDBDataSetTableAdapters.SubLineItemTableAdapter();
                this.thisTableAdapter.ClearBeforeFill = true;

                this.TableNewRow += new DataTableNewRowEventHandler(SubLineItemDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(SubLineItemDataTable_ColumnChanged);

                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void SubLineItemDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                SubLineItemRow subLineItemRow = e.Row as SubLineItemRow;

                subLineItemRow.id = FFDBDataSet.myDBGetNewID("id", "SubLineItem");
                subLineItemRow.envelopeID = SpclEnvelope.NULL;
                subLineItemRow.description = "";
                subLineItemRow.amount = 0.0m;

                autoChange = true;
            }

            private void SubLineItemDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                SubLineItemRow row;

                if (autoChange == false)
                    return;

                autoChange = false;
                row = e.Row as SubLineItemRow;

                if (e.Column.ColumnName == "amount")
                {
                    decimal newValue;
                    int tempValue;

                    newValue = Convert.ToDecimal(e.ProposedValue);
                    tempValue = Convert.ToInt32(newValue * 100);
                    newValue = Convert.ToDecimal(tempValue) / 100;

                    if (newValue < 0)
                        newValue = newValue * -1;

                    row.amount = newValue;
                }

                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Function Public
            ///////////////////////////////////////////////////////////////////////
            public void myFillTA()
            { this.thisTableAdapter.Fill(this); }

            public void myUpdateTA()
            { this.thisTableAdapter.Update(this); }


        }// END endpartial class SubLineItemDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance
