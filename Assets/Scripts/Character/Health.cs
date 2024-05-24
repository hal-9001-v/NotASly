using System;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(Interactor))]
public class Health : MonoBehaviour, IRespawnable
{
    [SerializeField] int maxHealth = 5;
    [SerializeField] int currentHealth;

    public bool invincible;
    public int CurrentHealth { get { return currentHealth; } }

    public Action<Vector3> OnDead;
    public Action<int, Vector3> OnHurt;
    public Action<int> OnHeal;
    public Action<int, int> OnChange;

    public UnityEvent OnHurtEvent;
    public UnityEvent OnDeadEvent;

    private void Awake()
    {
        Respawn();
    }

    public void TakeDamage(int damage, Vector3 origin = new Vector3(), HurtBox.HurtType type = HurtBox.HurtType.Physical)
    {
        damage = Mathf.Abs(damage);
        if (invincible == false)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        if (currentHealth == 0)
        {
            Kill();
        }
        else
        {
            OnHurt?.Invoke(damage, origin);
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

    public void Kill(Vector3 origin = new Vector3())
    {
        currentHealth = 0;
        OnDead?.Invoke(origin);
        OnDeadEvent?.Invoke();
    }

    //Use this for Unity Editor. The other function will not show up in the Unity Event
    public void Kill()
    {
        Kill(transform.position);
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

    public void Respawn()
    {
        Heal(maxHealth);
    }
}
