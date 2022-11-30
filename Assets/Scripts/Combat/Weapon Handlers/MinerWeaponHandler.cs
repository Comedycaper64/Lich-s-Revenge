using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerWeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponLogic;

    public void EnableWeapon()
    {
        weaponLogic.SetActive(true);
    }

    public void DisableWeapon()
    {
        weaponLogic.SetActive(false);
    }
}
