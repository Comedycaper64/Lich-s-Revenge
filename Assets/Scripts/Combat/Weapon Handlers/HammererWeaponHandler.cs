using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammererWeaponHandler : MonoBehaviour
{
    [SerializeField] private HammererWeapon weaponLogic;
    [SerializeField] private HammererSlam slamLogic;
    private int playerLayerMask = 1 << 8;

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

    public void Slam(float slamRadius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, slamRadius, playerLayerMask);
        Collider[] colliders1 = Physics.OverlapBox(transform.position, new Vector3(slamRadius, 1f, slamRadius), Quaternion.identity, playerLayerMask);
        foreach (Collider collider in colliders)
        {
            foreach (Collider collider1 in colliders1)
            {
                if (collider == collider1)
                {
                    Debug.Log(collider.gameObject.name);
                    weaponLogic.TrySlamAttack(collider);
                }
            }
        }
    }

    public void SetAttack(int attack, int attackKnockback) => weaponLogic.SetAttack(attack, attackKnockback);

    public void EnableSlamVisual(bool enable) => slamLogic.EnableSlamVisual(enable);
    public void SetSlamVisualLocation(Vector3 newLocation) => slamLogic.SetSlamVisualLocation(newLocation);
    public void SetupSlamVisual(float slamRadius) => slamLogic.SetupSlamVisual(slamRadius);
}
