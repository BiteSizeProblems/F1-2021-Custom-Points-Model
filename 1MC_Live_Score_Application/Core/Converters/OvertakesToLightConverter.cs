﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace _1MC_Live_Score_Application.Core.Converters
{
    public class OvertakesToLightConverter : IValueConverter
    {
        private static string basePath = "/Core/Images/";

        private static string unknownPath = basePath + "greyCircle.png";

        // PRACTICE & QUALIFYING
        private static string mostOvertakesPath = basePath + "upArrowBlue.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int val = (int)value;

                if (val == 1)
                {
                    return mostOvertakesPath;
                }
                else
                {
                    return unknownPath;
                }

            }
            else
            {
                return unknownPath;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
