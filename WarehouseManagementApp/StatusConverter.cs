using System;
using System.Globalization;
using System.Windows.Data;

namespace WarehouseManagementApp
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                return status == 1 ? "Active" : "Inactive";
            }
            return "Unknown";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string statusText)
            {
                return statusText == "Active" ? 1 : 0;
            }
            return 0;
        }
    }
}