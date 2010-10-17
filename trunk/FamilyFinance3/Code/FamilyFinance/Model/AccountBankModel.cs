using FamilyFinance.Database;

namespace FamilyFinance.Model
{
    class AccountBankModel : AccountModel
    {
        private FFDataSet.BankInfoRow bankInfoRow;

        public int AccountID
        {
            get
            {
                if (this.bankInfoRow == null)
                    return SpclAccount.NULL;
                else
                    return this.bankInfoRow.accountID;
            }
        }

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

                    this.saveRow();
                    this.RaisePropertyChanged("BankID");
                    this.RaisePropertyChanged("BankName");
                    this.RaisePropertyChanged("RoutingNumber");
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
                    
                    this.saveRow();
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

                    this.saveRow();
                    this.RaisePropertyChanged("AccountNormal");
                }
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
            this.bankInfoRow = MyData.getInstance().BankInfo.FindByaccountID(this.ID);
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
                this.saveRow();
            }
        }

        private void saveRow()
        {
            MyData.getInstance().saveBankInfoRow(this.bankInfoRow);
        }
    }
}
