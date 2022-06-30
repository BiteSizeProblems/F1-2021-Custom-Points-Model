using _1MC_Live_Score_Application.Core.Utils;
using _1MC_Live_Score_Application.Models;
using _1MC_Live_Score_Application.Structs.F122;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Data;
using static _1MC_Live_Score_Application.Structs.F122.Appendeces;

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

        public SettingsModel SettingsModel { get; set; }
        public ObservableCollection<DriverDataModel> Driver { get; set; }
        public ObservableCollection<TeamDataModel> Team { get; set; }

        private readonly object _driverLock = new object();
        private readonly object _teamLock = new object();

        private Timer fastTimer;
        private Timer fastTimer2;
        private Timer slowTimer;
        private Timer mediumTimer;

        public PacketSessionData latestSessionDataPacket { get; set; }
        public PacketFinalClassificationData latestFinalClassificationDataPacket { get; set; }
        public PacketLapData latestLapDataPacket { get; set; }
        public PacketLobbyInfoData latestLobbyInfoDataPacket { get; set; }
        public PacketParticipantData latestParticipantDataPacket { get; set; }
        public PacketSessionHistoryData latestSessionHistoryDataPacket { get; set; }

        private DataViewModel(): base()
        {
            Driver = new ObservableCollection<DriverDataModel>();
            Team = new ObservableCollection<TeamDataModel>();
            SettingsModel = new SettingsModel();

            BindingOperations.EnableCollectionSynchronization(Driver, _driverLock);
            BindingOperations.EnableCollectionSynchronization(Driver, _teamLock);

            for ( int i = 0; i < 22; i++)
            {
                Driver.Add(new DriverDataModel());

                Driver[i].ID = i + 1;
                Driver[i].Name = "...";
                Driver[i].Team = 0;
                Driver[i].CurrentPosition = i + 1;
            }

            for (int i = 0; i < 4; i++)
            {
                Team.Add(new TeamDataModel());
                Team[i].ID = i + 1;
                Team[i].Name = $"Team {i + 1}";
            }

            UDPC.OnLapDataReceive += UDPC_OnLapDataReceive;
            UDPC.OnSessionHistoryDataReceive += UDPC_OnSessionHistoryDataReceive;
            UDPC.OnFinalClassificationDataReceive += UDPC_OnFinalClassificationDataReceive;
            UDPC.OnLobbyInfoDataReceive += UDPC_OnLobbyInfoDataReceive;
            UDPC.OnParticipantDataReceive += UDPC_OnParticipantDataReceive;

            UDPC.OnFinalClassificationDataReceive += StoreFinalClassificationData;
            UDPC.OnLapDataReceive += StoreLapData;
            UDPC.OnLobbyInfoDataReceive += StoreLobbyInfoData;
            UDPC.OnSessionHistoryDataReceive += StoreSessionHistoryData;
            //UDPC.OnParticipantsDataReceive += StoreParticipantData;
            //UDPC.OnSessionDataReceive += StoreSessionData;

            fastTimer2 = new Timer(AssignTeamValues, null, 0, 1000);
            slowTimer = new Timer(CalculateTeamPoints, null, 0, 3000);

            if (SettingsModel.SimulationActive == false)
            {
                fastTimer = new Timer(GetDriverData, null, 0, 1000);
                mediumTimer = new Timer(CallDriversPerTeam, null, 0, 1000);
            }
        }

        // ASSIGN TEAM COLORS
        public void CreateTeamColors()
        {
            Team[0].TeamColor = SettingsModel.Team1Color;
            Team[1].TeamColor = SettingsModel.Team2Color;
            Team[2].TeamColor = SettingsModel.Team3Color;
            Team[3].TeamColor = SettingsModel.Team4Color;
        }

        // GET DRIVERS PER TEAM
        private void CallDriversPerTeam(object? state)
        {
            GetDriversPerTeam();
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

                Team[i].DriversNumActive = $"{numActiveDrivers} / {numDrivers}";

            }
        }

        // LOBBY INFO PACKET
        private void UDPC_OnLobbyInfoDataReceive(PacketLobbyInfoData packet)
        {
            Debug.WriteLine("Lobby Info Packet Received");

            SettingsModel.IsConnectionActive = true;

            for (int i = 0; i < 22; i++)
            {
                var lobbyData = packet.m_lobbyPlayers[i];

                Driver[i].Index = i;
                Driver[i].CarID = lobbyData.m_carNumber;

                if (lobbyData.m_aiControlled == 1)
                {
                    Driver[i].IsAI = true;
                }
                else
                {
                    Driver[i].IsAI = false;
                }

                // Driver Names
                string thisName = "";

                for (int j = 0; j < lobbyData.m_name.Length; j++)
                {
                    var thisChar = lobbyData.m_name[j];

                    thisName += thisChar.ToString();
                }

                if (thisName.Contains("Player") == true)
                {
                    Driver[i].Name = "Bad Name";
                }
                else
                {
                    Driver[i].Name = thisName.ToString();
                }
            }
        }

        // LAP DATA PACKET
        private void UDPC_OnLapDataReceive(PacketLapData packet)
        {
            SettingsModel.IsConnectionActive = true;

            for (int i = 0; i < 22; i++)
            {
                var lapData = packet.m_lapData[i];

                Driver[i].CurrentPosition = lapData.m_carPosition;
                Driver[i].GridPosition = lapData.m_gridPosition;
                Driver[i].ResultStatus = lapData.m_resultStatus;
                Driver[i].Penalties = TimeSpan.FromSeconds(lapData.m_penalties);
            }
        }

        // PARTICIPANT PACKET
        private void UDPC_OnParticipantDataReceive(PacketParticipantData packet)
        {
            for (int i = 0; i < 22; i++)
            {
                var participantData = packet.m_participants[i];

                Driver[i].CarID = participantData.m_raceNumber;

                if (participantData.m_yourTelemetry == 0)
                {
                    Driver[i].IsUDPPublic = false;
                }
                else
                {
                    Driver[i].IsUDPPublic = true;
                }

                if (participantData.m_aiControlled == 1)
                {
                    Driver[i].IsAI = true;
                }
                else
                {
                    Driver[i].IsAI = false;
                }

                // Driver Names
                string thisName = "";

                for (int j = 0; j < participantData.m_name.Length; j++)
                {
                    var thisChar = participantData.m_name[j];

                    thisName += thisChar.ToString();
                }

                if (thisName.Contains("Player") == true)
                {
                    Driver[i].Name = "Bad Name";
                }
                else
                {
                    Driver[i].Name = thisName.ToString();
                }
            }
        }

        // SESSION HISTORY DATA PACKET
        private void UDPC_OnSessionHistoryDataReceive(PacketSessionHistoryData packet)
        {
            var carId = packet.m_carIdx;
            var driverData = Driver[carId];

            driverData.FastestLapNum = packet.m_bestLapTimeLapNum;
            driverData.NumLaps = packet.m_numLaps;

            for (int i = 0; i < packet.m_lapHistoryData.Length; i++)
            {
                var sessionHistoryData = packet.m_lapHistoryData[i];

                var lapRef = i + 1;

                if (lapRef == packet.m_bestLapTimeLapNum && sessionHistoryData.m_lapTimeInMS != 0)
                {
                    Driver[carId].FastestLapTime = TimeSpan.FromMilliseconds(sessionHistoryData.m_lapTimeInMS);
                }
            }
        }

        // FINAL CLASSIFICATION DATA PACKET
        private void UDPC_OnFinalClassificationDataReceive(PacketFinalClassificationData packet)
        {
            Debug.WriteLine("Final Classification Packet Received");

            SettingsModel.IsConnectionActive = false;

            for (int i = 0; i < packet.m_classificationData.Length; i++)
            {
                var finalData = packet.m_classificationData[i];

                Driver[i].CurrentPosition = finalData.m_position;
                Driver[i].GridPosition = finalData.m_gridPosition;
                Driver[i].NumLaps = finalData.m_numLaps;
                Driver[i].FastestLapTime = TimeSpan.FromMilliseconds(finalData.m_bestLapTimeInMS);
                Driver[i].Penalties = TimeSpan.FromSeconds(finalData.m_penaltiesTime);
            }
        }

        private void StoreSessionHistoryData(PacketSessionHistoryData packet)
        {
            latestSessionHistoryDataPacket = packet;
        }
        private void StoreParticipantData(PacketParticipantData packet)
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
        }
        private void StoreFinalClassificationData(PacketFinalClassificationData packet)
        {
            latestFinalClassificationDataPacket = packet;
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
            PacketLobbyInfoData lobbyInfoDataPacket = latestLobbyInfoDataPacket;

            // LAP DATA
            for (int i = 0; i < 22; i++)
            {
                // REMOVE INVALID DRIVERS
                if (Driver[i].CurrentPosition == 0)
                {
                    Driver[i].Team = 0;
                }
                else
                {
                    // IS DRIVER ACTIVE?
                    if (Driver[i].ResultStatus == ResultStatus.Active || Driver[i].ResultStatus == ResultStatus.Finished)
                    {
                        Driver[i].IsActive = true;
                    }
                    else
                    {
                        Driver[i].IsActive = false;
                    }

                    // SET DRIVER PBP
                    Driver[i].PointsByPosition = PositionToPointsConverter.GetPoints(SettingsModel.PointsModel, Driver[i].CurrentPosition, Driver[i].IsActive);

                    SettingsModel.AllFastestLapsArray[i] = Driver[i].FastestLapTime;

                    // DOES DRIVER HAVE FASTEST LAP?
                    if (Driver[i].FastestLapTime == SettingsModel.FastestOverallLapTime)
                    {
                        Driver[i].HasFastestLap = true;
                    }
                    else
                    {
                        Driver[i].HasFastestLap = false;
                    }

                    SettingsModel.FastestOverallLapTime = SettingsModel.AllFastestLapsArray.Where(x => x != TimeSpan.Zero).DefaultIfEmpty().Min();

                    if (Driver[i].Penalties == TimeSpan.Zero)
                    {
                        Driver[i].HasPenalty = false;
                    }
                    else
                    {
                        Driver[i].HasPenalty = true;
                    }

                    if (Driver[i].GridPosition < Driver[i].CurrentPosition)
                    {
                        Driver[i].NumOvertakes = 0;
                    }
                    else
                    {
                        Driver[i].NumOvertakes = (Driver[i].GridPosition - Driver[i].CurrentPosition);
                    }
                }
            }
        }

        private void AssignTeamValues(object? state)
        {
            var overtakesArray = SettingsModel.TeamPositionChangeTotals;

            for (int i = 0; i < Team.Count; i++)
            {
                var thisTeam = i + 1;
                int pbp = 0;
                var overtakes = 0;

                for (int j = 0; j < 22; j++)
                {
                    var driversTeam = Driver[j].Team;
                    var driversPointsByPosition = Driver[j].PointsByPosition;

                    if (thisTeam == driversTeam)
                    {
                        if (Driver[j].IsActive == true) // Points By Position
                        {
                            pbp += driversPointsByPosition;
                        }

                        if (Driver[j].HasPenalty == true) // Has Penalty
                        {
                            Team[i].HasPenalty = true;
                        }

                        if (Driver[j].HasFastestLap == true) // Has Fastest Lap
                        {
                            Team[i].HasFastestLap = true;
                            SettingsModel.TeamWithFastestLap = i + 1;
                        }

                        overtakes += Driver[j].NumOvertakes;
                    }
                }

                Team[i].PointsByPosition = pbp;

                if (Team[i].ID != SettingsModel.TeamWithFastestLap)
                {
                    Team[i].HasFastestLap = false;
                }

                SettingsModel.OvertakesArray[i] = Team[i].NumOvertakes;
                overtakesArray[i] = overtakes;
                Team[i].NumOvertakes = overtakes;

                if (Team[i].NumOvertakes == overtakesArray.Max())
                {
                    Team[i].HasMostOvertakes = true;
                }
                else
                {
                    Team[i].HasMostOvertakes = false;
                }

            }

            SettingsModel.MostOvertakes = overtakesArray.Max();
        }

        private void CalculateTeamPoints(object state = null)
        {
            if (SettingsModel.PointsModel != null && Team.Count > 1)
            {
                CalculateTeamPointsByPositionChange();

                CalculateTeamPointsByFastestLap();

                CalculateTeamPointsByPenalties();

                CalculateTotalTeamPoints();
            }
        }

        private void CalculateTeamPointsByPenalties()
        {
            for (int i = 0; i < Team.Count; i++)
            {
                if (Team[i].HasPenalty == true)
                {
                    Team[i].NoPenaltiesPoint = 0;
                }
                else
                {
                    Team[i].NoPenaltiesPoint = Team[i].NumActiveDrivers;
                }
            }
        }

        private void CalculateTeamPointsByFastestLap()
        {
            for ( int i = 0; i < Team.Count; i++)
            {
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
            bool overtakesDuplicates;

            var overtakesArray = SettingsModel.OvertakesArray;

            Array.Sort(overtakesArray);

            if (overtakesArray[3] == overtakesArray[2] || overtakesArray[3] == overtakesArray[1])
            {
                overtakesDuplicates = true;
            }
            else
            {
                overtakesDuplicates = false;
            }

            for (int i = 0; i < Team.Count; i++)
            {
                Team[i].MostOvertakesPoint = MostOvertakesPointCalculator.GetOvertakesPoint(Team[i].HasMostOvertakes, overtakesDuplicates);
            }

        }

        private void CalculateTotalTeamPoints()
        {
            var totalScore = 0;

            for (int i = 0; i < Team.Count; i++)
            {
                Team[i].TotalPoints = Team[i].PointsByPosition + Team[i].FastestLapPoint + Team[i].NoPenaltiesPoint + Team[i].MostOvertakesPoint;

                totalScore += Team[i].TotalPoints;

                // DATA QUALITY TESTS
                if (SettingsModel.NumTeams == 2 || SettingsModel.NumTeams == 4)
                {
                    if (Team[i].TotalPoints > 87)
                    {
                        Team[i].InvalidScore = true;
                    }
                    else
                    {
                        Team[i].InvalidScore = false;
                    }
                }
                else if (SettingsModel.NumTeams == 3)
                {
                    if (Team[i].TotalPoints > 121)
                    {
                        Team[i].InvalidScore = true;
                    }
                    else
                    {
                        Team[i].InvalidScore = false;
                    }
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
            int[] twoTeams = { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int[] threeTeams = { 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1};
            int[] fourTeams = { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2};

            // Assign drivers names and points.
            for (int i = 0; i < 22; i++)
            {
                Driver[i].Name = driverNames[i];
                Driver[i].IsActive = driverIsActive[i];
                Driver[i].CurrentPosition = i + 1;
                Driver[i].HasFastestLap = driverHasFastestLap[i];
                Driver[i].HasPenalty = driverHasNoPenalties[i];
                Driver[i].PositionChanges = driverOvertakes[i];

                
                    Driver[i].PointsByPosition = PositionToPointsConverter.GetPoints(SettingsModel.PointsModel, Driver[i].CurrentPosition, Driver[i].IsActive);
                

                if (SettingsModel.NumTeams == 2)
                {
                    Driver[i].Team = twoTeams[i];
                }
                else if (SettingsModel.NumTeams == 3)
                {
                    Driver[i].Team = threeTeams[i];
                }
                else
                {
                    Driver[i].Team = fourTeams[i];
                }
            }
        }

        public void SimulateTeamPointsNew()
        {
            CalculateTeamPointsByFastestLap();

            CalculateTeamPointsByPenalties();

            CalculateTeamPointsByPositionChange();

            CalculateTotalTeamPoints();

        }
    }
}
