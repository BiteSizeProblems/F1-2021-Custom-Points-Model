using _1MC_Live_Score_Application.Core;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace _1MC_Live_Score_Application.Models
{
    public class TeamDataModel : ObservableObject
    {
        // Args CTOR
        public TeamDataModel(int t)
        {
            this.ID = t;
        }

        // No-Args CTOR
        public TeamDataModel()
        {

        }

        private ObservableCollection<DriverDataModel> _driver;
        public ObservableCollection<DriverDataModel> Driver
        {
            get { return _driver; }
            set { SetField(ref _driver, value, nameof(ID)); }
        }

        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetField(ref _id, value, nameof(ID)); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value, nameof(Name)); }
        }

        private Color _teamColor;
        public Color TeamColor
        {
            get { return _teamColor; }
            set { SetField(ref _teamColor, value, nameof(TeamColor)); }
        }

        private int _numDrivers;
        public int NumDrivers
        {
            get { return _numDrivers; }
            set { SetField(ref _numDrivers, value, nameof(NumDrivers)); }
        }

        private int _numActiveDrivers;
        public int NumActiveDrivers
        {
            get { return _numActiveDrivers; }
            set { SetField(ref _numActiveDrivers, value, nameof(NumActiveDrivers)); }
        }

        private string _driversNumActive;
        public string DriversNumActive
        {
            get { return _driversNumActive; }
            set { SetField(ref _driversNumActive, value, nameof(DriversNumActive)); }
        }

        private int _totalPoints;
        public int TotalPoints
        {
            get { return _totalPoints; }
            set { SetField(ref _totalPoints, value, nameof(TotalPoints)); }
        }

        private int _pointsByPosition;
        public int PointsByPosition
        {
            get { return _pointsByPosition; }
            set { SetField(ref _pointsByPosition, value, nameof(PointsByPosition)); }
        }

        private int _positionChanges;
        public int PositionChanges
        {
            get { return _positionChanges; }
            set { SetField(ref _positionChanges, value, nameof(PositionChanges)); }
        }

        private int _numOvertakes;
        public int NumOvertakes
        {
            get { return _numOvertakes; }
            set { SetField(ref _numOvertakes, value, nameof(NumOvertakes)); }
        }

        private bool _hasMostOvertakes;
        public bool HasMostOvertakes
        {
            get { return _hasMostOvertakes; }
            set { SetField(ref _hasMostOvertakes, value, nameof(HasMostOvertakes)); }
        }

        private bool _hasMostUniqueOvertakes;
        public bool HasMostUniqueOvertakes
        {
            get { return _hasMostUniqueOvertakes; }
            set { SetField(ref _hasMostUniqueOvertakes, value, nameof(HasMostUniqueOvertakes)); }
        }

        private int _mostOvertakesPoint;
        public int MostOvertakesPoint
        {
            get { return _mostOvertakesPoint; }
            set { SetField(ref _mostOvertakesPoint, value, nameof(MostOvertakesPoint)); }
        }

        private bool _hasFastestLap = false;
        public bool HasFastestLap
        {
            get { return _hasFastestLap; }
            set { SetField(ref _hasFastestLap, value, nameof(HasFastestLap)); }
        }

        private bool[] _fastestLapsArray;
        public bool[] FastestLapsArray;

        private int _fastestLapPoint;
        public int FastestLapPoint
        {
            get { return _fastestLapPoint; }
            set { SetField(ref _fastestLapPoint, value, nameof(FastestLapPoint)); }
        }

        private bool _hasNoPenalties = true;
        public bool HasNoPenalties
        {
            get { return _hasNoPenalties; }
            set { SetField(ref _hasNoPenalties, value, nameof(HasNoPenalties)); }
        }

        private bool _hasPenalty = false;
        public bool HasPenalty
        {
            get { return _hasPenalty; }
            set { SetField(ref _hasPenalty, value, nameof(HasPenalty)); }
        }

        private int _noPenaltiesPoint;
        public int NoPenaltiesPoint
        {
            get { return _noPenaltiesPoint; }
            set { SetField(ref _noPenaltiesPoint, value, nameof(NoPenaltiesPoint)); }
        }

        private bool _hasHighestScore;
        public bool HasHighestScore
        {
            get { return _hasHighestScore; }
            set { SetField(ref _hasHighestScore, value, nameof(HasHighestScore)); }
        }

        private bool _invalidScore;
        public bool InvalidScore
        {
            get { return _invalidScore; }
            set { SetField(ref _invalidScore, value, nameof(InvalidScore)); }
        }

    }
}
