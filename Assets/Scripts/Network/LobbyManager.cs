using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using Steamworks;
using UI.Views;
using UnityEngine;
using VContainer;

namespace Network
{
    public class LobbyManager : NetworkBehaviour
    {
        private PlayerListView _playerListView;
        private NetworkManager _networkManager;


        [Inject]
        public void Construct(PlayerListView playerListView, NetworkManager networkManager)
        {
            _playerListView = playerListView;
            _networkManager = networkManager;
            _playerListView.OnPlayerKickPressed += OnPlayerKickRequest;
        }

        private void OnDestroy()
        {
            _playerListView.OnPlayerKickPressed -= OnPlayerKickRequest;
        }


        private void OnPlayerKickRequest(CSteamID obj)
        {
            OnPlayerKickRequestServer(obj);
        }

        [Server]
        private void OnPlayerKickRequestServer(CSteamID obj)
        {
            Debug.LogError("Kicked Server");
            NetworkConnection networkConnection = SteamConnectionRegistry.GetConnection(obj);
            if (networkConnection != null)
            {
                KickPlayer(networkConnection);
            }
        }

        [TargetRpc]
        private void KickPlayer(NetworkConnection networkConnection)
        {
            Debug.LogError("Kicked target");


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