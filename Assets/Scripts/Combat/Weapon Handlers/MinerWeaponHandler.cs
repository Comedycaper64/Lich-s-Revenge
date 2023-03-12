using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Enemy.Miner;
using System;

public class MinerWeaponHandler : MonoBehaviour
{
    [SerializeField] private MinerWeapon weaponLogic;
    [SerializeField] public AudioClip attackSFX;

    private void Start() 
    {
        weaponLogic.SetHandler(this); 
    }

    public void EnableWeapon()
    {
        weaponLogic.gameObject.SetActive(true);
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(attackSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
    }

    public void DebugAttack()
    {
        DwarfMinerStateMachine stateMachine = GetComponent<DwarfMinerStateMachine>();
        if ((stateMachine.Player.transform.position - stateMachine.transform.position).magnitude < stateMachine.AttackRange)
        {
            stateMachine.Player.DealDamage(stateMachine.Stats.GetAttack());
        }
    }

    public void DisableWeapon()
    {
        weaponLogic.gameObject.SetActive(false);
    }

    public void SetAttack(float attack, float attackKnockback) => weaponLogic.SetAttack(attack, attackKnockback);
}
