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
    }
}
