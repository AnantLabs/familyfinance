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
        private int currentTransID;

        public List<AEPair> Changes;



        /////////////////////////
        //   Functions Private
        private void myChangeCreditDebit(ref LineItemRow lineBeingChange)
        {
            int thisSideCount = 0;
            int oppLineCount = 0;

            // Count how many lines have the sam creditDebit as the line and how many lines
            // are opposite this line.
            foreach (LineItemRow oppLine in this.LineItem)
            {
                if (oppLine.creditDebit == lineBeingChange.creditDebit)
                    thisSideCount++;
                else
                    oppLineCount++;
            }

            // If this is a simple transaction update both creditDebits
            if (thisSideCount == 1 && oppLineCount == 1)
            {
                this.LineItem[0].creditDebit = !this.LineItem[0].creditDebit;
                this.LineItem[1].creditDebit = !this.LineItem[1].creditDebit;
            }
            else // else just update the lineBeingchanged
                lineBeingChange.creditDebit = !lineBeingChange.creditDebit;
        }

        private void myChangeAmount(ref LineItemRow lineBeingChange, decimal newAmount)
        {
            int thisSideCount = 0;
            int oppLineCount = 0;
            int envCount;
            int eLineID;

            // Count how many lines have the sam creditDebit as the line and how many lines
            // are opposite this line.
            foreach (LineItemRow oppLine in this.LineItem)
            {
                if (oppLine.creditDebit == lineBeingChange.creditDebit)
                    thisSideCount++;
                else
                    oppLineCount++;
            }

            // If this is a simple transaction update both amounts
            if (thisSideCount == 1 && oppLineCount == 1)
            {
                this.LineItem[0].amount = newAmount;
                this.LineItem[1].amount = newAmount;

                // Now update the envelopeLines with the new amount;
                this.EnvelopeLine.myEnvelopeLineCount(this.LineItem[0].id, out envCount, out eLineID);
                if (envCount == 1)
                    this.EnvelopeLine.FindByid(eLineID).amount = newAmount;

                this.EnvelopeLine.myEnvelopeLineCount(this.LineItem[1].id, out envCount, out eLineID);
                if (envCount == 1)
                    this.EnvelopeLine.FindByid(eLineID).amount = newAmount;
            }

            // else just update the lineBeingchanged
            else
            {
                lineBeingChange.amount = newAmount;

                // Now update the envelopeLines with the new amount;
                this.EnvelopeLine.myEnvelopeLineCount(lineBeingChange.id, out envCount, out eLineID);
                if (envCount == 1)
                    this.EnvelopeLine.FindByid(eLineID).amount = newAmount;
            }
        }

        private void myChangeAccountID(ref LineItemRow lineBeingChange, int newAccountID)
        {
            int thisSideCount = 0;

            // Count how many lines have the same creditDebit as the line.
            foreach (LineItemRow oppLine in this.LineItem)
            {
                if (oppLine.creditDebit == lineBeingChange.creditDebit)
                    thisSideCount++;
            }

            // If thisSide has only one line (the given line) then update the oppLines.oppAccountID with the new
            // accountID
            if (thisSideCount == 1)
            {
                foreach (LineItemRow oppLine in this.LineItem)
                    if (oppLine.creditDebit != lineBeingChange.creditDebit)
                        oppLine.oppAccountID = newAccountID;
            }

            // Make the change to the one being changed.
            lineBeingChange.accountID = newAccountID;
        }

        private void myChangeOppAccountID(ref LineItemRow lineBeingChange, int newOppAccountID)
        {
            int oppLineCount = 0;

            // Count how many lines are opposite this line.
            foreach (LineItemRow oppLine in this.LineItem)
            {
                if (oppLine.creditDebit != lineBeingChange.creditDebit)
                    oppLineCount++;
            }

            // If oppLine is only one then update all the lines. The lines on the same saide as the one
            // getting the update get there oppAccount ids updated. Then the line opposite the one being
            // changed will get its accountID changed.
            if (oppLineCount == 1)
            {
                foreach (LineItemRow sameSideLine in this.LineItem)
                    if (sameSideLine.creditDebit == lineBeingChange.creditDebit)
                        sameSideLine.oppAccountID = newOppAccountID;
                    else
                        sameSideLine.accountID = newOppAccountID;
            }
        }

        private void myChangeEnvelopeID(ref LineItemRow lineBeingChange, int newEnvelopeID)
        {
            int envCount;
            int eLineID;

            lineBeingChange.envelopeID = newEnvelopeID;

            // Now update the envelopeLine If there is only one with the new envelopeID;
            this.EnvelopeLine.myEnvelopeLineCount(lineBeingChange.id, out envCount, out eLineID);
            if (envCount == 1 && newEnvelopeID > SpclEnvelope.NULL)
                this.EnvelopeLine.FindByid(eLineID).envelopeID = newEnvelopeID;

            else if (envCount == 1 && newEnvelopeID == SpclEnvelope.NULL)
                this.EnvelopeLine.FindByid(eLineID).Delete();
        }

        private void myAddChange(int accountID, int envelopeID)
        {
            bool found = false;

            foreach (AEPair pair in this.Changes)
            {
                // We want unique pairs of accountIDs and envelopeIDs. An account with an envelope
                // is better than just the same account with a null envelope. So if an account and a null envelope
                // are being added and ther is a mathing account don't add a new entry. If there is a complete
                // match don't add the entry, if there is an account and a null envelope already in the list
                // and we have a valid envelopeID replace the null envelope.

                if (pair.AccountID == accountID && envelopeID == SpclEnvelope.NULL)
                    found = true;

                else if (pair.AccountID == accountID && pair.EnvelopeID == envelopeID)
                    found = true;

                else if (pair.AccountID == accountID && pair.EnvelopeID == SpclEnvelope.NULL)
                {
                    // if a lone accountID is in the list 
                    pair.EnvelopeID = envelopeID;
                    found = true;
                }
            }

            if (!found)
                this.Changes.Add(new AEPair(accountID, envelopeID));
        }

        private void myFindChanges()
        {
            // Report EnvelopeLine Changes
            foreach (EnvelopeLineRow envLine in this.EnvelopeLine)
            {
                if (envLine.RowState == DataRowState.Modified)
                {
                    int orAccountID = (int)envLine.LineItemRow["accountID", DataRowVersion.Original];
                    int orEnvelopeID = (int)envLine["envelopeID", DataRowVersion.Original];
                    decimal orAmount = (decimal)envLine["amount", DataRowVersion.Original];

                    if (envLine.LineItemRow.accountID != orAccountID)
                    {
                        this.myAddChange(orAccountID, envLine.envelopeID);
                        this.myAddChange(envLine.LineItemRow.accountID, envLine.envelopeID);
                    }

                    if (envLine.envelopeID != orEnvelopeID)
                    {
                        this.myAddChange(envLine.LineItemRow.accountID, orEnvelopeID);
                        this.myAddChange(envLine.LineItemRow.accountID, envLine.envelopeID);
                    }

                    if (envLine.amount != orAmount)
                    {
                        this.myAddChange(envLine.LineItemRow.accountID, envLine.envelopeID);
                    }
                }
                else if (envLine.RowState == DataRowState.Added)
                {
                    this.myAddChange(envLine.LineItemRow.accountID, envLine.envelopeID);
                }
                else if (envLine.RowState == DataRowState.Deleted)
                {
                    int orEnvelopeID = (int)envLine["envelopeID", DataRowVersion.Original];
                    int lineItemID = (int)envLine["lineItemID", DataRowVersion.Original];
                    int orAccountID = (int)this.LineItem.FindByid(lineItemID)["accountID", DataRowVersion.Original];

                    this.myAddChange(orAccountID, orEnvelopeID);
                }
            }

            // report lineItem changes
            foreach (LineItemRow modLine in this.LineItem)
            {
                if (modLine.RowState == DataRowState.Modified)
                {
                    int orAccountID = (int)modLine["accountID", DataRowVersion.Original];
                    decimal orAmount = (decimal)modLine["amount", DataRowVersion.Original];
                    bool orCreditDebit = (bool)modLine["creditDebit", DataRowVersion.Original];

                    if (modLine.accountID != orAccountID)
                        this.myAddChange(orAccountID, SpclEnvelope.NULL);

                    if (modLine.accountID != orAccountID || modLine.amount != orAmount || modLine.creditDebit != orCreditDebit)
                        this.myAddChange(modLine.accountID, SpclEnvelope.NULL);

                }
                else if (modLine.RowState == DataRowState.Added)
                {
                    this.myAddChange(modLine.accountID, SpclEnvelope.NULL);
                }
                else if (modLine.RowState == DataRowState.Deleted)
                {
                    int orAccountID = (int)modLine["accountID", DataRowVersion.Original];
                    this.myAddChange(orAccountID, SpclEnvelope.NULL);
                }
            }
        }



        /////////////////////////
        //   Functions Public 
        public void myInit()
        {
            this.currentTransID = -1;

            this.AccountTA = new AccountTableAdapter();
            this.AccountTA.ClearBeforeFill = true;

            this.EnvelopeTA = new EnvelopeTableAdapter();
            this.EnvelopeTA.ClearBeforeFill = true;

            this.LineTypeTA = new LineTypeTableAdapter();
            this.LineTypeTA.ClearBeforeFill = true;


            // Setup for traching changes
            this.Changes = new List<AEPair>();
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
            this.myFindChanges();
            // only changes to the lines and envelopeLines
            this.LineItem.mySaveNewLines();
            this.EnvelopeLine.mySaveChanges();
            this.LineItem.mySaveChanges();
        }

        public List<AEPair> myGetChanges()
        {
            // Pass back the changes list and create a new list to work with.
            List<AEPair> change = this.Changes;

            this.Changes = new List<AEPair>();

            return change;
        }


        ///////////////////////////////////////////
        // Used by the Registry dataset
        public void myForwardLineEdits(FamilyFinance2.Forms.Main.RegistrySplit.RegistryDataSet.LineItemRow rLine)
        {
            LineItemRow tLine = this.LineItem.FindByid(rLine.id);

            // Copy the simple items.
            if (tLine.date != rLine.date)
                tLine.date = rLine.date;

            if (tLine.typeID != rLine.typeID)
                tLine.typeID = rLine.typeID;

            if (tLine.description != rLine.description)
                tLine.description = rLine.description;

            if (tLine.confirmationNumber != rLine.confirmationNumber)
                tLine.confirmationNumber = rLine.confirmationNumber;

            if (tLine.complete != rLine.complete)
                tLine.complete = rLine.complete;

            // The following affect other lines / envelopeLines 
            if (tLine.oppAccountID != rLine.oppAccountID)
                this.myChangeOppAccountID(ref tLine, rLine.oppAccountID);

            if (tLine.creditDebit != rLine.creditDebit)
                this.myChangeCreditDebit(ref tLine);

            if (tLine.amount != rLine.amount)
                this.myChangeAmount(ref tLine, rLine.amount);

            if (tLine.accountID != rLine.accountID)
                this.myChangeAccountID(ref tLine, rLine.accountID);

            if (tLine.envelopeID != rLine.envelopeID)
                this.myChangeEnvelopeID(ref tLine, rLine.envelopeID);


            // Update the error flags
            this.myCheckTransaction();
            rLine.transactionError = this.TransactionError;
            rLine.lineError = tLine.lineError;
        }

        public void myDeleteTransaction()
        {
            for (int i = 0; i < this.LineItem.Rows.Count; )
            {
                LineItemRow row = this.LineItem[i];
                this.myDeleteLine(row.id);
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

                foreach (EnvelopeLineRow envLine in this)
                    if (envLine.RowState != DataRowState.Deleted && envLine.lineItemID == lineID)
                        sum += envLine.amount;

                return sum;
            }

            public void myEnvelopeLineCount(int lineID, out int count, out int eLineID)
            {
                count = 0;
                eLineID = -1;

                foreach (EnvelopeLineRow envLine in this)
                    if (envLine.RowState != DataRowState.Deleted && envLine.lineItemID == lineID)
                    {
                        count++;
                        eLineID = envLine.id;
                    }
            }

        }

    }
}
