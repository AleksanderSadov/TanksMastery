using UnityEngine;
using UnityEngine.Events;

namespace Tanks.Gameplay
{
    public class TankHealth : MonoBehaviour
    {
        public float startingHealth = 100f;
        
        [HideInInspector] public float currentHealth;

        public UnityAction OnDeath;
        
        private bool isDead;

        private void OnEnable()
        {
            currentHealth = startingHealth;
            isDead = false;
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;

            if (currentHealth <= 0f && !isDead)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            OnDeath?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
