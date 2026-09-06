using UnityEngine;

namespace WebGLRescueArena
{
    public sealed class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        public int CurrentHealth { get; private set; }
        private void Awake() => CurrentHealth = maxHealth;
        public void TakeDamage(int amount)
        {
            CurrentHealth -= amount;
            GameEvents.RaisePlayerDamaged(amount);
            if (CurrentHealth < 0) GameEvents.RaisePlayerDied();
        }
    }
}
