using System.Windows.Controls;
using System.Windows;

namespace FamilyFinance.EditAccounts
{
    class AccountDetailsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BankTemplate { get; set; }
        public DataTemplate EmptyTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            AccountBankModel acc = item as AccountBankModel;

            if (acc == null || !acc.HasBankInfo)
                return EmptyTemplate;

            else
                return BankTemplate;
        }
    }
}
