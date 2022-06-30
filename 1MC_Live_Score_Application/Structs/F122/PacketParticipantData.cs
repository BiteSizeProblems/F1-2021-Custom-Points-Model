using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _1MC_Live_Score_Application.Structs.F122
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParticipantData
    {
        public byte m_aiControlled;           // Whether the vehicle is AI (1) or Human (0) controlled
        public byte m_driverId;       // Driver id - see appendix, 255 if network human
        public byte m_networkId;      // Network id – unique identifier for network players
        public byte m_teamId;                 // Team id - see appendix
        public byte m_myTeam;                 // My team flag – 1 = My Team, 0 = otherwise
        public byte m_raceNumber;             // Race number of the car
        public byte m_nationality;            // Nationality of the driver

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public char[] m_name;               // Name of participant in UTF-8 format – null terminated
                                            // Will be truncated with … (U+2026) if too long

        public byte m_yourTelemetry;          // The player's UDP setting, 0 = restricted, 1 = public

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketParticipantData
    {
        public PacketHeader m_header;            // Header

        public byte m_numActiveCars;  // Number of active cars in the data – should match number of
                               // cars on HUD

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public ParticipantData[] m_participants;
    }
}
