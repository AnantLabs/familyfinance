using System.Collections.Generic;
using System.Collections.ObjectModel;
using FamilyFinance.Buisness;
using System;

namespace ImportOldFFDB
{
    class AccountAppender
    {
        private OldFFDBDataSet.AccountDataTable sourceTable;
        private AccountTypeMerger accountTypeMerger;

        private Dictionary<int, int> sourceToDestinationID;
        
        public AccountAppender(OldFFDBDataSet.AccountDataTable sourceTable, 
            AccountTypeMerger accountTypeMerger)
        {
            sourceToDestinationID = new Dictionary<int, int>();

            this.sourceTable = sourceTable;
            this.accountTypeMerger = accountTypeMerger;
        }

        public void appendAccounts()
        {
            foreach (OldFFDBDataSet.AccountRow account in sourceTable)
                appendDestinationWithNewAccount(account);
        }

        private void appendDestinationWithNewAccount(OldFFDBDataSet.AccountRow sourceAccount)
        {
            if (sourceAccount.id <= 0)
                handleSpecialAccount(sourceAccount);

            else
            {
                AccountDRM newAccount = new AccountDRM();

                newAccount.Name = sourceAccount.name;
                newAccount.TypeID = accountTypeMerger.getDestinationIdFromSourceId(sourceAccount.typeID);
                newAccount.Catagory = FamilyFinance.Data.CatagoryCON.getCatagory(sourceAccount.catagory);
                newAccount.Closed = sourceAccount.closed;
                newAccount.UsesEnvelopes = sourceAccount.envelopes;

                if (newAccount.Catagory == FamilyFinance.Data.CatagoryCON.ACCOUNT)
                {
                    newAccount.HasBankInfo = true;
                    newAccount.AccountNormal = FamilyFinance.Data.PolarityCON.GetPlolartiy(sourceAccount.creditDebit);
                }

                sourceToDestinationID.Add(sourceAccount.id, newAccount.ID);
            }
        }

        private void handleSpecialAccount(OldFFDBDataSet.AccountRow account)
        {
            sourceToDestinationID.Add(account.id, account.id);
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
