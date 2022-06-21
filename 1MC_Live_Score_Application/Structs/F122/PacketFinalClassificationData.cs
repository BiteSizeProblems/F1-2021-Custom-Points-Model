using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _1MC_Live_Score_Application.Structs.F122
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FinalClassificationData
    {
        byte m_position;              // Finishing position
        byte m_numLaps;               // Number of laps completed
        byte m_gridPosition;          // Grid position of the car
        byte m_points;                // Number of points scored
        byte m_numPitStops;           // Number of pit stops made
        byte m_resultStatus;          // Result status - 0 = invalid, 1 = inactive, 2 = active
                                       // 3 = finished, 4 = didnotfinish, 5 = disqualified
                                       // 6 = not classified, 7 = retired
        uint m_bestLapTimeInMS;       // Best lap time of the session in milliseconds
        double m_totalRaceTime;         // Total race time in seconds without penalties
        byte m_penaltiesTime;         // Total penalties accumulated in seconds
        byte m_numPenalties;          // Number of penalties applied to this driver
        byte m_numTyreStints;         // Number of tyres stints up to maximum

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] m_tyreStintsActual;   // Actual tyres used by this driver

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] m_tyreStintsVisual;   // Visual tyres used by this driver

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] m_tyreStintsEndLaps;  // The lap number stints end on

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketFinalClassificationData
    {
        PacketHeader m_header;                      // Header

        byte m_numCars;          // Number of cars in the final classification

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        FinalClassificationData[] m_classificationData;
    }
}
