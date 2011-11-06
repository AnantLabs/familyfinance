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
        private void setupViews()
        {
            this.CreditsView = new ListCollectionView(this.TransactionModel.LineItems);
            this.CreditsView.Filter = new Predicate<Object>(CreditsFilter);
            this.CreditsView.CurrentChanged += new EventHandler(CreditOrDebitView_CurrentChanged);

            this.DebitsView = new ListCollectionView(this.TransactionModel.LineItems);
            this.DebitsView.Filter = new Predicate<Object>(DebitsFilter);
            this.DebitsView.CurrentChanged += new EventHandler(CreditOrDebitView_CurrentChanged);

            this.TransactionTypesView = new ListCollectionView(DataSetModel.Instance.TransactionTypes);

            this.AccountsView = new ListCollectionView(DataSetModel.Instance.Accounts);
        }

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

        private void CreditOrDebitView_CurrentChanged(object sender, EventArgs e)
        {
            ListCollectionView view = (ListCollectionView) sender;

            if (view.IsAddingNew)
            {
                LineItemDRM newLine = (LineItemDRM)CreditsView.CurrentAddItem;

                newLine.Amount = suggestedAmountDependingOnView(view);
                newLine.Polarity = determinePolarityDependingOnView(view);


            }
        }

        private decimal suggestedAmountDependingOnView(ListCollectionView view)
        {
            decimal suggestedAmount;

            if(view == DebitsView)
                suggestedAmount = TransactionModel.CreditSum - TransactionModel.DebitSum;
            else
                suggestedAmount = TransactionModel.DebitSum - TransactionModel.CreditSum;

            if (suggestedAmount < 0)
                suggestedAmount = 0;

            return suggestedAmount;
        }

        private PolarityCON determinePolarityDependingOnView(ListCollectionView view)
        {
            if (view == DebitsView)
                return PolarityCON.DEBIT;
            else
                return PolarityCON.CREDIT;
        }

        private void resetViewFilterIfNeeded(ListCollectionView view)
        {
            // When adding a new line the credit or debit filter might be applied
            // too soon and a ghost copy of the line might appear in the opposite
            // view or datagrid. So when an item is added to the view and after 
            // the polarity is set refresh the opposite view to remove the ghost 
            // line.
            if (view == DebitsView)
                this.CreditsView.Refresh();
            else
                this.DebitsView.Refresh();
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
