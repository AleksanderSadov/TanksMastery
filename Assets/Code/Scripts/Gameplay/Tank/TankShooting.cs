using UnityEngine;
using UnityEngine.Events;

namespace Tanks.Gameplay
{
    public class TankShooting : MonoBehaviour
    {
        public Rigidbody shell;
        public Transform fireTransform;
        public float minLaunchForce = 15f;
        public float maxLaunchForce = 30f;
        public float maxChargeTime = 0.75f;

        [HideInInspector] public float currentLaunchForce;
        [HideInInspector] public bool isFired;
        [HideInInspector] public float chargeSpeed;

        public UnityAction OnStartCharging;
        public UnityAction OnFired;

        private void OnEnable()
        {
            currentLaunchForce = minLaunchForce;
        }

        private void Start()
        {
            chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        }

        private void Update()
        {
            CheckFireAtMaxForce();
        }

        public void StartCharging()
        {
            isFired = false;
            currentLaunchForce = minLaunchForce;
            OnStartCharging?.Invoke();
        }

        public void IncrementCharge()
        {
            currentLaunchForce += chargeSpeed * Time.deltaTime;
        }

        public void Fire()
        {
            isFired = true;
            Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;
            shellInstance.velocity = currentLaunchForce * fireTransform.forward;
            currentLaunchForce = minLaunchForce;
            OnFired?.Invoke();
        }

        private void CheckFireAtMaxForce()
        {
            if (currentLaunchForce >= maxLaunchForce && !isFired)
            {
                currentLaunchForce = maxLaunchForce;
                Fire();
            }
        }
    }
}
