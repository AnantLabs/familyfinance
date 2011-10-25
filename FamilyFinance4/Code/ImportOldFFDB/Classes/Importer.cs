using System.Collections.Generic;
using FamilyFinance.Buisness;
using FamilyFinance.Data;

namespace ImportOldFFDB
{
    class Importer
    {
        OldFFDBDataSet sourceData = new OldFFDBDataSet();
        DataSetModel destinationData = DataSetModel.Instance;

        public void import()
        {
            fillsourceData();
            destinationData.loadData();

            AccountTypeMerger accountTypeMerger = new AccountTypeMerger(sourceData.AccountType, destinationData.AccountTypes);
            accountTypeMerger.merge();

            EnvelopeGroupMerger envelopeGroupMerger = new EnvelopeGroupMerger(sourceData.EnvelopeGroup, destinationData.EnvelopeGroups);
            envelopeGroupMerger.merge();

            TransactionTypeMerger transactionType = new TransactionTypeMerger(sourceData.LineType, destinationData.TransactionTypes);
            transactionType.merge();

            AccountAppender accountAppender = new AccountAppender(sourceData.Account, accountTypeMerger);
            accountAppender.appendAccounts();

            EnvelopeAppender envelopeAppender = new EnvelopeAppender(sourceData.Envelope, envelopeGroupMerger);
            envelopeAppender.appendEnvelopes();

            TransactionAppender transactionAppender = new TransactionAppender(transactionType, accountAppender, envelopeAppender);
            transactionAppender.appendLines(sourceData.LineItem);

            destinationData.saveData();
        }

        private void fillsourceData()
        {
            // Create anonymous table adapters to fill the Old Data set tables
            new OldFFDBDataSetTableAdapters.AccountTypeTableAdapter().Fill(sourceData.AccountType);
            new OldFFDBDataSetTableAdapters.EnvelopeGroupTableAdapter().Fill(sourceData.EnvelopeGroup);
            new OldFFDBDataSetTableAdapters.LineTypeTableAdapter().Fill(sourceData.LineType);
            new OldFFDBDataSetTableAdapters.AccountTableAdapter().Fill(sourceData.Account);
            new OldFFDBDataSetTableAdapters.EnvelopeTableAdapter().Fill(sourceData.Envelope);
            new OldFFDBDataSetTableAdapters.LineItemTableAdapter().Fill(sourceData.LineItem);
            new OldFFDBDataSetTableAdapters.EnvelopeLineTableAdapter().Fill(sourceData.EnvelopeLine);
        }

    }
}