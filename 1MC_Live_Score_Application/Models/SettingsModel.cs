using _1MC_Live_Score_Application.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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

        public TimeSpan[] AllFastestLapsArray = new TimeSpan[22];

        private TimeSpan _fastestOverallLapTime = TimeSpan.Zero;
        public TimeSpan FastestOverallLapTime
        {
            get { return _fastestOverallLapTime; }
            set { SetField(ref _fastestOverallLapTime, value, nameof(FastestOverallLapTime)); }
        }

        private Color _team1Color;
        public Color Team1Color
        {
            get { return _team1Color; }
            set { SetField(ref _team1Color, value, nameof(Team1Color)); }
        }

        private Color _team2Color;
        public Color Team2Color
        {
            get { return _team2Color; }
            set { SetField(ref _team2Color, value, nameof(Team2Color)); }
        }

        private Color _team3Color;
        public Color Team3Color
        {
            get { return _team3Color; }
            set { SetField(ref _team3Color, value, nameof(Team3Color)); }
        }

        private Color _team4Color;
        public Color Team4Color
        {
            get { return _team4Color; }
            set { SetField(ref _team4Color, value, nameof(Team4Color)); }
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { SetField(ref _filePath, value, nameof(FilePath)); }
        }

    }
}
