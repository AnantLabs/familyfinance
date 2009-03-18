using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Collections.Generic;

namespace FamilyFinance2
{
    partial class FFDBDataSet
    {
        partial class LineItemDataTable
        {
            ///////////////////////////////////////////////////////////////////////
            //   Local Variables
            ///////////////////////////////////////////////////////////////////////
            private FFDBDataSetTableAdapters.LineItemTableAdapter thisTableAdapter;
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

                this.thisTableAdapter = new FFDBDataSetTableAdapters.LineItemTableAdapter();
                this.thisTableAdapter.ClearBeforeFill = true;

                this.TableNewRow += new System.Data.DataTableNewRowEventHandler(LineItemDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(LineItemDataTable_ColumnChanged);

                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void LineItemDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                LineItemRow lineItemRow = e.Row as LineItemRow;
                int maxID = -1;
                int maxTransID = -1;

                // Find new ID and transactionID values
                foreach (LineItemRow row in this)
                {
                    if (maxID < row.id)
                        maxID = row.id;

                    if (maxTransID < row.transactionID)
                        maxTransID = row.transactionID;
                }

                if (maxID > 0)
                    maxID = 0;
                
                if (maxTransID > 0)
                    maxTransID = 0;

                lineItemRow.id = maxID + 1;
                lineItemRow.transactionID = maxTransID + 1;
                lineItemRow.date = DateTime.Now.Date;
                lineItemRow.lineTypeID = SpclLineType.NULL;
                lineItemRow.accountID = SpclAccount.NULL;
                lineItemRow.oppAccountID = SpclAccount.NULL;
                lineItemRow.description = "";
                lineItemRow.confirmationNumber = "";
                lineItemRow.envelopeID = SpclEnvelope.NULL;
                lineItemRow.complete = LineState.PENDING;          // (' ' - Null, 'C' - Complete, 'R' - Reconciled)
                lineItemRow.creditAmount = 0.0m;
                lineItemRow.lineError = false;
                lineItemRow.transactionError = false;

            }

            private void LineItemDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                LineItemRow row;
                string tmp;

                if (autoChange == false)
                    return;

                autoChange = false; 
                row = e.Row as LineItemRow;

                switch (e.Column.ColumnName)
                {
                    case "amount":
                        {
                            myValidateAmount(ref row, row.creditDebit, row.amount);
                            break;
                        }
                    case "creditDebit":
                        {
                            myValidateAmount(ref row, row.creditDebit, row.amount);
                            break;
                        }

                    case "debitAmount":
                        {
                            myValidateAmount(ref row, LineCD.DEBIT, row.debitAmount);
                            break;
                        }

                    case "creditAmount":
                        {
                            myValidateAmount(ref row, LineCD.CREDIT, row.creditAmount);
                            break;
                        }

                    case "complete":
                        {
                            tmp = e.ProposedValue as string;

                            if (tmp.Length > 1)
                                row.complete = tmp.Substring(0, 1);

                            break;
                        }
                }


                autoChange = true; 
            }


            ///////////////////////////////////////////////////////////////////////
            //   Function Private
            ///////////////////////////////////////////////////////////////////////
            private void myValidateAmount(ref LineItemRow row, bool newCD, decimal newAmount)
            {
                int temp;

                if (newAmount < 0)
                {
                    newAmount = newAmount * -1;
                    newCD = !newCD;
                }

                // Keep only to the Penny.
                temp = Convert.ToInt32(newAmount * 100);
                newAmount = temp / 100.0m;
                
                if( newAmount == 0)
                {
                    row.amount = newAmount;
                    row.creditDebit = newCD;
                    row.SetdebitAmountNull();
                    row.SetcreditAmountNull();
                }
                else if (newCD == LineCD.DEBIT)
                {
                    row.amount = newAmount;
                    row.creditDebit = LineCD.DEBIT;
                    row.debitAmount = newAmount;
                    row.SetcreditAmountNull();
                }
                else
                {
                    row.amount = newAmount;
                    row.creditDebit = LineCD.CREDIT;
                    row.creditAmount = newAmount;
                    row.SetdebitAmountNull();
                }
            }


            ///////////////////////////////////////////////////////////////////////
            //   Function Public
            ///////////////////////////////////////////////////////////////////////
            public void myFillTA()
            {
                this.thisTableAdapter.Fill(this);
                autoChange = false;

                foreach (LineItemRow row in this)
                {
                    if (row.creditDebit == LineCD.DEBIT)
                        row.debitAmount = row.amount;
                    else
                        row.creditAmount = row.amount;
                }

                autoChange = true;
                this.AcceptChanges();
            }

            public void myUpdateTA()
            { this.thisTableAdapter.Update(this); }

            

        }//END partial class LineItemDataTable
    }// END FamilyFinanceDBDataSet Partial Class
}// END nameSpace FamilyFinance
