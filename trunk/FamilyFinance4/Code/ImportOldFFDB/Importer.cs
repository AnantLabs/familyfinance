using System.Collections.Generic;
using FamilyFinance.Buisness;
using FamilyFinance.Data;

namespace ImportOldFFDB
{
    class Importer
    {
        private OldFFDBDataSet oldData = new OldFFDBDataSet();
        private DataSetModel myData = DataSetModel.Instance;

        private Dictionary<int, int> oldToNewIDAccountType = new Dictionary<int, int>();
        private Dictionary<int, int> oldToNewIDEnvelopeGroup = new Dictionary<int, int>();
        private Dictionary<int, int> oldToNewIDLineType = new Dictionary<int, int>();
        private Dictionary<int, int> oldToNewIDAccount = new Dictionary<int, int>();
        private Dictionary<int, int> oldToNewIDEnvelope = new Dictionary<int, int>();

        public void import()
        {
            DataSetModel.Instance.loadData();

            this.fillOldDataSet();

            this.mergeAccountType();
            this.mergeEnvelopeGroup();
            this.mergeLineType();
            this.appendAccount();
            this.appendEnvelope();
            this.appendLineItems();

            DataSetModel.Instance.saveData();
        }

        private void fillOldDataSet()
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

        private void mergeAccountType()
        {
            // Create a dictionary of the current account Types for efficient lookup by name.
            Dictionary<string, int> currentValues = new Dictionary<string, int>();

            foreach (AccountTypeDRM exsistingRow in this.myData.AccountTypes)
            {
                currentValues.Add(exsistingRow.Name, exsistingRow.ID);
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

        private void mergeEnvelopeGroup()
        {
            // Create a dictionary of the current account Types for efficient lookup by name.
            Dictionary<string, int> currentValues = new Dictionary<string, int>();

            foreach (EnvelopeGroupDRM exsistingRow in this.myData.EnvelopeGroups)
            {
                currentValues.Add(exsistingRow.Name, exsistingRow.ID);
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

        private void mergeLineType()
        {
            // Create a dictionary of the current account Types for efficient lookup by name.
            Dictionary<string, int> currentValues = new Dictionary<string, int>();

            foreach (TransactionTypeDRM exsistingRow in this.myData.TransactionTypes)
            {
                currentValues.Add(exsistingRow.Name, exsistingRow.ID);
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

                AccountDRM acc = new AccountDRM();
                acc.Name = oldRow.name;
                acc.TypeID = newAccountTypeID;
                acc.Catagory = CatagoryCON.getCatagory(oldRow.catagory);
                acc.Closed = oldRow.closed;
                acc.UsesEnvelopes = oldRow.envelopes;

                // Assume there is bank information if this accounts catagory is an account.
                if (acc.Catagory == CatagoryCON.ACCOUNT)
                {
                    acc.HasBankInfo = true;
                    acc.AccountNormal = PolarityCON.GetPlolartiy(oldRow.creditDebit);
                }

                // Save the old and new ids.
                this.oldToNewIDAccount.Add(oldRow.id, acc.ID);
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

                EnvelopeDRM env = new EnvelopeDRM();
                env.Name = oldRow.name;
                env.GroupID = newEnvelopeGroupID;
                env.FavoriteAccountID = AccountCON.NULL.ID;
                env.Closed = oldRow.closed;

                // Save the old and new ids.
                this.oldToNewIDEnvelope.Add(oldRow.id, env.ID);
            }
        }

        private int getEnvelopeGroupID(OldFFDBDataSet.EnvelopeRow oldRow)
        {
            int newEnvelopeGroupID;

            if (!this.oldToNewIDEnvelopeGroup.TryGetValue(oldRow.groupID, out newEnvelopeGroupID))
            {
                // This means there the Envelope has an envelopeGroup ID that is uknown.
                string error = "Error reading group id from Envelope. Envelope.ID = " + oldRow.id + " Envelope.groupID = " + oldRow.groupID;
                System.Console.WriteLine(error);
                throw new System.Exception(error);
            }

            return newEnvelopeGroupID;
        }

        private void appendLineItems()
        {
            Dictionary<int, TransactionModel> oldIDToNewTransaction = new Dictionary<int, TransactionModel>();
            // Append all the old LineItem / transactions from the OldFFDBDataSet to the current data.

            foreach (OldFFDBDataSet.LineItemRow oldRow in this.oldData.LineItem)
            {
                TransactionModel transactionModel;

                if (oldIDToNewTransaction.TryGetValue(oldRow.id, out transactionModel))
                {
                    // The old transaction id was found in the dictionary.
                    // So do nothing yet.
                }
                else
                {
                    transactionModel = new TransactionModel();

                    transactionModel.Date = oldRow.date;
                    transactionModel.Description = oldRow.description;
                    transactionModel.TypeID = getNewTypeIDFromOldID(oldRow.typeID);
                }

                LineItemDRM lineDRM = new LineItemDRM();

                lineDRM.ConfirmationNumber = oldRow.confirmationNumber;
                lineDRM.Amount = oldRow.amount;
                lineDRM.Polarity = PolarityCON.GetPlolartiy(oldRow.creditDebit);
                lineDRM.State = TransactionStateCON.GetState(oldRow.complete[0]);
                lineDRM.AccountID = getNewAccountIDFromOldID(oldRow.accountID);

                transactionModel.LineItems.Add(lineDRM);
            }
        }

        private int getNewTypeIDFromOldID(int oldTypeID)
        {
            int newTypeID;

            if(oldToNewIDLineType.TryGetValue(oldTypeID, out newTypeID))
            {
                // do nothing the new ID was found
            }
            else
            {
                newTypeID = TransactionTypeCON.NULL.ID;
            }

            return newTypeID;
        }

        private int getNewAccountIDFromOldID(int oldAccountID)
        {
            int newAccountID;

            if (oldToNewIDAccount.TryGetValue(oldAccountID, out newAccountID))
            {
                // do nothing the new ID was found
            }
            else
            {
                newAccountID = AccountCON.NULL.ID;
            }

            return newAccountID;
        }


    }
}
