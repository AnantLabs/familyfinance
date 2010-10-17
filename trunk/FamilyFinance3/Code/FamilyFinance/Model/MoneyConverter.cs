using System;
using System.Windows.Data;
using System.Globalization;



namespace FamilyFinance.Model
{
    [ValueConversion(typeof(Decimal), typeof(String))]
    class MoneyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal amount = (Decimal)value;
            return amount.ToString("$#0.00;($#0.00); ");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            decimal amount;

            if (Decimal.TryParse(strValue, out amount))
                return amount;

            return 0.0m;
        }
    }
}
