using Tanks.Gameplay;
using UnityEngine;

namespace Tanks.AI
{
    [RequireComponent(typeof(TeamMember))]
    public class FindTargetSensor : MonoBehaviour
    {
        private TeamManager teamManager;
        private TeamMember teamMember;

        private void Start()
        {
            teamManager = FindObjectOfType<TeamManager>();
            teamMember = GetComponent<TeamMember>();
        }

        public GameObject FindRandomTarget()
        {
            Team rivalTeam = teamManager.GetRivalTeam(teamMember.teamAffiliation);

            if (rivalTeam != null && rivalTeam.members.Count > 0)
            {
                int randomIndex = Random.Range(0, rivalTeam.members.Count);
                return rivalTeam.members[randomIndex].gameObject;
            }

            return null;
        }
    }
}
