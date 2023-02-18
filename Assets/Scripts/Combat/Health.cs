using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] public Image healthBar;
    [SerializeField] public AudioClip healSFX;

    private float health;
    public bool isDead;

    private bool invulnerable;

    public event Action OnTakeDamage;
    public event Action OnHeal;
    public event Action OnDie;

    public void SetMaxHealth(float newHealth)
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
        return health / maxHealth;
    }

    public void UpdateHealthbar()
    {
        healthBar.fillAmount = GetHealthNormalised();
    }

    public void DealDamage(float damage)
    {
        if (invulnerable) {return;}

        health = Mathf.Max(health - damage, 0);

        UpdateHealthbar();

        if (health <= 0) 
        {
            Die();
            return;
        }

        OnTakeDamage?.Invoke();
    }

    public void Die()
    {
        if (!isDead)
        {
            OnDie?.Invoke();
            isDead = true;
            SetInvulnerable(true);
        }
    }

    //Method to update all healths depending on new value from DifficultySlider
    public void AdjustHealth(float newHealth)
    {
        float currentNormalisedHealth = GetHealthNormalised();
        maxHealth = newHealth;
        health = maxHealth * currentNormalisedHealth;
        UpdateHealthbar();
    }

    public void Heal(float healing)
    {
        health = Mathf.Min(health + healing, maxHealth);
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(healSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
        UpdateHealthbar();
        OnHeal?.Invoke();
    }
}
