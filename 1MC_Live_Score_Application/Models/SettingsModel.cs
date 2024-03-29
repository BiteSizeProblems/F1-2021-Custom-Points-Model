﻿using _1MC_Live_Score_Application.Core;
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

        private bool _simulationActive = false;
        public bool SimulationActive
        {
            get { return _simulationActive; }
            set { SetField(ref _simulationActive, value, nameof(SimulationActive)); }
        }

        private bool _isConnectionActive = false;
        public bool IsConnectionActive
        {
            get { return _isConnectionActive; }
            set { SetField(ref _isConnectionActive, value, nameof(IsConnectionActive)); }
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

        private Color _team1Color = Color.FromRgb(255,255,255);
        public Color Team1Color
        {
            get { return _team1Color; }
            set { SetField(ref _team1Color, value, nameof(Team1Color)); }
        }

        private Color _team2Color = Color.FromRgb(255, 255, 255);
        public Color Team2Color
        {
            get { return _team2Color; }
            set { SetField(ref _team2Color, value, nameof(Team2Color)); }
        }

        private Color _team3Color = Color.FromRgb(255, 255, 255);
        public Color Team3Color
        {
            get { return _team3Color; }
            set { SetField(ref _team3Color, value, nameof(Team3Color)); }
        }

        private Color _team4Color = Color.FromRgb(255, 255, 255);
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

        private int _teamWithFastestLap;
        public int TeamWithFastestLap
        {
            get { return _teamWithFastestLap; }
            set { SetField(ref _teamWithFastestLap, value, nameof(TeamWithFastestLap)); }
        }

        private int _mostOvertakes;
        public int MostOvertakes
        {
            get { return _mostOvertakes; }
            set { SetField(ref _mostOvertakes, value, nameof(MostOvertakes)); }
        }

        private int[] _overtakesArray;
        public int[] OvertakesArray = new int[4];

        private int _totalScore;
        public int TotalScore
        {
            get { return _totalScore; }
            set { SetField(ref _totalScore, value, nameof(TotalScore)); }
        }

        private bool _invalidTotalScore;
        public bool InvalidTotalScore
        {
            get { return _invalidTotalScore; }
            set { SetField(ref _invalidTotalScore, value, nameof(InvalidTotalScore)); }
        }
    }
}
