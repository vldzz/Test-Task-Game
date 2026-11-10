namespace Network.Transport
{
    public interface INetworkTransport
    {
        void Initialize();
        void StartHost(string name, int maxPlayers = 4, bool isPrivate = true);
        void StartClient(string address);
    }
}