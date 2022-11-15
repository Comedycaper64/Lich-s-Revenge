using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private LichStats stats;
    [SerializeField] private GameObject weaponLogic;
    [SerializeField] private GameObject fireboltPrefab;
    [SerializeField] private Transform fireboltEmitter;

    private void Start() 
    {
        stats = gameObject.GetComponent<LichStats>();
    }

    public void EnableWeapon()
    {
        weaponLogic.SetActive(true);
    }

    public void DisableWeapon()
    {
        weaponLogic.SetActive(false);
    }

    public void SpawnFirebolt()
    {
        FireBoltProjectile firebolt = Instantiate(fireboltPrefab, fireboltEmitter.transform.position, transform.rotation).GetComponent<FireBoltProjectile>();
        firebolt.SetAttack(Mathf.RoundToInt(stats.GetLichAttack()), 10);
        firebolt.SetPlayerCollider(gameObject.GetComponent<CharacterController>());
    }
}
