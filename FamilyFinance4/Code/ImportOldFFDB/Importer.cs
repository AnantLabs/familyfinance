using System.Collections.Generic;
using System.Data;

using FamilyFinance.Data;
using FamilyFinance.Buisness;

namespace ImportOldFFDB
{
    class Importer
    {
        private OldFFDBDataSet oldData = new OldFFDBDataSet();
        private MyData myData = MyData.getInstance();

        private Dictionary<int, int> oldToNewIDAccountType = new Dictionary<int, int>();
        private Dictionary<int, int> oldToNewIDEnvelopeGroup = new Dictionary<int, int>();
        private Dictionary<int, int> oldToNewIDLineType = new Dictionary<int, int>();
        private Dictionary<int, int> oldToNewIDAccount = new Dictionary<int, int>();
        private Dictionary<int, int> oldToNewIDEnvelope = new Dictionary<int, int>();

        private void fillDataSet()
        {
            // Create anonymous table adapters to fill the Old Data set tables
            new OldFFDBDataSetTableAdapters.AccountTypeTableAdapter().Fill(oldData.AccountType);
            new OldFFDBDataSetTableAdapters.EnvelopeGroupTableAdapter().Fill(oldData.EnvelopeGroup);
            new OldFFDBDataSetTableAdapters.LineTypeTableAdapter().Fill(oldData.LineType);
            new OldFFDBDataSetTableAdapters.AccountTableAdapter().Fill(oldData.Account);
            new OldFFDBDataSetTableAdapters.EnvelopeTableAdapter().Fill(oldData.Envelope);
            new OldFFDBDataSetTableAdapters.LineItemTableAdapter().Fill(oldData.LineItem);
            new OldFFDBDataSetTableAdapters.EnvelopeLineTableAdapter().Fill(oldData.EnvelopeLine);
        }

        private void appendAccount()
        {
            // Add the Special cases NULL and MULTIPLE accounts to the oldToNewIDAccount dictionary.
            // because we don't want to duplicate the special cases.
            this.oldToNewIDAccount.Add(AccountCON.MULTIPLE.ID, AccountCON.MULTIPLE.ID);
            this.oldToNewIDAccount.Add(AccountCON.NULL.ID, AccountCON.NULL.ID);

            // Append all the old accounts from the OldFFDBDataSet to the current data.
            foreach (OldFFDBDataSet.AccountRow oldRow in this.oldData.Account)
            {
                int newAccountTypeID; 

                if(!this.oldToNewIDAccountType.TryGetValue(oldRow.typeID, out newAccountTypeID))
                {
                    // This means there the account has an accountType ID that is uknown.
                    string error = "Error reading type id from Account. Account.ID = " + oldRow.id + " Account.typeID = " + oldRow.typeID;
                    System.Console.WriteLine(error);
                    throw new System.Exception(error);
                }

                // Skip the special cases
                if (oldRow.id <= 0)
                    continue;

                AccountDRM acc = new AccountDRM(oldRow.name, newAccountTypeID, oldRow.catagory, oldRow.closed, oldRow.envelopes);

                // Assume there is bank information if this accounts catagory is an account.
                if (acc.CatagoryID == CatagoryCON.ACCOUNT.ID)
                {
                    acc.HasBankInfo = true;
                    acc.AccountNormal = oldRow.creditDebit;
                }

                // Save the old and new ids.
                this.oldToNewIDAccount.Add(oldRow.id, acc.ID);
            }
        }

        private void mergeAccountType()
        {
            // Create a dictionary of the current account Types for efficient lookup by name.
            Dictionary<string, int> currentValues = new Dictionary<string, int>();

            foreach (FFDataSet.AccountTypeRow exsistingRow in this.myData.AccountType)
            {
                currentValues.Add(exsistingRow.name, exsistingRow.id);
            }


            // Now add all the old accountTypes from the OldFFDBDataSet to the current data.
            foreach (OldFFDBDataSet.AccountTypeRow oldRow in this.oldData.AccountType)
            {
                int newID;

                if (currentValues.TryGetValue(oldRow.name, out newID))
                {
                    // The old account type name is alread in the current table.
                    // So do nothing yet.
                }
                else
                {
                    // Else we need to add the old account type to our current data and get the generated newID.
                    newID = new AccountTypeDRM(oldRow.name).ID;
                }
                
                // Save the old and new ids.
                this.oldToNewIDAccountType.Add(oldRow.id, newID);
            }
        }

        private void appendEnvelope()
        {
            // Add the Special cases NULL and Split, No_envelope envelopes to the oldToNewIDEnvelope dictionary.
            // because we don't want to duplicate the special cases.
            this.oldToNewIDEnvelope.Add(EnvelopeCON.SPLIT.ID, EnvelopeCON.SPLIT.ID);
            this.oldToNewIDEnvelope.Add(EnvelopeCON.NULL.ID, EnvelopeCON.NULL.ID);
            this.oldToNewIDEnvelope.Add(EnvelopeCON.NO_ENVELOPE.ID, EnvelopeCON.NO_ENVELOPE.ID);

            // Append all the old envelopes from the OldFFDBDataSet to the current data.
            foreach (OldFFDBDataSet.EnvelopeRow oldRow in this.oldData.Envelope)
            {
                int newEnvelopeGroupID;

                if (!this.oldToNewIDEnvelopeGroup.TryGetValue(oldRow.groupID, out newEnvelopeGroupID))
                {
                    // This means there the Envelope has an envelopeGroup ID that is uknown.
                    string error = "Error reading group id from Envelope. Envelope.ID = " + oldRow.id + " Envelope.groupID = " + oldRow.groupID;
                    System.Console.WriteLine(error);
                    throw new System.Exception(error);
                }

                // Skip the special cases
                if (oldRow.id <= 0)
                    continue;

                EnvelopeDRM env = new EnvelopeDRM(oldRow.name, newEnvelopeGroupID, AccountCON.NULL.ID, oldRow.closed);

                // Save the old and new ids.
                this.oldToNewIDEnvelope.Add(oldRow.id, env.ID);
            }
        }

        private void mergeEnvelopeGroup()
        {
            // Create a dictionary of the current account Types for efficient lookup by name.
            Dictionary<string, int> currentValues = new Dictionary<string, int>();

            foreach (FFDataSet.EnvelopeGroupRow exsistingRow in this.myData.EnvelopeGroup)
            {
                currentValues.Add(exsistingRow.name, exsistingRow.id);
            }


            // Now add all the old Envelope Groups from the OldFFDBDataSet to the current data.
            foreach (OldFFDBDataSet.EnvelopeGroupRow oldRow in this.oldData.EnvelopeGroup)
            {
                int newID;

                if (currentValues.TryGetValue(oldRow.name, out newID))
                {
                    // The old Envelope Group name is alread in the current table.
                    // So do nothing yet.
                }
                else
                {
                    // Else we need to add the old account type to our current data and get the generated newID.
                    newID = new EnvelopeGroupDRM(oldRow.name).ID;
                }

                // Save the old and new ids.
                this.oldToNewIDEnvelopeGroup.Add(oldRow.id, newID);
            }
        }

        private void appendLineItem()
        {
            // There are no special case Line Items

            // Append all the old LineItem / transactions from the OldFFDBDataSet to the current data.
            //foreach (OldFFDBDataSet.EnvelopeRow oldRow in this.oldData.Envelope)
            //{
            //    int newEnvelopeGroupID;

            //    if (!this.oldToNewIDEnvelopeGroup.TryGetValue(oldRow.groupID, out newEnvelopeGroupID))
            //    {
            //        // This means there the Envelope has an envelopeGroup ID that is uknown.
            //        string error = "Error reading group id from Envelope. Envelope.ID = " + oldRow.id + " Envelope.groupID = " + oldRow.groupID;
            //        System.Console.WriteLine(error);
            //        throw new System.Exception(error);
            //    }

            //    // Skip the special cases
            //    if (oldRow.id <= 0)
            //        continue;

            //    EnvelopeDRM env = new EnvelopeDRM(oldRow.name, newEnvelopeGroupID, AccountCON.NULL.ID, oldRow.closed);

            //    // Save the old and new ids.
            //    this.oldToNewIDEnvelope.Add(oldRow.id, env.ID);
            //}
        }

        private void mergeLineType()
        {
            // Create a dictionary of the current account Types for efficient lookup by name.
            Dictionary<string, int> currentValues = new Dictionary<string, int>();

            foreach (FFDataSet.TransactionTypeRow exsistingRow in this.myData.TransactionType)
            {
                currentValues.Add(exsistingRow.name, exsistingRow.id);
            }


            // Now add all the old Envelope Groups from the OldFFDBDataSet to the current data.
            foreach (OldFFDBDataSet.LineTypeRow oldRow in this.oldData.LineType)
            {
                int newID;

                if (currentValues.TryGetValue(oldRow.name, out newID))
                {
                    // The old line type name is alread in the current table.
                    // So do nothing yet.
                }
                else
                {
                    // Else we need to add the old account type to our current data and get the generated newID.
                    newID = new TransactionTypeDRM(oldRow.name).ID;
                }

                // Save the old and new ids.
                this.oldToNewIDLineType.Add(oldRow.id, newID);
            }
        }

        public void import()
        {
            MyData.getInstance().readData();

            this.fillDataSet();

            this.mergeAccountType();
            this.mergeEnvelopeGroup();
            this.mergeLineType();
            this.appendAccount();
            this.appendEnvelope();
            this.appendLineItem();

            MyData.getInstance().saveData();
        }

    }
}
