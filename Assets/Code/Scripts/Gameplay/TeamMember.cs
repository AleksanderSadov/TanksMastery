using UnityEngine;

namespace Tanks.Gameplay
{
    public class TeamMember : MonoBehaviour
    {
        public TEAM_AFFILIATION teamAffiliation;

        private TeamManager teamManager;

        private void Start()
        {
            teamManager = FindObjectOfType<TeamManager>();
            teamManager.AddMemberToTeam(this, teamAffiliation);
        }

        private void OnDestroy()
        {
            if (teamManager)
            {
                teamManager.RemoveMemberFromTeam(this, teamAffiliation);
            }
        }
    }
}