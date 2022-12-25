using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

public class LichAegis : MonoBehaviour
{
    [SerializeField] private Mana lichMana;
    [SerializeField] private PlayerCooldowns cooldowns;
    [SerializeField] private LichStats lichStats;
    [SerializeField] private Material absorbMaterial;
    [SerializeField] private Material blockMaterial;
    [SerializeField] private MeshRenderer aegisRenderer;
    [SerializeField] private Collider aegisCollider;

    public bool canEnable = true;
    private bool absorbing = false;
    private bool attackBuffed = false;

    private float remainingBuffTime = 0;

    private void Update() 
    {
        if (attackBuffed)
        {
            remainingBuffTime -= Time.deltaTime;
            if (remainingBuffTime < 0f)
            {
                lichStats.ResetAttack();
                attackBuffed = false;
            }
        }    
    }

    public void ToggleCanEnable(bool canEnable)
    {
        this.canEnable = canEnable;
    }

    public void ToggleAegis(bool enable)
    {
        if (canEnable)
        {
            aegisRenderer.material = blockMaterial;
            aegisCollider.enabled = enable;
            aegisRenderer.enabled = enable;
        }
    }

    public void ToggleAbsorb(bool enable)
    {
        aegisRenderer.material = absorbMaterial;
        aegisCollider.enabled = enable;
        aegisRenderer.enabled = enable;
        absorbing = enable;
    }

    public void DamageAegis(float damage, bool isProjectile)
    {
        if (absorbing)
        {
            if (isProjectile)
            {
                AbsorbProjectile();
            }
        }
        else if (!lichMana.TryUseMana(damage))
        {
            lichMana.UseMana(damage);
            ToggleAegis(false);
            ToggleCanEnable(false);
            cooldowns.SetAegisCooldown();
        }
    }

    private void AbsorbProjectile()
    {
        if (!attackBuffed)
        {
            lichStats.BuffAttack();
        }
        remainingBuffTime = lichStats.GetLichAbsorbBuffDuration();
        attackBuffed = true;
    }
}
