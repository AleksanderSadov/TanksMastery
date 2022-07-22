using UnityEngine;
using UnityEngine.Events;

namespace Tanks.Gameplay
{
    public class TankShooting : MonoBehaviour
    {
        public int playerNumber = 1;
        public Rigidbody shell;
        public Transform fireTransform;
        public float minLaunchForce = 15f;
        public float maxLaunchForce = 30f;
        public float maxChargeTime = 0.75f;

        public float currentLaunchForce { get; private set; }

        public UnityAction OnStartCharging;
        public UnityAction OnFired;

        private string fireButton;
        private float chargeSpeed;
        private bool fired;

        private void OnEnable()
        {
            currentLaunchForce = minLaunchForce;
        }

        private void Start()
        {
            fireButton = GameConstants.AXIS_NAME_PLAYER_FIRE + playerNumber;
            chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        }

        private void Update()
        {
            if (currentLaunchForce >= maxLaunchForce && !fired)
            {
                currentLaunchForce = maxLaunchForce;
                Fire();
            }
            else if (Input.GetButtonDown(fireButton))
            {
                fired = false;
                currentLaunchForce = minLaunchForce;
                OnStartCharging?.Invoke();
            }
            else if (Input.GetButton(fireButton) && !fired)
            {
                currentLaunchForce += chargeSpeed * Time.deltaTime;
            }
            else if (Input.GetButtonUp(fireButton) && !fired)
            {
                Fire();
            }
        }

        private void Fire()
        {
            fired = true;
            Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;
            shellInstance.velocity = currentLaunchForce * fireTransform.forward;
            currentLaunchForce = minLaunchForce;
            OnFired?.Invoke();
        }
    }
}
