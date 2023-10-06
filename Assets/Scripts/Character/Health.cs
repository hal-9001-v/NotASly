using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactor))]
public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;
    [SerializeField] int currentHealth;

    [SerializeField] bool invincible;
    public int CurrentHealth { get { return currentHealth; } }

    public Action OnDead;
    public Action<int> OnHurt;
    public Action<int> OnHeal;
    public Action<int, int> OnChange;

    public UnityEvent OnHurtEvent;
    public UnityEvent OnDeadEvent;

    private void Awake()
    {
        Heal(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        damage = Mathf.Abs(damage);
        if (invincible == false)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        if (currentHealth == 0)
        {
            OnDead?.Invoke();
            OnDeadEvent?.Invoke();
        }
        else
        {
            OnHurt?.Invoke(damage);
            OnHurtEvent?.Invoke();
        }

        OnChange?.Invoke(currentHealth, maxHealth);
    }

    public void Heal(int points)
    {
        currentHealth += Mathf.Abs(points);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHeal?.Invoke(points);
        OnChange?.Invoke(currentHealth, maxHealth);
    }

    [ContextMenu("Heal")]
    void DebugHeal()
    {
        Heal(1);
    }

    [ContextMenu("Damage")]
    void DebugDamage()
    {
        TakeDamage(1);
    }
}
