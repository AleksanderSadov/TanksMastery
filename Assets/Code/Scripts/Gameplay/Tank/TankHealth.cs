using UnityEngine;

namespace Tanks.Gameplay
{
    public class TankHealth : MonoBehaviour
    {
        public float startingHealth = 100f;
        public float currentHealth;
        public GameObject explosionPrefab;

        private AudioSource explosionAudio;
        private ParticleSystem explosionParticles;
        private bool dead;

        private void Awake()
        {
            explosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
            explosionAudio = explosionParticles.GetComponent<AudioSource>();
            explosionParticles.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            currentHealth = startingHealth;
            dead = false;
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;

            if (currentHealth <= 0f && !dead)
            {
                OnDeath();
            }
        }

        private void OnDeath()
        {
            dead = true;

            explosionParticles.transform.position = transform.position;
            explosionParticles.gameObject.SetActive(true);
            explosionParticles.Play();
            explosionAudio.Play();

            gameObject.SetActive(false);
        }
    }
}
