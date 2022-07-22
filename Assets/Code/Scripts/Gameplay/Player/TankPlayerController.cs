using UnityEngine;

namespace Tanks.Gameplay
{
    public enum PlayerControlsNumber
    {
        FIRST = 1,
        SECOND = 2,
    }

    [RequireComponent(typeof(TankPlayerMovement))]
    [RequireComponent(typeof(TankShooting))]
    public class TankPlayerController : MonoBehaviour
    {
        public PlayerControlsNumber playerControlsNumber = PlayerControlsNumber.FIRST;

        private TankPlayerMovement movement;
        private TankShooting shooting;
        private string fireButton;
        private string movementAxisName;
        private string turnAxisName;

        private void Start()
        {
            movement = GetComponent<TankPlayerMovement>();
            shooting = GetComponent<TankShooting>();

            movementAxisName = GameConstants.AXIS_NAME_PLAYER_VERTICAL + (int) playerControlsNumber;
            turnAxisName = GameConstants.AXIS_NAME_PLAYER_HORIZONTAL + (int) playerControlsNumber;
            fireButton = GameConstants.AXIS_NAME_PLAYER_FIRE + (int) playerControlsNumber;
        }

        private void Update()
        {
            HandleMovement();
            HandleShooting();
        }

        public void EnableControls()
        {
            if (movement != null)
            {
                movement.enabled = true;
            }

            if (shooting != null)
            {
                shooting.enabled = true;
            }

            this.enabled = true;
        }

        public void DisableControls()
        {
            if (movement != null)
            {
                movement.enabled = false;
            }

            if (shooting != null)
            {
                shooting.enabled = false;
            }

            this.enabled = false;
        }

        private void HandleMovement()
        {
            movement.movementInputValue = Input.GetAxis(movementAxisName);
            movement.turnInputValue = Input.GetAxis(turnAxisName);
        }

        private void HandleShooting()
        {
            if (Input.GetButtonDown(fireButton))
            {
                shooting.StartCharging();
            }
            else if (Input.GetButton(fireButton) && !shooting.isFired)
            {
                shooting.IncrementCharge();
            }
            else if (Input.GetButtonUp(fireButton) && !shooting.isFired)
            {
                shooting.Fire();
            }
        }
    }
}
