using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _1MC_Live_Score_Application.Structs.F122
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LobbyInfoData
    {
        public byte m_aiControlled;            // Whether the vehicle is AI (1) or Human (0) controlled
        public byte m_teamId;                  // Team id - see appendix (255 if no team currently selected)
        public byte m_nationality;             // Nationality of the driver

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        public char[] m_name;        // Name of participant in UTF-8 format – null terminated
                                     // Will be truncated with ... (U+2026) if too long
        public byte m_carNumber;               // Car number of the player
        public byte m_readyStatus;             // 0 = not ready, 1 = ready, 2 = spectating
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketLobbyInfoData
    {
        PacketHeader m_header;                       // Header

        // Packet specific data
        public byte m_numPlayers;               // Number of players in the lobby data

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public LobbyInfoData[] m_lobbyPlayers;
    }
}
