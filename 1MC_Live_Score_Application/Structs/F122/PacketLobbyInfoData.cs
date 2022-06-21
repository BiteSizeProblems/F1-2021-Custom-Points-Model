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
        byte m_aiControlled;            // Whether the vehicle is AI (1) or Human (0) controlled
        byte m_teamId;                  // Team id - see appendix (255 if no team currently selected)
        byte m_nationality;             // Nationality of the driver

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
        char[] m_name;        // Name of participant in UTF-8 format – null terminated
                                // Will be truncated with ... (U+2026) if too long
        byte m_carNumber;               // Car number of the player
        byte m_readyStatus;             // 0 = not ready, 1 = ready, 2 = spectating
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketLobbyInfoData
    {
        PacketHeader m_header;                       // Header

        // Packet specific data
        byte m_numPlayers;               // Number of players in the lobby data

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        LobbyInfoData[] m_lobbyPlayers;
    }
}
