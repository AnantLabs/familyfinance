using System;
using System.Windows.Data;
using System.Collections.ObjectModel;

using FamilyFinance.Buisness;
using FamilyFinance.Data;
using System.Reflection;

namespace FamilyFinance.Presentation.EditTransaction
{
    public class EditTransactionVM : ViewModel
    {
        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////
        public TransactionModel TransactionModel { get; private set; }

        public ListCollectionView CreditsView { get; private set; }

        public ListCollectionView DebitsView { get; private set; }

        public ListCollectionView EnvelopeLinesView { get; private set; } 


        public ListCollectionView TransactionTypesView { get; private set; }

        public ListCollectionView AccountsView { get; private set; }

        public ListCollectionView EnvelopesView { get; private set; }

        public decimal EnvelopeLineSum
        {
            get
            {
                return 0;
            }
        }


        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private bool CreditsFilter(object item)
        {
            LineItemDRM lineRow = (LineItemDRM)item;
            bool keepItem = false;

            if (lineRow.Polarity == PolarityCON.CREDIT)
                keepItem = true;

            return keepItem;
        }

        private bool DebitsFilter(object item)
        {
            LineItemDRM lineRow = (LineItemDRM)item;
            bool keepItem = false; 

            if (lineRow.Polarity == PolarityCON.DEBIT)
                keepItem = true;

            return keepItem;
        }

        private void setupViews()
        {
            this.CreditsView = new ListCollectionView(this.TransactionModel.LineItems);
            this.CreditsView.Filter = new Predicate<Object>(CreditsFilter);
            this.CreditsView.CurrentChanged += new EventHandler(CreditView_CurrentChanged);

            this.DebitsView = new ListCollectionView(this.TransactionModel.LineItems);
            this.DebitsView.Filter = new Predicate<Object>(DebitsFilter);
            this.DebitsView.CurrentChanged += new EventHandler(DebitView_CurrentChanged);

            this.TransactionTypesView = new ListCollectionView(DataSetModel.Instance.TransactionTypes);

            this.AccountsView = new ListCollectionView(DataSetModel.Instance.Accounts);

        }

        private void CreditView_CurrentChanged(object sender, EventArgs e)
        {
            if (this.CreditsView.IsAddingNew)
            {
                LineItemDRM newLine = (LineItemDRM)CreditsView.CurrentAddItem;
                newCreditLine(newLine);
            }
        }

        private void DebitView_CurrentChanged(object sender, EventArgs e)
        {
            if (this.DebitsView.IsAddingNew)
            {
                LineItemDRM newLine = (LineItemDRM)DebitsView.CurrentAddItem;
                newDebitLine(newLine);
            }
        }

        private void newDebitLine(LineItemDRM newLine)
        {
            decimal suggestedAmount = this.TransactionModel.CreditSum - this.TransactionModel.DebitSum;

            if (suggestedAmount < 0)
                suggestedAmount = 0;

            newLine.Polarity = PolarityCON.DEBIT;
            newLine.Amount = suggestedAmount;
        }

        private void newCreditLine(LineItemDRM newLine)
        {
            decimal suggestedAmount = this.TransactionModel.DebitSum - this.TransactionModel.CreditSum;

            if (suggestedAmount < 0)
                suggestedAmount = 0;

            newLine.Polarity = PolarityCON.CREDIT;
            newLine.Amount = suggestedAmount;
        }


     

        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public EditTransactionVM()
        {

        }

        public void loadTransaction(int transID)
        {
            this.TransactionModel = new TransactionModel(transID);
            this.setupViews();

            this.reportAllPropertiesChanged();
        }


    }
}
