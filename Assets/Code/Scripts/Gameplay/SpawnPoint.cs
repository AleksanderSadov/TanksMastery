using UnityEngine;

namespace Tanks.Gameplay
{
    public class SpawnPoint : MonoBehaviour
    {
        public TeamAffiliation teamAffiliation = TeamAffiliation.TEAM_ONE;
        public bool isOccupied = false;
    }
}
