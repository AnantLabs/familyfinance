using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;

using FamilyFinance.Model;

namespace FamilyFinance.EditTypes
{
    class EditTypesVM : ModelBase
    {
        public ObservableCollection<AccountTypeModel> AccountTypes { get; set; }

        public void fillAccountTypes()
        {
            this.AccountTypes = CollectionBuilder.getAccountTypesEditable();
            this.RaisePropertyChanged("AccountTypes");
        }

        public ObservableCollection<LineTypeModel> LineTypes { get; set; }

        public void fillLineTypes()
        {
            this.LineTypes = CollectionBuilder.getLineTypesEditable();
            this.RaisePropertyChanged("LineTypes");
        }

        public ObservableCollection<EnvelopeGroupModel> EnvelopeGroups { get; set; }

        public void fillEnvelopeGroups()
        {
            this.EnvelopeGroups = CollectionBuilder.getEnvelopeGroupsEditable();
            this.RaisePropertyChanged("EnvelopeGroups");
        }

        public ObservableCollection<BankModel> Banks { get; set; }

        public void fillBanks()
        {
            this.Banks = CollectionBuilder.getBanksEditable();
            this.RaisePropertyChanged("Banks");
        }

    }

}
