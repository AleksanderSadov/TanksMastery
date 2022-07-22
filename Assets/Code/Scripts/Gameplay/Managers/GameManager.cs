using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Tanks.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        const float MAX_DEPENETRATION_VELOCITY = float.PositiveInfinity;

        public int numberRoundsToWin = 5;
        public float startDelay = 3f;
        public float endDelay = 3f;
        public CameraControl cameraControl;
        public GameObject playerTankPrefab;
        public TankManager[] players;

        public Team roundWinnerTeam { get; private set; }
        public Team gameWinnerTeam { get; private set; }
        public int roundNumber { get; private set; }

        private WaitForSeconds startWait;
        private WaitForSeconds endWait;
        private TeamManager teamManager;

        private void Start()
        {
            Physics.defaultMaxDepenetrationVelocity = MAX_DEPENETRATION_VELOCITY;

            teamManager = FindObjectOfType<TeamManager>();

            startWait = new WaitForSeconds(startDelay);
            endWait = new WaitForSeconds(endDelay);

            SpawnPlayersTanks();
            SetCameraTargets();

            StartCoroutine(GameLoop());
        }

        private void SpawnPlayersTanks()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].instance = Instantiate(
                    playerTankPrefab,
                    players[i].spawnPoint.position,
                    players[i].spawnPoint.rotation
                ) as GameObject;

                players[i].instance.GetComponent<TeamMember>().teamAffiliation =
                    i == 0 ? TeamAffiliation.TEAM_ONE : TeamAffiliation.TEAM_TWO;
                players[i].Setup();
            }
        }

        private void SetCameraTargets()
        {
            Transform[] targets = new Transform[players.Length];

            for (int i = 0; i < targets.Length; i++)
            {
                targets[i] = players[i].instance.transform;
            }

            cameraControl.targets = targets;
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

            ResetAllTanks();
            DisableTankControl();
            cameraControl.SetStartPositionAndSize();

            yield return startWait;
        }

        private IEnumerator RoundPlaying()
        {
            EventManager.Broadcast(Events.RoundStartedEvent);

            EnableTankControl();

            while (!IsRoundOver())
            {
                yield return null;
            }
        }

        private IEnumerator RoundEnding()
        {
            DisableTankControl();

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
            List<TeamMember> allParticipants = teamManager.GetAllParticipants();

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
            List<TeamMember> allParticipants = teamManager.GetAllParticipants();

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

        private void ResetAllTanks()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].Reset();
            }
        }

        private void EnableTankControl()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].EnableControl();
            }
        }

        private void DisableTankControl()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].DisableControl();
            }
        }
    }
}
