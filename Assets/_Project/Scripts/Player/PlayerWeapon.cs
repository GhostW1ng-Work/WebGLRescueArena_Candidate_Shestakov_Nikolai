using UnityEngine;
using UnityEngine.Pool;

namespace WebGLRescueArena
{
    public sealed class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader input;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireRate = 0.12f;
        [SerializeField] private float projectileSpeed = 18f;
        [SerializeField] private int damage = 10;

        private ObjectPool<Projectile> projectilePool;
        private float nextShotTime;

        private void Awake()
        {
            projectilePool = new ObjectPool<Projectile>(
                createFunc: () =>
                {
                    Projectile proj = Instantiate(projectilePrefab);
                    proj.InitializePool(projectilePool);
                    return proj;
                },
                actionOnGet: proj => proj.gameObject.SetActive(true),
                actionOnRelease: proj => proj.gameObject.SetActive(false),
                actionOnDestroy: proj => Destroy(proj.gameObject),
                defaultCapacity: 20,
                maxSize: 100
            );
        }

        private void Update()
        {
            if (!input.FireHeld || Time.time < nextShotTime) return;

            nextShotTime = Time.time + fireRate;

            Projectile projectile = projectilePool.Get();
            projectile.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            projectile.Launch(projectileSpeed, damage);
        }
    }
}