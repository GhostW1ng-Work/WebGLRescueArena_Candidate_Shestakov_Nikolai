using UnityEngine;

namespace WebGLRescueArena
{
    public sealed class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private MobileJoystick joystick;
        [SerializeField] private MobileFireButton fireButton;

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        public Vector2 Move
        {
            get
            {
                float h = Input.GetAxisRaw("Horizontal");
                float v = Input.GetAxisRaw("Vertical");

                if (joystick != null)
                {
                    Vector2 joyVal = joystick.Value;
                    h += joyVal.x;
                    v += joyVal.y;
                }

                return new Vector2(h, v);
            }
        }

        public bool FireHeld => Input.GetMouseButton(0) || (fireButton != null && fireButton.IsPressed);

        public Vector3 AimPoint(Vector3 origin)
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                if (mainCamera == null) return origin + transform.forward;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, origin);

            return plane.Raycast(ray, out float distance) ? ray.GetPoint(distance) : origin + transform.forward;
        }
    }
}