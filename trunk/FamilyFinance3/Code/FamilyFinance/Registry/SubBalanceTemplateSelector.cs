using System.Windows.Controls;
using System.Windows;

namespace FamilyFinance.Registry
{
    class SubBalanceTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SubBalanceTemplate { get; set; }
        public DataTemplate EmptyTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            BalanceModel bModel = item as BalanceModel;

            if (bModel == null || bModel.SubBalances == null || bModel.SubBalances.Count == 0)
                return EmptyTemplate;

            else
                return SubBalanceTemplate;
        }
    }
}
