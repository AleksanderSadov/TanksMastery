using UnityEngine;
using static Tanks.Gameplay.GameModeManager;

namespace Tanks.Gameplay
{
    public class DataPersistenceManager : MonoBehaviour
    {
        public static DataPersistenceManager Instance;

        public GameMode currentGameMode; 

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
