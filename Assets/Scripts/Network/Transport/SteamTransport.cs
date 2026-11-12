using FishNet.Managing;
using Steamworks;
using Steamworks.NET;
using UnityEngine;
using Constants = Utils.Constants;

namespace Network.Transport
{
    /// <summary>
    /// Uses FishySteamworks & Steam P2P
    /// </summary>
    public class SteamTransport : INetworkTransport
    {
        private NetworkManager networkTransport;

        protected Callback<LobbyCreated_t> lobbyCreatedCallback;
        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequestedCallback;
        protected Callback<LobbyEnter_t> lobbyEnterCallback;

        public void Initialize()
        {
            Debug.Log("Initializing Steam P2P");
        }

        public SteamTransport(NetworkManager networkManager)
        {
            this.networkTransport = networkManager;

            if (SteamManager.Initialized)
            {
                lobbyCreatedCallback = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
                gameLobbyJoinRequestedCallback = Callback<GameLobbyJoinRequested_t>.Create(OnGameJoinRequested);
                lobbyEnterCallback = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            }
        }

        public void StartHost(string name, int maxPlayers = 4, bool isPrivate = true)
        {
            if (!SteamManager.Initialized || !SteamAPI.Init())
            {
                Debug.LogError("Steam P2P Transport is not initialized");
                return;
            }

            ELobbyType lobbyType = isPrivate
                ? ELobbyType.k_ELobbyTypePrivate
                : ELobbyType.k_ELobbyTypeFriendsOnly;

            SteamMatchmaking.CreateLobby(lobbyType, maxPlayers);
            Debug.Log("Creating Lobby");
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

        /// <summary>
        /// Setup Lobby Data & Start transport
        /// </summary>
        /// <param name="callback"></param>
        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                Debug.LogError("Failed to create lobby : " + callback.m_eResult.ToString());
                return;
            }

            CSteamID lobbyID = new CSteamID(callback.m_ulSteamIDLobby);
            LobbyService.SetLastLobbyID(lobbyID);
            string steamID = SteamMisc.GetSteamIDString();
            string lobbyName = SteamFriends.GetPersonaName() + "'s Lobby";

            SteamMatchmaking.SetLobbyData(lobbyID, Constants.HOST_ADDRESS_STR, steamID);
            SteamMatchmaking.SetLobbyData(lobbyID, Constants.NAME, lobbyName);

            FishySteamworks.FishySteamworks transport =
                networkTransport.TransportManager.Transport as FishySteamworks.FishySteamworks;

            if (transport != null)
            {
                transport.SetClientAddress(steamID);
            }

            networkTransport.ServerManager.StartConnection();
            networkTransport.ClientManager.StartConnection();

            Debug.Log("Steam Lobby Created");
        }

        private void OnGameJoinRequested(GameLobbyJoinRequested_t callback)
        {
            Debug.Log("Game Join Requested");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            CSteamID lobbyID = new CSteamID(callback.m_ulSteamIDLobby);
            LobbyService.SetLastLobbyID(lobbyID);
            string hostSteamID = SteamMatchmaking.GetLobbyData(lobbyID, Constants.HOST_ADDRESS_STR);

            if (string.IsNullOrEmpty(hostSteamID))
            {
                Debug.LogError("Fail to connect to lobby: missing lobby ID");
                return;
            }

            Debug.Log("Successfully connected to lobby!");

            StartClient(hostSteamID);
        }
    }
}