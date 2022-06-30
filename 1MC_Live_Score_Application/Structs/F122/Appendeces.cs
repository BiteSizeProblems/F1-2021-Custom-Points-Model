using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1MC_Live_Score_Application.Structs.F122
{
    public static class Appendeces
    {
        /// <summary>
        /// Type of the packet (In header's PocketID property).
        /// </summary>
        public enum PacketTypes : byte
        {
            /// <summary>
            /// Contains all motion data for player’s car – only sent while player is in control
            /// </summary>
            CarMotion,
            /// <summary>
            /// Data about the session – track, time left
            /// </summary>
            Session,
            /// <summary>
            /// Data about all the lap times of cars in the session
            /// </summary>
            LapData,
            /// <summary>
            /// Various notable events that happen during a session
            /// </summary>
            Event,
            /// <summary>
            /// List of participants in the session, mostly relevant for multiplayer
            /// </summary>
            Participants,
            /// <summary>
            /// Packet detailing car setups for cars in the race
            /// </summary>
            CarSetups,
            /// <summary>
            /// Telemetry data for all cars
            /// </summary>
            CarTelemetry,
            /// <summary>
            /// Status data for all cars
            /// </summary>
            CarStatus,
            /// <summary>
            /// Final classification confirmation at the end of a race
            /// </summary>
            FinalClassification,
            /// <summary>
            /// Information about players in a multiplayer lobby
            /// </summary>
            LobbyInfo,
            /// <summary>
            /// Damage status for all cars
            /// </summary>
            CarDamage,
            /// <summary>
            /// Lap and tyre data for session
            /// </summary>
            SessionHistory
        }

        public static List<LapValid> LapValidChecker(byte val)
        {
            List<LapValid> b = new List<LapValid>();

            Appendeces.IsValid(val, LapValid.FullLapValid, ref b);
            Appendeces.IsValid(val, LapValid.Sector1Valid, ref b);
            Appendeces.IsValid(val, LapValid.Sector2Valid, ref b);
            Appendeces.IsValid(val, LapValid.Sector3Valid, ref b);

            return b;
        }

        private static void IsValid(byte uint32b, LapValid key, ref List<LapValid> b)
        {
            if (((LapValid)uint32b).HasFlag(key)) b.Add(key);
        }

        public enum LapValid
        {
            FullLapValid = 0x01,
            Sector1Valid = 0x02,
            Sector2Valid = 0x04,
            Sector3Valid = 0x08,
        }

        public enum PenaltyTypes : byte
        {
            DriveThrough,
            StopGo,
            GridPenalty,
            PenaltyReminder,
            TimePenalty,
            Warning,
            Disqualified,
            RemovedFromFormationLap,
            ParkedTooLongTimer,
            TyreRegulations,
            ThisLapInvalidated,
            ThisAndNextLapInvalidated,
            ThisLapInvalidatedWithoutReason,
            ThisAndNextLapInvalidatedWithoutReason,
            ThisAndPreviousLapInvalidated,
            ThisAndPreviousLapInvalidatedWithoutReason,
            Retired,
            BlackFlagTimer,
        }

        public enum InfringementTypes : byte
        {
            BlockingBySlowDriving,
            BlockingByWrongWayDriving,
            ReversingOffTheStartLine,
            BigCollision,
            SmallCollision,
            CollisionFailedToHandBackPositionSingle,
            CollisionFailedToHandBackPositionMultiple,
            CornerCuttingGainedTime,
            CornerCuttingOvertakeSingle,
            CornerCuttingOvertakeMultiple,
            CrossedPitExitLane,
            IgnoringBlueFlags,
            IgnoringYellowFlags,
            IgnoringDriveThrough,
            TooManyDriveThroughs,
            DriveThroughReminderServeWithinnLaps,
            DriveThroughReminderServeThisLap,
            PitLaneSpeeding,
            ParkedForTooLong,
            IgnoringTyreRegulations,
            TooManyPenalties,
            MultipleWarnings,
            ApproachingDisqualification,
            TyreRegulationsSelectSingle,
            TyreRegulationsSelectMultiple,
            LapInvalidatedCornerCutting,
            LapInvalidatedRunningWide,
            CornerCuttingRanWideGainedTimeMinor,
            CornerCuttingRanWideGainedTimeSignificant,
            CornerCuttingRanWideGainedTimeExtreme,
            LapInvalidatedWallRiding,
            LapInvalidatedFlashbackUsed,
            LapInvalidatedResetToTrack,
            BlockingThePitlane,
            JumpStart,
            SafetyCarToCarCollision,
            SafetyCarIllegalOvertake,
            SafetyCarExceedingAllowedPace,
            VirtualSafetyCarExceedingAllowedPace,
            FormationLapBelowAllowedSpeed,
            RetiredMechanicalFailure,
            RetiredTerminallyDamaged,
            SafetyCarFallingTooFarBack,
            BlackFlagTimer,
            UnservedStopGoPenalty,
            UnservedDriveThroughPenalty,
            EngineComponentChange,
            GearboxChange,
            LeagueGridPenalty,
            RetryPenalty,
            IllegalTimeGain,
            MandatoryPitstop,
        }

        public enum SessionTypes : byte
        {
            UNKNOWN,
            P1,
            P2,
            P3,
            P_SHORT,
            Q1,
            Q2,
            Q3,
            Q_SHORT,
            Q_ONESHOT,
            RACE,
            RACE_TWO,
            TIME_TRIAL,
        }

        public enum DriverStatus : sbyte
        {
            Unknown = -1,
            In_Garage = 0,
            Flying_Lap = 1,
            In_Lap = 2,
            Out_Lap = 3,
            On_Track = 4,
        }

        public enum ResultStatus : sbyte
        {
            Unknown = -1,
            Invalid = 0,
            Inactive = 1,
            Active = 2,
            Finished = 3,
            DNF = 4,
            DSQ = 5,
            Not_Classified = 6,
            Retired = 7,
        }

        public enum EventTypes : byte
        {
            Unknown,
            SessionStarted,
            SessionEnded,
            FastestLap,
            Retirement,
            DRSenabled,
            DRSdisabled,
            TeamMateInPits,
            ChequeredFlag,
            RaceWinner,
            PenaltyIssued,
            SpeedTrapTriggered,
            StartLights,
            LightsOut,
            DriveThroughServed,
            StopGoServed,
            Flashback,
            ButtonStatus,
        }
    }
}
