using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using FamilyFinance.Buisness;
using FamilyFinance.Data;

namespace FamilyFinance.Presentation.EditTypes
{
    public class EditTypesVM : ViewModel
    {
        public enum Table { AccountType, EnvelopeGroup, TransactionType, Bank };

        ///////////////////////////////////////////////////////////
        // Local variables

        ///////////////////////////////////////////////////////////
        // Properties
        //public System.Collections.IEnumerable TableCollection { get; private set; }
        public string Title { get; private set; }

        public int MaxNameLength { get; private set; }
        public int MaxRoutingLength { get; private set; }

        private ICollectionView _TableCollection;
        public ICollectionView TableCollection 
        {
            get
            {
                return _TableCollection;
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
                    this._TableCollection = CollectionViewSource.GetDefaultView(new AccountTypeTM().EditableAccountTypes);
                    break;

                case Table.TransactionType:
                    this.Title = "Transaction Types";
                    this.MaxNameLength = TransactionTypeCON.NameMaxLength;
                    this.MaxRoutingLength = 0;
                    this._TableCollection = CollectionViewSource.GetDefaultView(new TransactionTypeTM().EditableAccountType);
                    break;

                case Table.EnvelopeGroup:
                    this.Title = "Envelope Groups";
                    this.MaxNameLength = EnvelopeGroupCON.NameMaxLength;
                    this.MaxRoutingLength = 0;
                    this._TableCollection = CollectionViewSource.GetDefaultView(new EnvelopeGroupTM().EditableEnvelopeGroups);
                    break;

                case Table.Bank:
                    this.Title = "Banks";
                    this.MaxNameLength = BankCON.NameMaxLength;
                    this.MaxRoutingLength = BankCON.RountingNumMaxLength;
                    this._TableCollection = CollectionViewSource.GetDefaultView(new BankTM().EditableBanks);
                    break;
            }

            // By default sort the table alphabetically by the name colum.
            this._TableCollection.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            // TODO: find a faster sort.
        }


    }
}
