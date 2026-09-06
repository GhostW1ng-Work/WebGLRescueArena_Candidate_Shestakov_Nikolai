using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WebGLRescueArena
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyController enemyPrefab;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private Transform enemyContainer;
        [SerializeField] private int startingEnemies = 12;
        [SerializeField] private int normalCap = 55;
        [SerializeField] private int stressCap = 140;
        [SerializeField] private float spawnInterval = 1f;

        private int cap;
        private readonly Queue<EnemyController> pool = new Queue<EnemyController>();
        private readonly List<EnemyController> activeEnemies = new List<EnemyController>();
        private WaitForSeconds yieldDelay;

        public int ActiveEnemyCount => activeEnemies.Count;

        private void Start()
        {
            cap = normalCap;
            yieldDelay = new WaitForSeconds(spawnInterval);

            PrewarmPool(stressCap);

            for (int index = 0; index < startingEnemies; index++)
            {
                SpawnEnemy();
            }

            StartCoroutine(SpawnLoop());
        }

        public void EnableStressMode()
        {
            cap = stressCap;
            spawnInterval = 0.12f;
            yieldDelay = new WaitForSeconds(spawnInterval);
        }

        private void PrewarmPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                EnemyController enemy = Instantiate(enemyPrefab, enemyContainer);
                enemy.gameObject.SetActive(false);
                pool.Enqueue(enemy);
            }
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                if (ActiveEnemyCount < cap)
                {
                    SpawnEnemy();
                }
                yield return yieldDelay;
            }
        }

        private void SpawnEnemy()
        {
            if (spawnPoints == null || spawnPoints.Length == 0) return;

            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            EnemyController enemy;

            if (pool.Count > 0)
            {
                enemy = pool.Dequeue();
                enemy.transform.SetPositionAndRotation(point.position, Quaternion.identity);
                enemy.gameObject.SetActive(true);
            }
            else
            {
                enemy = Instantiate(enemyPrefab, point.position, Quaternion.identity, enemyContainer);
            }

            activeEnemies.Add(enemy);
        }

        public void DespawnEnemy(EnemyController enemy)
        {
            if (enemy == null) return;

            if (activeEnemies.Remove(enemy))
            {
                enemy.gameObject.SetActive(false);
                pool.Enqueue(enemy);
            }
        }
    }
}