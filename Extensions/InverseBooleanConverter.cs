using System;
using System.Globalization;
using System.Windows.Data;

namespace BankApp.Extensions
{
    public class CanEditMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isManager = (bool)values[0];
            string columnName = (string)values[1];

            if (isManager)
            {
                return false;
            }
            else if (columnName == "MobileNumber")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}


