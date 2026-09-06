using UnityEngine;

namespace WebGLRescueArena
{
    public sealed class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 30;
        [SerializeField] private int scoreValue = 10;
        [SerializeField] private GameObject deathEffectPrefab;
        private int currentHealth;
        private void Awake() => currentHealth = maxHealth;
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth > 0) return;
            if (deathEffectPrefab != null) Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            GameEvents.RaiseEnemyKilled(scoreValue);
            Destroy(gameObject);
        }
    }
}
