using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FamilyFinance.Data;

namespace FamilyFinance.Buisness
{
    /// <summary>
    /// A modle of an account row in the dataset. This also manages a bankInfo row, if the
    /// user wants to attach bank information to the account row.
    /// </summary>
    public class AccountDRM : DataRowModel
    {
        ///////////////////////////////////////////////////////////////////////
        // Local variables
        ///////////////////////////////////////////////////////////////////////
        protected FFDataSet.AccountRow accountRow;
        private FFDataSet.BankInfoRow bankInfoRow;
        
        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        public int ID
        {
            get
            {
                return this.accountRow.id;
            }
        }

        public string Name 
        {
            get 
            {
                return this.accountRow.name;
            }

            set
            {
                this.accountRow.name = value;
            }
        }

        public int TypeID
        {
            get 
            { 
                return this.accountRow.typeID; 
            }

            set
            {
                this.accountRow.typeID = value;
            }
        }

        public string TypeName
        {
            get
            {
                return this.accountRow.AccountTypeRow.name;
            }
        }

        public CatagoryCON Catagory
        {
            get
            {
                return CatagoryCON.getCatagory(this.accountRow.catagory);
            }

            set
            {
                this.accountRow.catagory = value.ID;

                this.reportPropertyChangedWithName("CatagoryName");
                this.reportPropertyChangedWithName("UsesEnvelopes");
                this.reportPropertyChangedWithName("CanUseEnvelopes");
            }
        }

        public string CatagoryName
        {
            get
            {
                return CatagoryCON.getName(this.accountRow.catagory);
            }
        }

        public bool Closed
        {
            get
            {
                return this.accountRow.closed;
            }

            set
            {
                this.accountRow.closed = value;
            }
        }

        public bool UsesEnvelopes
        {
            get
            {
                return this.accountRow.envelopes;
            }

            set
            {
                this.accountRow.envelopes = value;
            }
        }

        public bool CanUseEnvelopes
        {
            get
            {
                return (this.accountRow.catagory == CatagoryCON.ACCOUNT.ID);
            }
        }

        public bool HasBankInfo
        {
            get
            {
                return (this.bankInfoRow != null);
            }

            set
            {
                if (value == true && this.bankInfoRow == null)
                {
                    this.bankInfoRow = DataSetModel.Instance.NewBankInfoRow(this.accountRow);
                }
                else if (value == false && this.bankInfoRow != null)
                {
                    this.bankInfoRow.Delete();
                    this.bankInfoRow = null;
                }

                this.reportPropertyChangedWithName("AccountNumber");
                this.reportPropertyChangedWithName("AccountNormal");
                this.reportPropertyChangedWithName("NormalName");
                this.reportPropertyChangedWithName("BankName");
                this.reportPropertyChangedWithName("RoutingNumber");
            }
        }

        public string AccountNumber
        {
            get
            {
                if (this.bankInfoRow == null)
                    return "";
                else
                    return this.bankInfoRow.accountNumber;
            }

            set
            {
                if (this.bankInfoRow != null)
                {
                    this.bankInfoRow.accountNumber = value;
                }
            }
        }

        public PolarityCON AccountNormal
        {
            get
            {
                if (this.bankInfoRow == null)
                    return null;
                else
                    return PolarityCON.GetPlolartiy(this.bankInfoRow.polarity);
            }

            set
            {
                if (this.bankInfoRow != null)
                {
                    this.bankInfoRow.polarity = value.Value;
                }
            }
        }

        public string NormalName
        {
            get
            {
                if (this.bankInfoRow == null)
                    return "";

                else
                    return PolarityCON.GetPlolartiy(this.bankInfoRow.polarity).Name;
            }
        }

        public int BankID
        {
            get
            {
                if (this.bankInfoRow == null)
                    return BankCON.NULL.ID;
                else
                    return this.bankInfoRow.bankID;
            }

            set
            {
                if (this.bankInfoRow != null)
                {
                    this.bankInfoRow.bankID = value;

                    this.reportPropertyChangedWithName("BankName");
                    this.reportPropertyChangedWithName("RoutingNumber");
                }
            }
        }

        public string BankName
        {
            get
            {
                if (this.bankInfoRow == null)
                    return "";
                else
                    return this.bankInfoRow.BankRow.name;
            }
        }

        public string RoutingNumber
        {
            get
            {
                if (this.bankInfoRow == null)
                    return "";
                else
                    return this.bankInfoRow.BankRow.routingNumber;
            }
        }

        public decimal EndingBalance
        {
            get
            {
                return this.getEndingBalance();
            }
        }
        

        ///////////////////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////////////////
        public AccountDRM()
        {
            this.accountRow = DataSetModel.Instance.NewAccountRow();
            this.bankInfoRow = null;
        }

        public AccountDRM(FFDataSet.AccountRow accountRow, FFDataSet.BankInfoRow bankRow)
        {
            this.accountRow = accountRow;
            this.bankInfoRow = bankRow;
        }

        public bool IsSpecial()
        {
            if (AccountCON.isSpecial(this.ID))
                return true;
            else
                return false;
        }

        public decimal getEndingBalance()
        {
            decimal credits = 0;
            decimal debits = 0;

            FamilyFinance.Data.FFDataSet.LineItemRow[] lines;
            lines = this.accountRow.GetLineItemRows();

            foreach (FFDataSet.LineItemRow line in lines)
            {
                if (line.polarity == PolarityCON.CREDIT.Value)
                    credits += line.amount;
                else
                    debits += line.amount;
            }

            if (this.AccountNormal == PolarityCON.CREDIT)
                return credits - debits;
            else
                return debits - credits;
        }

        public decimal getClearedBalance()
        {
            decimal credits = 0;
            decimal debits = 0;

            FamilyFinance.Data.FFDataSet.LineItemRow[] lines;
            lines = this.accountRow.GetLineItemRows();

            foreach (FFDataSet.LineItemRow line in lines)
            {
                if (line.state == TransactionStateCON.CLEARED.Value
                    || line.state == TransactionStateCON.RECONSILED.Value)
                {
                    if (line.polarity == PolarityCON.CREDIT.Value)
                        credits += line.amount;
                    else
                        debits += line.amount;
                }
            }

            if (this.AccountNormal == PolarityCON.CREDIT)
                return credits - debits;
            else
                return debits - credits;
        }

        public decimal getReconciledBalance()
        {
            decimal credits = 0;
            decimal debits = 0;

            FamilyFinance.Data.FFDataSet.LineItemRow[] lines;
            lines = this.accountRow.GetLineItemRows();

            foreach (FFDataSet.LineItemRow line in lines)
            {
                if (line.state == TransactionStateCON.RECONSILED.Value)
                {
                    if (line.polarity == PolarityCON.CREDIT.Value)
                        credits += line.amount;
                    else
                        debits += line.amount;
                }
            }

            if (this.AccountNormal == PolarityCON.CREDIT)
                return credits - debits;
            else
                return debits - credits;
        }
        
        public void dependentLineItemChanged()
        {
            this.reportPropertyChangedWithName("EndingBalance");
        }

        

        public void deleteRowFromDataset()
        {
            this.accountRow.Delete();
        }

        public RegistryLineModel[] getTransactionLines()
        {
            FFDataSet.LineItemRow[] rawLines = this.accountRow.GetLineItemRows();
            List<RegistryLineModel> transactionList = new List<RegistryLineModel>();
            TransactionLine tempTransactionLine;

            foreach(FFDataSet.LineItemRow rawLine in rawLines)
            {
                // TODO: add in a search to remove duplicate transactions in the same lineItem list.
                tempTransactionLine = new TransactionLine(rawLine);
                transactionList.Add(new RegistryLineModel(tempTransactionLine));
            }

            return transactionList.ToArray();
        }
    }
}
