using UnityEngine;

namespace Tanks.Gameplay
{
    public class TeamMember : MonoBehaviour
    {
        public TeamAffiliation teamAffiliation;

        private TeamManager teamManager;
        private TankHealth tankHealth;

        private void Start()
        {
            teamManager = FindObjectOfType<TeamManager>();
            teamManager.AddMemberToTeam(this);

            tankHealth = GetComponent<TankHealth>();
            if (tankHealth)
            {
                tankHealth.OnDeath += TryReplaceBotWithPlayer;
            }
        }

        private void OnDestroy()
        {
            if (teamManager)
            {
                teamManager.RemoveMemberFromTeam(this);
            }

            if (tankHealth)
            {
                tankHealth.OnDeath -= TryReplaceBotWithPlayer;
            }
        }

        private void TryReplaceBotWithPlayer()
        {
            if (TryGetComponent(out TankPlayerController playerController))
            {
                teamManager.TryReplaceBotWithPlayer(this);
            }
        }
    }
}