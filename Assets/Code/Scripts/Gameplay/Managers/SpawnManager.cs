using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Gameplay
{
    [RequireComponent(typeof(GameManager))]
    [RequireComponent(typeof(TeamManager))]
    public class SpawnManager : MonoBehaviour
    {
        public TankPlayerController tankPlayerPrefab;
        public TankEnemyController tankEnemyPrefab;
        public GameObject teamOneParent;
        public GameObject teamTwoParent;

        private GameMode gameMode;
        private GameManager gameManager;
        private TeamManager teamManager;
        private Dictionary<TeamAffiliation, List<SpawnPoint>> spawnPoints;

        private void Start()
        {
            gameManager = GetComponent<GameManager>();
            teamManager = GetComponent<TeamManager>();

            gameMode = gameManager.gameMode;
            GetSpawnPoints();

            EventManager.AddListener<RoundStartingEvent>(OnRoundStarting);
            EventManager.AddListener<RoundStartedEvent>(OnRoundStarted);
            EventManager.AddListener<RoundEndingEvent>(OnRoundEnding);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<RoundStartingEvent>(OnRoundStarting);
            EventManager.RemoveListener<RoundStartedEvent>(OnRoundStarted);
            EventManager.RemoveListener<RoundEndingEvent>(OnRoundEnding);
        }

        private void OnRoundStarting(RoundStartingEvent evt)
        {
            DestroyExistingTanks();
            SpawnTanks();
            DisableEnemyAI();
            DisablePlayersControls();
        }

        private void OnRoundStarted(RoundStartedEvent evt)
        {
            EnableEnemyAI();
            EnablePlayersControls();
        }

        private void OnRoundEnding(RoundEndingEvent evt)
        {
            DisableEnemyAI();
            DisablePlayersControls();
        }

        private void DestroyExistingTanks()
        {
            foreach (TeamMember participant in teamManager.allParticipants)
            {
                Destroy(participant.gameObject);
            }
        }

        private void SpawnTanks()
        {
            switch (gameMode)
            {
                case GameMode.SOLO_1VS1:
                    SpawnTanksSolo1VS1();
                    break;
            }
        }

        private void SpawnTanksSolo1VS1()
        {
            SpawnPoint spawnPoint = GetRandomFreeSpawnPoint(TeamAffiliation.TEAM_ONE);
            TankPlayerController player = Instantiate(
                tankPlayerPrefab,
                spawnPoint.transform.position,
                tankPlayerPrefab.transform.rotation,
                teamOneParent.transform
            );
            player.playerControlsNumber = PlayerControlsNumber.FIRST;
            player.GetComponent<TeamMember>().teamAffiliation = TeamAffiliation.TEAM_ONE;
            teamManager.AddMemberToTeam(player.GetComponent<TeamMember>());

            spawnPoint = GetRandomFreeSpawnPoint(TeamAffiliation.TEAM_TWO);
            TankEnemyController enemy = Instantiate(
                tankEnemyPrefab,
                spawnPoint.transform.position,
                tankPlayerPrefab.transform.rotation,
                teamTwoParent.transform
            );
            enemy.GetComponent<TeamMember>().teamAffiliation = TeamAffiliation.TEAM_TWO;
            teamManager.AddMemberToTeam(enemy.GetComponent<TeamMember>());
        }

        private void GetSpawnPoints()
        {
            spawnPoints = new Dictionary<TeamAffiliation, List<SpawnPoint>>();
            spawnPoints.Add(TeamAffiliation.TEAM_ONE, new List<SpawnPoint>());
            spawnPoints.Add(TeamAffiliation.TEAM_TWO, new List<SpawnPoint>());

            SpawnPoint[] allSpawnPoints = FindObjectsOfType<SpawnPoint>();
            foreach (SpawnPoint spawnPoint in allSpawnPoints)
            {
                spawnPoints[spawnPoint.teamAffiliation].Add(spawnPoint);
            }
        }

        private SpawnPoint GetRandomFreeSpawnPoint(TeamAffiliation teamAffiliation)
        {
            List<SpawnPoint> freeSpawnPoints = spawnPoints[teamAffiliation].FindAll(
                spawnPoint => spawnPoint.teamAffiliation == teamAffiliation
            );
            int randomIndex = Random.Range(0, freeSpawnPoints.Count);

            return freeSpawnPoints[randomIndex];
        }

        private void EnablePlayersControls()
        {
            foreach (TankPlayerController playerController in teamManager.players)
            {
                playerController.enabled = true;
            }
        }

        private void DisablePlayersControls()
        {
            foreach (TankPlayerController playerController in teamManager.players)
            {
                playerController.enabled = false;
            }
        }

        private void EnableEnemyAI()
        {
            foreach (TankEnemyAI enemyAI in teamManager.bots)
            {
                enemyAI.enabled = true;
            }
        }

        private void DisableEnemyAI()
        {
            foreach (TankEnemyAI enemyAI in teamManager.bots)
            {
                enemyAI.enabled = false;
            }
        }
    }
}
