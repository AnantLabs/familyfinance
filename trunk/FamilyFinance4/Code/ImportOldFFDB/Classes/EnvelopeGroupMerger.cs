using System.Collections.Generic;
using System.Collections.ObjectModel;
using FamilyFinance.Buisness;
using System;

namespace ImportOldFFDB
{
    class EnvelopeGroupMerger
    {
        private ObservableCollection<EnvelopeGroupDRM> destinationCollection;
        private OldFFDBDataSet.EnvelopeGroupDataTable sourceTable;

        private Dictionary<int, int> sourceToDestinationID;
        private Dictionary<string, int> destinationNames;

        public EnvelopeGroupMerger(OldFFDBDataSet.EnvelopeGroupDataTable sourceTable, 
            ObservableCollection<EnvelopeGroupDRM> destinationCollection)
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
            foreach (EnvelopeGroupDRM type in destinationCollection)
                destinationNames.Add(type.Name, type.ID);
        }

        private void mergeSourceValuesIntoDestination()
        {
            foreach (OldFFDBDataSet.EnvelopeGroupRow group in sourceTable)
                mergeEnvelopeGroupIntoDestination(group);
        }

        private void mergeEnvelopeGroupIntoDestination(OldFFDBDataSet.EnvelopeGroupRow group)
        {
            int destinationID;

            if (destinationNames.TryGetValue(group.name, out destinationID))
                sourceToDestinationID.Add(group.id, destinationID);
            
            else
                appendDestinationWithNewType(group);
            
        }

        private void appendDestinationWithNewType(OldFFDBDataSet.EnvelopeGroupRow group)
        {
            EnvelopeGroupDRM newEnvelopeGroup = new EnvelopeGroupDRM(group.name);

            sourceToDestinationID.Add(group.id, newEnvelopeGroup.ID);
            destinationNames.Add(newEnvelopeGroup.Name, newEnvelopeGroup.ID);
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
                throw new Exception("Invalid Source Envelope Group ID");
            }

        }

    }
}
