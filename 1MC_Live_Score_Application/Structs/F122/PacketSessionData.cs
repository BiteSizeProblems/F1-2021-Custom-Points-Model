using System.Runtime.InteropServices;

namespace _1MC_Live_Score_Application.Structs.F122
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MarshalZone
    {
        float m_zoneStart;   // Fraction (0..1) of way through the lap the marshal zone starts
        byte m_zoneFlag;    // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WeatherForecastSample
    {
        byte m_sessionType;              // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P, 5 = Q1
                                         // 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ, 10 = R, 11 = R2
                                         // 12 = R3, 13 = Time Trial
        byte m_timeOffset;               // Time in minutes the forecast is for
        byte m_weather;                  // Weather - 0 = clear, 1 = light cloud, 2 = overcast
                                          // 3 = light rain, 4 = heavy rain, 5 = storm
        sbyte m_trackTemperature;         // Track temp. in degrees Celsius
        sbyte m_trackTemperatureChange;   // Track temp. change – 0 = up, 1 = down, 2 = no change
        sbyte m_airTemperature;           // Air temp. in degrees celsius
        sbyte m_airTemperatureChange;     // Air temp. change – 0 = up, 1 = down, 2 = no change
        byte m_rainPercentage;           // Rain percentage (0-100)
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketSessionData
    {
        PacketHeader m_header;

        byte m_weather;                // Weather - 0 = clear, 1 = light cloud, 2 = overcast
                                       // 3 = light rain, 4 = heavy rain, 5 = storm
        sbyte m_trackTemperature;        // Track temp. in degrees celsius
        sbyte m_airTemperature;          // Air temp. in degrees celsius
        byte m_totalLaps;              // Total number of laps in this race
        ushort m_trackLength;               // Track length in metres
        byte m_sessionType;            // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P
                                       // 5 = Q1, 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ
                                       // 10 = R, 11 = R2, 12 = R3, 13 = Time Trial
        sbyte m_trackId;                 // -1 for unknown, see appendix
        byte m_formula;                    // Formula, 0 = F1 Modern, 1 = F1 Classic, 2 = F2,
                                            // 3 = F1 Generic, 4 = Beta, 5 = Supercars
                                            // 6 = Esports, 7 = F2 2021
        ushort m_sessionTimeLeft;       // Time left in session in seconds
        ushort m_sessionDuration;       // Session duration in seconds
        byte m_pitSpeedLimit;          // Pit speed limit in kilometres per hour
        byte m_gamePaused;                // Whether the game is paused – network game only
        byte m_isSpectating;           // Whether the player is spectating
        byte m_spectatorCarIndex;      // Index of the car being spectated
        byte m_sliProNativeSupport;    // SLI Pro support, 0 = inactive, 1 = active

        byte m_numMarshalZones;            // Number of marshal zones to follow

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        MarshalZone[] m_marshalZones;         	// List of marshal zones – max 21

        byte m_safetyCarStatus;           // 0 = no safety car, 1 = full
                                          // 2 = virtual, 3 = formation lap
        byte m_networkGame;               // 0 = offline, 1 = online

        byte m_numWeatherForecastSamples; // Number of weather samples to follow

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
        WeatherForecastSample[] m_weatherForecastSamples;   // Array of weather forecast samples

        byte m_forecastAccuracy;          // 0 = Perfect, 1 = Approximate
        byte m_aiDifficulty;              // AI Difficulty rating – 0-110
        uint m_seasonLinkIdentifier;      // Identifier for season - persists across saves
        uint m_weekendLinkIdentifier;     // Identifier for weekend - persists across saves
        uint m_sessionLinkIdentifier;     // Identifier for session - persists across saves
        byte m_pitStopWindowIdealLap;     // Ideal lap to pit on for current strategy (player)
        byte m_pitStopWindowLatestLap;    // Latest lap to pit on for current strategy (player)
        byte m_pitStopRejoinPosition;     // Predicted position to rejoin at (player)
        byte m_steeringAssist;            // 0 = off, 1 = on
        byte m_brakingAssist;             // 0 = off, 1 = low, 2 = medium, 3 = high
        byte m_gearboxAssist;             // 1 = manual, 2 = manual & suggested gear, 3 = auto
        byte m_pitAssist;                 // 0 = off, 1 = on
        byte m_pitReleaseAssist;          // 0 = off, 1 = on
        byte m_ERSAssist;                 // 0 = off, 1 = on
        byte m_DRSAssist;                 // 0 = off, 1 = on
        byte m_dynamicRacingLine;         // 0 = off, 1 = corners only, 2 = full
        byte m_dynamicRacingLineType;     // 0 = 2D, 1 = 3D
        byte m_gameMode;                  // Game mode id - see appendix
        byte m_ruleSet;                   // Ruleset - see appendix
        uint m_timeOfDay;                 // Local time of day - minutes since midnight
        byte m_sessionLength;             // 0 = None, 2 = Very Short, 3 = Short, 4 = Medium
                                          // 5 = Medium Long, 6 = Long, 7 = Full
    }
}
