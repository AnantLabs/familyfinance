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
                    this._TableCollectionView = (ListCollectionView) CollectionViewSource.GetDefaultView(new AccountTypeTM().EditableAccountTypes);
                    break;

                case Table.TransactionType:
                    this.Title = "Transaction Types";
                    this.MaxNameLength = TransactionTypeCON.NameMaxLength;
                    this.MaxRoutingLength = 0;
                    this._TableCollectionView = (ListCollectionView) CollectionViewSource.GetDefaultView(new TransactionTypeTM().EditableTransactionType);
                    break;

                case Table.EnvelopeGroup:
                    this.Title = "Envelope Groups";
                    this.MaxNameLength = EnvelopeGroupCON.NameMaxLength;
                    this.MaxRoutingLength = 0;
                    this._TableCollectionView = (ListCollectionView) CollectionViewSource.GetDefaultView(new EnvelopeGroupTM().EditableEnvelopeGroups);
                    break;

                case Table.Bank:
                    this.Title = "Banks";
                    this.MaxNameLength = BankCON.NameMaxLength;
                    this.MaxRoutingLength = BankCON.RountingNumMaxLength;
                    this._TableCollectionView = (ListCollectionView) CollectionViewSource.GetDefaultView(new BankTM().EditableBanks);
                    break;
            }

            // By default sort the table alphabetically by the name colum.
            // This slower sort is fime with these small lists.
            this._TableCollectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            
        }


    }
}
