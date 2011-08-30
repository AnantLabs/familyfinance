using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using FamilyFinance.Buisness;
using FamilyFinance.Data;
using System.Collections;

namespace FamilyFinance.Presentation.EditTypes
{
    public class EditTypesVM : ViewModel
    {
        public enum Table { AccountType, EnvelopeGroup, TransactionType, Bank };

        ///////////////////////////////////////////////////////////
        // Local variables

        ///////////////////////////////////////////////////////////
        // Properties
        public string Title { get; private set; }

        public int MaxNameLength { get; private set; }
        public int MaxRoutingLength { get; private set; }
        

        // Using ICollectionView so we can set the sorting here.
        private ListCollectionView _TableCollectionView;
        public ListCollectionView TableCollectionView 
        {
            get
            {
                return _TableCollectionView;
            }
        }



        ///////////////////////////////////////////////////////////
        // Private functions
        private bool AccountTypeFilter(object item)
        {
            AccountTypeDRM envRow = (AccountTypeDRM)item;
            bool keepItem = true; // Assume the item will be shown in the list

            if (envRow.ID == AccountTypeCON.NULL.ID)
                keepItem = false;

            return keepItem;
        }

        private bool TransactionTypeFilter(object item)
        {
            TransactionTypeDRM envRow = (TransactionTypeDRM)item;
            bool keepItem = true; // Assume the item will be shown in the list

            if (envRow.ID == TransactionTypeCON.NULL.ID)
                keepItem = false;

            return keepItem;
        }

        private bool EnvelopeGroupFilter(object item)
        {
            EnvelopeGroupDRM envRow = (EnvelopeGroupDRM)item;
            bool keepItem = true; // Assume the item will be shown in the list

            if (envRow.ID == EnvelopeGroupCON.NULL.ID)
                keepItem = false;

            return keepItem;
        }

        private bool BanksFilter(object item)
        {
            BankDRM envRow = (BankDRM)item;
            bool keepItem = true; // Assume the item will be shown in the list

            if (envRow.ID == BankCON.NULL.ID)
                keepItem = false;

            return keepItem;
        }

        
        ///////////////////////////////////////////////////////////
        // Public functions
        public EditTypesVM(Table type)
        {

            // set the known values.
            switch (type)
            {
                case Table.AccountType:
                    this.Title = "Account Types";
                    this.MaxNameLength = AccountTypeCON.NameMaxLength;
                    this.MaxRoutingLength = 0;
                    this._TableCollectionView = new ListCollectionView(DataSetModel.getInstance().AccountTypes);
                    this._TableCollectionView.Filter = this.AccountTypeFilter;
                    break;

                case Table.TransactionType:
                    this.Title = "Transaction Types";
                    this.MaxNameLength = TransactionTypeCON.NameMaxLength;
                    this.MaxRoutingLength = 0;
                    this._TableCollectionView = new ListCollectionView(DataSetModel.getInstance().TransactionTypes);
                    this._TableCollectionView.Filter = this.TransactionTypeFilter;
                    break;

                case Table.EnvelopeGroup:
                    this.Title = "Envelope Groups";
                    this.MaxNameLength = EnvelopeGroupCON.NameMaxLength;
                    this.MaxRoutingLength = 0;
                    this._TableCollectionView = new ListCollectionView(DataSetModel.getInstance().EnvelopeGroups);
                    this._TableCollectionView.Filter = this.EnvelopeGroupFilter;
                    break;

                case Table.Bank:
                    this.Title = "Banks";
                    this.MaxNameLength = BankCON.NameMaxLength;
                    this.MaxRoutingLength = BankCON.RountingNumMaxLength;
                    this._TableCollectionView = new ListCollectionView(DataSetModel.getInstance().Banks);
                    this._TableCollectionView.Filter = this.BanksFilter;
                    break;
            }

            // By default sort the table alphabetically by the name colum.
            // This slower sort is fime with these small lists.
            this._TableCollectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            
        }


    }
}
