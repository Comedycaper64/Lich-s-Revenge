using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using UnityEngine.VFX;

//Script used by the player's Absorb ability
public class LichAegis : MonoBehaviour
{
    [SerializeField] private Health lichHealth;
    [SerializeField] private LichStats lichStats;
    [SerializeField] private LichBones lichBones;
    [SerializeField] private MeshRenderer aegisRenderer;
    [SerializeField] private Collider aegisCollider;
    [SerializeField] public GameObject buffVFX;
    [SerializeField] public AudioClip absorbBuffSFX;
    private VisualEffect activeBuffVFX;

    public bool absorbing = false;
    public bool blocking = false;
    public bool attackBuffed = false;

    private float remainingBuffTime = 0;

    private void Update() 
    {
        //Decrements time on the attack buff if it's active
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

    //Late update allows the BuffVFX to follow the player more smoothly
    private void LateUpdate() 
    {
        if (attackBuffed && activeBuffVFX)
        {
            activeBuffVFX.transform.position = transform.position;
        }
    }

    //Mechanic found in the fifth scenario. Causes miner enemy to be knocked back and take damage when hitting the shield that surrounds the player
    public void ToggleAegis(bool enable)
    {
        blocking = enable;
        aegisCollider.enabled = enable;
        aegisRenderer.enabled = enable;
        lichHealth.SetInvulnerable(enable);
    }

    //Toggle for the Absorb ability
    public void ToggleAbsorb(bool enable)
    {
        aegisCollider.enabled = enable;
        aegisRenderer.enabled = enable;
        lichHealth.SetInvulnerable(enable);
        absorbing = enable;
    }

    //Aegis refers to the Absorb shield around the player during the Absorb ability
    public void DamageAegis()
    {
        //If a bone is available, the player's attack is increased
        if (!attackBuffed && !lichBones.TryUseBones(1)) {return;}

        if (!attackBuffed)
        {
            lichStats.BuffAttack();
        }
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(absorbBuffSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
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
