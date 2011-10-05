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
                ViewModel.refreshViewFilter(this._EnvelopesView);
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
                ViewModel.refreshViewFilter(this._EnvelopesView);
            }
        }

        private ListCollectionView _EnvelopesView;
        public ListCollectionView EnvelopesView
        {
            get
            {
                if (this._EnvelopesView == null)
                {
                    this._EnvelopesView = new ListCollectionView(DataSetModel.Instance.Envelopes);
                    this._EnvelopesView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    this._EnvelopesView.Filter = new Predicate<Object>(EditableEnvelopesFilter); 
                }

                return _EnvelopesView;
            }
        }

        public ListCollectionView EnvelopeGroupView
        {
            get
            {
                ListCollectionView temp = new ListCollectionView(DataSetModel.Instance.EnvelopeGroups);
                temp.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

                return temp;
            }
        }

        public ListCollectionView FavoriteAccountsView
        {
            get 
            {
                ListCollectionView favoriteAccView = new ListCollectionView(DataSetModel.Instance.Accounts);

                favoriteAccView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                favoriteAccView.Filter = this.FavoriteAccountsFilter;

                return favoriteAccView;
            }
        }


        ///////////////////////////////////////////////////////////
        // Private functions
        private bool EditableEnvelopesFilter(object item)
        {
            EnvelopeDRM envRow = (EnvelopeDRM)item;
            bool keepItem = true; // Assume the item will be shown in the list

            if (EnvelopeCON.isSpecial(envRow.ID))
                keepItem = false;

            // Remove the item if we don't want to see closed envelopes or it's not in the search.
            else if (!this._ShowClosed && envRow.Closed)
                keepItem = false;

            else if (!String.IsNullOrEmpty(this._SearchText) && !envRow.Name.ToLower().Contains(this.SearchText.ToLower()))
                keepItem = false;

            return keepItem;
        }

        private bool FavoriteAccountsFilter(object item)
        {
            AccountDRM accRow = (AccountDRM)item;
            bool keepItem = false; // Assume the item will NOT be in the list

            if (accRow.Catagory == CatagoryCON.ACCOUNT)
                keepItem = true;

            return keepItem;
        }

        ///////////////////////////////////////////////////////////
        // Public functions
        public EditEnvelopesVM()
        {
            this._ShowClosed = false;
            this._SearchText = "";
        }


    }
}
