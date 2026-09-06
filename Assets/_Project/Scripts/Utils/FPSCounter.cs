using UnityEngine;
namespace WebGLRescueArena { public sealed class FPSCounter : MonoBehaviour { public float FramesPerSecond { get; private set; } private void Update() => FramesPerSecond = 1f / Time.unscaledDeltaTime; } }
