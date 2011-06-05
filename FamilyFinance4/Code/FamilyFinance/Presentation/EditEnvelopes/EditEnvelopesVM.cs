using System;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Generic;

using FamilyFinance.Buisness;
using FamilyFinance.Data;

namespace FamilyFinance.Presentation.EditEnvelopes
{
    class EditEnvelopesVM : ViewModel
    {
                
        ///////////////////////////////////////////////////////////
        // Properties

        private bool _ShowClosed;
        public bool ShowClosed
        {
            get
            {
                return _ShowClosed;
            }
            set
            {
                this._ShowClosed = value;
                this.refreshViewFilter(this._EnvelopesView);
            }
        }

        private string _SearchText;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                this._SearchText = value;
                this.refreshViewFilter(this._EnvelopesView);
            }
        }

        private ListCollectionView _EnvelopesView;
        public ListCollectionView EnvelopesView
        {
            get
            {
                return _EnvelopesView;
            }
        }

        public ListCollectionView EnvelopeGroupView
        {
            get
            {
                ListCollectionView temp;
                    
                temp = (ListCollectionView) CollectionViewSource.GetDefaultView(new EnvelopeGroupTM().AllEnvelopeGroups);
                temp.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

                return temp;
            }
        }

        public ListCollectionView FavoriteAccountsView
        {
            get 
            {
                ListCollectionView temp;

                temp = (ListCollectionView)CollectionViewSource.GetDefaultView(new AccountTM().FavoriteAccounts);
                temp.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

                return temp;
            }
        }


        ///////////////////////////////////////////////////////////
        // Private functions
        private bool Filter(object item)
        {
            EnvelopeDRM envRow = (EnvelopeDRM)item;
            bool keepItem = true; // Assume the item will be shown in the list

            // Remove the item if we don't want to see closed envelopes or it's not in the search.
            if (!this._ShowClosed && envRow.Closed)
                keepItem = false;

            else if (!String.IsNullOrEmpty(this._SearchText) && !envRow.Name.ToLower().Contains(this.SearchText.ToLower()))
                keepItem = false;

            return keepItem;
        }


        ///////////////////////////////////////////////////////////
        // Public functions
        public EditEnvelopesVM()
        {
            this._ShowClosed = false;
            this._SearchText = "";

            this._EnvelopesView = (ListCollectionView)CollectionViewSource.GetDefaultView(new EnvelopeTM().EditableEnvelopes);
            this._EnvelopesView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            this._EnvelopesView.Filter = new Predicate<Object>(Filter); 
        }


    }
}
