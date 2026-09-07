using UnityEngine;
using UnityEngine.Pool;

namespace WebGLRescueArena
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifetime = 2.5f;
        [SerializeField] private GameObject impactEffectPrefab;

        private Rigidbody body;
        private ObjectPool<Projectile> poolOwner;
        private int damage;
        private float speed;
        private float deactivateTime;
        private bool isReleased;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        public void InitializePool(ObjectPool<Projectile> pool)
        {
            poolOwner = pool;
        }

        public void Launch(float speedValue, int damageValue)
        {
            speed = speedValue;
            damage = damageValue;
            deactivateTime = Time.time + lifetime;
            isReleased = false;
        }

        private void Update()
        {
            if (isReleased) return;

            transform.Translate(Vector3.forward * (speed * Time.deltaTime));

            if (Time.time >= deactivateTime)
            {
                ReturnToPool();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isReleased) return;

            if (other.TryGetComponent<EnemyHealth>(out var enemy))
            {
                enemy.TakeDamage(damage);
            }

            if (impactEffectPrefab != null)
            {
                Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            }

            ReturnToPool();
        }

        private void ReturnToPool()
        {
            if (isReleased) return;

            isReleased = true;

            if (poolOwner != null)
            {
                poolOwner.Release(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}