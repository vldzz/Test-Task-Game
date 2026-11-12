using System.Collections.Generic;

namespace Steamworks.NET
{
    public static class SteamMisc
    {
        public static CSteamID GetSteamID()
        {
            return SteamUser.GetSteamID();
        }
        
        public static string GetSteamIDString()
        {
            return SteamUser.GetSteamID().ToString();
        }

        public static string GetPersonaName(CSteamID playerSteamID)
        {
            return SteamFriends.GetFriendPersonaName(playerSteamID);
        }

        public static List<CSteamID> GetCurrentLobbyPlayers(CSteamID lobbyID)
        {
            List<CSteamID> playerIDs = new List<CSteamID>();

            int playerCount = SteamMatchmaking.GetNumLobbyMembers(lobbyID);
            for (int i = 0; i < playerCount; i++)
            {
                CSteamID playerSteamID = SteamMatchmaking.GetLobbyMemberByIndex(lobbyID, i);
                playerIDs.Add(playerSteamID);
            }

            return playerIDs;
        }
    }
}