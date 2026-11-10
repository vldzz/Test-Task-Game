namespace Steamworks.NET
{
    public static class SteamMisc
    {
        public static string GetSteamID()
        {
            return SteamUser.GetSteamID().ToString();
        }
    }
}