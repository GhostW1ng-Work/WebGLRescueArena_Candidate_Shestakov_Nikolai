using UnityEngine;

namespace WebGLRescueArena
{
    public sealed class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private MobileJoystick joystick;
        [SerializeField] private MobileFireButton fireButton;
        public Vector2 Move => new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical")) + (joystick == null ? Vector2.zero : joystick.Value);
        public bool FireHeld => UnityEngine.Input.GetMouseButton(0) || (fireButton != null && fireButton.IsPressed);
        public Vector3 AimPoint(Vector3 origin)
        {
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            Plane plane = new Plane(Vector3.up, origin);
            return plane.Raycast(ray, out float distance) ? ray.GetPoint(distance) : origin + transform.forward;
        }
    }
}
