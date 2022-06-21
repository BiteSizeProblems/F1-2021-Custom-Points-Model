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

        private Timer slowTimer;

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

            ResetTeams();

            //UDPC.OnSessionDataReceive += StoreSessionData;
            //UDPC.OnFinalClassificationDataReceive += StoreFinalClassificationData;
            UDPC.OnLapDataReceive += StoreLapData;
            //UDPC.OnLobbyInfoDataReceive += StoreLobbyInfoData;
            //UDPC.OnParticipantsDataReceive += StoreParticipantData;
            //UDPC.OnSessionHistoryDataReceive += StoreSessionHistoryData;

            slowTimer = new Timer(CalculateDriverPoints, null, 0, 1000);

        }

        public void ResetTeams()
        {
            if (Team == null)
            {
                for (int i = 0; i < 4; i++)
                {
                    Team.Add(new TeamDataModel());
                    Team[i].Team = i + 1;
                    Team[i].ID = i + 1;
                }

                Team1 = Team[0];
                Team2 = Team[1];
                Team3 = Team[2];
                Team4 = Team[3];

                GetDriversPerTeam();
            }
            else
            {
                Team.Clear();

                for (int i = 0; i < 4; i++)
                {
                    Team.Add(new TeamDataModel());
                    Team[i].Team = i + 1;
                    Team[i].ID = i + 1;
                }

                Team1 = Team[0];
                Team2 = Team[1];
                Team3 = Team[2];
                Team4 = Team[3];

                GetDriversPerTeam();
            }

        }

        private void GetDriversPerTeam()
        {
            for (int i = 0; i < 22; i++)
            {
                var driversTeam = Driver[i].Team;

                for (int j = 0; j < Team.Count; j++)
                {

                    if (driversTeam == j + 1)
                    {
                        Team[j].NumDrivers += 1;
                    }

                }
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
        }
        private void StoreSessionData(PacketSessionData packet)
        {
            latestSessionDataPacket = packet;
        }

        public void SubmitSettings()
        {
            CalculateTotalTeamPoints();
        }

        private void CalculateDriverPoints(object? state)
        {
            PacketLapData lapDataPacket = latestLapDataPacket;
            //PacketSessionData SessionDataPacket = latestSessionDataPacket;
            //PacketFinalClassificationData FinalClassificationDataPacket = latestFinalClassificationDataPacket;
            //PacketLobbyInfoData LobbyInfoDataPacket = latestLobbyInfoDataPacket;
            //PacketParticipantsData ParticipantDataPacket = latestParticipantDataPacket;
            //PacketSessionHistoryData SessionHistoryDataPacket = latestSessionHistoryDataPacket;

            if (latestLapDataPacket.lapData != null)
            {
                for (int i = 0; i < latestLapDataPacket.lapData.Length; i++)
                {
                    var lapData = lapDataPacket.lapData[i];

                    if ( lapData.carPosition == 0)
                    {
                        Driver[i].Team = 0;
                    }

                    if ( lapData.resultStatus == Appendeces.ResultStatus.Active || lapData.resultStatus == Appendeces.ResultStatus.Finished)
                    {
                        Driver[i].IsActive = true;
                    }

                    if ( lapData.penalties != 0)
                    {
                        Driver[i].HasNoPenalties = false;
                    }
                    else
                    {
                        Driver[i].HasNoPenalties = true;
                    }

                    Driver[i].CurrentPosition = lapData.carPosition;
                    Driver[i].GridPosition = lapData.gridPosition;
                    Driver[i].PositionChanges = lapData.carPosition - lapData.gridPosition;

                    if (Driver[i].IsActive == true)
                    {
                        Driver[i].PointsByPosition = PositionToPointsConverter.GetPoints(SettingsModel.PointsModel, Driver[i].CurrentPosition);
                    }

                    CalculateTeamPointsByPosition();

                    //CalculateTeamPointsByPositionChange();

                    //CalculateTeamPointsByFastestLap();

                    //CalculateTeamPointsByPenalties();

                    //CalculateTotalTeamPoints();
                    
                }
            }
        }

        private void CalculateTeamPointsByPosition()
        {

            for (int i = 0; i < 22; i++)
            {
                var driversTeam = Driver[i].Team;

                if (Driver[i].IsActive == true)
                {
                    var driversPointsByPosition = Driver[i].PointsByPosition;

                    for (int j = 0; j < Team.Count; j++)
                    {
                        int activeDrivers = 0;

                        if (driversTeam == j + 1)
                        {
                            activeDrivers = activeDrivers += 1;
                            
                            Team[j].PointsByPosition += driversPointsByPosition;
                        }

                        Team[j].NumActiveDrivers = activeDrivers;
                    }
                }
            }
        }

        private void CalculateTotalTeamPoints()
        {
            for ( int i = 0; i < SettingsModel.NumTeams; i++)
            {
                Team[i].TotalPoints = Team[i].PointsByPosition + Team[i].FastestLapPoint + Team[i].PointsByPosition + Team[i].PointsByPosition;
            }
        }

        private void CalculateTeamPointsByPenalties()
        {
            
        }

        private void CalculateTeamPointsByFastestLap()
        {
            
        }

        private void CalculateTeamPointsByPositionChange()
        {
            for (int i = 0; i < 22; i++)
            {
                if (Driver[i].Team == 1)
                {
                    //Team[0].PositionChanges += Driver[i].PositionChanges; // Calculate total Points
                }
                else if (Driver[i].Team == 2)
                {
                    //Team[1].PositionChanges += Driver[i].PositionChanges;
                }
                else if (Driver[i].Team == 3)
                {
                    //Team[2].PositionChanges += Driver[i].PositionChanges;
                }
                else if (Driver[i].Team == 4)
                {
                    //Team[3].PositionChanges += Driver[i].PositionChanges;
                }
            }

            for (int i = 0; i < SettingsModel.NumTeams; i++)
            {
                //SettingsModel.TeamPositionChangeTotals[i] = Team[i].PositionChanges;

                //var mostPositionChanges = SettingsModel.TeamPositionChangeTotals.Where(x => x != 0).DefaultIfEmpty().Min();

                //if (Team[i].PositionChanges == mostPositionChanges)
                {
                    //Team[i].HasMostPositionChanges = 1;
                }
            }
        }

        public void SubmitSettings_Simulate()
        {
            SimulateDrivers();
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

            SimulateTeamPoints();

        }

        public void SimulateTeamPoints()
        {
            for (int i = 0; i < 22; i++)
            {
                var driversTeam = Driver[i].Team;
                var driverOvertakes = Driver[i].PositionChanges;

                for (int j = 0; j < Team.Count; j++)
                {
                    if (driversTeam == j + 1)
                    {
                        Team[j].NumDrivers += 1;
                        Team[j].PositionChanges += driverOvertakes;
                    }
                }

                if (Driver[i].IsActive == true)
                {
                    var driversPointsByPosition = Driver[i].PointsByPosition;
                    var driverHasFastestLap = Driver[i].HasFastestLap;
                    var driverHasNoPenalties = Driver[i].HasNoPenalties;

                    for (int j = 0; j < Team.Count; j++)
                    {
                        if (driversTeam == j + 1)
                        {
                            Team[j].NumActiveDrivers += 1;
                            Team[j].PointsByPosition += driversPointsByPosition;

                            if (driverHasFastestLap == true)
                            {
                                Team[j].HasFastestLap = true;
                            }

                            if (driverHasNoPenalties == false)
                            {
                                Team[j].HasNoPenalties = false;
                            }
                        }
                    }
                }
            }

            int[] overtakesArray = new int[Team.Count];

            for (int i = 0; i < Team.Count; i++)
            {
                if (Team[i].HasFastestLap == true)
                {
                    Team[i].FastestLapPoint = Team[i].NumActiveDrivers;
                }

                if (Team[i].HasNoPenalties == true)
                {
                    Team[i].NoPenaltiesPoint = Team[i].NumActiveDrivers;
                }

                overtakesArray[i] = Team[i].PositionChanges;

                if (Team[i].PositionChanges == overtakesArray.Max())
                {
                    Team[i].HasMostPositionChanges = true;
                    Team[i].MostOvertakesPoint += 1;
                }
                else
                {
                    Team[i].HasMostPositionChanges = false;
                    Team[i].MostOvertakesPoint += 0;
                }

                Team[i].TotalPoints = Team[i].PointsByPosition + Team[i].FastestLapPoint + Team[i].NoPenaltiesPoint + Team[i].MostOvertakesPoint;

            }
        }

    }
}
