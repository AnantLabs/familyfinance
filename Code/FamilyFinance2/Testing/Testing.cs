using System;
using System.Collections.Generic;
using System.Text;
using FamilyFinance2.Testing.StressDataSetTableAdapters;
using FamilyFinance2.SharedElements;

namespace FamilyFinance2.Testing
{
    class Testing
    {
        private StressDataSet stressDS;
        private Random rnd;

        private AccountTypeTableAdapter accountTypeTA;
        private AccountTableAdapter accountTA;
        private LineItemTableAdapter lineTA;
        private LineTypeTableAdapter lineTypeTA;
        private EnvelopeGroupTableAdapter envGroupTA;
        private EnvelopeTableAdapter envelopeTA;
        private EnvelopeLineTableAdapter envLineTA;

        private int aTypeID;
        private int lTypeID;
        private int eGroupID;
        private int accountID;
        private int envelopeID;
        private int lineID;
        private int transID;
        private int eLineID;

        private DateTime date;

        public void stressFillDataBase()
        {
            stressDS = new StressDataSet();
            rnd = new Random();

            accountTypeTA = new AccountTypeTableAdapter();
            lineTypeTA = new LineTypeTableAdapter();
            envGroupTA = new EnvelopeGroupTableAdapter();
            accountTA = new AccountTableAdapter();
            envelopeTA = new EnvelopeTableAdapter();
            lineTA = new LineItemTableAdapter();
            envLineTA = new EnvelopeLineTableAdapter();

            // Reset the database
            DBquery.dropTables();
            DBquery.buildTables();

            // Fill the data tables
            accountTypeTA.Fill(stressDS.AccountType);
            lineTypeTA.Fill(stressDS.LineType);
            envGroupTA.Fill(stressDS.EnvelopeGroup);
            accountTA.Fill(stressDS.Account);
            envelopeTA.Fill(stressDS.Envelope);
            lineTA.Fill(stressDS.LineItem);
            envLineTA.Fill(stressDS.EnvelopeLine);

            accountID = DBquery.getNewID("id", "Account");
            aTypeID = DBquery.getNewID("id", "AccountType");
            envelopeID = DBquery.getNewID("id", "Envelope");
            eGroupID = DBquery.getNewID("id", "EnvelopeGroup");
            eLineID = DBquery.getNewID("id", "EnvelopeLine");
            lTypeID = DBquery.getNewID("id", "LineType");

            lineID = DBquery.getNewID("id", "LineItem");
            transID = DBquery.getNewID("transactionID", "LineItem");
            date = DateTime.Now.AddMonths(-12*100).Date;

            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();
            addAccount();

            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();
            addEnvelope();

            int num;
            // Make alot of entries
            while (date < DateTime.Now.AddMonths(3).Date)
            {
                num = rnd.Next(0, 10000);

                if (num == 0)
                    addAccountType();
                else if (num == 1)
                    addLineType();
                else if (num == 2)
                    addEnvelopeGroup();

                if (10 < num && num < 15)
                    addAccount();

                if (30 < num && num < 34)
                    addEnvelope();


                if (100 < num && num < 200)
                    complexTrans();
                else
                    simpleTrans();
            }

            // Save the data
            accountTypeTA.Update(stressDS.AccountType);
            lineTypeTA.Update(stressDS.LineType);
            envGroupTA.Update(stressDS.EnvelopeGroup);
            accountTA.Update(stressDS.Account);
            envelopeTA.Update(stressDS.Envelope);
            lineTA.Update(stressDS.LineItem);
            envLineTA.Update(stressDS.EnvelopeLine);
        }

        private void addAccountType()
        {
            stressDS.AccountType.AddAccountTypeRow(aTypeID, "aType" + aTypeID.ToString());
            aTypeID++;
        }

        private void addLineType()
        {
            stressDS.LineType.AddLineTypeRow(lTypeID, "lType" + lTypeID.ToString());
            lTypeID++;
        }

        private void addEnvelopeGroup()
        {
            stressDS.EnvelopeGroup.AddEnvelopeGroupRow(eGroupID, "group" + eGroupID.ToString());
            eGroupID++;
        }

        private void addAccount()
        {
            int aType = rnd.Next(1, aTypeID);
            byte catagory = Convert.ToByte(rnd.Next(1, 4));
            bool creditDebit = Convert.ToBoolean(rnd.Next(0, 2));
            bool envelopes = false;
            bool open = Convert.ToBoolean(rnd.Next(0, 2)); 

            if (catagory == SpclAccountCat.ACCOUNT)
                envelopes = true; // Convert.ToBoolean(rnd.Next(0, 2));

            stressDS.Account.AddAccountRow(accountID, "account" + accountID.ToString(), stressDS.AccountType.FindByid(aType), catagory, open, creditDebit, envelopes);
            accountID++;
        }

        private void addEnvelope()
        {
            int eGroup = rnd.Next(1, eGroupID);

            stressDS.Envelope.AddEnvelopeRow(envelopeID, "envelope" + envelopeID.ToString(), stressDS.EnvelopeGroup.FindByid(eGroup), false);
            envelopeID++;
        }

        private void simpleTrans()
        {
            StressDataSet.LineItemRow lineRowC = stressDS.LineItem.NewLineItemRow();
            StressDataSet.LineItemRow lineRowD = stressDS.LineItem.NewLineItemRow();

            decimal amount = rnd.Next(1, 100000) / 100.0m;

            if (rnd.Next(0, 5) == 0)
                date = date.AddDays(-3);
            else
                date = date.AddDays(1);

            lineRowC.id = lineID;
            lineRowD.id = lineID + 1;
            lineRowC.transactionID = transID;
            lineRowD.transactionID = transID;
            lineRowC.date = date;
            lineRowD.date = date;
            lineRowC.amount = amount;
            lineRowD.amount = amount;
            lineRowC.description = "";
            lineRowD.description = "";
            lineRowC.confirmationNumber = "";
            lineRowD.confirmationNumber = "";
            lineRowC.complete = LineState.CLEARED;
            lineRowD.complete = LineState.CLEARED;
            lineRowC.creditDebit = LineCD.DEBIT;
            lineRowD.creditDebit = LineCD.CREDIT;
            lineRowC.typeID = rnd.Next(1, lTypeID);
            lineRowD.typeID = rnd.Next(1, lTypeID);
            lineRowC.accountID = lineRowD.oppAccountID = rnd.Next(1, accountID);
            lineRowD.accountID = lineRowC.oppAccountID = rnd.Next(1, accountID);

            this.addEnvLines(ref lineRowC);
            this.addEnvLines(ref lineRowD);

            this.stressDS.LineItem.AddLineItemRow(lineRowC);
            this.stressDS.LineItem.AddLineItemRow(lineRowD);
            
            transID++;
            lineID += 2;
        }

        private void complexTrans()
        {
            StressDataSet.LineItemRow lineRow;
            decimal cSum = 0.0m;
            decimal dSum = 0.0m;
            int creditAccount = 0;
            int debitAccount = 0;
            int creditCount = 0;
            int debitCount = 0;
            int num = rnd.Next(3, 8);

            date = date.AddDays(1.0);

            for (int i = 0; i < num; i++)
            {

                lineRow = stressDS.LineItem.NewLineItemRow();
                lineRow.id = lineID;
                lineRow.transactionID = transID;
                lineRow.date = date;
                lineRow.description = "";
                lineRow.confirmationNumber = "";
                lineRow.complete = LineState.CLEARED;
                lineRow.typeID = rnd.Next(1, lTypeID);
                lineRow.oppAccountID = SpclAccount.MULTIPLE; // ASSUME it is going to be split

                if (i + 1 == num && cSum > dSum)
                {
                    lineRow.amount = cSum - dSum;
                    lineRow.creditDebit = LineCD.DEBIT;
                    debitCount++;
                    lineRow.accountID = debitAccount = rnd.Next(1, accountID);
                }
                else if (i + 1 == num && dSum >= cSum)
                {
                    lineRow.amount = dSum - cSum;
                    lineRow.creditDebit = LineCD.CREDIT;
                    creditCount++;
                    lineRow.accountID = creditAccount = rnd.Next(1, accountID);
                }
                else 
                {
                    lineRow.amount = rnd.Next(1, 100000) / 100.0m;

                    if (Convert.ToBoolean(rnd.Next(0, 2)))
                    {
                        lineRow.creditDebit = LineCD.CREDIT;
                        creditCount++;
                        cSum += lineRow.amount;
                        lineRow.accountID = creditAccount = rnd.Next(1, accountID);
                    }
                    else
                    {
                        lineRow.creditDebit = LineCD.DEBIT;
                        debitCount++;
                        dSum += lineRow.amount;
                        lineRow.accountID = debitAccount = rnd.Next(1, accountID);
                    }
                }

                this.addEnvLines(ref lineRow);
                this.stressDS.LineItem.AddLineItemRow(lineRow);
                lineID++;
            } // END For loop

            // loop backwards and update the oppAccount fields for this transaction
            if (creditCount == 1)
            {
                int id = lineID;
                while (true)
                {
                    id--;
                    lineRow = stressDS.LineItem.FindByid(id);

                    if(lineRow.transactionID != transID)
                        break;

                    if (lineRow.creditDebit == LineCD.DEBIT)
                        lineRow.oppAccountID = creditAccount;
                }
            }

            if (debitCount == 1)
            {
                int id = lineID;
                while (true)
                {
                    id--;
                    lineRow = stressDS.LineItem.FindByid(id);

                    if (lineRow.transactionID != transID)
                        break;

                    if (lineRow.creditDebit == LineCD.CREDIT)
                        lineRow.oppAccountID = debitAccount;
                }
            }

            transID++;
        }

        private void addEnvLines(ref StressDataSet.LineItemRow line)
        {
            if (false == line.AccountRowByFK_Line_accountID.envelopes)
            {
                line.envelopeID = SpclEnvelope.NULL;
                return;
            }
            else
            {
                StressDataSet.EnvelopeLineRow eRow = null;
                decimal sum = 0.0m;
                int num = rnd.Next(1, 5);

                for (int i = 0; i < num; i++)
                {
                    eLineID++;

                    eRow = stressDS.EnvelopeLine.NewEnvelopeLineRow();
                    eRow.id = eLineID;
                    eRow.lineItemID = line.id;
                    eRow.envelopeID = rnd.Next(0, envelopeID);
                    eRow.description = "";

                    if (i + 1 == num)
                    {
                        eRow.amount = line.amount - sum;
                    }
                    else
                    {
                        eRow.amount = rnd.Next(-10000, 100000) / 100.0m;
                        sum += eRow.amount;
                    }

                    this.stressDS.EnvelopeLine.AddEnvelopeLineRow(eRow);
                }

                if (num == 1)
                    line.envelopeID = eRow.envelopeID;
                else
                    line.envelopeID = SpclEnvelope.SPLIT;

            }
        }

        
    }
}
