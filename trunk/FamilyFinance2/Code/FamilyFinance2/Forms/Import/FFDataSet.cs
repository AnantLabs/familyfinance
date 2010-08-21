using System;
using System.Collections.Generic;

using FamilyFinance2.SharedElements;
using FamilyFinance2.Forms.Import.FFDataSetTableAdapters;

namespace FamilyFinance2.Forms.Import 
{
    public partial class FFDataSet 
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Local Constants and variables
        ////////////////////////////////////////////////////////////////////////////////////////////
        private int AccountID;
        private int AccountTypeID;
        private int EnvelopeID;
        private int EnvelopeLineID;
        private int LineItemID;
        private int TransactionID;
        private int LineTypeID;

        private Dictionary<string, int> accountMap; // Cache holding the name and id of accounts being added. used for quick lookup of new id.
        private Dictionary<string, int> envelopeMap;

        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Table adapters
        ////////////////////////////////////////////////////////////////////////////////////////////
        private AccountTypeTableAdapter accountTypeTA;
        private AccountTableAdapter accountTA;
        private EnvelopeTableAdapter envelopeTA;
        private EnvelopeLineTableAdapter eLineTA;
        private LineItemTableAdapter lineItemTA;
        private LineTypeTableAdapter lineTypeTA;


        ////////////////////////////////////////////////////////////////////////////////////////////
        //    Internal Events
        ////////////////////////////////////////////////////////////////////////////////////////////
        private void Account_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            (e.Row as AccountRow).id = this.AccountID++;
        }

        private void AccountType_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            (e.Row as AccountTypeRow).id = this.AccountTypeID++;
        }

        private void Envelope_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            (e.Row as EnvelopeRow).id = this.EnvelopeID++;
        }

        private void EnvelopeLine_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            (e.Row as EnvelopeLineRow).id = this.EnvelopeLineID++;
            (e.Row as EnvelopeLineRow).description = "";
        }

        private void LineItem_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            int maxTransID = 0;
            LineItemRow newLine = e.Row as LineItemRow;

            foreach (LineItemRow row in this.LineItem)
                if (row.transactionID > maxTransID)
                    maxTransID = row.transactionID;

            if (maxTransID >= this.TransactionID)
                newLine.transactionID = maxTransID + 1;
            else
                newLine.transactionID = this.TransactionID;

            newLine.id = this.LineItemID++;
            newLine.description = "";
            newLine.confirmationNumber = "";
        }

        private void LineType_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
        {
            (e.Row as LineTypeRow).id = this.LineTypeID++;
        }

        private void ColumnChanging(object sender, System.Data.DataColumnChangeEventArgs e)
        {
            if ("name" == e.Column.ColumnName)
            {
                int max = e.Column.MaxLength;
                string pValue = (e.ProposedValue) as string;

                if (pValue.Length > max)
                    e.ProposedValue = pValue.Substring(0, max);
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////
        //   Public Functions
        ////////////////////////////////////////////////////////////////////////////////////////////
        public void mySaveData()
        {
            this.accountTypeTA.Update(AccountType);
            this.accountTA.Update(Account);
            this.envelopeTA.Update(Envelope);
            this.lineTypeTA.Update(LineType);
            this.lineItemTA.Update(LineItem);
            this.eLineTA.Update(EnvelopeLine);
        }

        public void myInit()
        {
            // Initialize Table Adapters
            this.accountTypeTA = new AccountTypeTableAdapter();
            this.accountTA = new AccountTableAdapter();
            this.envelopeTA = new EnvelopeTableAdapter();
            this.lineTypeTA = new LineTypeTableAdapter();
            this.lineItemTA = new LineItemTableAdapter();
            this.eLineTA = new EnvelopeLineTableAdapter();

            // Initialize Maps
            this.accountMap = new Dictionary<string, int>();
            this.envelopeMap = new Dictionary<string, int>();

            // Fillup certain tables (Tables that can allow overlap of imported data)
            this.accountTypeTA.Fill(this.AccountType);
            this.lineTypeTA.Fill(this.LineType);

            // Setup ID Counters
            this.AccountID = DBquery.getNewID("id", "Account");
            this.AccountTypeID = DBquery.getNewID("id", "AccountType");
            this.EnvelopeID = DBquery.getNewID("id", "Envelope");
            this.LineTypeID = DBquery.getNewID("id", "LineType");
            this.LineItemID = DBquery.getNewID("id", "LineItem");
            this.TransactionID = DBquery.getNewID("transactionID", "LineItem");
            this.EnvelopeLineID = DBquery.getNewID("id", "EnvelopeLine");

            // Subscribe to table events
            this.Account.TableNewRow += new System.Data.DataTableNewRowEventHandler(Account_TableNewRow);
            this.Account.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);

            this.AccountType.TableNewRow += new System.Data.DataTableNewRowEventHandler(AccountType_TableNewRow);
            this.AccountType.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);

            this.Envelope.TableNewRow += new System.Data.DataTableNewRowEventHandler(Envelope_TableNewRow);
            this.Envelope.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);

            this.EnvelopeLine.TableNewRow += new System.Data.DataTableNewRowEventHandler(EnvelopeLine_TableNewRow);
            this.EnvelopeLine.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);

            this.LineItem.TableNewRow += new System.Data.DataTableNewRowEventHandler(LineItem_TableNewRow);
            this.LineItem.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);

            this.LineType.TableNewRow += new System.Data.DataTableNewRowEventHandler(LineType_TableNewRow);
            this.LineType.ColumnChanging += new System.Data.DataColumnChangeEventHandler(ColumnChanging);

        }

        private int myImportAccountType(string typeName)
        {
            if (String.IsNullOrEmpty(typeName))
                return SpclAccountType.NULL;

            // Replace the known bad Quicken names.
            switch (typeName)
            {
                case "Bank":
                    typeName = "Checking";
                    break;
                case "CCard":
                    typeName = "Credit Card";
                    break;
                case "Oth L":
                    typeName = "Loan";
                    break;
            }

            // Search for an exsisting Account type with the same name
            foreach (AccountTypeRow row in this.AccountType)
            {
                if (row.name == typeName)
                    return row.id;
            }

            // else it was not found
            AccountTypeRow newRow = this.AccountType.NewAccountTypeRow();
            newRow.name = typeName;

            this.AccountType.AddAccountTypeRow(newRow);

            return newRow.id;
        }

        public int myImportAccount(string name, string type, byte catagory)
        {
            // Check to see if the account is already in the map and table.
            int id;

            if (name.StartsWith("["))
                name = name.Substring(1, name.Length - 2);

            if (this.accountMap.TryGetValue(name, out id))
                return id;

            // else create a new account row.
            AccountRow newAccount = this.Account.NewAccountRow();

            newAccount.name = name;
            newAccount.typeID = this.myImportAccountType(type);
            newAccount.catagory = catagory;
            newAccount.closed = false;
            newAccount.envelopes = false; // Assume false, decide later.

            if (type == "CCard" || type == "Oth L" || catagory == SpclAccountCat.INCOME)
                newAccount.creditDebit = LineCD.CREDIT;
            else
                newAccount.creditDebit = LineCD.DEBIT;

            this.Account.AddAccountRow(newAccount);
            this.accountMap.Add(name, newAccount.id);

            return newAccount.id;
        }

        public bool myGetAccount(string name, out int accountID)
        {
            // Check to see if the account is already in the map and table.
            if (name.StartsWith("["))
                name = name.Substring(1, name.Length - 2);

            return this.accountMap.TryGetValue(name, out accountID);
        }

        public int myImportEnvelope(string name)
        {
            // Check to see if the envelope is already in the map and table.
            int id;

            if (name.StartsWith("["))
                name = name.Substring(1, name.Length - 2);

            if (string.IsNullOrEmpty(name))
                return SpclEnvelope.NOENVELOPE;

            if (this.envelopeMap.TryGetValue(name, out id))
                return id;

            // else create a new account row.
            EnvelopeRow newEnvelope = this.Envelope.NewEnvelopeRow();

            newEnvelope.name = name;
            newEnvelope.groupID = SpclEnvelopeGroup.NULL;
            newEnvelope.closed = false;

            this.Envelope.AddEnvelopeRow(newEnvelope);
            this.envelopeMap.Add(name, newEnvelope.id);

            return newEnvelope.id;
        }

        public int myImportLineType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return SpclLineType.NULL;

            // Replace the known bad Quicken names.
            switch (typeName)
            {
                case "DEBIT":
                    typeName = "Debit";
                    break;
                case "DEP":
                    typeName = "Deposit";
                    break;
                case "TXFR":
                    typeName = "Transfer";
                    break;
            }

            // Search for an exsisting Line type with the same name
            foreach (LineTypeRow row in this.LineType)
            {
                if (row.name == typeName)
                    return row.id;
            }

            // else it was not found
            LineTypeRow newRow = this.LineType.NewLineTypeRow();
            newRow.name = typeName;

            this.LineType.AddLineTypeRow(newRow);

            return newRow.id;
        }

        public void myFillForAutoDistribute()
        {
            this.lineItemTA.Fill(this.LineItem);
            this.eLineTA.Fill(this.EnvelopeLine);
        }

    }
}

namespace FamilyFinance2.Forms.Import.FFDataSetTableAdapters {
    
    
    public partial class EnvelopeLineTableAdapter {
    }
}
