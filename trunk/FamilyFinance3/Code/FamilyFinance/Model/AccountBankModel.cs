using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;


using FamilyFinance.Database;
using FamilyFinance.Model;

namespace FamilyFinance.Model
{
    class AccountBankModel : AccountModel
    {

        private FFDataSet.BankInfoRow bankInfoRow;

        public int BankID
        {
            get
            {
                if (this.bankInfoRow == null)
                    return SpclBank.NULL;
                else
                    return this.bankInfoRow.bankID;
            }

            set
            {
                if (this.bankInfoRow != null)
                {
                    this.bankInfoRow.bankID = value;
                    MyData.getInstance().saveBankInfoRow(this.bankInfoRow);
                    this.RaisePropertyChanged("BankID");
                    this.RaisePropertyChanged("BankID");
                    this.RaisePropertyChanged("RoutingNumber");
                }
            }
        }

        public string BankAccountNumber
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
                    MyData.getInstance().saveBankInfoRow(this.bankInfoRow);
                    this.RaisePropertyChanged("BankAccountNumber");
                }
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

        public bool AccountNormal
        {
            get
            {
                if (this.bankInfoRow == null)
                    return LineCD.DEBIT;
                else
                    return this.bankInfoRow.creditDebit;
            }

            set
            {
                if (this.bankInfoRow != null)
                {
                    this.bankInfoRow.creditDebit = value;
                    MyData.getInstance().saveBankInfoRow(this.bankInfoRow);
                    this.RaisePropertyChanged("AccountNormal");
                }
            }
        }

        public bool CanUseEnvelopes
        {
            get
            {
                if (this.accountRow.catagory == SpclAccountCat.ACCOUNT)
                    return true;

                else
                    return false;
            }
        }

        public string BankInfo
        {
            get
            {
                if (this.bankInfoRow == null)
                    return "Add";
                else
                    return "Show";
            }
        }

        public AccountBankModel(FFDataSet.AccountRow aRow) : base(aRow)
        {
            this.bankInfoRow = MyData.getInstance().BankInfo.FindByaccountID(this.accountRow.id);
        }

        public AccountBankModel() : base()
        {
            this.bankInfoRow = null;
        }

        public void addBankInfo()
        {
            if (this.bankInfoRow == null)
            {
                this.bankInfoRow = MyData.getInstance().BankInfo.NewBankInfoRow();
                MyData.getInstance().BankInfo.AddBankInfoRow(this.bankInfoRow);


            }
        }
    }
}
