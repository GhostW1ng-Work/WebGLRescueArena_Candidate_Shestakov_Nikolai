using UnityEngine;

namespace WebGLRescueArena
{
    public sealed class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private int damage = 8;
        [SerializeField] private float attackRange = 1.35f;
        [SerializeField] private float attackCooldown = 0.9f;
        private float nextAttack;
        public void Tick(Transform target)
        {
            if (target == null || Time.time < nextAttack || Vector3.Distance(transform.position, target.position) > attackRange) return;
            nextAttack = Time.time + attackCooldown;
            target.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
