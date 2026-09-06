using UnityEngine;

namespace WebGLRescueArena
{
    [RequireComponent(typeof(EnemyAttack))]
    public sealed class EnemyController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private LayerMask obstructionMask;
        private Transform target;
        private EnemyAttack attack;
        private EnemyManager manager;
        private void Awake() => attack = GetComponent<EnemyAttack>();
        private void Start()
        {
            manager = GetComponentInParent<EnemyManager>();
            if (manager != null) manager.Register(this);
        }
        private void OnDestroy()
        {
            if (manager != null) manager.Unregister(this);
        }
        private void Update()
        {
            if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 direction = target.position - transform.position;
            direction.y = 0f;
            float distance = Vector3.Distance(transform.position, target.position);
            if (Physics.Raycast(transform.position + Vector3.up * 0.4f, direction.normalized, distance, obstructionMask)) return;
            if (distance > 1.1f) transform.position += direction.normalized * (moveSpeed * Time.deltaTime);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            attack.Tick(target);
        }
        public void Tick() { }
    }
}
