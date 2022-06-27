using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1MC_Live_Score_Application.Core.Utils
{
    public static class PositionToPointsConverter
    {
        public static Dictionary<int, int> GetPointsModel(int NumberOfTeams)
        {
            Dictionary<int, int> selectedPointsMap;

            Dictionary<int, int> PointsModel2 = new Dictionary<int, int>()
            {
                { 1, 21 },
                { 2, 18 },
                { 3, 15 },
                { 4, 12 },
                { 5, 10 },
                { 6, 8 },
                { 7, 6 },
                { 8, 4 },
                { 9, 2 },
                { 10, 1 },
                { 11, 0 },
                { 12, 0 },
                { 13, 0 },
                { 14, 0 },
                { 15, 0 },
                { 16, 0 },
                { 17, 0 },
                { 18, 0 },
                { 19, 0 },
                { 20, 0 },
                { 21, 0 },
                { 22, 0 }
            };

            Dictionary<int, int> PointsModel3 = new Dictionary<int, int>()
            {
                { 1, 30 },
                { 2, 25 },
                { 3, 21 },
                { 4, 18 },
                { 5, 16 },
                { 6, 14 },
                { 7, 12 },
                { 8, 10 },
                { 9, 8 },
                { 10, 6 },
                { 11, 5 },
                { 12, 4 },
                { 13, 3 },
                { 14, 2 },
                { 15, 1 },
                { 16, 0 },
                { 17, 0 },
                { 18, 0 },
                { 19, 0 },
                { 20, 0 },
                { 21, 0 },
                { 22, 0 }
            };

            Dictionary<int, int> PointsModel4 = new Dictionary<int, int>()
            {
                { 1, 21 },
                { 2, 18 },
                { 3, 15 },
                { 4, 12 },
                { 5, 10 },
                { 6, 9 },
                { 7, 8 },
                { 8, 7 },
                { 9, 6 },
                { 10, 5 },
                { 11, 4 },
                { 12, 3 },
                { 13, 3 },
                { 14, 2 },
                { 15, 2 },
                { 16, 1 },
                { 17, 1 },
                { 18, 1 },
                { 19, 1 },
                { 20, 1 },
                { 21, 0 },
                { 22, 0 }
            };

            if (NumberOfTeams == 2)
            {
                return PointsModel2;
            }
            else if (NumberOfTeams == 3)
            {
                return PointsModel3;
            }
            else if (NumberOfTeams == 4)
            {
                return PointsModel4;
            }
            else
            {
                return null;
            }

        }

        public static int GetPoints(Dictionary<int, int> PointsModel, int CurrentPosition, bool IsActive)
        {
            if ( PointsModel != null && IsActive == true)
            {
                var Model = PointsModel.ToDictionary(entry => entry.Key, entry => entry.Value);
                return Model[CurrentPosition];
            }
            else
            {
                return 0;
            }
        }
    }
}
