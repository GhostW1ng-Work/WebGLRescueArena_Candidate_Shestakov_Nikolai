using System.Collections;
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
        public int ActiveEnemyCount => enemyContainer.childCount;
        private void Start() { cap = normalCap; StartCoroutine(SpawnLoop()); for (int index = 0; index < startingEnemies; index++) SpawnEnemy(); }
        public void EnableStressMode() { cap = stressCap; spawnInterval = 0.12f; }
        private IEnumerator SpawnLoop()
        {
            while (true) { if (ActiveEnemyCount < cap) SpawnEnemy(); yield return new WaitForSeconds(spawnInterval); }
        }
        private void SpawnEnemy()
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, point.position, Quaternion.identity, enemyContainer);
        }
    }
}
