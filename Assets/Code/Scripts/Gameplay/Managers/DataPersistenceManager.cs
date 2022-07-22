using UnityEngine;

namespace Tanks.Gameplay
{
    public class DataPersistenceManager : MonoBehaviour
    {
        public static DataPersistenceManager Instance;

        public GameMode gameMode; 

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
