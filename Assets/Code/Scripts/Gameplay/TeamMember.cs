using UnityEngine;

namespace Tanks.Gameplay
{
    public class TeamMember : MonoBehaviour
    {
        public TeamAffiliation teamAffiliation;

        private TeamManager teamManager;

        private void Start()
        {
            teamManager = FindObjectOfType<TeamManager>();
            teamManager.AddMemberToTeam(this);
        }

        private void OnDestroy()
        {
            if (teamManager)
            {
                teamManager.RemoveMemberFromTeam(this);
            }
        }
    }
}