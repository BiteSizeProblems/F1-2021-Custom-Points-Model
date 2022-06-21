﻿using _1MC_Live_Score_Application.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _1MC_Live_Score_Application.Structs.F121.Appendeces;

namespace _1MC_Live_Score_Application.Models
{
    public class DriverDataModel : ObservableObject
    {
        // Args CTOR
        public DriverDataModel(int i)
        {
            this.ID = i;
        }

        // No-Args CTOR
        public DriverDataModel( )
        {

        }

        private int _iD;
        public int ID
        {
            get { return _iD; }
            set { SetField(ref _iD, value, nameof(ID)); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value, nameof(Name)); }
        }

        private int _team;
        public int Team
        {
            get { return _team; }
            set { SetField(ref _team, value, nameof(Team)); }
        }

        private int _teamIndex;
        public int TeamIndex
        {
            get { return _teamIndex; }
            set { SetField(ref _teamIndex, value, nameof(TeamIndex)); }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetField(ref _isActive, value, nameof(IsActive)); }
        }

        private int _currentPosition;
        public int CurrentPosition
        {
            get { return _currentPosition; }
            set { SetField(ref _currentPosition, value, nameof(CurrentPosition)); }
        }

        private int _gridPosition;
        public int GridPosition
        {
            get { return _gridPosition; }
            set { SetField(ref _gridPosition, value, nameof(GridPosition)); }
        }

        private int _positionChanges;
        public int PositionChanges
        {
            get { return _positionChanges; }
            set { SetField(ref _positionChanges, value, nameof(PositionChanges)); }
        }

        private int _pointsByPosition;
        public int PointsByPosition
        {
            get { return _pointsByPosition; }
            set { SetField(ref _pointsByPosition, value, nameof(PointsByPosition)); }
        }

        private ResultStatus _resultStatus;
        public ResultStatus ResultStatus
        {
            get { return _resultStatus; }
            set { SetField(ref _resultStatus, value, nameof(ResultStatus)); }
        }

        private bool _hasFastestLap;
        public bool HasFastestLap
        {
            get { return _hasFastestLap; }
            set { SetField(ref _hasFastestLap, value, nameof(HasFastestLap)); }
        }

        private bool _hasNoPenalties;
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