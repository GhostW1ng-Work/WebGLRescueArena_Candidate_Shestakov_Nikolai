using System.Collections.Generic;
using UnityEngine;

namespace WebGLRescueArena
{
    public sealed class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Transform player;
        private readonly List<EnemyController> enemies = new List<EnemyController>();
        private float[] distances = new float[128];

        public int Count => enemies.Count;

        public void Register(EnemyController enemy)
        {
            if (enemy != null && !enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
        }

        public void Unregister(EnemyController enemy)
        {
            enemies.Remove(enemy);
        }

        private void Update()
        {
            if (player == null) return;

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                }
            }

            int count = enemies.Count;
            if (count == 0) return;

            if (distances.Length < count)
            {
                distances = new float[Mathf.NextPowerOfTwo(count)];
            }

            Vector3 playerPos = player.position;

            for (int i = 0; i < count; i++)
            {
                Vector3 enemyPos = enemies[i].transform.position;
                float dx = enemyPos.x - playerPos.x;
                float dy = enemyPos.y - playerPos.y;
                float dz = enemyPos.z - playerPos.z;
                distances[i] = dx * dx + dy * dy + dz * dz;
            }

            for (int i = 1; i < count; i++)
            {
                EnemyController keyEnemy = enemies[i];
                float keyDist = distances[i];
                int j = i - 1;

                while (j >= 0 && distances[j] > keyDist)
                {
                    enemies[j + 1] = enemies[j];
                    distances[j + 1] = distances[j];
                    j--;
                }

                enemies[j + 1] = keyEnemy;
                distances[j + 1] = keyDist;
            }

            for (int i = 0; i < count; i++)
            {
                enemies[i].Tick();
            }
        }
    }
}