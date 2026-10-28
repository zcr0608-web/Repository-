using UnityEngine;
using UnityEngine.Events;

public class MobileStrikePlayerHealth : MonoBehaviour
{
    [Min(1)] public float maxHealth = 100f;
    public UnityEvent<float, float> onHealthChanged;
    public UnityEvent onDied;

    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0f;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        onHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || amount <= 0f) return;
        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        onHealthChanged?.Invoke(CurrentHealth, maxHealth);
        if (IsDead) onDied?.Invoke();
    }

    public void RestoreFullHealth()
    {
        CurrentHealth = maxHealth;
        onHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }
}
