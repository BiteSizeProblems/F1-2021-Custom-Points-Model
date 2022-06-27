using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace _1MC_Live_Score_Application.Core.Converters
{
    public class ConnectionStatusBoolToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (bool)value;
            string message = "Listening...";

            if (status == true)
            {
                message = "Active";
            }
            else if (status == false)
            {
                message = "Inactive";
            }

            return message;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "Unknown";
        }
    }
}
