using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammererWeaponHandler : MonoBehaviour
{
    [SerializeField] private HammererWeapon weaponLogic;

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
