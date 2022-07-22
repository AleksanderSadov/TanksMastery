using System;
using UnityEngine;

namespace Tanks.Gameplay
{
    [Serializable]
    public class TankManager
    {
        public enum PlayerControlsNumber
        {
            FIRST = 1,
            SECOND = 2,
        }

        public PlayerControlsNumber playerControlsNumber;
        public TEAM_AFFILIATION teamAffiliation;
        public Transform spawnPoint;

        [HideInInspector] public GameObject instance;

        private TankMovement movement;
        private TankShooting shooting;
        private GameObject canvasGameObject;

        public void Setup()
        {
            movement = instance.GetComponent<TankMovement>();
            shooting = instance.GetComponent<TankShooting>();
            canvasGameObject = instance.GetComponentInChildren<Canvas>().gameObject;

            movement.playerNumber = (int) playerControlsNumber;
            shooting.playerNumber = (int) playerControlsNumber;
        }

        public void DisableControl()
        {
            movement.enabled = false;
            shooting.enabled = false;

            canvasGameObject.SetActive(false);
        }

        public void EnableControl()
        {
            movement.enabled = true;
            shooting.enabled = true;

            canvasGameObject.SetActive(true);
        }

        public void Reset()
        {
            instance.transform.position = spawnPoint.position;
            instance.transform.rotation = spawnPoint.rotation;

            instance.SetActive(false);
            instance.SetActive(true);
        }
    }
}
