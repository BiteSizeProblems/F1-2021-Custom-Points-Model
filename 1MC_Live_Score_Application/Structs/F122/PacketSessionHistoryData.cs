﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _1MC_Live_Score_Application.Structs.F122
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LapHistoryData
    {
        uint m_lapTimeInMS;           // Lap time in milliseconds
        ushort m_sector1TimeInMS;       // Sector 1 time in milliseconds
        ushort m_sector2TimeInMS;       // Sector 2 time in milliseconds
        ushort m_sector3TimeInMS;       // Sector 3 time in milliseconds
        byte m_lapValidBitFlags;      // 0x01 bit set-lap valid,      0x02 bit set-sector 1 valid
                                       // 0x04 bit set-sector 2 valid, 0x08 bit set-sector 3 valid
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TyreStintHistoryData
    {
        byte m_endLap;                // Lap the tyre usage ends on (255 of current tyre)
        byte m_tyreActualCompound;    // Actual tyres used by this driver
        byte m_tyreVisualCompound;    // Visual tyres used by this driver
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketSessionHistoryData
    {
        PacketHeader m_header;                   // Header

        byte m_carIdx;                   // Index of the car this lap data relates to
        byte m_numLaps;                  // Num laps in the data (including current partial lap)
        byte m_numTyreStints;            // Number of tyre stints in the data

        byte m_bestLapTimeLapNum;        // Lap the best lap time was achieved on
        byte m_bestSector1LapNum;        // Lap the best Sector 1 time was achieved on
        byte m_bestSector2LapNum;        // Lap the best Sector 2 time was achieved on
        byte m_bestSector3LapNum;        // Lap the best Sector 3 time was achieved on

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        LapHistoryData[] m_lapHistoryData;   // 100 laps of data max

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        TyreStintHistoryData[] m_tyreStintsHistoryData; // Maximum of 8 stints
    }
}
