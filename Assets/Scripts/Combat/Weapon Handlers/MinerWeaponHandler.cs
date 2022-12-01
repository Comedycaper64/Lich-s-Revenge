using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Enemy.Miner;
using System;

public class MinerWeaponHandler : MonoBehaviour
{
    [SerializeField] private MinerWeapon weaponLogic;

    private void Start() 
    {
        weaponLogic.SetHandler(this); 
    }

    public void EnableWeapon()
    {
        weaponLogic.gameObject.SetActive(true);
    }

    public void DisableWeapon()
    {
        weaponLogic.gameObject.SetActive(false);
    }

    public void SetAttack(int attack, int attackKnockback) => weaponLogic.SetAttack(attack, attackKnockback);
}
