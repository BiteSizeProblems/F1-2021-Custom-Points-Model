using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace _1MC_Live_Score_Application.Core.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte alpha;
            byte pos = 0;

            string hex = value.ToString().Replace("#", "");

            if (hex.Length == 8)
            {
                alpha = System.Convert.ToByte(hex.Substring(pos, 2), 16);
                pos = 2;
            }
            else
            {
                alpha = System.Convert.ToByte("ff", 16);
            }

            byte red = System.Convert.ToByte(hex.Substring(pos, 2), 16);

            pos += 2;
            byte green = System.Convert.ToByte(hex.Substring(pos, 2), 16);

            pos += 2;
            byte blue = System.Convert.ToByte(hex.Substring(pos, 2), 16);

            return new SolidColorBrush(Color.FromArgb(alpha, red, green, blue));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "#FFFFFFFF";
        }
    }
}
