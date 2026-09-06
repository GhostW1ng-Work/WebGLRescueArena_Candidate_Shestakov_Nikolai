using UnityEngine;
namespace WebGLRescueArena { public sealed class AutoDestroy : MonoBehaviour { [SerializeField] private float delay = 3f; private void Start() => Destroy(gameObject, delay); } }
