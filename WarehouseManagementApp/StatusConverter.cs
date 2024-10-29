using System;
using System.Globalization;
using System.Windows.Data;

namespace WarehouseManagementApp
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool status)
            {
                return status ? "Active" : "Inactive";
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return stringValue.Equals("Active", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}