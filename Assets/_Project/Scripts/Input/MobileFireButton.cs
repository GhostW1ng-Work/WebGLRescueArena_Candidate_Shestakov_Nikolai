using UnityEngine;
using UnityEngine.EventSystems;

namespace WebGLRescueArena
{
    public sealed class MobileFireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsPressed { get; private set; }
        public void OnPointerDown(PointerEventData data) => IsPressed = true;
        public void OnPointerUp(PointerEventData data) => IsPressed = false;
    }
}
