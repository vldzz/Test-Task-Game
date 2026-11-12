using System;
using Steamworks;
using Steamworks.NET;
using UnityEngine;

namespace Network
{
    public class LobbyService
    {
        protected Callback<LobbyChatUpdate_t> lobbyChatUpdateCallback;

        public event Action<CSteamID, string> OnAddPlayer;
        public event Action<CSteamID> OnRemovePlayer;

        private static CSteamID _lastLobbyID;
        public static CSteamID LastLobbyID => _lastLobbyID;

        public LobbyService()
        {
            if (SteamManager.Initialized)
            {
                lobbyChatUpdateCallback = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdated);
            }
        }

        private void OnLobbyChatUpdated(LobbyChatUpdate_t callback)
        {
            CSteamID changedUser = new CSteamID(callback.m_ulSteamIDUserChanged);
            string personaName = SteamMisc.GetPersonaName(changedUser);

            if (((EChatMemberStateChange)callback.m_rgfChatMemberStateChange &
                 EChatMemberStateChange.k_EChatMemberStateChangeEntered) != 0)
            {
                Debug.Log($"Player {personaName} join lobby");
                OnAddPlayer?.Invoke(changedUser, personaName);
            }
            else if (((EChatMemberStateChange)callback.m_rgfChatMemberStateChange &
                      EChatMemberStateChange.k_EChatMemberStateChangeLeft) != 0)
            {
                Debug.Log($"Player {personaName} left");
                OnRemovePlayer?.Invoke(changedUser);
            }
            else if (((EChatMemberStateChange)callback.m_rgfChatMemberStateChange &
                      EChatMemberStateChange.k_EChatMemberStateChangeDisconnected) != 0)
            {
                Debug.Log($"Player {personaName} disconnected");
                OnRemovePlayer?.Invoke(changedUser);
            }
            else if (((EChatMemberStateChange)callback.m_rgfChatMemberStateChange &
                      EChatMemberStateChange.k_EChatMemberStateChangeKicked) != 0)
            {
                Debug.Log($"Player {personaName} was kicked");
                OnRemovePlayer?.Invoke(changedUser);
            }
        }

        public static void SetLastLobbyID(CSteamID lobbyID)
        {
            _lastLobbyID = lobbyID;
        }
    }
}