using Tanks.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Tanks.Shared.GameConstants;

namespace Tanks.UI
{
    public class LaunchGameMode : MonoBehaviour
    {
        private const string mainGameSceneName = "MainGame";

        public GameMode gameMode;

        public void LaunchMode()
        {
            DataPersistenceManager.Instance.currentGameMode = gameMode;
            SceneManager.LoadScene(mainGameSceneName);
        }
    }
}
