using System;
using System.Collections.Generic;
using Network;
using Steamworks;
using Steamworks.NET;
using UnityEngine;
using VContainer;

namespace UI.Views
{
    public class PlayerListView : MonoBehaviour
    {
        private LobbyService _lobbyService;

        [SerializeField] private PlayerDataView playerDataViewPrefab;
        [SerializeField] private Transform contentParent;

        private List<PlayerDataView> playersDataView = new List<PlayerDataView>();

        public event Action<CSteamID> OnPlayerKickPressed;

        [Inject]
        public void Construct(LobbyService lobbyService)
        {
            _lobbyService = lobbyService;
            _lobbyService.OnAddPlayer += AddPlayer;
            _lobbyService.OnRemovePlayer += RemovePlayer;
        }

        private void OnDestroy()
        {
            _lobbyService.OnAddPlayer -= AddPlayer;
            _lobbyService.OnRemovePlayer -= RemovePlayer;
        }

        private void Start()
        {
            PlayerDataView.OnKickButtonPressed += OnPlayerKick;
        }

        private void OnEnable()
        {
            if (LobbyService.LastLobbyID.IsValid())
            {
                UpdatePlayerList(LobbyService.LastLobbyID);
            }
        }

        private void OnPlayerKick(CSteamID steamID)
        {
            OnPlayerKickPressed?.Invoke(steamID);
        }


        private void UpdatePlayerList(CSteamID steamIDLobby)
        {
            ClearList();

            List<CSteamID> currentLobbyPlayersID = SteamMisc.GetCurrentLobbyPlayers(steamIDLobby);
            foreach (CSteamID playerSteamID in currentLobbyPlayersID)
            {
                AddPlayer(playerSteamID);
            }
        }

        private void AddPlayer(CSteamID playerSteamID, string playerName = null)
        {
            if (playerName == null)
            {
                playerName = SteamMisc.GetPersonaName(playerSteamID);
            }

            PlayerDataView dataView = Instantiate(playerDataViewPrefab, contentParent);
            dataView.SetPlayerName(playerName);
            dataView.SetPlayerSteamID(playerSteamID);
            playersDataView.Add(dataView);
        }

        private void RemovePlayer(CSteamID playerSteamID)
        {
            int playerIndex = -1;
            for (int i = 0; i < playersDataView.Count; i++)
            {
                if (playersDataView[i].SteamID == playerSteamID)
                {
                    playerIndex = i;
                    break;
                }
            }

            if (playerIndex == -1)
            {
                Debug.LogError("Can't find player index");
                return;
            }

            Destroy(playersDataView[playerIndex].gameObject);
            playersDataView.RemoveAt(playerIndex);
        }

        private void ClearList()
        {
            for (int i = 0; i < playersDataView.Count; i++)
            {
                Destroy(playersDataView[i].gameObject);
            }

            playersDataView.Clear();
        }
    }
}