using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using Steamworks;

namespace Network
{
    public class SteamConnectionRegistry : NetworkBehaviour
    {
        private static Dictionary<CSteamID, NetworkConnection> _connections =
            new Dictionary<CSteamID, NetworkConnection>();

        public override void OnStartServer()
        {
            base.OnStartServer();
            base.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            base.ServerManager.OnRemoteConnectionState -= OnRemoteConnectionState;
            _connections.Clear();
        }

        private void OnRemoteConnectionState(NetworkConnection connection, RemoteConnectionStateArgs args)
        {
            if (args.ConnectionState == RemoteConnectionState.Started)
            {
                string address = connection.GetAddress();
                if (ulong.TryParse(address, out ulong id))
                {
                    CSteamID steamID = new CSteamID(id);
                    _connections[steamID] = connection;
                }
            }
            else if (args.ConnectionState == RemoteConnectionState.Stopped)
            {
                CSteamID steamID = CSteamID.Nil;
                foreach (var keyValuePair in _connections)
                {
                    if (keyValuePair.Value == connection)
                    {
                        steamID = keyValuePair.Key;
                        break;
                    }
                }

                if (steamID != CSteamID.Nil)
                {
                    _connections.Remove(steamID);
                }
            }
        }

        public static NetworkConnection GetConnection(CSteamID steamID)
        {
            return _connections[steamID];
        }
    }
}