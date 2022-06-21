using _1MC_Live_Score_Application.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1MC_Live_Score_Application.Models
{
    /// <summary>
    /// Contains all user settings prior to UDP connection.
    /// </summary>
    public class SettingsModel : ObservableObject
    {
        private int _numTeams = 2;
        public int NumTeams
        {
            get { return _numTeams; }
            set { SetField(ref _numTeams, value, nameof(NumTeams)); }
        }

        private string _series;
        public string Series
        {
            get { return _series; }
            set { SetField(ref _series, value, nameof(Series)); }
        }

        private string _round;
        public string Round
        {
            get { return _round; }
            set { SetField(ref _round, value, nameof(Round)); }
        }

        public int[] AvailableTeams;

        private string _isUDPactive;
        public string IsUDPactive
        {
            get { return _isUDPactive; }
            set { SetField(ref _isUDPactive, value, nameof(IsUDPactive)); }
        }

        public int[] TeamPositionChangeTotals = new int[4];

        public bool _settingsEntered = false;

        public Dictionary<int, int> PointsModel;

    }
}
