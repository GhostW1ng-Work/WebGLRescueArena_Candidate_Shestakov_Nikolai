using UnityEngine;

namespace WebGLRescueArena
{
    public sealed class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 30;
        [SerializeField] private int scoreValue = 10;
        [SerializeField] private GameObject deathEffectPrefab;

        private int currentHealth;
        private EnemySpawner spawner;

        private void Awake()
        {
            currentHealth = maxHealth;
            spawner = FindObjectOfType<EnemySpawner>();
        }

        private void OnEnable()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (currentHealth <= 0) return;

            currentHealth -= damage;
            if (currentHealth > 0) return;

            if (deathEffectPrefab != null)
            {
                Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            }

            GameEvents.RaiseEnemyKilled(scoreValue);

            if (spawner != null)
            {
                EnemyController enemyController = GetComponent<EnemyController>();
                spawner.DespawnEnemy(enemyController);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}