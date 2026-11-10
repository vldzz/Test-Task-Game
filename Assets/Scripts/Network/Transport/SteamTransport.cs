using FishNet.Managing;
using Steamworks;
using UnityEngine;

namespace Network.Transport
{
    /// <summary>
    /// Uses FishySteamworks & Steam P2P
    /// </summary>
    public class SteamTransport : INetworkTransport
    {
        private NetworkManager networkTransport;

        public void Initialize()
        {
            Debug.Log("Initializing Steam P2P");
        }

        public SteamTransport(NetworkManager networkManager)
        {
            this.networkTransport = networkManager;
        }

        public void StartHost(string name, int maxPlayers = 4, bool isPrivate = true)
        {
            if (!SteamManager.Initialized || !SteamAPI.Init())
            {
                Debug.LogError("Steam P2P Transport is not initialized");
                return;
            }

            networkTransport.ServerManager.StartConnection();
            networkTransport.ClientManager.StartConnection();
        }

        public void StartClient(string address)
        {
            if (!SteamManager.Initialized)
            {
                Debug.LogError("Steam P2P Transport is not initialized");
                return;
            }

            FishySteamworks.FishySteamworks transport =
                networkTransport.TransportManager.Transport as FishySteamworks.FishySteamworks;

            if (transport != null)
            {
                transport.SetClientAddress(address);
            }

            networkTransport.ClientManager.StartConnection();
        }
    }
}