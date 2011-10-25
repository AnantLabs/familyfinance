using System.Collections.Generic;
using System.Collections.ObjectModel;
using FamilyFinance.Buisness;
using System;

namespace ImportOldFFDB
{
    class TransactionTypeMerger
    {
        private ObservableCollection<TransactionTypeDRM> destinationCollection;
        private OldFFDBDataSet.LineTypeDataTable sourceTable;

        private Dictionary<int, int> sourceToDestinationID;
        private Dictionary<string, int> destinationNames;

        public TransactionTypeMerger(OldFFDBDataSet.LineTypeDataTable sourceTable, 
            ObservableCollection<TransactionTypeDRM> destinationCollection)
        {
            sourceToDestinationID = new Dictionary<int, int>();
            destinationNames = new Dictionary<string, int>();

            this.sourceTable = sourceTable;
            this.destinationCollection = destinationCollection;
        }

        public void merge()
        {
            populateDictionaryWithExsistingNamesForFastLookup();
            mergeSourceValuesIntoDestination();
        }

        private void populateDictionaryWithExsistingNamesForFastLookup()
        {
            foreach (TransactionTypeDRM type in destinationCollection)
                destinationNames.Add(type.Name, type.ID);
        }

        private void mergeSourceValuesIntoDestination()
        {
            foreach (OldFFDBDataSet.LineTypeRow type in sourceTable)
                mergeTransactionTypeIntoDestination(type);
        }

        private void mergeTransactionTypeIntoDestination(OldFFDBDataSet.LineTypeRow type)
        {
            int destinationID;

            if (destinationNames.TryGetValue(type.name, out destinationID))
                sourceToDestinationID.Add(type.id, destinationID);
            
            else
                appendDestinationWithNewType(type);
            
        }

        private void appendDestinationWithNewType(OldFFDBDataSet.LineTypeRow type)
        {
            TransactionTypeDRM newTransactionType = new TransactionTypeDRM(type.name);

            sourceToDestinationID.Add(type.id, newTransactionType.ID);
            destinationNames.Add(newTransactionType.Name, newTransactionType.ID);
        }

        public int getDestinationIdFromSourceId(int sourceID)
        {
            int destinationID;

            if (sourceToDestinationID.TryGetValue(sourceID, out destinationID))
            {
                return destinationID;
            }
            else
            {
                throw new Exception("Invalid Source Line Type ID");
            }

        }

    }
}
