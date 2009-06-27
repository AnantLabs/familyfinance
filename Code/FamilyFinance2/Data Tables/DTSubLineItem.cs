using System;
using System.Text;
using System.Data;
using System.Linq;
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
            private int newID;
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

                newID = 1;
                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void SubLineItemDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                SubLineItemRow subLineItemRow = e.Row as SubLineItemRow;

                subLineItemRow.id = newID++;
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
            { 
                this.thisTableAdapter.Fill(this);
                this.newID = FFDBDataSet.myDBGetNewID("id", "SubLineItem");
            }

            public void myFillTAByTransactionID(int transID)
            {
                this.thisTableAdapter.FillByTransactionID(this, transID);
                this.newID = FFDBDataSet.myDBGetNewID("id", "SubLineItem");
            }

            public void mySaveChanges()
            { this.thisTableAdapter.Update(this); }

            public decimal mySubLineSum(int lineID)
            {
                decimal sum = 0.0m;

                var amounts = from line in this
                              where line.lineItemID == lineID
                              select line.amount;

                foreach (var amount in amounts)
                    sum += amount;

                return sum;
            }

            public decimal mySubLineSum(int lineID, out int count, out short envelopeID)
            {
                decimal sum = 0.0m;

                var results = from line in this
                              where line.lineItemID == lineID
                              select new { line.amount, line.envelopeID };

                foreach (var row in results)
                    sum += row.amount;

                count = results.Count();

                if (count <= 0)
                    envelopeID = SpclEnvelope.NULL;
                else if (count == 1)
                    envelopeID = results.ElementAt(0).envelopeID;
                else 
                    envelopeID = SpclEnvelope.SPLIT;

                return sum;
            }


        }// END endpartial class SubLineItemDataTable
    }// END partial class FamilyFinanceDBDataSet
} // END namespace FamilyFinance
