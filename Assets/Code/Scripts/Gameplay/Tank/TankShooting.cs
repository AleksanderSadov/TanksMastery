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
        public AnimationCurve optimalForceOverDistance;

        [Header("Debug")]
        public float currentLaunchForce;
        public float targetLaunchForce;
        public bool isFired = false;
        public bool isCharging = false;
        public bool isReloading = false;
        public float chargeSpeed;
        public float lastTimeFired = 0;

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

        public void CalculateOptimalForceForTarget(GameObject target)
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);
            targetLaunchForce = optimalForceOverDistance.Evaluate(distance);
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
