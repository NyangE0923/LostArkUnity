using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour, IDamageSystem
{
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;
    protected int minHealth = 0;

    public int Health { get => health; set => health = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int MinHealth { get => minHealth; set => minHealth = value; }

    protected virtual void Start()
    {
        Health = MaxHealth;
    }

    protected virtual void Update()
    {

    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        Mathf.Clamp(Health, 0, MaxHealth);

        if (Health <= MinHealth)
        {
            Die();
            Health = MinHealth;
        }
    }

    protected virtual void Die()
    {

    }
}
