using System;
using System.Collections.Generic;

using FamilyFinance.Model;
using FamilyFinance.Custom;
using FamilyFinance.Database;

namespace FamilyFinance.EditTransaction
{
    class EditTransactionVM : FamilyFinance.Registry.TransactionModel
    {

        ///////////////////////////////////////////////////////////////////////
        // Properties to access this object.
        ///////////////////////////////////////////////////////////////////////
        public MyObservableCollection<LineItemModel> Credits { get; private set; }
        public MyObservableCollection<LineItemModel> Debits { get; private set; }

       


        public EditTransactionVM() : base(null)
        {
        }

        public void setTransaction(int transID)
        {
            MyObservableCollection<LineItemModel> credits = new MyObservableCollection<LineItemModel>();
            MyObservableCollection<LineItemModel> debits = new MyObservableCollection<LineItemModel>();
            this.transactionRow = MyData.getInstance().Transaction.FindByid(transID);
            FFDataSet.LineItemRow[] lines = this.transactionRow.GetLineItemRows();


            foreach (FFDataSet.LineItemRow line in lines)
            {
                if (line.creditDebit == LineCD.CREDIT)
                    credits.Add(new LineItemModel(line));
                else
                    debits.Add(new LineItemModel(line));
            }

            LineItemModel.TransactionID = transID;

            this.Debits = debits;
            this.Credits = credits;



            this.RaisePropertyChanged("Date");
            this.RaisePropertyChanged("TypeID");
            this.RaisePropertyChanged("TypeName");
            this.RaisePropertyChanged("Description");
            this.RaisePropertyChanged("ConfirmationNumber");
            this.RaisePropertyChanged("Complete");
            this.RaisePropertyChanged("TransactionError");
            this.RaisePropertyChanged("Credits");
            this.RaisePropertyChanged("Debits");

        }


    }
}
