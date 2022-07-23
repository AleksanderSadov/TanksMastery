using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Gameplay
{
    public enum TeamAffiliation
    {
        TEAM_ONE,
        TEAM_TWO,
    }

    public class TeamManager : MonoBehaviour
    {
        public List<Team> teams;
        public List<TeamMember> allParticipants = new List<TeamMember>();
        public List<TankPlayerController> players = new List<TankPlayerController>();
        public List<TankEnemyAI> bots = new List<TankEnemyAI>();

        public void AddMemberToTeam(TeamMember teamMember)
        {
            Team team = GetTeam(teamMember.teamAffiliation);
            if (team == null)
            {
                return;
            }

            if (!team.members.Contains(teamMember))
            {
                team.members.Add(teamMember);
                team.ColorMemberToTeamColors(teamMember);

                if (!allParticipants.Contains(teamMember))
                {
                    allParticipants.Add(teamMember);
                }

                if (teamMember.TryGetComponent(out TankPlayerController playerController))
                {
                    if (!players.Contains(playerController))
                    {
                        players.Add(playerController);
                    }
                }

                if (teamMember.TryGetComponent(out TankEnemyAI enemyAI))
                {
                    if (!bots.Contains(enemyAI))
                    {
                        bots.Add(enemyAI);
                    }
                }
            }
        }

        public void RemoveMemberFromTeam(TeamMember teamMember)
        {
            Team team = GetTeam(teamMember.teamAffiliation);
            if (team == null)
            {
                return;
            }

            if (team.members.Contains(teamMember))
            {
                team.members.Remove(teamMember);
            }

            if (allParticipants.Contains(teamMember))
            {
                allParticipants.Remove(teamMember);
            }

            if (teamMember.TryGetComponent(out TankPlayerController playerController))
            {
                if (players.Contains(playerController))
                {
                    players.Remove(playerController);
                }
            }

            if (teamMember.TryGetComponent(out TankEnemyAI enemyAI))
            {
                if (bots.Contains(enemyAI))
                {
                    bots.Remove(enemyAI);
                }
            }
        }

        public Team GetTeam(TeamAffiliation teamAffiliation)
        {
            foreach (Team team in teams)
            {
                if (team.teamAffiliation == teamAffiliation)
                {
                    return team;
                }
            }

            return null;
        }

        public Team GetRivalTeam(TeamAffiliation allyTeamAffiliation)
        {
            foreach (Team team in teams)
            {
                if (team.teamAffiliation != allyTeamAffiliation)
                {
                    return team;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class Team
    {
        public TeamAffiliation teamAffiliation;
        public Color teamColor;
        public string teamLabel;
        public int roundsWon;
        public List<TeamMember> members = new List<TeamMember>();

        [HideInInspector]
        public string coloredTeamText
        {
            get
            {
                return "<color=#" + ColorUtility.ToHtmlStringRGB(teamColor) + ">TEAM " + teamLabel + "</color>";
            }
            private set
            {

            }
        }

        public void ColorMemberToTeamColors(TeamMember member)
        {
            MeshRenderer[] renderers = member.GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = teamColor;
            }
        }

        public bool isAllMembersDead()
        {
            int numberMembersAlive = 0;
            foreach (TeamMember member in members)
            {
                if (member.gameObject.activeSelf)
                {
                    numberMembersAlive++;
                }
            }

            return numberMembersAlive == 0;
        }
    }
}
