using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1MC_Live_Score_Application.Core.Utils
{
    public static class MostOvertakesPointCalculator
    {
        public static int GetOvertakesPoint(bool hasMostOvertakes, bool isDuplicates)
        {
            int points;

            if (isDuplicates == false)
            {
                if (hasMostOvertakes == true)
                {
                    points = 1;
                }
                else
                {
                    points = 0;
                }
            }
            else
            {
                points = 0;
            }
            

            return points;
        }

    }
}
