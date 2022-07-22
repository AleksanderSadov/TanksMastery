using System;
using UnityEngine;
using static Tanks.Gameplay.TankPlayerController;

namespace Tanks.Gameplay
{
    [Serializable]
    public class TankManager
    {
        public PlayerControlsNumber playerControlsNumber;
        public TeamAffiliation teamAffiliation;
        public Transform spawnPoint;

        [HideInInspector] public GameObject instance;

        private TankMovement movement;
        private TankShooting shooting;
        private TankPlayerController playerController;
        private GameObject canvasGameObject;

        public void Setup()
        {
            movement = instance.GetComponent<TankMovement>();
            shooting = instance.GetComponent<TankShooting>();
            playerController = instance.GetComponent<TankPlayerController>();
            canvasGameObject = instance.GetComponentInChildren<Canvas>().gameObject;
            playerController.playerControlsNumber = playerControlsNumber;
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
