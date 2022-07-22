using Tanks.Gameplay;
using UnityEngine;

namespace Tanks.UI
{
    [RequireComponent(typeof(TankShooting))]
    public class TankShootingFX : MonoBehaviour
    {
        public AudioSource shootingAudio;
        public AudioClip chargingClip;
        public AudioClip fireClip;

        private TankShooting shooting;

        private void OnEnable()
        {
            if (shooting)
            {
                shooting.OnStartCharging += OnStartCharging;
                shooting.OnFired += OnFired;
            }
        }

        private void Start()
        {
            shooting = GetComponent<TankShooting>();
            shooting.OnStartCharging += OnStartCharging;
            shooting.OnFired += OnFired;
        }

        private void OnDisable()
        {
            shootingAudio.Stop();

            if (shooting)
            {
                shooting.OnStartCharging -= OnStartCharging;
                shooting.OnFired -= OnFired;
            }
        }

        private void OnStartCharging()
        {
            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }

        private void OnFired()
        {
            shootingAudio.clip = fireClip;
            shootingAudio.Play();
        }
    }
}
