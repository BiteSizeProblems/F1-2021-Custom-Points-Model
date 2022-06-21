using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _1MC_Live_Score_Application.Structs.F122
{
    /// <summary>
    /// Each packet has the following header
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketHeader
    {
        /// <summary>
        /// 2022
        /// </summary>
        public ushort packetFormat;

        /// <summary>
        /// Game major version - "X.00"
        /// </summary>
        public byte gameMajorVersion;

        /// <summary>
        /// Game minor version - "1.XX"
        /// </summary>
        public byte gameMinorVersion;

        /// <summary>
        /// Version of this packet type, all start from 1
        /// </summary>
        public byte packetVersion;

        /// <summary>
        /// Identifier for the packet type, see <see cref="PacketID"/>
        /// </summary>
        public byte packetId;

        /// <summary>
        /// Unique identifier for the session
        /// </summary>
        public ulong sessionUID;

        /// <summary>
        /// Session timestamp
        /// </summary>
        public float sessionTime;

        /// <summary>
        /// Identifier for the frame the data was retrieved on
        /// </summary>
        public uint frameIdentifier;

        /// <summary>
        /// Index of player's car in the array
        /// </summary>
        public byte playerCarIndex;

        /// <summary>
        /// Index of secondary player's car in the array (splitscreen)
        /// </summary>
        public byte secondaryPlayerCarIndex;
    }
}
