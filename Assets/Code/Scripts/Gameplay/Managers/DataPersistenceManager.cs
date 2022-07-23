using UnityEngine;

namespace Tanks.Gameplay
{
    public class DataPersistenceManager : MonoBehaviour
    {
        public static DataPersistenceManager Instance;

        public int numberPlayersInTeamOne = 1;
        public int numberPlayersInTeamTwo = 0;
        public int numberBotsInTeamOne = 1;
        public int numberBotsInTeamTwo = 0;

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
