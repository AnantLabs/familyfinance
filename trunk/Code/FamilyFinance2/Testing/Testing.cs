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
            FFDataBase.myExecuteFile(Properties.Resources.Drop_Tables, false);
            FFDataBase.myExecuteFile(Properties.Resources.Build_Tables, false);

            // Fill the data tables
            accountTypeTA.Fill(stressDS.AccountType);
            lineTypeTA.Fill(stressDS.LineType);
            envGroupTA.Fill(stressDS.EnvelopeGroup);
            accountTA.Fill(stressDS.Account);
            envelopeTA.Fill(stressDS.Envelope);
            lineTA.Fill(stressDS.LineItem);
            envLineTA.Fill(stressDS.EnvelopeLine);

            accountID = FFDataBase.myDBGetNewID("id", "Account") - 1;
            aTypeID = FFDataBase.myDBGetNewID("id", "AccountType") - 1;
            envelopeID = FFDataBase.myDBGetNewID("id", "Envelope") - 1;
            eGroupID = FFDataBase.myDBGetNewID("id", "EnvelopeGroup") - 1;
            eLineID = FFDataBase.myDBGetNewID("id", "EnvelopeLine") - 1;
            lTypeID = FFDataBase.myDBGetNewID("id", "LineType") - 1;

            lineID = FFDataBase.myDBGetNewID("id", "LineItem") - 1;
            transID = FFDataBase.myDBGetNewID("transactionID", "LineItem") - 1;
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
                num = rnd.Next(0, 1000);

                if (num == 0)
                    addAccountType();

                else if (num == 1)
                    addLineType();

                else if (num == 2)
                    addEnvelopeGroup();

                else if (num <= 10)
                    addAccount();

                else if (num <= 6)
                    addEnvelope();


                if (num <= 200)
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
            aTypeID++;
            stressDS.AccountType.AddAccountTypeRow(aTypeID, "aType" + aTypeID.ToString());
        }

        private void addLineType()
        {
            lTypeID++;
            stressDS.LineType.AddLineTypeRow(lTypeID, "lType" + lTypeID.ToString());
        }

        private void addEnvelopeGroup()
        {
            eGroupID++;
            stressDS.EnvelopeGroup.AddEnvelopeGroupRow(eGroupID, "group" + eGroupID.ToString());
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

            accountID++;
            stressDS.Account.AddAccountRow(accountID, "account" + accountID.ToString(), stressDS.AccountType.FindByid(aType), catagory, open, creditDebit, envelopes);
        }

        private void addEnvelope()
        {
            int eGroup = rnd.Next(1, eGroupID);

            envelopeID++;
            stressDS.Envelope.AddEnvelopeRow(envelopeID, "envelope" + envelopeID.ToString(), stressDS.EnvelopeGroup.FindByid(eGroup), false);
        }

        private void simpleTrans()
        {
            StressDataSet.LineItemRow lineRowC = stressDS.LineItem.NewLineItemRow();
            StressDataSet.LineItemRow lineRowD = stressDS.LineItem.NewLineItemRow();

            decimal amount = rnd.Next(1, 100000) / 100.0m;

            transID++;
            lineID++;

            if (rnd.Next(0, 5) == 0)
                date = date.AddDays(-3);
            else
                date = date.AddDays(1);

            lineRowC.id = lineID;
            lineRowC.transactionID = transID;
            lineRowC.date = date;
            lineRowC.amount = amount;
            lineRowC.description = "";
            lineRowC.confirmationNumber = "";
            lineRowC.complete = LineState.CLEARED;
            lineRowC.creditDebit = LineCD.DEBIT;
            lineRowC.typeID = rnd.Next(1, lTypeID);
            lineRowC.accountID = rnd.Next(1, accountID);

            this.stressDS.LineItem.AddLineItemRow(lineRowC);
            this.addEnvLines(ref lineRowC);
            
            lineID++;

            lineRowD.id = lineID;
            lineRowD.transactionID = transID;
            lineRowD.date = date;
            lineRowD.amount = amount;
            lineRowD.description = "";
            lineRowD.confirmationNumber = "";
            lineRowD.complete = LineState.CLEARED;
            lineRowD.creditDebit = LineCD.CREDIT;
            lineRowD.typeID = rnd.Next(1, lTypeID);
            lineRowD.accountID = rnd.Next(1, accountID);

            this.stressDS.LineItem.AddLineItemRow(lineRowD);
            this.addEnvLines(ref lineRowD);
        }

        private void complexTrans()
        {
            StressDataSet.LineItemRow lineRow;
            decimal cSum = 0.0m;
            decimal dSum = 0.0m;
            int num = rnd.Next(3, 8);

            transID++;
            date = date.AddDays(1.0);

            for (int i = 0; i < num; i++)
            {
                lineID++;

                lineRow = stressDS.LineItem.NewLineItemRow();
                lineRow.id = lineID;
                lineRow.transactionID = transID;
                lineRow.date = date;
                lineRow.description = "";
                lineRow.confirmationNumber = "";
                lineRow.complete = LineState.CLEARED;
                lineRow.creditDebit = Convert.ToBoolean(rnd.Next(0, 2));
                lineRow.typeID = rnd.Next(1, lTypeID);
                lineRow.accountID = rnd.Next(1, accountID);

                if (i + 1 == num && cSum > dSum)
                {
                    lineRow.amount = cSum - dSum;
                    lineRow.creditDebit = LineCD.DEBIT;
                }
                else if (i + 1 == num && dSum >= cSum)
                {
                    lineRow.amount = dSum - cSum;
                    if(rnd.Next(0,5) == 0)
                        lineRow.creditDebit = LineCD.DEBIT; //Error transaction
                    else
                        lineRow.creditDebit = LineCD.CREDIT;

                }
                else 
                {
                    lineRow.amount = rnd.Next(1, 100000) / 100.0m;

                    if (lineRow.creditDebit == LineCD.CREDIT)
                        cSum += lineRow.amount;
                    else
                        dSum += lineRow.amount;
                }

                this.stressDS.LineItem.AddLineItemRow(lineRow);
                this.addEnvLines(ref lineRow);
            }
        }

        private void addEnvLines(ref StressDataSet.LineItemRow line)
        {
            if (false == line.AccountRow.envelopes)
            {
                return;
            }
            else if (rnd.Next(0, 10) == 0) 
            {
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
            }
        }

        
    }
}
