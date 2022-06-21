using _1MC_Live_Score_Application.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1MC_Live_Score_Application.Models
{
    public class TeamDataModel : ObservableObject
    {
        // Args CTOR
        public TeamDataModel(int t)
        {
            this.Team = t;
        }

        // No-Args CTOR
        public TeamDataModel()
        {

        }

        private int _team;
        public int Team
        {
            get { return _team; }
            set { SetField(ref _team, value, nameof(Team)); }
        }

        private int _id;
        public int ID
        {
            get { return _id; }
            set { SetField(ref _id, value, nameof(ID)); }
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

        private int _estimatedTeamPoints = 0;
        public int EstimatedTeamPoints
        {
            get { return _estimatedTeamPoints; }
            set { SetField(ref _estimatedTeamPoints, value, nameof(EstimatedTeamPoints)); }
        }

        private int _positionChanges;
        public int PositionChanges
        {
            get { return _positionChanges; }
            set { SetField(ref _positionChanges, value, nameof(PositionChanges)); }
        }

        private bool _hasMostPositionChanges;
        public bool HasMostPositionChanges
        {
            get { return _hasMostPositionChanges; }
            set { SetField(ref _hasMostPositionChanges, value, nameof(HasMostPositionChanges)); }
        }

        private int _mostOvertakesPoint;
        public int MostOvertakesPoint
        {
            get { return _mostOvertakesPoint; }
            set { SetField(ref _mostOvertakesPoint, value, nameof(MostOvertakesPoint)); }
        }

        private bool _hasFastestLap;
        public bool HasFastestLap
        {
            get { return _hasFastestLap; }
            set { SetField(ref _hasFastestLap, value, nameof(HasFastestLap)); }
        }

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

        private int _noPenaltiesPoint;
        public int NoPenaltiesPoint
        {
            get { return _noPenaltiesPoint; }
            set { SetField(ref _noPenaltiesPoint, value, nameof(NoPenaltiesPoint)); }
        }

    }
}
