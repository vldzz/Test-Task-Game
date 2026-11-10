using System;
using UnityEngine;

namespace Network.Transport
{
    /// <summary>
    /// Uses ip:port for connection
    /// </summary>
    public class IpTransport : INetworkTransport
    {
        public void Initialize()
        {
            Debug.Log("Initializing IP");
        }

        public void StartHost(string name, int maxPlayers = 4, bool isPrivate = true)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }

        public void StartClient(string address)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}