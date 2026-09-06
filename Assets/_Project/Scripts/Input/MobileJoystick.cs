using UnityEngine;
using UnityEngine.EventSystems;

namespace WebGLRescueArena
{
    public sealed class MobileJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform handle;
        [SerializeField] private float radius = 70f;
        public Vector2 Value { get; private set; }
        public void OnDrag(PointerEventData data) { Value = Vector2.ClampMagnitude(data.delta / radius, 1f); handle.anchoredPosition = Value * radius; }
        public void OnPointerUp(PointerEventData data) { Value = Vector2.zero; handle.anchoredPosition = Vector2.zero; }
    }
}
