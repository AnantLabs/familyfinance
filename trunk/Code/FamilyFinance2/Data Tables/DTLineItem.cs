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
            private int newID;
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

                this.TableNewRow += new DataTableNewRowEventHandler(LineItemDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(LineItemDataTable_ColumnChanged);

                newID = 1;
                autoChange = true;
            }


            ///////////////////////////////////////////////////////////////////////
            //   Internal Events
            ///////////////////////////////////////////////////////////////////////
            private void LineItemDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                LineItemRow lineItemRow = e.Row as LineItemRow;
                int lineID = FFDBDataSet.myDBGetNewID("id", "LineItem");

                if (lineID > newID)
                    newID = (lineItemRow.id = lineID) + 1;
                else
                    lineItemRow.id = newID++;

                lineItemRow.transactionID = FFDBDataSet.myDBGetNewID("transactionID", "LineItem");
                lineItemRow.date = DateTime.Now.Date;
                lineItemRow.lineTypeID = SpclLineType.NULL;
                lineItemRow.accountID = SpclAccount.NULL;
                lineItemRow.oppAccountID = SpclAccount.NULL;
                lineItemRow.description = "";
                lineItemRow.confirmationNumber = "";
                lineItemRow.envelopeID = SpclEnvelope.NULL;
                lineItemRow.complete = LineState.PENDING;
                lineItemRow.creditAmount = 0.0m;
                lineItemRow.transactionError = false;
                lineItemRow.lineError = false;
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
            public void myFill()
            {
                this.thisTableAdapter.Fill(this);
            }

            public void myFillTAByAccount(short accountID)
            {
                bool accountCD;
                decimal balance;
                decimal endingBalance;

                // Turn off the auto change and fillup the table.
                autoChange = false;
                this.thisTableAdapter.FillByAccount(this, accountID);

                // If this is empty there is nothing to do.
                if (this.Rows.Count <= 0)
                    return;

                // Set the balances and get the accounts CD.
                balance = 0.0m;
                endingBalance = (this.Rows[0] as LineItemRow).AccountRowByFK_Line_accountID.endingBalance;
                accountCD = (this.Rows[0] as LineItemRow).AccountRowByFK_Line_accountID.creditDebit;

                // Set balances By going down the list
                if (accountCD == LineCD.DEBIT)
                {
                    foreach (LineItemRow row in this)
                        if (row.creditDebit == LineCD.DEBIT)
                        {
                            row.debitAmount = row.amount;
                            row.SetcreditAmountNull();
                            row.balanceAmount = balance += row.amount;
                        }
                        else
                        {
                            row.creditAmount = row.amount;
                            row.SetdebitAmountNull();
                            row.balanceAmount = balance -= row.amount;
                        }
                }
                else
                {
                    foreach (LineItemRow row in this)
                        if (row.creditDebit == LineCD.DEBIT)
                        {
                            row.debitAmount = row.amount;
                            row.SetcreditAmountNull();
                            row.balanceAmount = balance -= row.amount;
                        }
                        else
                        {
                            row.creditAmount = row.amount;
                            row.SetdebitAmountNull();
                            row.balanceAmount = balance += row.amount;
                        }
                }

                //if(endingBalance != balance)

                this.AcceptChanges();
                autoChange = true;
            }

            public void myFillBalance()
            {
                bool accountCD;
                decimal balance;
                decimal endingBalance;

                // Turn off the auto change.
                autoChange = false;

                // If this is empty there is nothing to do.
                if (this.Rows.Count <= 0)
                    return;

                // Set the balances and get the accounts CD.
                balance = 0.0m;
                endingBalance = (this.Rows[0] as LineItemRow).AccountRowByFK_Line_accountID.endingBalance;
                accountCD = (this.Rows[0] as LineItemRow).AccountRowByFK_Line_accountID.creditDebit;

                // Set balances By going down the list
                if (accountCD == LineCD.DEBIT)
                {
                    foreach (LineItemRow row in this)
                        if (row.creditDebit == LineCD.DEBIT)
                            row.balanceAmount = balance += row.amount;
                        
                        else
                            row.balanceAmount = balance -= row.amount;
                }
                else
                {
                    foreach (LineItemRow row in this)
                        if (row.creditDebit == LineCD.DEBIT)
                            row.balanceAmount = balance -= row.amount;
                        
                        else
                            row.balanceAmount = balance += row.amount;
                }

                //if(endingBalance != balance)

                this.AcceptChanges();
                autoChange = true;
            }

            public void myUpdateTA()
            { this.thisTableAdapter.Update(this); }

            

        }//END partial class LineItemDataTable
    }// END FamilyFinanceDBDataSet Partial Class
}// END nameSpace FamilyFinance
