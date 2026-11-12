using FishNet.Managing;
using FishNet.Transporting;
using Network;
using Network.Transport;
using Steamworks;
using UI.Views;
using UnityEngine;
using VContainer;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private MainMenuView mainMenuView;
        private INetworkTransport _networkTransport;
        private NetworkManager _networkManager;


        [Inject]
        public void Construct(INetworkTransport networkTransport, NetworkManager networkManager)
        {
            Debug.LogError("Controller Construct");
            _networkTransport = networkTransport;
            _networkManager = networkManager;

            _networkManager.ClientManager.OnClientConnectionState += OnClientConnectionStateChanged;
            networkTransport.Initialize();

            mainMenuView.ShowMainMenuPanel();

            mainMenuView.OnStartHostPress += StartHost;
            mainMenuView.OnStartClientPress += StartClient;
            mainMenuView.OnStopConnectionPress += StopConnection;
        }

        private void OnDestroy()
        {
            if (_networkManager != null && _networkTransport != null)
            {
                _networkManager.ClientManager.OnClientConnectionState -= OnClientConnectionStateChanged;
            }

            mainMenuView.OnStartHostPress -= StartHost;
            mainMenuView.OnStartClientPress -= StartClient;
            mainMenuView.OnStopConnectionPress -= StopConnection;
        }

        private void OnClientConnectionStateChanged(ClientConnectionStateArgs args)
        {
            Debug.LogError("ARGS: " + args);
            if (args.ConnectionState == LocalConnectionState.Started)
            {
                mainMenuView.ShowLobbyPanel();
            }
            else if (args.ConnectionState == LocalConnectionState.Stopped)
            {
                mainMenuView.ShowMainMenuPanel();
            }
        }

        public void StartHost(string lobbyName, int maxPlayers, bool isPrivate)
        {
            _networkTransport.StartHost(lobbyName, maxPlayers, isPrivate);
        }

        public void StartClient(string address)
        {
            _networkTransport.StartClient(address);
        }

        public void StopConnection()
        {
            if (LobbyService.LastLobbyID.IsValid())
            {
                SteamMatchmaking.LeaveLobby(LobbyService.LastLobbyID);
                LobbyService.SetLastLobbyID(CSteamID.Nil);
            }

            if (_networkManager.IsServerStarted)
            {
                _networkManager.ServerManager.StopConnection(true);
            }
            else if (_networkManager.IsClientStarted)
            {
                _networkManager.ClientManager.StopConnection();
            }
        }
    }
}