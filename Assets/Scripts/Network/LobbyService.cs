using System;
using EIS.Runtime.UI.Notification;
using Steamworks;
using Steamworks.NET;

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
                OnAddPlayer?.Invoke(changedUser, personaName);
                Notifier.Builder
                    .CreateNew()
                    .SetIcon("success")
                    .SetNotificationType(Notifier.NotificationType.LEFT_PANEL)
                    .SetText($"Player {personaName} join lobby")
                    .ShowNotification(4);
            }
            else if (((EChatMemberStateChange)callback.m_rgfChatMemberStateChange &
                      EChatMemberStateChange.k_EChatMemberStateChangeLeft) != 0)
            {
                OnRemovePlayer?.Invoke(changedUser);
                ShowFailNotification(personaName, "left");
            }
            else if (((EChatMemberStateChange)callback.m_rgfChatMemberStateChange &
                      EChatMemberStateChange.k_EChatMemberStateChangeDisconnected) != 0)
            {
                OnRemovePlayer?.Invoke(changedUser);
                ShowFailNotification(personaName, "disconnected");
            }
            else if (((EChatMemberStateChange)callback.m_rgfChatMemberStateChange &
                      EChatMemberStateChange.k_EChatMemberStateChangeKicked) != 0)
            {
                OnRemovePlayer?.Invoke(changedUser);
                ShowFailNotification(personaName, "was kicked");
            }
        }

        private void ShowFailNotification(string personaName, string suffix)
        {
            Notifier.Builder
                .CreateNew()
                .SetIcon("fail")
                .SetNotificationType(Notifier.NotificationType.LEFT_PANEL)
                .SetText($"Player {personaName} {suffix}")
                .ShowNotification(4);
        }

        public static void SetLastLobbyID(CSteamID lobbyID)
        {
            _lastLobbyID = lobbyID;
        }
    }
}