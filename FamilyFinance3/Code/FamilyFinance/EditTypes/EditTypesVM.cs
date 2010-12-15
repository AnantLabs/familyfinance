using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

using FamilyFinance.Model;
using FamilyFinance.Database;

namespace FamilyFinance.EditTypes
{
    class EditTypesVM : ModelBase
    {
        public ObservableCollection<AccountTypeModel> AccountTypes
        {
            get
            {
                ObservableCollection<AccountTypeModel> types = new ObservableCollection<AccountTypeModel>();

                IEnumerable<int> idList =
                    (from AccountType in MyData.getInstance().AccountType
                     where AccountType.id > 0
                     orderby AccountType.name
                     select AccountType.id);

                foreach (int id in idList)
                    types.Add(new AccountTypeModel(MyData.getInstance().AccountType.FindByid(id)));

                return types;
            }
        }

        public ObservableCollection<LineTypeModel> LineTypes
        {
            get
            {
                ObservableCollection<LineTypeModel> types = new ObservableCollection<LineTypeModel>();

                IEnumerable<int> idList =
                    (from LineType in MyData.getInstance().LineType
                     where LineType.id > 0
                     orderby LineType.name
                     select LineType.id);

                foreach (int id in idList)
                    types.Add(new LineTypeModel(MyData.getInstance().LineType.FindByid(id)));

                return types;
            }
        }

        public ObservableCollection<EnvelopeGroupModel> EnvelopeGroups
        {
            get
            {
                ObservableCollection<EnvelopeGroupModel> groups = new ObservableCollection<EnvelopeGroupModel>();

                IEnumerable<int> idList =
                    (from EnvelopeGroup in MyData.getInstance().EnvelopeGroup
                     where EnvelopeGroup.id > 0
                     orderby EnvelopeGroup.name
                     select EnvelopeGroup.id);

                foreach (int id in idList)
                    groups.Add(new EnvelopeGroupModel(MyData.getInstance().EnvelopeGroup.FindByid(id)));


                return groups;
            }
        }

        public ObservableCollection<BankModel> Banks
        {
            get
            {
                ObservableCollection<BankModel> banks = new ObservableCollection<BankModel>();

                IEnumerable<int> idList =
                    (from Bank in MyData.getInstance().Bank
                     where Bank.id > 0
                     orderby Bank.name
                     select Bank.id);

                foreach (int id in idList)
                    banks.Add(new BankModel(MyData.getInstance().Bank.FindByid(id)));

                return banks;
            }
        }

    }

}
