using System;
using FishNet;
using Steamworks;
using Steamworks.NET;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class PlayerDataView : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private Button kickButton;
        private CSteamID _steamID;

        public static event Action<CSteamID> OnKickButtonPressed;

        public string PlayerName => playerNameText.text;
        public CSteamID SteamID => _steamID;

        private void Awake()
        {
            kickButton.gameObject.SetActive(InstanceFinder.IsServerStarted);
        }

        public void SetPlayerName(string playerName)
        {
            playerNameText.SetText(playerName);
        }

        public void SetPlayerSteamID(CSteamID steamID)
        {
            _steamID = steamID;
            if (_steamID == SteamMisc.GetSteamID())
            {
                kickButton.gameObject.SetActive(false);
            }
        }

        public void OnKickButtonPress()
        {
            OnKickButtonPressed?.Invoke(_steamID);
        }
    }
}