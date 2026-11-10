using System.Collections;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Steamworks.NET
{
    public class SteamInitWait : MonoBehaviour
    {
        IEnumerator Start()
        {
            while (true)
            {
                if (SteamManager.Initialized)
                {
                    break;
                }

                yield return new WaitForEndOfFrame();
            }

            SceneManager.LoadScene(1);
        }
    }
}