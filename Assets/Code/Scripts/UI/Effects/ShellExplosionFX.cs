using Tanks.Gameplay;
using UnityEngine;

namespace Tanks.UI
{
    [RequireComponent(typeof(ShellExplosion))]
    public class ShellExplosionFX : MonoBehaviour
    {
        public ParticleSystem explosionParticles;
        public AudioSource explosionAudio;

        private ShellExplosion shellExplosion;

        private void OnEnable()
        {
            if (shellExplosion)
            {
                shellExplosion.OnExplosion += OnExplosion;
            }
        }

        private void Start()
        {
            shellExplosion = GetComponent<ShellExplosion>();
            shellExplosion.OnExplosion += OnExplosion;
        }

        private void OnDisable()
        {
            if (shellExplosion)
            {
                shellExplosion.OnExplosion -= OnExplosion;
            }
        }

        private void OnExplosion()
        {
            explosionParticles.transform.parent = null;
            explosionParticles.Play();
            explosionAudio.Play();

            Destroy(explosionParticles.gameObject, explosionParticles.main.duration);
        }
    }
}

