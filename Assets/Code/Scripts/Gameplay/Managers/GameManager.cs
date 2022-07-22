using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Tanks.Gameplay
{
    public enum GameMode
    {
        SOLO_1VS1,
        TEAM_2VS2,
        PVP_1VS1,
    }

    public class GameManager : MonoBehaviour
    {
        const float MAX_DEPENETRATION_VELOCITY = float.PositiveInfinity;

        public int numberRoundsToWin = 5;
        public float startDelay = 3f;
        public float endDelay = 3f;
        public CameraControl cameraControl;
        public GameObject playerTankPrefab;
        public GameMode gameMode = GameMode.SOLO_1VS1;

        public Team roundWinnerTeam { get; private set; }
        public Team gameWinnerTeam { get; private set; }
        public int roundNumber { get; private set; }

        private WaitForSeconds startWait;
        private WaitForSeconds endWait;
        private TeamManager teamManager;

        private void Start()
        {
            if (DataPersistenceManager.Instance != null)
            {
                gameMode = DataPersistenceManager.Instance.gameMode;
            }

            Physics.defaultMaxDepenetrationVelocity = MAX_DEPENETRATION_VELOCITY;

            teamManager = FindObjectOfType<TeamManager>();

            startWait = new WaitForSeconds(startDelay);
            endWait = new WaitForSeconds(endDelay);

            StartCoroutine(GameLoop());
        }

        private IEnumerator GameLoop()
        {
            yield return StartCoroutine(RoundStarting());
            yield return StartCoroutine(RoundPlaying());
            yield return StartCoroutine(RoundEnding());

            if (gameWinnerTeam != null)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                StartCoroutine(GameLoop());
            }
        }

        private IEnumerator RoundStarting()
        {
            roundNumber++;
            RoundStartingEvent roundStartingEvent = Events.RoundStartingEvent;
            roundStartingEvent.roundNumber = roundNumber;
            EventManager.Broadcast(Events.RoundStartingEvent);

            yield return startWait;
        }

        private IEnumerator RoundPlaying()
        {
            EventManager.Broadcast(Events.RoundStartedEvent);

            while (!IsRoundOver())
            {
                yield return null;
            }
        }

        private IEnumerator RoundEnding()
        {
            roundWinnerTeam = GetRoundWinnerTeam();
            if (roundWinnerTeam != null)
            {
                roundWinnerTeam.roundsWon++;
            }
            gameWinnerTeam = GetGameWinnerTeam();

            RoundEndingEvent roundEndingEvent = Events.RoundEndingEvent;
            roundEndingEvent.teams = teamManager.teams;
            roundEndingEvent.roundWinnerTeam = roundWinnerTeam;
            roundEndingEvent.gameWinnerTeam = gameWinnerTeam;
            EventManager.Broadcast(Events.RoundEndingEvent);

            yield return endWait;
        }

        private bool IsRoundOver()
        {
            List<TeamMember> allParticipants = teamManager.allParticipants;

            int numberParticipantsAlive = 0;
            foreach (TeamMember participant in allParticipants)
            {
                if (participant.gameObject.activeSelf)
                {
                    numberParticipantsAlive++;
                }
            }

            return numberParticipantsAlive <= 1;
        }

        private Team GetRoundWinnerTeam()
        {
            List<TeamMember> allParticipants = teamManager.allParticipants;

            foreach (TeamMember participant in allParticipants)
            {
                if (participant.gameObject.activeSelf)
                {
                    return teamManager.GetTeam(participant.teamAffiliation);
                }
            }

            return null;
        }

        private Team GetGameWinnerTeam()
        {
            List<Team> teams = teamManager.teams;

            foreach (Team team in teams)
            {
                if (team.roundsWon >= numberRoundsToWin)
                {
                    return team;
                }
            }

            return null;
        }
    }
}
