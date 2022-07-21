using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;
using Tanks.Shared;

namespace Tanks.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        const float MAX_DEPENETRATION_VELOCITY = float.PositiveInfinity;

        public int numberRoundsToWin = 5;
        public float startDelay = 3f;
        public float endDelay = 3f;
        public CameraControl cameraControl;
        public TextMeshProUGUI messageText;
        public GameObject tankPrefab;
        public TankManager[] tanks;

        public TankManager roundWinner { get; private set; }
        public TankManager gameWinner { get; private set; }
        public int roundNumber { get; private set; }

        private WaitForSeconds startWait;
        private WaitForSeconds endWait;

        private void Start()
        {
            Physics.defaultMaxDepenetrationVelocity = MAX_DEPENETRATION_VELOCITY;



            startWait = new WaitForSeconds(startDelay);
            endWait = new WaitForSeconds(endDelay);

            SpawnAllTanks();
            SetCameraTargets();

            StartCoroutine(GameLoop());
        }

        private void SpawnAllTanks()
        {
            for (int i = 0; i < tanks.Length; i++)
            {
                tanks[i].instance = Instantiate(
                    tankPrefab,
                    tanks[i].spawnPoint.position,
                    tanks[i].spawnPoint.rotation
                ) as GameObject;
                tanks[i].playerNumber = i + 1;
                tanks[i].Setup();
            }
        }

        private void SetCameraTargets()
        {
            Transform[] targets = new Transform[tanks.Length];

            for (int i = 0; i < targets.Length; i++)
            {
                targets[i] = tanks[i].instance.transform;
            }

            cameraControl.targets = targets;
        }

        private IEnumerator GameLoop()
        {
            yield return StartCoroutine(RoundStarting());
            yield return StartCoroutine(RoundPlaying());
            yield return StartCoroutine(RoundEnding());

            if (gameWinner != null)
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
            ResetAllTanks();
            DisableTankControl();
            cameraControl.SetStartPositionAndSize();

            roundNumber++;
            RoundStartingEvent roundStartingEvent = Events.RoundStartingEvent;
            roundStartingEvent.roundNumber = roundNumber;
            EventManager.Broadcast(Events.RoundStartingEvent);

            yield return startWait;
        }

        private IEnumerator RoundPlaying()
        {
            EnableTankControl();

            messageText.text = string.Empty;

            while (!OneTankLeft())
            {
                yield return null;
            }
        }

        private IEnumerator RoundEnding()
        {
            DisableTankControl();

            roundWinner = GetRoundWinner();
            if (roundWinner != null)
            {
                roundWinner.wins++;
            }

            gameWinner = GetGameWinner();

            EventManager.Broadcast(Events.RoundEndingEvent);

            yield return endWait;
        }

        private bool OneTankLeft()
        {
            int numberTanksLeft = 0;

            for (int i = 0; i < tanks.Length; i++)
            {
                if (tanks[i].instance.activeSelf)
                {
                    numberTanksLeft++;
                }
            }

            return numberTanksLeft <= 1;
        }

        private TankManager GetRoundWinner()
        {
            for (int i = 0; i < tanks.Length; i++)
            {
                if (tanks[i].instance.activeSelf)
                {
                    return tanks[i];
                }
            }

            return null;
        }

        private TankManager GetGameWinner()
        {
            for (int i = 0; i < tanks.Length; i++)
            {
                if (tanks[i].wins == numberRoundsToWin)
                {
                    return tanks[i];
                }
            }

            return null;
        }

        private void ResetAllTanks()
        {
            for (int i = 0; i < tanks.Length; i++)
            {
                tanks[i].Reset();
            }
        }

        private void EnableTankControl()
        {
            for (int i = 0; i < tanks.Length; i++)
            {
                tanks[i].EnableControl();
            }
        }

        private void DisableTankControl()
        {
            for (int i = 0; i < tanks.Length; i++)
            {
                tanks[i].DisableControl();
            }
        }
    }
}
