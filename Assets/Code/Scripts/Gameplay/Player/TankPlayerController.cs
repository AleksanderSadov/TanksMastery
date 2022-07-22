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
