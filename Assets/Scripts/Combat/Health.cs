using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Image healthBar;

    private int health;
    public bool isDead;

    private bool invulnerable;

    public event Action OnTakeDamage;
    public event Action OnDie;

    public void SetMaxHealth(int newHealth)
    {
        maxHealth = newHealth;
        health = maxHealth;
        UpdateHealthbar();
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

    public float GetHealthNormalised()
    {
        return (float)health / maxHealth;
    }

    public void UpdateHealthbar()
    {
        healthBar.fillAmount = GetHealthNormalised();
    }

    public void DealDamage(int damage)
    {
        if (invulnerable) {return;}

        health = Mathf.Max(health - damage, 0);

        UpdateHealthbar();

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
    }

    public void Heal(int healing)
    {
        health = Mathf.Min(health + healing, maxHealth);
        UpdateHealthbar();
    }
}
