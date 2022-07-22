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

    [Serializable]
    public class Team
    {
        public TeamAffiliation teamAffiliation;
        public Color teamColor;
        public string teamLabel;
        public int roundsWon;
        public List<TeamMember> members = new List<TeamMember>();

        [HideInInspector] public string coloredTeamText
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
    }

    public class TeamManager : MonoBehaviour
    {
        public List<Team> teams;

        public void AddMemberToTeam(TeamMember teamMember, TeamAffiliation teamAffiliation)
        {
            Team team = GetTeam(teamAffiliation);
            if (team == null)
            {
                return;
            }

            if (!team.members.Contains(teamMember))
            {
                team.members.Add(teamMember);
                team.ColorMemberToTeamColors(teamMember);
            }
        }

        public void RemoveMemberFromTeam(TeamMember teamMember, TeamAffiliation teamAffiliation)
        {
            Team team = GetTeam(teamAffiliation);
            if (team == null)
            {
                return;
            }

            if (team.members.Contains(teamMember))
            {
                team.members.Remove(teamMember);
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

        public List<TeamMember> GetAllParticipants()
        {
            List<TeamMember> allParticipants = new List<TeamMember>();

            foreach (Team team in teams)
            {
                foreach (TeamMember member in team.members)
                {
                    if (!allParticipants.Contains(member))
                    {
                        allParticipants.Add(member);
                    }
                }
            }

            return allParticipants;
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
}
