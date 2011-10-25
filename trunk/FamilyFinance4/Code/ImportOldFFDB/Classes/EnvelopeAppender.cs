using System.Collections.Generic;
using System.Collections.ObjectModel;
using FamilyFinance.Buisness;
using System;

namespace ImportOldFFDB
{
    class EnvelopeAppender
    {
        private OldFFDBDataSet.EnvelopeDataTable sourceTable;
        private EnvelopeGroupMerger envelopeGroupMerger;

        private Dictionary<int, int> sourceToDestinationID;
        
        public EnvelopeAppender(OldFFDBDataSet.EnvelopeDataTable sourceTable, 
            EnvelopeGroupMerger envelopeGroupMerger)
        {
            sourceToDestinationID = new Dictionary<int, int>();

            this.sourceTable = sourceTable;
            this.envelopeGroupMerger = envelopeGroupMerger;
        }

        public void appendEnvelopes()
        {
            foreach (OldFFDBDataSet.EnvelopeRow Envelope in sourceTable)
                appendDestinationWithNewEnvelope(Envelope);
        }

        private void appendDestinationWithNewEnvelope(OldFFDBDataSet.EnvelopeRow sourceEnvelope)
        {
            if (sourceEnvelope.id <= 0)
                handleSpecialEnvelope(sourceEnvelope);

            else
            {
                EnvelopeDRM newEnvelope = new EnvelopeDRM();

                newEnvelope.Name = sourceEnvelope.name;
                newEnvelope.GroupID = envelopeGroupMerger.getDestinationIdFromSourceId(sourceEnvelope.groupID);
                newEnvelope.Closed = sourceEnvelope.closed;

                sourceToDestinationID.Add(sourceEnvelope.id, newEnvelope.ID);
            }
        }

        private void handleSpecialEnvelope(OldFFDBDataSet.EnvelopeRow Envelope)
        {
            sourceToDestinationID.Add(Envelope.id, Envelope.id);
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
                throw new Exception("Invalid Source Envelope ID");
            }

        }

    }
}
