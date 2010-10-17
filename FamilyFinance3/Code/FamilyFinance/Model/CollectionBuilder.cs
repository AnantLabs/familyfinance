using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

using FamilyFinance.Database;
using FamilyFinance.EditAccounts;

namespace FamilyFinance.Model
{
    class CollectionBuilder
    {
        
        /// <summary>
        /// Prevents instantiation of this class.
        /// </summary>
        private CollectionBuilder()
        {
        }

        /// <summary>
        /// Gets the credit and debit array.
        /// </summary>
        /// <returns>List of the catagories.</returns>
        public static CreditDebitModel[] getCreditDebitArray()
        {
            CreditDebitModel[] array = new CreditDebitModel[2];

            array[0] = CreditDebitModel.CREDIT;
            array[1] = CreditDebitModel.DEBIT;

            return array;
        }

        /// <summary>
        /// Returns an array of all the catagories.
        /// </summary>
        /// <returns>List of the catagories.</returns>
        public static CatagoryModel[] getCatagoryArray()
        {
            CatagoryModel[] cats = new CatagoryModel[3];

            cats[0] = CatagoryModel.INCOME;
            cats[1] = CatagoryModel.ACCOUNT;
            cats[2] = CatagoryModel.EXPENCE;

            return cats;
        }

        /// <summary>
        /// Gets all the accounts.
        /// </summary>
        public static List<IdNameCat> getAccountAll()
        {
            List<IdNameCat> accounts = new List<IdNameCat>();

            foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
                accounts.Add(new IdNameCat(row.id, row.name, CatagoryModel.getShortName(row.catagory)));

            accounts.Sort(new INCComparer());

            return accounts;
        }

        /// <summary>
        /// Gets all the account accounts.
        /// </summary>
        public static List<IdName> getAccountAccount()
        {
            List<IdName> accounts = new List<IdName>();

            foreach (FFDataSet.AccountRow row in MyData.getInstance().Account)
            {
                if(row.catagory == SpclAccountCat.ACCOUNT || row.id == SpclAccount.NULL)
                    accounts.Add(new IdName(row.id, row.name));
            }

            accounts.Sort(new INComparer());

            return accounts;
        }


        /// <summary>
        /// Gets the accounts matching the criteria.
        /// </summary>
        public static ObservableCollection<AccountBankModel> getAccountsEditable(bool incIncome, bool incAccount, bool incExpence, bool showClosed, string searchText)
        {
            ObservableCollection<AccountBankModel> accounts = new ObservableCollection<AccountBankModel>();

            List<byte> cats = new List<byte>();

            if (incIncome)
                cats.Add(1);

            if (incAccount)
                cats.Add(2);

            if (incExpence)
                cats.Add(3);

            foreach (FFDataSet.AccountRow aRow in MyData.getInstance().Account)
            {
                bool validID = aRow.id > 0;
                bool validCat = cats.Contains(aRow.catagory);
                bool inSearch = aRow.name.ToLower().Contains(searchText.ToLower());
                bool doShow = showClosed || !aRow.closed;

                if (validID && validCat && inSearch && doShow)
                    accounts.Add(new AccountBankModel(aRow));
            }

            return accounts;

        }


        /// <summary>
        /// Gets all account types.
        /// </summary>
        public static List<IdName> getAccountTypesAll()
        {
            List<IdName> types = new List<IdName>();

            foreach (FFDataSet.AccountTypeRow row in MyData.getInstance().AccountType)
                types.Add(new IdName(row.id, row.name));

            types.Sort(new INComparer());

            return types;
        }


        /// <summary>
        /// Gets all account types.
        /// </summary>
        public static ObservableCollection<AccountTypeModel> getAccountTypesEditable()
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

        /// <summary>
        /// Gets all banks.
        /// </summary>
        public static List<IdName> getBanksAll()
        {
            List<IdName> banks = new List<IdName>();


            foreach (FFDataSet.BankRow bRow in MyData.getInstance().Bank)
            {
                banks.Add(new IdName(bRow.id, bRow.name));
            }

            banks.Sort(new INComparer());

            return banks;
        }

        /// <summary>
        /// Gets all banks.
        /// </summary>
        public static ObservableCollection<BankModel> getBanksEditable()
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

        /// <summary>
        /// Gets all envelopes.
        /// </summary>
        public static List<IdName> getEnvelopesAll()
        {
            List<IdName> envelopes = new List<IdName>();

            foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
                envelopes.Add(new IdName(row.id, row.name));

            envelopes.Sort(new INComparer());

            return envelopes;
        }

        /// <summary>
        /// Gets the envelopes matching the criteria.
        /// </summary>
        public static ObservableCollection<EnvelopeGoalModel> getEnvelopesEditable(bool showClosed, string searchText)
        {
            ObservableCollection<EnvelopeGoalModel> envelopes = new ObservableCollection<EnvelopeGoalModel>();

            foreach (FFDataSet.EnvelopeRow row in MyData.getInstance().Envelope)
            {
                bool validID = row.id > 0;
                bool inSearch = row.name.ToLower().Contains(searchText.ToLower());
                bool doShow = showClosed || !row.closed;

                if (validID && inSearch && doShow)
                    envelopes.Add(new EnvelopeGoalModel(row));
            }

            return envelopes;
        }

        /// <summary>
        /// Gets all of envelope groups.
        /// </summary>
        public static List<IdName> getEnvelopeGroupsAll()
        {
            List<IdName> types = new List<IdName>();

            foreach (FFDataSet.EnvelopeGroupRow row in MyData.getInstance().EnvelopeGroup)
                types.Add(new IdName(row.id, row.name));

            types.Sort(new INComparer());

            return types;
        }

        /// <summary>
        /// Gets all of envelope groups.
        /// </summary>
        public static ObservableCollection<EnvelopeGroupModel> getEnvelopeGroupsEditable()
        {
            ObservableCollection<EnvelopeGroupModel> types = new ObservableCollection<EnvelopeGroupModel>();

            IEnumerable<int> idList =
                (from EnvelopeGroup in MyData.getInstance().EnvelopeGroup
                 where EnvelopeGroup.id > 0
                 orderby EnvelopeGroup.name
                 select EnvelopeGroup.id);

            foreach (int id in idList)
                types.Add(new EnvelopeGroupModel(MyData.getInstance().EnvelopeGroup.FindByid(id)));

            return types;
        }


        /// <summary>
        /// Gets editable Line Types.
        /// </summary>
        public static ObservableCollection<LineTypeModel> getLineTypesEditable()
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


        public static ObservableCollection<LineItemRegModel> getRegistryLinesEditable(int accountID)
        {
            ObservableCollection<LineItemRegModel> reg = new ObservableCollection<LineItemRegModel>();

            foreach (FFDataSet.LineItemRow line in MyData.getInstance().LineItem)
                if (line.accountID == accountID)
                    reg.Add(new LineItemRegModel(line));

            return reg;
        }

        public static List<IdName> getLineTypesAll()
        {
            List<IdName> types = new List<IdName>();

            foreach (FFDataSet.LineTypeRow row in MyData.getInstance().LineType)
                types.Add(new IdName(row.id, row.name));

            types.Sort(new INComparer());

            return types;
        }
    }
}
