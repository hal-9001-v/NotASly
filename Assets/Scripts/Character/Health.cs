using System;
using UnityEngine;

[RequireComponent(typeof(Interactor))]
public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;
    [SerializeField] int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }

    public Action OnDead;
    public Action<int> OnHurt;

    private void Awake()
    {
        Heal(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        damage = Mathf.Abs(damage);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth == 0)
        {
            OnDead?.Invoke();
        }
        else
        {
            OnHurt?.Invoke(damage);
        }
    }

    public void Heal(int points)
    {
        currentHealth += Mathf.Abs(points);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}
