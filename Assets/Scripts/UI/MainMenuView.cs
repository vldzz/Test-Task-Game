using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MainMenuView : MonoBehaviour
    {
        [Header("Panels")] [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject hostConfigPanel;
        [SerializeField] private GameObject lobbyPanel;

        [Header("Lobby Configs")] [SerializeField]
        private TMP_InputField nameInputField;

        [SerializeField] private TMP_Dropdown playerCountDropdown;
        [SerializeField] private TMP_Dropdown privateTypeDropdown;
        [SerializeField] private TMP_Text errorText;

        private readonly List<string> playerCounts = new List<string>() { "2", "3", "4" };
        private readonly List<string> privateType = new List<string>() { "Private", "Friends Only" };


        public event Action<string, int, bool> OnStartHostPress;
        public event Action<string> OnStartClientPress;
        public event Action OnStopConnectionPress;


        #region Button Click Events

        public void OnClickConfigHostButton()
        {
            ShowHostConfigPanel();
        }

        public void OnClickStartHostButton()
        {
            if (!ValidateConfigs())
            {
                return;
            }

            string lobbyName = nameInputField.text;
            int maxPlayers = int.Parse(playerCounts.ElementAt(playerCountDropdown.value));
            bool isPrivate = privateTypeDropdown.value == privateType.IndexOf("Private");

            OnStartHostPress?.Invoke(lobbyName, maxPlayers, isPrivate);
        }

        public void OnClickJoinButton(string address)
        {
            OnStartClientPress?.Invoke(address);
        }

        public void OnClickBackButton()
        {
            ShowMainMenuPanel();
        }

        public void OnClickLeaveButton()
        {
            OnStopConnectionPress?.Invoke();
        }

        #endregion

        private bool ValidateConfigs()
        {
            errorText.SetText("");

            string lobbyName = nameInputField.text;
            if (lobbyName.Length == 0)
            {
                errorText.SetText("Please enter a valid lobby name.");
                return false;
            }

            if (lobbyName.Length >= 20)
            {
                errorText.SetText("Lobby Name is too long.");
                return false;
            }

            return true;
        }

        #region UI View

        public void ShowLobbyPanel()
        {
            mainMenuPanel.SetActive(false);
            lobbyPanel.SetActive(true);
            hostConfigPanel.SetActive(false);
        }

        public void ShowMainMenuPanel()
        {
            mainMenuPanel.SetActive(true);
            lobbyPanel.SetActive(false);
            hostConfigPanel.SetActive(false);
        }


        public void ShowHostConfigPanel()
        {
            nameInputField.text = "";
            errorText.text = "";
            playerCountDropdown.options.Clear();
            playerCountDropdown.AddOptions(playerCounts);

            privateTypeDropdown.options.Clear();
            privateTypeDropdown.AddOptions(privateType);


            mainMenuPanel.SetActive(false);
            lobbyPanel.SetActive(false);
            hostConfigPanel.SetActive(true);
        }

        #endregion
    }
}