using _1MC_Live_Score_Application.Core.Utils;
using _1MC_Live_Score_Application.Models;
using _1MC_Live_Score_Application.Structs.F121;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Data;

namespace _1MC_Live_Score_Application.ViewModels
{
    public class DataViewModel : BaseViewModel
    {
        // Module Set-Up (Singleton Instance with thread safety)
        private static DataViewModel? _instance = null;
        private static readonly object _singletonLock = new();
        public static DataViewModel GetInstance()
        {
            lock (_singletonLock)
            {
                if (_instance == null) { _instance = new DataViewModel(); }
                return _instance;
            }
        }

        public DriverDataModel? _driver { get; set; }

        private TeamDataModel _team1;
        public TeamDataModel Team1
        {
            get { return _team1; }
            set { SetField(ref _team1, value, nameof(Team1)); }
        }

        private TeamDataModel _team2;
        public TeamDataModel Team2
        {
            get { return _team2; }
            set { SetField(ref _team2, value, nameof(Team2)); }
        }

        private TeamDataModel _team3;
        public TeamDataModel Team3
        {
            get { return _team3; }
            set { SetField(ref _team3, value, nameof(Team3)); }
        }

        private TeamDataModel _team4;
        public TeamDataModel Team4
        {
            get { return _team4; }
            set { SetField(ref _team4, value, nameof(Team4)); }
        }

        public SettingsModel SettingsModel { get; set; }
        public ObservableCollection<DriverDataModel> Driver { get; set; }
        public ObservableCollection<TeamDataModel> Team { get; set; }

        private readonly object _driverLock = new object();
        private readonly object _teamLock = new object();

        private Timer fastTimer;
        private Timer slowTimer;
        private Timer mediumTimer;

        public PacketSessionData latestSessionDataPacket { get; set; }
        public PacketFinalClassificationData latestFinalClassificationDataPacket { get; set; }
        public PacketLapData latestLapDataPacket { get; set; }
        public PacketLobbyInfoData latestLobbyInfoDataPacket { get; set; }
        public PacketParticipantsData latestParticipantDataPacket { get; set; }
        public PacketSessionHistoryData latestSessionHistoryDataPacket { get; set; }

        private DataViewModel(): base()
        {
            Driver = new ObservableCollection<DriverDataModel>();
            Team = new ObservableCollection<TeamDataModel>();
            SettingsModel = new SettingsModel();

            SettingsModel.IsUDPactive = "Connection: Waiting";

            BindingOperations.EnableCollectionSynchronization(Driver, _driverLock);
            BindingOperations.EnableCollectionSynchronization(Driver, _teamLock);

            for ( int i = 0; i < 22; i++)
            {
                Driver.Add(new DriverDataModel());

                Driver[i].ID = i + 1;
                Driver[i].Name = "...";
                Driver[i].Team = 1;
            }

            //UDPC.OnSessionDataReceive += StoreSessionData;
            UDPC.OnFinalClassificationDataReceive += StoreFinalClassificationData;
            UDPC.OnLapDataReceive += StoreLapData;
            //UDPC.OnLobbyInfoDataReceive += StoreLobbyInfoData;
            //UDPC.OnParticipantsDataReceive += StoreParticipantData;
            UDPC.OnSessionHistoryDataReceive += StoreSessionHistoryData;

            fastTimer = new Timer(GetDriverData, null, 0, 1000);
            mediumTimer = new Timer(CallDriversPerTeam, null, 0, 5000);
            slowTimer = new Timer(CalculateTeamPoints, null, 0, 3000);

        }

        private void CallDriversPerTeam(object? state)
        {
            GetDriversPerTeam();
        }

        public void CreateTeams()
        {
            for (int i = 0; i < 4; i++)
            {
                Team.Add(new TeamDataModel());
                Team[i].ID = i + 1;
                Team[i].Name = $"Team {i+1}";
            }

            Team1 = Team[0];
            Team2 = Team[1];
            Team3 = Team[2];
            Team4 = Team[3];

            CreateTeamColors();

        }

        public void CreateTeamColors()
        {
            Team1.TeamColor = SettingsModel.Team1Color;
            Team2.TeamColor = SettingsModel.Team2Color;
            Team3.TeamColor = SettingsModel.Team3Color;
            Team4.TeamColor = SettingsModel.Team4Color;
        }

        public void GetDriversPerTeam()
        {
            for (int i = 0; i < Team.Count; i++)
            {
                var thisTeam = i + 1;
                int numDrivers = 0;
                int numActiveDrivers = 0;

                for (int j = 0; j < 22; j++)
                {
                    var driversTeam = Driver[j].Team;

                    if (thisTeam == driversTeam)
                    {
                        numDrivers += 1;

                        if (Driver[i].IsActive == true)
                        {
                            numActiveDrivers += 1;
                        }
                    }
                }

                Team[i].NumDrivers = numDrivers;
                Team[i].NumActiveDrivers = numActiveDrivers;

            }
        }

        private void StoreSessionHistoryData(PacketSessionHistoryData packet)
        {
            latestSessionHistoryDataPacket = packet;
        }
        private void StoreParticipantData(PacketParticipantsData packet)
        {
            latestParticipantDataPacket = packet;
        }
        private void StoreLobbyInfoData(PacketLobbyInfoData packet)
        {
            latestLobbyInfoDataPacket = packet;
        }
        private void StoreLapData(PacketLapData packet)
        {
            latestLapDataPacket = packet;

            SettingsModel.IsUDPactive = "Connection: Active";
        }
        private void StoreFinalClassificationData(PacketFinalClassificationData packet)
        {
            latestFinalClassificationDataPacket = packet;

            SettingsModel.IsUDPactive = "Connection: Inactive";
        }
        private void StoreSessionData(PacketSessionData packet)
        {
            latestSessionDataPacket = packet;
        }

        private void GetDriverData(object? state = null)
        {
            PacketLapData lapDataPacket = latestLapDataPacket;
            PacketSessionHistoryData sessionHistoryDataPacket = latestSessionHistoryDataPacket;
            PacketFinalClassificationData finalClassificationDataPacket = latestFinalClassificationDataPacket;

            // LAP DATA
            if (latestLapDataPacket.lapData != null)
            {
                for (int i = 0; i < latestLapDataPacket.lapData.Length; i++)
                {
                    var lapData = lapDataPacket.lapData[i];

                    if (lapData.carPosition == 0)
                    {
                        Driver[i].Team = 0;
                    }

                    if (lapData.resultStatus == Appendeces.ResultStatus.Active || lapData.resultStatus == Appendeces.ResultStatus.Finished)
                    {
                        Driver[i].IsActive = true;
                    }
                    else
                    {
                        Driver[i].IsActive = false;
                    }

                    if (lapData.penalties != 0)
                    {
                        Driver[i].HasNoPenalties = false;
                    }
                    else
                    {
                        Driver[i].HasNoPenalties = true;
                    }

                    Driver[i].ResultStatus = lapData.resultStatus;

                    Driver[i].CurrentPosition = lapData.carPosition;
                    Driver[i].GridPosition = lapData.gridPosition;
                    Driver[i].PositionChanges = lapData.carPosition - lapData.gridPosition;

                    if (Driver[i].IsActive == true)
                    {
                        Driver[i].PointsByPosition = PositionToPointsConverter.GetPoints(SettingsModel.PointsModel, Driver[i].CurrentPosition);
                    }
                }
            }

            // SESSION HISTORY DATA
            if ( latestSessionHistoryDataPacket.m_lapHistoryData != null)
            {
                var carId = latestSessionHistoryDataPacket.m_carIdx;
                var driverData = Driver[carId];

                driverData.FastestLapNum = latestSessionHistoryDataPacket.m_bestLapTimeLapNum;
                driverData.NumLaps = latestSessionHistoryDataPacket.m_numLaps;

                for (int i = 0; i < latestSessionHistoryDataPacket.m_lapHistoryData.Length; i++)
                {
                    var sessionHistoryData = latestSessionHistoryDataPacket.m_lapHistoryData[i];

                    var lapRef = i + 1;

                    if ( lapRef == sessionHistoryDataPacket.m_bestLapTimeLapNum)
                    {
                        Driver[carId].FastestLapTime = TimeSpan.FromMilliseconds(sessionHistoryData.m_lapTimeInMS);
                    }
                }

                for (int i = 0; i < 22; i++)
                {
                    SettingsModel.AllFastestLapsArray[i] = Driver[i].FastestLapTime;

                    if (Driver[i].FastestLapTime == SettingsModel.FastestOverallLapTime)
                    {
                        Driver[i].HasFastestLap = true;
                    }
                    else
                    {
                        Driver[i].HasFastestLap = false;
                    }
                }

                SettingsModel.FastestOverallLapTime = SettingsModel.AllFastestLapsArray.Where(x => x != TimeSpan.Zero).DefaultIfEmpty().Min();
            }

            // FINAL CLASSIFICATION DATA
            if (finalClassificationDataPacket.m_classificationData != null)
            {
                for (int i = 0; i < latestFinalClassificationDataPacket.m_classificationData.Length; i++)
                {
                    var finalData = latestFinalClassificationDataPacket.m_classificationData[i];

                    Driver[i].CurrentPosition = (byte)finalData.m_position;
                    Driver[i].NumLaps = (int)finalData.m_numLaps;
                    Driver[i].FastestLapTime = TimeSpan.FromMilliseconds(finalData.m_bestLapTimeInMS);

                    if (finalData.m_penaltiesTime == 0)
                    {
                        Driver[i].HasNoPenalties = true;
                    }
                    else
                    {
                        Driver[i].HasNoPenalties = false;
                    }

                    if (finalData.m_resultStatus == Appendeces.ResultStatus.Active || finalData.m_resultStatus == Appendeces.ResultStatus.Finished)
                    {
                        Driver[i].IsActive = true;

                        Driver[i].PointsByPosition = PositionToPointsConverter.GetPoints(SettingsModel.PointsModel, Driver[i].CurrentPosition);
                    }
                    else
                    {
                        Driver[i].IsActive = false;

                        Driver[i].PointsByPosition = 0;
                    }
                }
            }
        }

        private void CalculateTeamPoints(object state = null)
        {
            if (SettingsModel.PointsModel != null && Team.Count > 1)
            {
                CalculateTeamPointsByPosition();

                CalculateTeamPointsByPositionChange();

                CalculateTeamPointsByFastestLap();

                CalculateTeamPointsByPenalties();

                CalculateTotalTeamPoints();
            }
        }

        private void CalculateTeamPointsByPosition()
        {
            for ( int i = 0; i < Team.Count; i++)
            {
                var thisTeam = i + 1;
                int pbp = 0;

                for (int j = 0; j < 22; j++)
                {
                    var driversTeam = Driver[j].Team;
                    var driversPointsByPosition = Driver[j].PointsByPosition;

                    if (thisTeam == driversTeam && Driver[i].IsActive == true)
                    {
                        pbp += driversPointsByPosition;
                    }
                }

                Team[i].PointsByPosition = pbp;

            }
        }

        private void CalculateTotalTeamPoints()
        {
            for (int i = 0; i < SettingsModel.NumTeams; i++)
            {
                Team[i].TotalPoints = Team[i].PointsByPosition + Team[i].FastestLapPoint + Team[i].NoPenaltiesPoint + Team[i].MostOvertakesPoint;
            }
        }

        private void CalculateTeamPointsByPenalties()
        {
            for (int i = 0; i < Team.Count; i++)
            {
                var thisTeam = i + 1;

                for (int j = 0; j < 22; j++)
                {
                    if (Driver[j].Team == thisTeam)
                    {
                        if (Driver[j].HasNoPenalties == false)
                        {
                            Team[i].HasNoPenalties = false;
                        }
                    }
                }

                if (Team[i].HasNoPenalties == true)
                {
                    Team[i].NoPenaltiesPoint = Team[i].NumActiveDrivers;
                }
                else
                {
                    Team[i].NoPenaltiesPoint = 0;
                }
            }
        }

        private void CalculateTeamPointsByFastestLap()
        {
            for ( int i = 0; i < Team.Count; i++)
            {
                var thisTeam = i + 1;

                for ( int j = 0; j < 22; j++)
                {
                    if (Driver[j].Team == thisTeam)
                    {
                        if (Driver[j].HasFastestLap == true)
                        {
                            for ( int k = 0; k < Team.Count; k++)
                            {
                                Team[k].HasFastestLap = false;
                            }

                            Team[i].HasFastestLap = true;
                        }
                        else if (Driver[j].HasFastestLap == false && Driver[j].Team != thisTeam)
                        {
                            Team[i].HasFastestLap = false;
                        }
                    }
                }

                if (Team[i].HasFastestLap == true)
                {
                    Team[i].FastestLapPoint = Team[i].NumActiveDrivers;
                }
                else
                {
                    Team[i].FastestLapPoint = 0;
                }
            }
        }

        private void CalculateTeamPointsByPositionChange()
        {
            var overtakesArray = SettingsModel.TeamPositionChangeTotals;

            for ( int i = 0; i < Team.Count; i++ )
            {
                var thisTeam = i + 1;
                var overtakes = 0;

                for ( int j = 0; j < 22; j++)
                {
                    if (Driver[j].Team == thisTeam)
                    {
                        overtakes += Driver[j].PositionChanges;
                    }
                }

                overtakesArray[i] = overtakes;

                Team[i].PositionChanges = overtakes;

            }

            var mostPositionChanges = overtakesArray.Min();

            bool overtakesDuplicates = false;

            Array.Sort(overtakesArray);
            foreach (int i in overtakesArray)
            {
                if (overtakesArray[0] == overtakesArray[1] || overtakesArray[0] == overtakesArray[2])
                {
                    overtakesDuplicates = true;
                }
                else
                {
                    overtakesDuplicates = false;
                }
            }

            if (overtakesDuplicates == false) // if there are no duplicates
            {
                for (int i = 0; i < Team.Count; i++)
                {
                    if (Team[i].PositionChanges == mostPositionChanges) // if team has most overtakes
                    {
                        Team[i].MostOvertakesPoint = 1;
                    }
                    else // if team does not
                    {
                        Team[i].MostOvertakesPoint = 0;
                    }
                }
            }
            else // if there are duplicates
            {
                for (int i = 0; i < Team.Count; i++)
                {
                    Team[i].MostOvertakesPoint = 0;
                }
            }
        }

        public void SimulateDrivers()
        {
            string[] driverNames = { "Jack", "Ben", "Henry", "Callum", "Alex", "Boris", "Devin", "David", "Lewis", "Fernando", "Ervin", "Caleb", "James", "Seb", "Victor", "Jacob", "Barny", "Blake", "Bill", "Marcus", "Marvin", "Phillip", };
            bool[] driverIsActive = { true, true, true, true, true, true, false, false, false, true, false, false, true, true, false, true, false, true, false, true, true, false };
            bool[] driverHasFastestLap = { false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            int[] driverOvertakes = { -2, 1, -3, 3, 2, -10, 1, -1, 2, 4, 0, 0, 6, 0, 0, 0, 0, 0, 12, 0, 0, 0 };
            bool[] driverHasNoPenalties = { false, true, true, false, true, false, true, true, true, true, false, true, false, true, false, false, true, false, false, true, true, false };

            // Assign drivers names and points.
            for (int i = 0; i < 22; i++)
            {
                Driver[i].Name = driverNames[i];
                Driver[i].IsActive = driverIsActive[i];
                Driver[i].CurrentPosition = i + 1;
                Driver[i].HasFastestLap = driverHasFastestLap[i];
                Driver[i].HasNoPenalties = driverHasNoPenalties[i];
                Driver[i].PositionChanges = driverOvertakes[i];

                if (Driver[i].IsActive == true)
                {
                    Driver[i].PointsByPosition = PositionToPointsConverter.GetPoints(SettingsModel.PointsModel, Driver[i].CurrentPosition);
                }
            }
        }

        public void SimulateTeamPointsNew()
        {
            CalculateTeamPointsByPosition();

            CalculateTeamPointsByFastestLap();

            CalculateTeamPointsByPenalties();

            CalculateTeamPointsByPositionChange();

            CalculateTotalTeamPoints();

        }
    }
}
