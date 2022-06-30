using System;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Timers;
using _1MC_Live_Score_Application.Structs.F121;
using static _1MC_Live_Score_Application.Structs.F121.Appendeces;

namespace _1MC_Live_Score_Application.Core.UDP
{
    public class UDPConnection
    {
        /// <summary>
        /// Time required to time out in MS.
        /// </summary>
        private const float TIMEOUT_IN_MS = 500.0f;

        private UdpClient _client;
        private IPEndPoint _endPoint;

        private Timer _timeoutTimer;

        /// <summary>
        /// Indicates if we are currently connected.
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// Connection status change delegate.
        /// </summary>
        /// <param name="connected">True if connected, false if disconnected.</param>
        public delegate void ConnectStatusChangeDelegate(bool connected);

        /// <summary>
        /// Called when connect status changed. 
        /// Connected bool indicates if there is a connection (true) or disconnection (false).
        /// </summary>
        public event ConnectStatusChangeDelegate OnConnectStatusChanged;

        // Delegates
        public delegate void SessionDataReceiveDelegate(PacketSessionData packet);
        public delegate void LapDataReceiveDelegate(PacketLapData packet);
        public delegate void ParticipantsDataReceiveDelegate(PacketParticipantsData packet);
        public delegate void FinalClassificationDataReceiveDelegate(PacketFinalClassificationData packet);
        public delegate void LobbyInfoDataReceiveDelegate(PacketLobbyInfoData packet);
        public delegate void SessionHistoryDataReceiveDelegate(PacketSessionHistoryData packet);

        public PacketParticipantsData LastParticipantsPacket { get; private set; }

        // Packet events
        public event SessionDataReceiveDelegate OnSessionDataReceive;
        public event LapDataReceiveDelegate OnLapDataReceive;
        public event ParticipantsDataReceiveDelegate OnParticipantsDataReceive;
        public event FinalClassificationDataReceiveDelegate OnFinalClassificationDataReceive;
        public event LobbyInfoDataReceiveDelegate OnLobbyInfoDataReceive;
        public event SessionHistoryDataReceiveDelegate OnSessionHistoryDataReceive;

        // === Singleton Instance with Thread Saftey ===
        private static UDPConnection _instance = null;
        private static object _singletonLock = new object();
        public static UDPConnection GetInstance()
        {
            lock (_singletonLock)
            {
                if (_instance == null) { _instance = new UDPConnection(20777); }
                return _instance;
            }
        }

        /// <summary>
        /// Constructs telemetry client and sets it up for receiving data.
        /// </summary>
        /// <param name="port">The port to listen to. This should match the game setting.</param>
        private UDPConnection(int port)
        {
            _client = new UdpClient(port);
            _endPoint = new IPEndPoint(IPAddress.Any, 0);

            _timeoutTimer = new Timer(TIMEOUT_IN_MS);
            _timeoutTimer.AutoReset = false;
            _timeoutTimer.Elapsed += TimeoutEvent;

            // Start receiving updates.
            _client.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        /// <summary>
        /// Called when data is received.
        /// </summary>
        /// <param name="result">Resulting data.</param>
        private void ReceiveCallback(IAsyncResult result)
        {
            // Handle connected event.
            if (Connected == false)
            {
                Connected = true;
                OnConnectStatusChanged?.Invoke(true);
            }

            // Restart the timeout timer.
            _timeoutTimer.Stop();
            _timeoutTimer.Start();

            // Get data we received.
            byte[] data = _client.EndReceive(result, ref _endPoint);

            // Start receiving again.
            _client.BeginReceive(new AsyncCallback(ReceiveCallback), null);

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                // Get the header to retrieve the packet ID.
                PacketHeader header = (PacketHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketHeader));

                // Switch on packet id, and call the correct event.
                // Cast the packet to the correct type based on the ID.
                switch (header.packetId)
                {
                    case PacketTypes.Session:
                        PacketSessionData sessionData = (PacketSessionData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketSessionData));
                        OnSessionDataReceive?.Invoke(sessionData);
                        break;
                    case PacketTypes.LapData:
                        PacketLapData lapData = (PacketLapData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketLapData));
                        OnLapDataReceive?.Invoke(lapData);
                        break;
                    case PacketTypes.Participants:
                        PacketParticipantsData participantsData = (PacketParticipantsData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketParticipantsData));
                        OnParticipantsDataReceive?.Invoke(participantsData);
                        break;
                    case PacketTypes.FinalClassification:
                        PacketFinalClassificationData finalClassificationData = (PacketFinalClassificationData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketFinalClassificationData));
                        OnFinalClassificationDataReceive?.Invoke(finalClassificationData);
                        break;
                    case PacketTypes.LobbyInfo:
                        PacketLobbyInfoData lobbyInfoData = (PacketLobbyInfoData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketLobbyInfoData));
                        OnLobbyInfoDataReceive?.Invoke(lobbyInfoData);
                        break;
                    case PacketTypes.SessionHistory:
                        PacketSessionHistoryData sessionHistoryData = (PacketSessionHistoryData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PacketSessionHistoryData));
                        OnSessionHistoryDataReceive?.Invoke(sessionHistoryData);
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Failed to receive F1 2022 packet.");
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Called when no data is received for a period of time.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Elapsed event arguments.</param>
        private void TimeoutEvent(object sender, ElapsedEventArgs e)
        {
            Connected = false;
            OnConnectStatusChanged?.Invoke(false);
        }
    }
}
