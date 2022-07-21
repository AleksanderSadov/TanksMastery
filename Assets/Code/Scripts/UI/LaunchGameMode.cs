using Tanks.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Tanks.Gameplay.GameModeManager;

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
