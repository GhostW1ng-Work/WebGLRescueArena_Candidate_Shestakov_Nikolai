using UnityEngine;

namespace WebGLRescueArena
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader input;
        [SerializeField] private float moveSpeed = 7f;
        private Rigidbody body;
        private void Awake() => body = GetComponent<Rigidbody>();
        private void FixedUpdate() { Vector2 move = input.Move.normalized; body.MovePosition(body.position + new Vector3(move.x, 0f, move.y) * (moveSpeed * Time.fixedDeltaTime)); }
        private void Update() { Vector3 target = input.AimPoint(transform.position); Vector3 direction = target - transform.position; direction.y = 0f; if (direction.sqrMagnitude > 0.01f) transform.rotation = Quaternion.LookRotation(direction); }
    }
}
