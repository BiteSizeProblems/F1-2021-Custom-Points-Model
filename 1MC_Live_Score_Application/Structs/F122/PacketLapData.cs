using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _1MC_Live_Score_Application.Structs.F122
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LapData
    {
        uint m_lastLapTimeInMS;            // Last lap time in milliseconds
        uint m_currentLapTimeInMS;     // Current time around the lap in milliseconds
        ushort m_sector1TimeInMS;           // Sector 1 time in milliseconds
        ushort m_sector2TimeInMS;           // Sector 2 time in milliseconds
        float m_lapDistance;         // Distance vehicle is around current lap in metres – could
                                     // be negative if line hasn’t been crossed yet
        float m_totalDistance;       // Total distance travelled in session in metres – could
                                     // be negative if line hasn’t been crossed yet
        float m_safetyCarDelta;            // Delta in seconds for safety car
        byte m_carPosition;             // Car race position
        byte m_currentLapNum;       // Current lap number
        byte m_pitStatus;               // 0 = none, 1 = pitting, 2 = in pit area
        byte m_numPitStops;                 // Number of pit stops taken in this race
        byte m_sector;                  // 0 = sector1, 1 = sector2, 2 = sector3
        byte m_currentLapInvalid;       // Current lap invalid - 0 = valid, 1 = invalid
        byte m_penalties;               // Accumulated time penalties in seconds to be added
        byte m_warnings;                  // Accumulated number of warnings issued
        byte m_numUnservedDriveThroughPens;  // Num drive through pens left to serve
        byte m_numUnservedStopGoPens;        // Num stop go pens left to serve
        byte m_gridPosition;            // Grid position the vehicle started the race in
        byte m_driverStatus;            // Status of driver - 0 = in garage, 1 = flying lap
                                        // 2 = in lap, 3 = out lap, 4 = on track
        byte m_resultStatus;              // Result status - 0 = invalid, 1 = inactive, 2 = active
                                          // 3 = finished, 4 = didnotfinish, 5 = disqualified
                                          // 6 = not classified, 7 = retired
        byte m_pitLaneTimerActive;          // Pit lane timing, 0 = inactive, 1 = active
        ushort m_pitLaneTimeInLaneInMS;      // If active, the current time spent in the pit lane in ms
        ushort m_pitStopTimerInMS;           // Time of the actual pit stop in ms
        byte m_pitStopShouldServePen;   	 // Whether the car should serve a penalty at this stop
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketLapData
    {
        PacketHeader m_header;              // Header

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        LapData[] m_lapData;         // Lap data for all cars on track

        byte m_timeTrialPBCarIdx;  // Index of Personal Best car in time trial (255 if invalid)
        byte m_timeTrialRivalCarIdx; 	// Index of Rival car in time trial (255 if invalid)
    }
}
