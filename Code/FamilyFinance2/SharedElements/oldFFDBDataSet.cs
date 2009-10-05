using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Data.SqlServerCe;
using System.Collections.Generic;
using FamilyFinance2.FFDBDataSetTableAdapters;

namespace FamilyFinance2 
{
    public partial class FFDBDataSet 
    {


        ///////////////////////////////////////////////////////////////////////
        //   Functions Public 
        ///////////////////////////////////////////////////////////////////////
        public void myCheckTransactionInTable(int transID)
        {
            List<int> lineIDList = new List<int>();
            decimal creditSum = 0.0m;
            decimal debitSum = 0.0m;
            bool transError;
            short creditOppAccountID = SpclAccount.NULL;
            short debitOppAccountID = SpclAccount.NULL;
            int creditCount = 0;
            int debitCount = 0;

            // Gather information from the transaction
            foreach (LineItemRow line in this.LineItem)
            {
                if (line.RowState != DataRowState.Deleted && line.transactionID == transID && line.RowState != DataRowState.Detached)
                {
                    if (line.creditDebit == LineCD.CREDIT)
                    {
                        lineIDList.Add(line.id);
                        creditCount++;
                        creditSum += line.amount;
                        debitOppAccountID = line.accountID; // This is usefull when there is only one credit
                    }
                    else
                    {
                        lineIDList.Add(line.id);
                        debitCount++;
                        debitSum += line.amount;
                        creditOppAccountID = line.accountID; // This is usefull when there is only one debit
                    }
                }
            }

            // If there are no lines, there is nothing to do.
            if (creditCount == 0 && debitCount == 0)
                return;

            // Determine if there is a transaction error.
            transError = (creditSum != debitSum);

            // Determine the oppAccount values for complex transactions
            if (creditCount > 1)
                debitOppAccountID = SpclAccount.MULTIPLE;

            if (debitCount > 1)
                creditOppAccountID = SpclAccount.MULTIPLE;


            // Check each line for Line errors and set transaction errors
            foreach (int id in lineIDList)
            {
                int subCount;
                short envelopeID;
                decimal subSum = this.SubLineItem.mySubLineSum(id, out subCount, out envelopeID);
                LineItemRow line = this.LineItem.FindByid(id);
                bool usesEnvelopes = line.AccountRowByFK_Line_accountID.envelopes;
                bool lineError;

                // Determine if there is a lineError and set the value.
                if ((usesEnvelopes && line.amount == subSum) || (!usesEnvelopes && subSum == 0.0m))
                    lineError = false;
                else
                    lineError = true;

                // Set the Line Error if needed
                if (line.lineError != lineError)
                    line.lineError = lineError;

                // Set the Transaction Error if needed
                if (line.transactionError != transError)
                    line.transactionError = transError;

                // Set the OppacountID if needed
                if (line.creditDebit == LineCD.CREDIT)
                {
                    if (line.oppAccountID != creditOppAccountID)
                        line.oppAccountID = creditOppAccountID;
                }
                else
                {
                    if (line.oppAccountID != debitOppAccountID)
                        line.oppAccountID = debitOppAccountID;
                }

                // Set the EnvelopeID if needed
                if (line.envelopeID != envelopeID)
                    line.envelopeID = envelopeID;
            }
        }

        public void myCheckTransactionInDataBase(int lineID, int transID)
        {

            
        }

        public void mySaveTransaction()
        {
            this.LineItem.mySaveNewLines();
            this.SubLineItem.mySaveChanges();
            this.LineItem.mySaveChanges();
        }


    }
}
