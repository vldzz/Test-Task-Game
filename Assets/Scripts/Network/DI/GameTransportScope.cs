using System;
using FishNet.Managing;
using Network.Transport;
using Steamworks;
using UI;
using UI.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Network.DI
{
    public class GameTransportScope : LifetimeScope
    {
        [SerializeField] private NetworkManager networkManager;
        [SerializeField] private MainMenuController mainMenuController;
        [SerializeField] private PlayerListView playerListView;
        [SerializeField] private LobbyManager lobbyManager;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(networkManager);
            builder.RegisterComponent(mainMenuController);
            builder.RegisterComponent(playerListView);
            builder.RegisterComponent(lobbyManager);

            bool isSteamAvailable = false;
            try
            {
                isSteamAvailable = SteamAPI.Init();
                Debug.Log("Initializing Steam API : " + isSteamAvailable);
            }
            catch (Exception e)
            {
                Debug.Log("Steam API initialization failed: " + e);
            }


            if (isSteamAvailable)
            {
                builder.Register<SteamTransport>(Lifetime.Singleton).As<INetworkTransport>();
            }
            else
            {
                builder.Register<IpTransport>(Lifetime.Singleton).As<INetworkTransport>();
            }

            builder.Register<LobbyService>(Lifetime.Singleton);
        }
    }
}