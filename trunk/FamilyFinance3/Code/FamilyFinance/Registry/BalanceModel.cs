using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using FamilyFinance.Database;
using FamilyFinance.Model;
using FamilyFinance.Custom;

namespace FamilyFinance.Registry
{
    class BalanceModel: ModelBase
    {
        private enum BalType { Account, Income, Expence, Envelope, SubEnvelope, SubAccount }
        
        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        public int AccountID { get; private set; }
        public int EnvelopeID { get; private set; }
        public string Name { get; private set; }
        public string GroupName { get; private set; }
        public decimal Balance { get; private set; }

        public MyObservableCollection<BalanceModel> SubBalances { get; set; }

        private readonly BalType _Type;
        
        ///////////////////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////////////////
        private void setAccountBalance()
        {
            FFDataSet.LineItemRow[] lines = MyData.getInstance().Account.FindByid(this.AccountID).GetLineItemRows();
            FFDataSet.BankInfoRow bankInfo = MyData.getInstance().BankInfo.FindByaccountID(this.AccountID);
            decimal cSum = 0.0m;
            decimal dSum = 0.0m;
            bool lineCD = LineCD.DEBIT; // default

            foreach (FFDataSet.LineItemRow line in lines)
            {
                if (line.creditDebit == LineCD.CREDIT)
                    cSum += line.amount;
                else
                    dSum += line.amount;
            }


            if (bankInfo != null && bankInfo.creditDebit == LineCD.CREDIT)
                lineCD = LineCD.CREDIT;

            if (lineCD == LineCD.DEBIT)
                Balance = dSum - cSum;
            else
                Balance = cSum - dSum;
        }

        private void setEnvelopeBalance()
        {
            FFDataSet.EnvelopeLineRow[] subLines = MyData.getInstance().Envelope.FindByid(this.EnvelopeID).GetEnvelopeLineRows();
            decimal cSum = 0.0m;
            decimal dSum = 0.0m;

            foreach (FFDataSet.EnvelopeLineRow subLine in subLines)
            {
                if (subLine.LineItemRow.creditDebit == LineCD.CREDIT)
                    cSum += subLine.amount;
                else
                    dSum += subLine.amount;
            }

            Balance = dSum - cSum;
        }

        private void setSubBalance()
        {
            FFDataSet.EnvelopeLineRow[] subLines = MyData.getInstance().Envelope.FindByid(this.EnvelopeID).GetEnvelopeLineRows();
            decimal cSum = 0.0m;
            decimal dSum = 0.0m;

            foreach (FFDataSet.EnvelopeLineRow subLine in subLines)
            {
                if (subLine.LineItemRow.accountID == this.AccountID)
                {
                    if (subLine.LineItemRow.creditDebit == LineCD.CREDIT)
                        cSum += subLine.amount;
                    else
                        dSum += subLine.amount;
                }
            }

            Balance = dSum - cSum;
        }

        private void setSubBalanceColection()
        {
            bool envelopes = MyData.getInstance().Account.FindByid(this.AccountID).envelopes;
            MyObservableCollection<BalanceModel> tempSubs = new MyObservableCollection<BalanceModel>();

            if (this._Type == BalanceModel.BalType.Account && envelopes == true)
            {
                int[] ids =
                    (from EnvelopeLine in MyData.getInstance().EnvelopeLine
                        where EnvelopeLine.LineItemRow.accountID == this.AccountID
                        select EnvelopeLine.envelopeID).Distinct().ToArray();

                foreach (int envID in ids)
                {
                    BalanceModel bm = new BalanceModel(this.AccountID, envID, true);
                    if(bm.Balance != 0.0m)
                        tempSubs.Add(bm);
                }
            }
            else if (this._Type == BalanceModel.BalType.Envelope)
            {
                int[] ids =
                    (from EnvelopeLine in MyData.getInstance().EnvelopeLine
                    where EnvelopeLine.envelopeID == this.EnvelopeID
                    select EnvelopeLine.LineItemRow.accountID).Distinct().ToArray();

                foreach (int accID in ids)
                {
                    BalanceModel bm = new BalanceModel(accID, this.EnvelopeID, false);
                    if (bm.Balance != 0.0m)
                        tempSubs.Add(bm);
                }
            }

            if (tempSubs.Count > 0)
            {
                tempSubs.Sort(new BalanceModelComparer());
                this.SubBalances = tempSubs;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public BalanceModel(FFDataSet.AccountRow aRow)
        {
            if (aRow == null)
                throw new ArgumentNullException("AccountRow");


            this.AccountID = aRow.id;
            this.EnvelopeID = SpclEnvelope.NULL;
            this.Name = aRow.name;
            this.GroupName = aRow.AccountTypeRow.name;

            if (aRow.catagory == SpclAccountCat.ACCOUNT)
            {
                this._Type = BalanceModel.BalType.Account;
                this.setAccountBalance();
                this.setSubBalanceColection();
            }
            else if (aRow.catagory == SpclAccountCat.EXPENSE)
            {
                this._Type = BalanceModel.BalType.Expence;
                this.Balance = 0.0m;
            } 
            else if (aRow.catagory == SpclAccountCat.INCOME)
            {
                this._Type = BalanceModel.BalType.Income;
                this.Balance = 0.0m;
            }

        }

        public BalanceModel(FFDataSet.EnvelopeRow eRow)
        {
            if (eRow == null)
                throw new ArgumentNullException("EnvelopeRow");

            this.AccountID = SpclAccount.NULL;
            this.EnvelopeID = eRow.id;
            this.Name = eRow.name;
            this.GroupName = eRow.EnvelopeGroupRow.name;
            this._Type = BalanceModel.BalType.Envelope;

            this.setEnvelopeBalance();
            this.setSubBalanceColection();
        }

        public BalanceModel(int accountID, int envelopeID, bool subEnvelope)
        {
            this.AccountID = accountID;
            this.EnvelopeID = envelopeID;

            if (subEnvelope)
            {
                this.Name = MyData.getInstance().Envelope.FindByid(envelopeID).name;
                this.GroupName = null;
                this._Type = BalanceModel.BalType.SubEnvelope;

                this.setSubBalance();
            }
            else
            {
                this.Name = MyData.getInstance().Account.FindByid(accountID).name;
                this.GroupName = null;
                this._Type = BalanceModel.BalType.SubAccount;

                this.setSubBalance();
            }
        }

    }

    class BalanceModelComparer : System.Collections.Generic.IComparer<BalanceModel>
    {
        public int Compare(BalanceModel x, BalanceModel y)
        {
            return string.Compare(x.Name, y.Name);
        }
    }
}
