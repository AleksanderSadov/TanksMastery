using Tanks.Gameplay;
using UnityEngine;

namespace Tanks.UI
{
    [RequireComponent(typeof(TankHealth))]
    public class TankHealthFX : MonoBehaviour
    {
        public GameObject explosionPrefab;

        private TankHealth health;
        private AudioSource explosionAudio;
        private ParticleSystem explosionParticles;

        private void Awake()
        {
            explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
            explosionAudio = explosionParticles.GetComponent<AudioSource>();
            explosionParticles.gameObject.SetActive(false);
        }

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
            explosionParticles.transform.position = transform.position;
            explosionParticles.gameObject.SetActive(true);
            explosionParticles.Play();
            explosionAudio.Play();
        }
    }
}
