using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using UnityEngine.VFX;

public class LichAegis : MonoBehaviour
{
    //[SerializeField] private Mana lichMana;
    //[SerializeField] private PlayerCooldowns cooldowns;
    [SerializeField] private Health lichHealth;
    [SerializeField] private LichStats lichStats;
    [SerializeField] private LichBones lichBones;
    //[SerializeField] private Material absorbMaterial;
    //[SerializeField] private Material blockMaterial;
    [SerializeField] private MeshRenderer aegisRenderer;
    [SerializeField] private Collider aegisCollider;
    [SerializeField] public GameObject buffVFX;
    private VisualEffect activeBuffVFX;

    //public bool canEnable = true;
    //public bool blocking = false;
    public bool absorbing = false;
    public bool attackBuffed = false;

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

    private void LateUpdate() 
    {
        if (attackBuffed && activeBuffVFX)
        {
            activeBuffVFX.transform.position = transform.position;
        }
    }

    // public void ToggleCanEnable(bool canEnable)
    // {
    //     this.canEnable = canEnable;
    // }

    // public void ToggleAegis(bool enable)
    // {
    //     if (canEnable)
    //     {
    //         blocking = enable;
    //         aegisRenderer.material = blockMaterial;
    //         aegisCollider.enabled = enable;
    //         aegisRenderer.enabled = enable;
    //         lichHealth.SetInvulnerable(enable);
    //     }
    // }

    public void ToggleAbsorb(bool enable)
    {
        //blocking = enable;
        //aegisRenderer.material = absorbMaterial;
        aegisCollider.enabled = enable;
        aegisRenderer.enabled = enable;
        lichHealth.SetInvulnerable(enable);
        absorbing = enable;
    }

    // public void DamageAegis(float damage, bool isProjectile)
    // {
    //     if (absorbing)
    //     {
    //         if (isProjectile)
    //         {
    //             AbsorbProjectile();
    //         }
    //     }
    //     else if (!lichMana.TryUseMana(damage))
    //     {
    //         lichMana.UseMana(damage);
    //         ToggleAegis(false);
    //         ToggleCanEnable(false);
    //         cooldowns.SetAegisCooldown();
    //     }
    // }

    public void DamageAegis()
    {
        if (!lichBones.TryUseBones(1)) {return;}

        if (!attackBuffed)
        {
            lichStats.BuffAttack();
        }
        remainingBuffTime = lichStats.GetLichAbsorbBuffDuration();
        if (activeBuffVFX)
        {
            Destroy(activeBuffVFX.gameObject);
        }
        activeBuffVFX = Instantiate(buffVFX, transform.position, Quaternion.identity).GetComponent<VisualEffect>();
        activeBuffVFX.SetFloat("Lifetime", lichStats.GetLichAbsorbBuffDuration());
        Destroy(activeBuffVFX.gameObject, lichStats.GetLichAbsorbBuffDuration() + 1f);
        attackBuffed = true;
    }
}
