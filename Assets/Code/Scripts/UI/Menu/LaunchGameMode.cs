using Tanks.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.UI
{
    public class LaunchGameMode : MonoBehaviour
    {
        private const string mainGameSceneName = "MainGame";

        public GameMode gameMode;

        public void LaunchMode()
        {
            DataPersistenceManager.Instance.gameMode = gameMode;
            SceneManager.LoadScene(mainGameSceneName);
        }
    }
}
