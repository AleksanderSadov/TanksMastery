using Tanks.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tanks.UI
{
    public class LaunchGameMode : MonoBehaviour
    {
        private const string mainGameSceneName = "MainGame";

        [Range(0, 2)]
        public int numberPlayersInTeamOne = 1;
        [Range(0, 4)]
        public int numberBotsInTeamOne = 0;
        [Range(0, 2)]
        public int numberPlayersInTeamTwo = 0;
        [Range(0, 4)]
        public int numberBotsInTeamTwo = 1;

        public void LaunchMode()
        {
            DataPersistenceManager.Instance.numberPlayersInTeamOne = numberPlayersInTeamOne;
            DataPersistenceManager.Instance.numberPlayersInTeamTwo = numberPlayersInTeamTwo;
            DataPersistenceManager.Instance.numberBotsInTeamOne = numberBotsInTeamOne;
            DataPersistenceManager.Instance.numberBotsInTeamTwo = numberBotsInTeamTwo;

            SceneManager.LoadScene(mainGameSceneName);
        }
    }
}
