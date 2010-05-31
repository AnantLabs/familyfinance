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

        // Simple tables (for reference only) have their adapters here.
        private AccountTableAdapter AccountTA;
        private EnvelopeTableAdapter EnvelopeTA;
        private LineTypeTableAdapter LineTypeTA;

        public bool TransactionError;

        /////////////////////////
        //   Properties 
        private int currentTransID;
        public int CurrentTransactionID
        {
            get { return currentTransID; }
        }



        /////////////////////////
        //   Functions Public 
        public void myInit()
        {
            this.AccountTA = new AccountTableAdapter();
            this.AccountTA.ClearBeforeFill = true;

            this.EnvelopeTA = new EnvelopeTableAdapter();
            this.EnvelopeTA.ClearBeforeFill = true;

            this.LineTypeTA = new LineTypeTableAdapter();
            this.LineTypeTA.ClearBeforeFill = true;

            this.currentTransID = -1;
        }


        public void myFillAccountTable()
        {
            this.AccountTA.Fill(this.Account);
        }

        public void myFillEnvelopeTable()
        {
            this.EnvelopeTA.Fill(this.Envelope);
        }

        public void myFillLineTypeTable()
        {
            this.LineTypeTA.Fill(this.LineType);
        }

        public void myFillLineItemAndSubLine(int transID)
        {
            this.currentTransID = transID;
            this.LineItem.myFill(transID);
            this.EnvelopeLine.myFill(transID);
        }



        public void myDeleteLine(int lineID)
        {
            try
            {
                // Delete the Envelope Lines
                foreach (EnvelopeLineRow envLine in this.EnvelopeLine)
                {
                    if (envLine.RowState != DataRowState.Deleted)
                        if (envLine.lineItemID == lineID)
                            envLine.Delete();
                }

                // Delete the line
                this.LineItem.FindByid(lineID).Delete();
            }
            catch
            {
                return;
            }
        }

        public void myDeleteEnvelopeLine(int envLineID)
        {
            try
            {
                // Delete the Envelope Line
                this.EnvelopeLine.FindByid(envLineID).Delete();
            }
            catch
            {
                return;
            }
        }

        public void myCheckTransaction()
        {
            decimal temp, temp2;
            myCheckTransaction(out temp, out temp2);
        }

        public void myCheckTransaction(out decimal creditSum, out decimal debitSum)
        {
            creditSum = 0.00m;
            debitSum = 0.00m;

            foreach (LineItemRow line in this.LineItem)
            {
                if (line.RowState != DataRowState.Deleted)
                {
                    bool lineError = false;
                    decimal envSum = this.EnvelopeLine.myEnvelopeLineSum(line.id);

                    // Determine if there is a line error
                    if (line.AccountRowByFK_Line_accountID.envelopes && line.amount == envSum)
                        lineError = false;
                    else if (!line.AccountRowByFK_Line_accountID.envelopes && 0.00m == envSum)
                        lineError = false;
                    else
                        lineError = true;

                    // Set line error if needed
                    if (line.lineError != lineError)
                        line.lineError = lineError;

                    // Find credit and debit sums
                    if (line.creditDebit == LineCD.CREDIT)
                        creditSum += line.amount;
                    else
                        debitSum += line.amount;
                }
            }

            // Determin if there is a transaction error.
            if (creditSum != debitSum)
                this.TransactionError = true;
            else
                this.TransactionError = false;
        }

        public void mySaveChanges()
        {
            // only changes to the lines and envelopeLines
            this.LineItem.mySaveNewLines();
            this.EnvelopeLine.mySaveChanges();
            this.LineItem.mySaveChanges();
        }


        ///////////////////////////////////////////
        // Used by the Registry dataset
        public void myForwardLineEdits(FamilyFinance2.Forms.Main.RegistrySplit.RegistryDataSet.LineItemRow newRow)
        {
            LineItemRow oldRow = this.LineItem.FindByid(newRow.id);
            int tranSize = this.LineItem.Rows.Count;

            // Copy the simple items.
            if (oldRow.date != newRow.date)
                oldRow.date = newRow.date;

            if (oldRow.typeID != newRow.typeID)
                oldRow.typeID = newRow.typeID;

            if (oldRow.description != newRow.description)
                oldRow.description = newRow.description;

            if (oldRow.confirmationNumber != newRow.confirmationNumber)
                oldRow.confirmationNumber = newRow.confirmationNumber;

            if (oldRow.envelopeID != newRow.envelopeID)
                oldRow.envelopeID = newRow.envelopeID;

            if (oldRow.complete != newRow.complete)
                oldRow.complete = newRow.complete;

            if (oldRow.accountID != newRow.accountID)
                oldRow.accountID = newRow.accountID;  //Needs something else

            if (oldRow.oppAccountID != newRow.oppAccountID)
                oldRow.oppAccountID = newRow.oppAccountID;  //Needs something else

            if (oldRow.amount != newRow.amount)
                oldRow.amount = newRow.amount;  //Needs something else

            if (oldRow.creditDebit != newRow.creditDebit)
                oldRow.creditDebit = newRow.creditDebit;  //Needs something else

            // line error 
            // transaction error

            newRow.AcceptChanges();
        }

        public void myQuickFinishSubLines(ref LineItemRow line)
        {
            int subLineCount = 0;

            // Count the subLines
            foreach (EnvelopeLineRow subLine in this.EnvelopeLine)
                if (subLine.lineItemID == line.id)
                    subLineCount++;

            // determine if we should delete the sub lines of this lineitem.
            bool delete = (line == null || line.AccountRowByFK_Line_accountID.envelopes == false);

            if (delete)
            {
                foreach (EnvelopeLineRow subLine in this.EnvelopeLine)
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
                    EnvelopeLineRow newRow = this.EnvelopeLine.NewEnvelopeLineRow();

                    newRow.lineItemID = line.id;
                    newRow.envelopeID = line.envelopeID;
                    newRow.description = line.description;
                    newRow.amount = line.amount;

                    subLineCount++;

                    this.EnvelopeLine.Rows.Add(newRow);
                }
            }
            // If there is one subLine forward simple changes from LineItem to SublineItem.
            else if (subLineCount == 1)
            {
                foreach (EnvelopeLineRow subLine in this.EnvelopeLine)
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


        ///////////////////////////////////////////////////////////////////////
        //   LineItem Data Table 
        ///////////////////////////////////////////////////////////////////////
        partial class LineItemDataTable
        {
            /////////////////////////
            //   Local Variables
            private LineItemTableAdapter LineItemTA;
            private int newLineID;
            private bool stayOut;


            /////////////////////////
            //   Function Overrides
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(LineItemDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(LineItemDataTable_ColumnChanged);

                this.LineItemTA = new LineItemTableAdapter();
                this.LineItemTA.ClearBeforeFill = true;

                this.newLineID = -1;
                this.stayOut = false;
            }


            /////////////////////////
            //   Internal Events
            private void LineItemDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                stayOut = true;
                LineItemRow newRow = e.Row as LineItemRow;

                newRow.id = newLineID++;
                //newRow.transactionID = this.transactionID;
                newRow.date = DateTime.Now.Date;
                newRow.typeID = SpclLineType.NULL;
                newRow.accountID = SpclAccount.NULL;
                newRow.oppAccountID = SpclAccount.NULL;
                newRow.description = "";
                newRow.confirmationNumber = "";
                newRow.envelopeID = SpclEnvelope.NULL;
                newRow.complete = LineState.PENDING;
                newRow.amount = 0.0m;
                newRow.creditDebit = LineCD.CREDIT;
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
                        // Do not accept negative numbers in this column
                        if (row.amount < 0)
                            row.amount = decimal.Negate(row.amount);

                        // Keep only to the penny.
                        row.amount = decimal.Round(row.amount, 2);
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



            /////////////////////////
            //   Public Functions
            public void myFill(int transID)
            {
                this.LineItemTA.FillByTransID(this, transID);
                this.newLineID = DBquery.getNewID("id", "LineItem");
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
        //   Envelope Line Data Table 
        ///////////////////////////////////////////////////////////////////////
        partial class EnvelopeLineDataTable
        {
            /////////////////////////
            //   Local Variables
            private EnvelopeLineTableAdapter EnvelopeLineTA;
            private int newID;
            private bool stayOut;


            /////////////////////////
            //   Function Overridden
            public override void EndInit()
            {
                base.EndInit();

                this.TableNewRow += new DataTableNewRowEventHandler(EnvelopeLineDataTable_TableNewRow);
                this.ColumnChanged += new DataColumnChangeEventHandler(EnvelopeLineDataTable_ColumnChanged);

                this.EnvelopeLineTA = new EnvelopeLineTableAdapter();
                this.EnvelopeLineTA.ClearBeforeFill = true;
            
                this.newID = -1;
                this.stayOut = false;
            }


            /////////////////////////
            //   Internal Events
            private void EnvelopeLineDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
                stayOut = true;
                EnvelopeLineRow newRow = e.Row as EnvelopeLineRow;

                newRow.id = this.newID++;
                //newRow.lineItemID = this.currentLineID;
                newRow.envelopeID = SpclEnvelope.NULL;
                newRow.description = "";
                newRow.amount = 0.0m;

                stayOut = false;
            }

            private void EnvelopeLineDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
            {
                if (stayOut)
                    return;

                stayOut = true;
                EnvelopeLineRow row = e.Row as EnvelopeLineRow;

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
            public void myFill(int transID)
            {
                this.EnvelopeLineTA.FillByTransID(this, transID);
                this.newID = DBquery.getNewID("id", "EnvelopeLine");
            }

            public void mySaveChanges()
            {
                this.EnvelopeLineTA.Update(this);
            }

            public decimal myEnvelopeLineSum(int lineID)
            {
                decimal sum = 0.0m;
                int subCount = 0;

                foreach (EnvelopeLineRow envLine in this)
                    if (envLine.RowState != DataRowState.Deleted && envLine.lineItemID == lineID)
                    {
                        sum += envLine.amount;
                        subCount++;
                    }

                return sum;
            }

        }

    }
}
