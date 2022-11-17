using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int health;
    public bool isDead;

    private bool invulnerable;

    public event Action OnTakeDamage;
    public event Action OnDie;

    public void SetMaxHealth(int newHealth)
    {
        maxHealth = newHealth;
        health = maxHealth;
    }

    public void SetInvulnerable(bool invuln)
    {
        if (invuln)
        {
            invulnerable = true;
        }
        else
        {
            invulnerable = false;
        }
    }

    public void DealDamage(int damage)
    {
        if (invulnerable) {return;}

        health = Mathf.Max(health - damage, 0);

        if (health <= 0) 
        {
            if (!isDead)
            {
                OnDie?.Invoke();
                isDead = true;
            }
            return;
        }

        OnTakeDamage?.Invoke();

        Debug.Log(health);
    }
}
