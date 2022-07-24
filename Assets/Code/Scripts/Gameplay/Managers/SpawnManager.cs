using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Gameplay
{
    [RequireComponent(typeof(TeamManager))]
    public class SpawnManager : MonoBehaviour
    {
        public TankPlayerController tankPlayerPrefab;
        public TankEnemyController tankEnemyPrefab;
        public GameObject teamOneParent;
        public GameObject teamTwoParent;
        public int numberPlayersInTeamOne = 1;
        public int numberPlayersInTeamTwo = 0;
        public int numberBotsInTeamOne = 0;
        public int numberBotsInTeamTwo = 1;

        private TeamManager teamManager;
        private List<SpawnPoint> spawnPoints;

        private void Awake()
        {
            teamManager = GetComponent<TeamManager>();

            if (DataPersistenceManager.Instance != null)
            {
                numberPlayersInTeamOne = DataPersistenceManager.Instance.numberPlayersInTeamOne;
                numberPlayersInTeamTwo = DataPersistenceManager.Instance.numberPlayersInTeamTwo;
                numberBotsInTeamOne = DataPersistenceManager.Instance.numberBotsInTeamOne;
                numberBotsInTeamTwo = DataPersistenceManager.Instance.numberBotsInTeamTwo;
            }

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
            ResetSpawnPoints();

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

        private void ResetSpawnPoints()
        {
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                spawnPoint.isOccupied = false;
            }
        }

        private void SpawnTanks()
        {
            if (numberPlayersInTeamOne >= 1)
            {
                SpawnPlayer(TeamAffiliation.TEAM_ONE, PlayerControlsNumber.FIRST);
            }

            if (numberPlayersInTeamOne >= 2)
            {
                SpawnPlayer(TeamAffiliation.TEAM_ONE, PlayerControlsNumber.SECOND);
            }

            if (numberPlayersInTeamTwo >= 1)
            {
                SpawnPlayer(TeamAffiliation.TEAM_TWO, PlayerControlsNumber.SECOND);
            }

            if (numberPlayersInTeamTwo >= 2)
            {
                SpawnPlayer(TeamAffiliation.TEAM_TWO, PlayerControlsNumber.SECOND);
            }

            if (numberBotsInTeamOne > 0)
            {
                for (int i = 0; i < numberBotsInTeamOne; i++)
                {
                    SpawnBot(TeamAffiliation.TEAM_ONE);
                }
            }

            if (numberBotsInTeamTwo > 0)
            {
                for (int i = 0; i < numberBotsInTeamTwo; i++)
                {
                    SpawnBot(TeamAffiliation.TEAM_TWO);
                }
            }
        }

        private void SpawnPlayer(TeamAffiliation teamAffiliation, PlayerControlsNumber playerControlsNumber)
        {
            SpawnPoint spawnPoint = GetRandomFreeSpawnPoint(teamAffiliation);
            TankPlayerController player = Instantiate(
                tankPlayerPrefab,
                spawnPoint.transform.position,
                spawnPoint.transform.rotation,
                GetHierarchySpawnParent(teamAffiliation).transform
            );
            player.playerControlsNumber = playerControlsNumber;
            player.GetComponent<TeamMember>().teamAffiliation = teamAffiliation;
            teamManager.AddMemberToTeam(player.GetComponent<TeamMember>());
            spawnPoint.isOccupied = true;
        }

        private void SpawnBot(TeamAffiliation teamAffiliation)
        {
            SpawnPoint spawnPoint = GetRandomFreeSpawnPoint(teamAffiliation);
            TankEnemyController enemy = Instantiate(
                tankEnemyPrefab,
                spawnPoint.transform.position,
                spawnPoint.transform.rotation,
                GetHierarchySpawnParent(teamAffiliation).transform
            );
            enemy.GetComponent<TeamMember>().teamAffiliation = teamAffiliation;
            teamManager.AddMemberToTeam(enemy.GetComponent<TeamMember>());
            spawnPoint.isOccupied = true;
        }

        private GameObject GetHierarchySpawnParent(TeamAffiliation teamAffiliation)
        {
            GameObject spawnParent = null;

            switch (teamAffiliation)
            {
                case TeamAffiliation.TEAM_ONE:
                    spawnParent = teamOneParent;
                    break;
                case TeamAffiliation.TEAM_TWO:
                    spawnParent = teamTwoParent;
                    break;
            }

            return spawnParent;
        }

        private void GetSpawnPoints()
        {
            spawnPoints = new List<SpawnPoint>();

            SpawnPoint[] allSpawnPoints = FindObjectsOfType<SpawnPoint>();
            foreach (SpawnPoint spawnPoint in allSpawnPoints)
            {
                spawnPoints.Add(spawnPoint);
            }
        }

        private SpawnPoint GetRandomFreeSpawnPoint(TeamAffiliation teamAffiliation)
        {
            List<SpawnPoint> freeSpawnPoints = spawnPoints.FindAll(
                spawnPoint => !spawnPoint.isOccupied && spawnPoint.teamAffiliation == teamAffiliation
            );
            int randomIndex = Random.Range(0, freeSpawnPoints.Count);

            return freeSpawnPoints[randomIndex];
        }

        private void EnablePlayersControls()
        {
            foreach (TankPlayerController playerController in teamManager.players)
            {
                playerController.EnableControls();
            }
        }

        private void DisablePlayersControls()
        {
            foreach (TankPlayerController playerController in teamManager.players)
            {
                playerController.DisableControls();
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
                enemyAI.GetComponent<TankEnemyController>().ResetDestination();
            }
        }
    }
}
