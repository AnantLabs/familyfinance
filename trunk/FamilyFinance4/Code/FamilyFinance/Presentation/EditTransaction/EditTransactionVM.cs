﻿using System;
using System.Windows.Data;
using System.Collections;
using System.Collections.ObjectModel;

using FamilyFinance.Buisness;
using FamilyFinance.Buisness.Sorters;
using FamilyFinance.Data;

namespace FamilyFinance.Presentation.EditTransaction
{
    public class EditTransactionVM : ViewModel
    {
        private EditTransactionWindow parentWindow;
        private LineItemModel currentLineItem;


        ///////////////////////////////////////////////////////////
        // Properties
        ///////////////////////////////////////////////////////////  
        public TransactionModel TransactionModel { get; private set; }

        public ListCollectionView TransactionTypesView { get; private set; }

        public ListCollectionView CreditsView { get; private set; }

        public ListCollectionView DebitsView { get; private set; }

        public ListCollectionView AccountsView 
        {
            get
            {
                ListCollectionView listView = new ListCollectionView(DataSetModel.Instance.Accounts);

                listView.CustomSort = new AccountsCategoryNameComparer();
                listView.Filter = new Predicate<Object>(AccountsFilter);

                return listView;
            }
        }

        public ListCollectionView EnvelopesView
        {
            get
            {
                ListCollectionView listView = new ListCollectionView(DataSetModel.Instance.Envelopes);

                listView.CustomSort = new EnvelopesNameComparer();
                listView.Filter = new Predicate<Object>(EnvelopesFilter);

                return listView;
            }
        }

        public ListCollectionView EnvelopeLinesView { get; private set; }

        public decimal EnvelopeLineSum
        {
            get
            {
                if (this.currentLineItem == null)
                    return 0;
                else
                    return this.currentLineItem.EnvelopeLineSum;
            }
        }


        ///////////////////////////////////////////////////////////
        // View Filters
        ///////////////////////////////////////////////////////////
        private bool CreditsFilter(object item)
        {
            LineItemModel lineRow = (LineItemModel)item;
            bool keepItem = false;

            if (lineRow.Polarity == PolarityCON.CREDIT)
                keepItem = true;

            return keepItem;
        }

        private bool DebitsFilter(object item)
        {
            LineItemModel lineRow = (LineItemModel)item;
            bool keepItem = false; 

            if (lineRow.Polarity == PolarityCON.DEBIT)
                keepItem = true;

            return keepItem;
        }

        private bool AccountsFilter(object item)
        {
            AccountDRM account = (AccountDRM)item;
            bool keepItem = true;

            if (account.ID == AccountCON.MULTIPLE.ID)
                keepItem = false;

            return keepItem;
        }

        private bool EnvelopesFilter(object item)
        {
            EnvelopeDRM account = (EnvelopeDRM)item;
            bool keepItem = true;

            if (account.ID == EnvelopeCON.SPLIT.ID)
                keepItem = false;

            return keepItem;
        }



        ///////////////////////////////////////////////////////////
        // Event Functions
        ///////////////////////////////////////////////////////////
        private void CreditOrDebitView_CurrentChanged(object sender, EventArgs e)
        {
            ListCollectionView view = (ListCollectionView) sender;

            if (view.IsAddingNew)
            {
                modifyNewLineItem(view);
            }

            setCurrentLine((LineItemModel)view.CurrentItem);

        }

        private void EnvelopeLinesView_CurrentChanged(object sender, EventArgs e)
        {

            if (this.EnvelopeLinesView.IsAddingNew)
            {
                EnvelopeLineDRM newELine = (EnvelopeLineDRM)this.EnvelopeLinesView.CurrentAddItem;

                newELine.Amount = suggestedSubLineAmountDependingOnCurrentLine();
            }

            //this.reportPropertyChangedWithName("EnvelopeLineSum");
        }


        ///////////////////////////////////////////////////////////
        // Private functions
        ///////////////////////////////////////////////////////////
        private void setupViews()
        {            
            this.TransactionTypesView = new ListCollectionView(DataSetModel.Instance.TransactionTypes);
            this.TransactionTypesView.CustomSort = new TransactionTypesComparer();

            this.CreditsView = new ListCollectionView(this.TransactionModel.LineItems);
            this.CreditsView.Filter = new Predicate<Object>(CreditsFilter);
            this.CreditsView.CurrentChanged += new EventHandler(CreditOrDebitView_CurrentChanged);

            this.DebitsView = new ListCollectionView(this.TransactionModel.LineItems);
            this.DebitsView.Filter = new Predicate<Object>(DebitsFilter);
            this.DebitsView.CurrentChanged += new EventHandler(CreditOrDebitView_CurrentChanged);
        }

        private bool alreadySettingCurrentLine = false;
        private void setCurrentLine(LineItemModel line)
        {
            if (this.alreadySettingCurrentLine)
                return;
            else
                this.alreadySettingCurrentLine = true;

            this.currentLineItem = line;

            this.tellParentWindowToDeselectLines();

            if (this.currentLineItem != null)
            {
                //currentLineItem.setParentTransactionVM(this);

                if (currentLineItem.supportsEnvelopeLines())
                {
                    this.EnvelopeLinesView = new ListCollectionView(currentLineItem.EnvelopeLines);
                    this.EnvelopeLinesView.CurrentChanged += new EventHandler(EnvelopeLinesView_CurrentChanged);
                }
                else
                {
                    this.EnvelopeLinesView = null;
                }
            }
            else
            {
                this.EnvelopeLinesView = null;
            }

            this.reportPropertyChangedWithName("EnvelopeLinesView");
            this.reportPropertyChangedWithName("EnvelopeLineSum");

            this.alreadySettingCurrentLine = false;
        }
        
        private void modifyNewLineItem(ListCollectionView view)
        {
            LineItemModel newLine = (LineItemModel)view.CurrentAddItem;

            newLine.Amount = suggestedLineItemAmountDependingOnView(view);
            newLine.Polarity = determinePolarityDependingOnView(view);

            removeGhostLine(view);
        }

        private decimal suggestedLineItemAmountDependingOnView(ListCollectionView view)
        {
            decimal suggestedAmount;

            if(view == DebitsView)
                suggestedAmount = this.TransactionModel.CreditSum - this.TransactionModel.DebitSum;
            else
                suggestedAmount = this.TransactionModel.DebitSum - this.TransactionModel.CreditSum;

            if (suggestedAmount < 0)
                suggestedAmount = 0;

            return suggestedAmount;
        }

        private decimal suggestedSubLineAmountDependingOnCurrentLine()
        {
            decimal suggestedAmount = 0;

            if (this.currentLineItem != null)
                suggestedAmount = this.currentLineItem.Amount - this.currentLineItem.EnvelopeLineSum;

            return suggestedAmount;
        }


        private PolarityCON determinePolarityDependingOnView(ListCollectionView view)
        {
            if (view == DebitsView)
                return PolarityCON.DEBIT;
            else
                return PolarityCON.CREDIT;
        }

        private void removeGhostLine(ListCollectionView view)
        {
            // When adding a new line the credit or debit filter might be applied
            // too soon and a ghost copy of the line might appear in the opposite
            // view and datagrid. So when an item is added to the view and after 
            // the polarity is set refresh the opposite view to remove the ghost 
            // line.
            if (view == DebitsView)
                this.CreditsView.Refresh();
            else
                this.DebitsView.Refresh();
        }




        private void tellParentWindowToDeselectLines()
        {
            if (this.parentWindow != null)
            {
                if (this.currentLineItem == null)
                {
                    this.parentWindow.unselectFromDestinationDataGrid();
                    this.parentWindow.unselectFromSourceDataGRid();
                }
                else if (this.currentLineItem.Polarity == PolarityCON.CREDIT)
                    this.parentWindow.unselectFromDestinationDataGrid();
                else
                    this.parentWindow.unselectFromSourceDataGRid();
            }
        }



        ///////////////////////////////////////////////////////////
        // Public functions
        ///////////////////////////////////////////////////////////
        public EditTransactionVM()
        {

        }

        public void setParentWindow(EditTransactionWindow editWindow)
        {
            this.parentWindow = editWindow;
        }

        public void loadTransaction(int transID)
        {
            this.TransactionModel = new TransactionModel(transID);
            this.setupViews();

            this.reportAllPropertiesChanged();
        }

    }
}
