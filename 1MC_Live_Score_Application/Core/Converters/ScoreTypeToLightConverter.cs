﻿using _1MC_Live_Score_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace _1MC_Live_Score_Application.Core.Converters
{
    public class ScoreTypeToLightConverter : IValueConverter
    {
        private static string basePath = "/Core/Images/";

        private static string unknownPath = basePath + "crown.png";

        // PRACTICE & QUALIFYING
        private static string mostPointsPath = basePath + "crown.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                bool val = (bool)value;

                if (val == true)
                {
                    return mostPointsPath;
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
