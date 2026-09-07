using UnityEngine;

namespace WebGLRescueArena
{
    [RequireComponent(typeof(EnemyAttack))]
    public sealed class EnemyController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private LayerMask obstructionMask;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float yOffset = 1f;
        private static Transform cachedPlayerTarget;

        private EnemyAttack attack;
        private EnemyManager manager;
        private Transform myTransform;

        private const float StoppingDistanceSqr = 1.21f;
        private const float GroundRayDistance = 5f;
        private const float GroundRayOffset = 2f;

        private void Awake()
        {
            attack = GetComponent<EnemyAttack>();
            myTransform = transform;
        }

        private void OnEnable()
        {
            if (cachedPlayerTarget == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    cachedPlayerTarget = playerObj.transform;
                }
            }

            if (manager == null)
            {
                manager = GetComponentInParent<EnemyManager>();
            }

            if (manager != null)
            {
                manager.Register(this);
            }
        }

        private void OnDisable()
        {
            if (manager != null)
            {
                manager.Unregister(this);
            }
        }

        public void Tick()
        {
            if (cachedPlayerTarget == null) return;

            Vector3 currentPos = myTransform.position;
            Vector3 targetPos = cachedPlayerTarget.position;

            Vector3 direction = targetPos - currentPos;
            direction.y = 0f;

            float sqrDistance = direction.sqrMagnitude;

            if (sqrDistance > StoppingDistanceSqr)
            {
                float distance = Mathf.Sqrt(sqrDistance);

                if (!Physics.Raycast(currentPos + Vector3.up * 0.4f, direction / distance, distance, obstructionMask))
                {
                    currentPos += (direction / distance) * (moveSpeed * Time.deltaTime);
                }
            }

            if (Physics.Raycast(currentPos + Vector3.up * GroundRayOffset, Vector3.down, out RaycastHit hit, GroundRayDistance, groundMask, QueryTriggerInteraction.Ignore))
            {
                currentPos.y = hit.point.y + yOffset;
            }

            myTransform.position = currentPos;

            targetPos.y = currentPos.y;
            myTransform.LookAt(targetPos);

            attack.Tick(cachedPlayerTarget);
        }
    }
}