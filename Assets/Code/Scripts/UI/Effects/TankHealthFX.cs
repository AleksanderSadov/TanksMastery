using Tanks.Gameplay;
using UnityEngine;

namespace Tanks.UI
{
    [RequireComponent(typeof(TankHealth))]
    public class TankHealthFX : MonoBehaviour
    {
        public GameObject explosionPrefab;

        private TankHealth health;

        private void OnEnable()
        {
            if (health)
            {
                health.OnDeath += OnDeath;
            }
        }

        private void Start()
        {
            health = GetComponent<TankHealth>();
            health.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            if (health)
            {
                health.OnDeath -= OnDeath;
            }
        }

        private void OnDeath()
        {
            GameObject explosion = Instantiate(explosionPrefab, transform);
            ParticleSystem explosionParticles = explosion.GetComponent<ParticleSystem>();
            explosionParticles.transform.position = transform.position;
            explosionParticles.Play();
            Destroy(explosionParticles.gameObject, explosionParticles.main.duration);

            AudioSource explosionAudio = explosionParticles.GetComponent<AudioSource>();
            explosionAudio.Play();
        }
    }
}
