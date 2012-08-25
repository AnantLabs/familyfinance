using System.Collections.Generic;
using System.Collections.ObjectModel;
using FamilyFinance.Buisness;
using System;

namespace ImportOldFFDB
{
    class TransactionAppender
    {
        private TransactionTypeMerger transactionTypeMerger;
        private AccountAppender accountAppender;
        private EnvelopeAppender envelopeAppender;

        private Dictionary<int, TransactionDRM> transactionDictionary;

        public TransactionAppender(TransactionTypeMerger transactionTypeMerger,
            AccountAppender accountAppender,
            EnvelopeAppender envelopeAppender)
        {
            transactionDictionary = new Dictionary<int, TransactionDRM>();

            this.transactionTypeMerger = transactionTypeMerger;
            this.accountAppender = accountAppender;
            this.envelopeAppender = envelopeAppender;
        }

        public void appendLines(OldFFDBDataSet.LineItemDataTable sourceTable)
        {
            foreach (OldFFDBDataSet.LineItemRow line in sourceTable)
                appendLine(line);
        }

        private void appendLine(OldFFDBDataSet.LineItemRow sourceLine)
        {
            TransactionDRM transaction;

            if (transactionDictionary.TryGetValue(sourceLine.transactionID, out transaction))
                addLineToTransaction(transaction, sourceLine);

            else
            {
                transaction = new TransactionDRM();

                transaction.Date = sourceLine.date;
                transaction.Description = getDescription(sourceLine);
                transaction.TypeID = transactionTypeMerger.getDestinationIdFromSourceId(sourceLine.typeID);

                addLineToTransaction(transaction, sourceLine);
                transactionDictionary.Add(sourceLine.transactionID, transaction);
            }
        }


        private string getDescription(OldFFDBDataSet.LineItemRow sourceLine)
        {
            if (sourceLine.IsdescriptionNull())
                return null;
            else
                return sourceLine.description;
        }

        private void addLineToTransaction(TransactionDRM transaction, OldFFDBDataSet.LineItemRow sourceLine)
        {
            LineItemDRM newLine = new LineItemDRM();

            newLine.setParentTransaction(transaction);
            newLine.AccountID = accountAppender.getDestinationIdFromSourceId(sourceLine.accountID);
            newLine.Amount = sourceLine.amount;
            newLine.ConfirmationNumber = getConfirmationNumber(sourceLine);
            newLine.Polarity = FamilyFinance.Data.PolarityCON.GetPlolartiy(sourceLine.creditDebit);
            newLine.State = FamilyFinance.Data.TransactionStateCON.GetState(sourceLine.complete[0]);

            addEnvelopeLines(newLine, sourceLine);
        }

        private string getConfirmationNumber(OldFFDBDataSet.LineItemRow sourceLine)
        {
            if (sourceLine.IsconfirmationNumberNull())
                return null;
            else
                return sourceLine.confirmationNumber;
        }

        private void addEnvelopeLines(LineItemDRM newLine, OldFFDBDataSet.LineItemRow sourceLine)
        {
            OldFFDBDataSet.EnvelopeLineRow[] eLines = sourceLine.GetEnvelopeLineRows();

            foreach (OldFFDBDataSet.EnvelopeLineRow eLine in eLines)
                addEnvelopeLine(newLine, eLine);
        }

        private void addEnvelopeLine(LineItemDRM newLine, OldFFDBDataSet.EnvelopeLineRow sourceELine)
        {
            EnvelopeLineDRM newELine = new EnvelopeLineDRM(newLine);

            newELine.EnvelopeID = envelopeAppender.getDestinationIdFromSourceId(sourceELine.envelopeID);
            newELine.Description = getELineDescription(sourceELine);
            newELine.Amount = sourceELine.amount;
        }

        private string getELineDescription(OldFFDBDataSet.EnvelopeLineRow sourceELine)
        {
            if (sourceELine.IsdescriptionNull())
                return null;
            else
                return sourceELine.description;
        }



    }
}
