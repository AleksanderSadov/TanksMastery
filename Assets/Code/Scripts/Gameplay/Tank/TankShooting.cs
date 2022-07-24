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
        public float reloadTime = 0.75f;

        [HideInInspector] public float currentLaunchForce;
        [HideInInspector] public float targetLaunchForce;
        [HideInInspector] public bool isFired = false;
        [HideInInspector] public bool isCharging = false;
        [HideInInspector] public bool isReloading = false;
        [HideInInspector] public float chargeSpeed;
        [HideInInspector] public float lastTimeFired = 0;

        public UnityAction OnStartCharging;
        public UnityAction OnFired;

        private void OnEnable()
        {
            currentLaunchForce = minLaunchForce;
        }

        private void Start()
        {
            targetLaunchForce = maxLaunchForce;
            chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        }

        private void Update()
        {
            if (isCharging)
            {
                CheckFireAtTargetLaunchForce();
            }

            if (lastTimeFired > 0 && Time.time - lastTimeFired < reloadTime)
            {
                isReloading = true;
            }
            else
            {
                isReloading = false;
            }
        }

        public void StartCharging()
        {
            if (isReloading)
            {
                return;
            }

            isFired = false;
            isCharging = true;
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
            isCharging = false;
            lastTimeFired = Time.time;
            Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;
            shellInstance.velocity = currentLaunchForce * fireTransform.forward;
            currentLaunchForce = minLaunchForce;
            OnFired?.Invoke();
        }

        public void ResetCharge()
        {
            currentLaunchForce = minLaunchForce;
            targetLaunchForce = maxLaunchForce;
            isCharging = false;
        }

        private void CheckFireAtTargetLaunchForce()
        {
            if (currentLaunchForce >= maxLaunchForce && !isFired)
            {
                currentLaunchForce = maxLaunchForce;
                Fire();
                return;
            }

            if (currentLaunchForce >= targetLaunchForce && !isFired)
            {
                currentLaunchForce = targetLaunchForce;
                Fire();
                return;
            }
        }
    }
}
