using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int health;
    public bool isDead;

    public event Action OnTakeDamage;
    public event Action OnDie;

    private void Start() 
    {
        //health = maxHealth;    
    }

    public void SetMaxHealth(int newHealth)
    {
        maxHealth = newHealth;
        health = maxHealth;
    }

    public void DealDamage(int damage)
    {
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
