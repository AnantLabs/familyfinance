using System.Collections.Generic;
using System.Collections.ObjectModel;
using FamilyFinance.Buisness;
using System;

namespace ImportOldFFDB
{
    class AccountTypeMerger
    {
        private ObservableCollection<AccountTypeDRM> destinationCollection;
        private OldFFDBDataSet.AccountTypeDataTable sourceTable;

        private Dictionary<int, int> sourceToDestinationID;
        private Dictionary<string, int> destinationNames;
        
        public AccountTypeMerger(OldFFDBDataSet.AccountTypeDataTable sourceTable, 
            ObservableCollection<AccountTypeDRM> destinationCollection)
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
            foreach (AccountTypeDRM type in destinationCollection)
                destinationNames.Add(type.Name, type.ID);
        }

        private void mergeSourceValuesIntoDestination()
        {
            foreach (OldFFDBDataSet.AccountTypeRow type in sourceTable)
                mergeAccountTypeIntoDestination(type);
        }

        private void mergeAccountTypeIntoDestination(OldFFDBDataSet.AccountTypeRow type)
        {
            int destinationID;

            if (destinationNames.TryGetValue(type.name, out destinationID))
                sourceToDestinationID.Add(type.id, destinationID);
            
            else
                appendDestinationWithNewType(type);
            
        }

        private void appendDestinationWithNewType(OldFFDBDataSet.AccountTypeRow type)
        {
            AccountTypeDRM newAccountType = new AccountTypeDRM(type.name);

            sourceToDestinationID.Add(type.id, newAccountType.ID);
            destinationNames.Add(newAccountType.Name, newAccountType.ID);
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
                throw new Exception("Invalid Source Account Type ID");
            }

        }

    }
}
