using System;
using System.Data;
using System.Collections.Generic;
using FamilyFinance2.SharedElements;
using FamilyFinance2.Forms.Transaction.TransactionDataSetTableAdapters;

namespace FamilyFinance2.Forms.Transaction 
{
    public partial class TransactionDataSet 
    {
        //////////////////////////
        //   Local Variables
        private AccountTableAdapter AccountTA;
        private AEBalanceTableAdapter AEBalanceTA;
        private EnvelopeTableAdapter EnvelopeTA;
        private LineTypeTableAdapter LineTypeTA;


        /////////////////////////
        //   Functions private 


        /////////////////////////
        //   Functions Public 
        public void myInit()
        {
            this.AccountTA = new AccountTableAdapter();
            this.AccountTA.ClearBeforeFill = true;

            this.AEBalanceTA = new AEBalanceTableAdapter();
            this.AEBalanceTA.ClearBeforeFill = true;

            this.EnvelopeTA = new EnvelopeTableAdapter();
            this.EnvelopeTA.ClearBeforeFill = true;

            this.LineTypeTA = new LineTypeTableAdapter();
            this.LineTypeTA.ClearBeforeFill = true;

            this.LineItem.myInit();
            this.SubLineItem.myInit();
        }

        public void mySetCurrentLineID(int lineID)
        {
            this.SubLineItem.mySetCurrentLineID(lineID);
        }


        public void myFillAccountTable()
        {
            this.AccountTA.Fill(this.Account);
        }

        public void myFillEnvelopeTable()
        {
            this.EnvelopeTA.Fill(this.Envelope);
        }

        public void myFillAEBalanceTable()
        {
            this.AEBalanceTA.Fill(this.AEBalance);
        }

        public void myFillLineTypeTable()
        {
            this.LineTypeTA.Fill(this.LineType);
        }

        public void myFillLineItemAndSubLine(int transID)
        {
            this.LineItem.myFill(transID);
            this.SubLineItem.myFill(transID);
        }


        public void myCheckTransaction()
        {
            decimal creditSum = 0.00m;
            decimal debitSum = 0.00m;
            bool transError = false;

            // Find credit and debit sums
            foreach (LineItemRow line in this.LineItem)
            {
                if (line.RowState != DataRowState.Deleted)
                {
                    if (line.creditDebit == LineCD.CREDIT)
                        creditSum += line.amount;
                    else
                        debitSum += line.amount;
                }
            }

            if (creditSum != debitSum)
                transError = true;
            else 
                transError = false;

            // Determine if there is a line error
            foreach (LineItemRow line in this.LineItem)
            {
                if (line.RowState != DataRowState.Deleted)
                {
                    bool lineError = false;
                    decimal subSum = this.SubLineItem.mySubLineSum(line.id);

                    if (line.AccountRowByFK_Line_accountID.envelopes && line.amount == subSum)
                        lineError = false;
                    else if (!line.AccountRowByFK_Line_accountID.envelopes && 0.00m == subSum)
                        lineError = false;
                    else
                        lineError = true;

                    // Set transaction and line errors if they need to be changed
                    if (line.lineError != lineError)
                        line.lineError = lineError;

                    if(line.transactionError != transError)
                        line.transactionError = transError;
                }
            }
        }

        public void myRippleBalanceChanges()
        {
            // Assumes all lineItems in a transaction are finished being updated. Now push
            // the balance changes to the other tables and save the changes to the database.

            foreach (LineItemRow line in this.LineItem)
            {
                switch (line.RowState)
                {
                    // Handle New lines
                    case System.Data.DataRowState.Added:
                        {
                            int newAccountID = line.accountID;
                            bool newCD = line.creditDebit;
                            decimal newAmount = line.amount;

                            this.Account.myDoTransaction(newAccountID, newCD, newAmount);
                            continue;
                        }
                    // Handle modified lines
                    case System.Data.DataRowState.Modified:
                        {
                            int newAccountID = line.accountID;
                            bool newCD = line.creditDebit;
                            decimal newAmount = line.amount;

                            int oldAccountID = Convert.ToInt32(line["accountID", System.Data.DataRowVersion.Original]);
                            bool oldCD = Convert.ToBoolean(line["creditDebit", System.Data.DataRowVersion.Original]);
                            decimal oldAmount = Convert.ToDecimal(line["amount", System.Data.DataRowVersion.Original]);

                            bool criticalChanges = (newAccountID != oldAccountID || newCD != oldCD || newAmount != oldAmount);

                            if (criticalChanges)
                                this.Account.myUndoDoTransaction(oldAccountID, oldCD, oldAmount, newAccountID, newCD, newAmount);
                            
                            continue;
                        }

                    // Handle deleted lines
                    case System.Data.DataRowState.Deleted:
                        {
                            int oldAccountID = Convert.ToInt32(line["accountID", System.Data.DataRowVersion.Original]);
                            bool oldCD = Convert.ToBoolean(line["creditDebit", System.Data.DataRowVersion.Original]);
                            decimal oldAmount = Convert.ToDecimal(line["amount", System.Data.DataRowVersion.Original]);

                            this.Account.myUndoTransaction(oldAccountID, oldCD, oldAmount);
                        }
                        continue;

                } // end switch
            } // end foreach LineItem

            foreach (SubLineItemRow subLine in this.SubLineItem)
            {
                switch (subLine.RowState)
                {
                    // Handle New SubLines
                    case System.Data.DataRowState.Added:
                        {
                            decimal newAmount = subLine.amount;
                            int newEnvelopeID = subLine.envelopeID;
                            int newAccountID = subLine.LineItemRow.accountID;
                            bool newCD = subLine.LineItemRow.creditDebit;

                            this.Envelope.myDoTransaction(newEnvelopeID, newCD, newAmount);
                            this.AEBalance.myDoTransaction(newAccountID, newEnvelopeID, newCD, newAmount);
                            continue;
                        }
                    // Handle Modified SubLines
                    case System.Data.DataRowState.Modified:
                        {
                            decimal newAmount = subLine.amount;
                            int newEnvelopeID = subLine.envelopeID;
                            int newAccountID = subLine.LineItemRow.accountID;
                            bool newCD = subLine.LineItemRow.creditDebit;
                            decimal oldAmount = Convert.ToDecimal(subLine["amount", System.Data.DataRowVersion.Original]);
                            int oldEnvelopeID = Convert.ToInt32(subLine["envelopeID", System.Data.DataRowVersion.Original]);
                            int oldAccountID = Convert.ToInt32(subLine.LineItemRow["accountID", System.Data.DataRowVersion.Original]);
                            bool oldCD = Convert.ToBoolean(subLine.LineItemRow["creditDebit", System.Data.DataRowVersion.Original]);

                            bool criticalChanges = (newAccountID != oldAccountID
                                             || newEnvelopeID != oldEnvelopeID
                                             || newCD != oldCD
                                             || newAmount != oldAmount);

                            if (criticalChanges)
                            {
                                this.Envelope.myUndoDoTransaction(oldEnvelopeID, oldCD, oldAmount, newEnvelopeID, newCD, newAmount);
                                this.AEBalance.myUndoDoTransaction(oldAccountID, oldEnvelopeID, oldCD, oldAmount, newAccountID, newEnvelopeID, newCD, newAmount);
                            }
                            continue;
                        }
                    // Handle deleted SubLines
                    case System.Data.DataRowState.Deleted:
                        {
                            decimal oldAmount = Convert.ToDecimal(subLine["amount", System.Data.DataRowVersion.Original]);
                            int oldEnvelopeID = Convert.ToInt32(subLine["envelopeID", System.Data.DataRowVersion.Original]);
                            int oldAccountID = Convert.ToInt32(subLine.LineItemRow["accountID", System.Data.DataRowVersion.Original]);
                            bool oldCD = Convert.ToBoolean(subLine.LineItemRow["creditDebit", System.Data.DataRowVersion.Original]);

                            this.Envelope.myUndoTransaction(oldEnvelopeID, oldCD, oldAmount);
                            this.AEBalance.myUndoTransaction(oldAccountID, oldEnvelopeID, oldCD, oldAmount);
                            continue;
                        }
                } // END switch
            } // END foreach subLines
        }

        public void myQuickFinish(ref LineItemRow line)
        {
            int subLineCount = 0;

            // Count the subLines
            foreach (SubLineItemRow subLine in this.SubLineItem)
                if (subLine.lineItemID == line.id)
                    subLineCount++;

            // determine if we should delete the sub lines of this lineitem.
            bool delete = ( line == null || line.AccountRowByFK_Line_accountID.envelopes == false );

            if (delete)
            {
                foreach (SubLineItemRow subLine in this.SubLineItem)
                {
                    if (subLine.lineItemID == line.id)
                        subLine.Delete();
                }
            }
            // If there is no subLine for this Line make one because this account has envelopes
            else if (subLineCount == 0)
            {
                bool envelopes = line.AccountRowByFK_Line_accountID.envelopes;
                bool validID = (line.envelopeID > SpclEnvelope.NULL);

                if (envelopes && validID)
                {
                    SubLineItemRow newRow = this.SubLineItem.NewSubLineItemRow();

                    newRow.lineItemID = line.id;
                    newRow.envelopeID = line.envelopeID;
                    newRow.description = line.description;
                    newRow.amount = line.amount;

                    subLineCount++;

                    this.SubLineItem.Rows.Add(newRow);
                }
            }
            // If there is one subLine forward simple changes from LineItem to SublineItem.
            else if (subLineCount == 1)
            {
                foreach (SubLineItemRow subLine in this.SubLineItem)
                {
                    if (subLine.lineItemID == line.id)
                    {
                        if (subLine.amount != line.amount)
                            subLine.amount = line.amount;

                        if (subLine.envelopeID != line.envelopeID)
                            subLine.envelopeID = line.envelopeID;
                    }
                }
            }

            
        }

        public void mySaveChanges()
        {
            this.AccountTA.Update(this.Account);
            this.EnvelopeTA.Update(this.Envelope);
            this.AEBalanceTA.Update(this.AEBalance);

            this.LineItem.mySaveNewLines();
            this.SubLineItem.mySaveChanges();
            this.LineItem.mySaveChanges();
        }


        ///////////////////////////////////////////////////////////////////////
        //   Account Data Table 
        ///////////////////////////////////////////////////////////////////////
        partial class AccountDataTable
        {
            public void myUndoTransaction(int oldAccountID, bool oldCD, decimal oldAmount)
            {
                AccountRow row = FindByid(oldAccountID);

                // Undo the old Amount
                if (row.creditDebit == LineCD.DEBIT)
                {
                    if (oldCD == LineCD.CREDIT)
                        row.endingBalance += oldAmount;
                    else
                        row.endingBalance -= oldAmount;
                }
                else
                {
                    if (oldCD == LineCD.CREDIT)
                        row.endingBalance -= oldAmount;
                    else
                        row.endingBalance += oldAmount;
                }
            }

            public void myDoTransaction(int newAccountID, bool newCD, decimal newAmount)
            {
                AccountRow row = FindByid(newAccountID);

                //  Update to the new amount
                if (row.creditDebit == LineCD.DEBIT)
                {
                    if (newCD == LineCD.CREDIT)
                        row.endingBalance -= newAmount;
                    else
                        row.endingBalance += newAmount;
                }
                else
                {
                    if (newCD == LineCD.CREDIT)
                        row.endingBalance += newAmount;
                    else
                        row.endingBalance -= newAmount;
                }
            }

            public void myUndoDoTransaction(int oldAccountID, bool oldCD, decimal oldAmount, int newAccountID, bool newCD, decimal newAmount)
            {
                AccountRow oldRow = FindByid(oldAccountID);
                AccountRow newRow = FindByid(newAccountID);

                // Undo the old Amount
                if (oldRow.creditDebit == LineCD.DEBIT)
                {
                    if (oldCD == LineCD.CREDIT)
                        oldRow.endingBalance += oldAmount;
                    else
                        oldRow.endingBalance -= oldAmount;
                }
                else
                {
                    if (oldCD == LineCD.CREDIT)
                        oldRow.endingBalance -= oldAmount;
                    else
                        oldRow.endingBalance += oldAmount;
                }

                //  Update to the new amount
                if (newRow.creditDebit == LineCD.DEBIT)
                {
                    if (newCD == LineCD.CREDIT)
                        newRow.endingBalance -= newAmount;
                    else
                        newRow.endingBalance += newAmount;
                }
                else
                {
                    if (newCD == LineCD.CREDIT)
                        newRow.endingBalance += newAmount;
                    else
                        newRow.endingBalance -= newAmount;
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        //   Envelope Data Table 
        ///////////////////////////////////////////////////////////////////////
        partial class EnvelopeDataTable
        {
            public void myUndoTransaction(int oldEnvelopeID, bool oldCD, decimal oldAmount)
            {
                EnvelopeRow row = this.FindByid(oldEnvelopeID);

                // Undo the old Amount
                if (oldCD == LineCD.CREDIT)
                    row.endingBalance += oldAmount;

                else
                    row.endingBalance -= oldAmount;
            }

            public void myDoTransaction(int newEnvelopeID, bool newCD, decimal newAmount)
            {
                EnvelopeRow row = FindByid(newEnvelopeID);

                //  Update to the new amount
                if (newCD == LineCD.CREDIT)
                    row.endingBalance -= newAmount;

                else
                    row.endingBalance += newAmount;
            }

            public void myUndoDoTransaction(int oldEnvelopeID, bool oldCD, decimal oldAmount, int newEnvelopeID, bool newCD, decimal newAmount)
            {
                EnvelopeRow oldEnvelopeRow = FindByid(oldEnvelopeID);
                EnvelopeRow newEnvelopeRow = FindByid(newEnvelopeID);

                // Undo the old Amount
                if (oldCD == LineCD.CREDIT)
                    oldEnvelopeRow.endingBalance += oldAmount;

                else
                    oldEnvelopeRow.endingBalance -= oldAmount;


                //  Update to the new amount
                if (newCD == LineCD.CREDIT)
                    newEnvelopeRow.endingBalance -= newAmount;

                else
                    newEnvelopeRow.endingBalance += newAmount;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        //   AEBalance Data Table 
        ///////////////////////////////////////////////////////////////////////
        partial class AEBalanceDataTable
        {
            public AEBalanceRow myGetRow(int accountID, int envelopeID)
            {
                AEBalanceRow row = this.FindByaccountIDenvelopeID(accountID, envelopeID);

                if (row == null)
                {
                    row = this.NewAEBalanceRow();

                    row.accountID = accountID;
                    row.envelopeID = envelopeID;
                    row.endingBalance = 0.0m;

                    this.Rows.Add(row);
                }

                return row;
            }

            public void myUndoTransaction(int oldAccountID, int oldEnvelopeID, bool oldCD, decimal oldAmount)
            {
                AEBalanceRow row = this.myGetRow(oldAccountID, oldEnvelopeID);

                if (row == null)
                    return;

                // Undo the old Amount
                if (oldCD == LineCD.CREDIT)
                    row.endingBalance += oldAmount;

                else
                    row.endingBalance -= oldAmount;
            }

            public void myDoTransaction(int newAccountID, int newEnvelopeID, bool newCD, decimal newAmount)
            {
                AEBalanceRow row = this.myGetRow(newAccountID, newEnvelopeID);

                if (row == null)
                    return;

                // Do the new Amount
                if (newCD == LineCD.CREDIT)
                    row.endingBalance -= newAmount;

                else
                    row.endingBalance += newAmount;
            }

            public void myUndoDoTransaction(int oldAccountID, int oldEnvelopeID, bool oldCD, decimal oldAmount, int newAccountID, int newEnvelopeID, bool newCD, decimal newAmount)
            {
                AEBalanceRow oldRow = this.myGetRow(oldAccountID, oldEnvelopeID);
                AEBalanceRow newRow = this.myGetRow(newAccountID, newEnvelopeID);

                if (oldRow == null || newRow == null)
                    return;

                // Undo the old Amount
                if (oldCD == LineCD.CREDIT)
                    oldRow.endingBalance += oldAmount;

                else
                    oldRow.endingBalance -= oldAmount;


                // Do the new Amount
                if (newCD == LineCD.CREDIT)
                    newRow.endingBalance -= newAmount;

                else
                    newRow.endingBalance += newAmount;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        //   LineItem Data Table 
        ///////////////////////////////////////////////////////////////////////
        partial class LineItemDataTable
        {
            /////////////////////////
            //   Local Variables
            private LineItemTableAdapter LineItemTA;
            private int transactionID;
            private int newLineID;
            private bool stayOut;


            /////////////////////////
            //   Function Overrides
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(LineItemDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(LineItemDataTable_ColumnChanged);

                this.newLineID = -1;
                this.transactionID = -1;
                this.stayOut = false;
            }


            /////////////////////////
            //   Internal Events
            private void LineItemDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                stayOut = true;
                LineItemRow newRow = e.Row as LineItemRow;

                newRow.id = newLineID++;
                newRow.transactionID = this.transactionID;
                newRow.date = DateTime.Now.Date;
                newRow.lineTypeID = SpclLineType.NULL;
                newRow.accountID = SpclAccount.NULL;
                newRow.oppAccountID = SpclAccount.NULL;
                newRow.description = "";
                newRow.confirmationNumber = "";
                newRow.envelopeID = SpclEnvelope.NULL;
                newRow.complete = LineState.PENDING;
                newRow.amount = 0.0m;
                newRow.creditDebit = LineCD.CREDIT;
                newRow.transactionError = false;
                newRow.lineError = false;

                stayOut = false;
            }

            private void LineItemDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                if (stayOut)
                    return;

                stayOut = true;
                LineItemRow row = e.Row as LineItemRow;

                switch (e.Column.ColumnName)
                {
                    case "amount":
                        myValidateAmount(ref row, row.creditDebit, row.amount);
                        break;

                    case "creditDebit":
                        myValidateAmount(ref row, row.creditDebit, row.amount);
                        break;

                    case "complete":
                        string tmp = e.ProposedValue as string;
                        if (tmp.Length > 1)
                            row.complete = tmp.Substring(0, 1);
                        break;
                }

                stayOut = false;
            }


            /////////////////////////
            //   Private Functions
            private void myValidateAmount(ref LineItemRow row, bool newCD, decimal newAmount)
            {
                if (newAmount < 0)
                {
                    newAmount = newAmount * -1;
                    newCD = !newCD;
                }

                // Keep only to the penny.
                newAmount = Convert.ToInt32(newAmount * 100) / 100.0m;

                row.amount = newAmount;
                row.creditDebit = newCD;
            }


            /////////////////////////
            //   Public Functions
            public void myInit()
            {
                this.LineItemTA = new LineItemTableAdapter();
                this.LineItemTA.ClearBeforeFill = true;
            }
            
            public void myFill(int transID)
            {
                this.LineItemTA.FillByTransID(this, transID);
                this.transactionID = transID;
                this.newLineID = FFDataBase.myDBGetNewID("id", "LineItem");
            }

            public void mySaveNewLines()
            {
                foreach (LineItemRow line in this)
                {
                    if (line.RowState == DataRowState.Added)
                        this.LineItemTA.Update(line);
                }
            }

            public void mySaveChanges()
            {
                this.LineItemTA.Update(this);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        //   SubLineItem Data Table 
        ///////////////////////////////////////////////////////////////////////
        partial class SubLineItemDataTable
        {
            /////////////////////////
            //   Local Variables
            private SubLineItemTableAdapter SubLineTA;
            public int currentLineID;
            private int newID;
            private bool stayOut;


            /////////////////////////
            //   Function Overridden
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(SubLineItemDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(SubLineItemDataTable_ColumnChanged);

                this.newID = -1;
                this.stayOut = false;
            }


            /////////////////////////
            //   Internal Events
            private void SubLineItemDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                stayOut = true;
                SubLineItemRow newRow = e.Row as SubLineItemRow;

                newRow.id = this.newID++;
                newRow.lineItemID = this.currentLineID;
                newRow.envelopeID = SpclEnvelope.NULL;
                newRow.description = "";
                newRow.amount = 0.0m;

                stayOut = false;
            }

            private void SubLineItemDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                if (stayOut)
                    return;

                stayOut = true;
                SubLineItemRow row = e.Row as SubLineItemRow;

                if (e.Column.ColumnName == "amount")
                {
                    decimal newValue;
                    int tempValue;

                    // only keep two decimal points
                    newValue = Convert.ToDecimal(e.ProposedValue);
                    tempValue = Convert.ToInt32(newValue * 100);
                    newValue = Convert.ToDecimal(tempValue) / 100;

                    // allow negative values in this column

                    row.amount = newValue;
                }

                stayOut = false;
            }


            /////////////////////////
            //   Function Public
            public void myInit()
            {
                this.SubLineTA = new SubLineItemTableAdapter();
                this.SubLineTA.ClearBeforeFill = true;
            }

            public void myFill(int transID)
            {
                this.SubLineTA.FillByTransID(this, transID);
                this.newID = FFDataBase.myDBGetNewID("id", "SubLineItem");
            }

            public void mySaveChanges()
            {
                this.SubLineTA.Update(this);
            }

            public void mySetCurrentLineID(int lineID)
            {
                this.currentLineID = lineID;
            }

            public decimal mySubLineSum(int lineID)
            {
                decimal sum = 0.0m;

                foreach (SubLineItemRow subLine in this)
                    if(subLine.RowState != DataRowState.Deleted && subLine.lineItemID == lineID)
                        sum += subLine.amount;

                return sum;
            }

        }

    }
}

