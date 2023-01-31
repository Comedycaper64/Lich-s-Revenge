using System.Collections;
using System.Collections.Generic;
using Stats;
using Units.Enemy.Ranger;
using UnityEngine;

public class RangerWeaponHandler : MonoBehaviour
{
    private Transform playerTransform;
    private DwarfRangerStats rangerStats;
    private LineRenderer aimRenderer;
    private Vector3 playerDirection;
    public bool weaponFired = false;
    private RangerWeapon projectile;

    [SerializeField] private GameObject rangerProjectile;
    [SerializeField] public Transform projectileEmitter;    
    [SerializeField] private Material aimingMaterial;
    [SerializeField] private Material firingMaterial;

    [SerializeField] public AudioClip rangerAimSFX;
    [SerializeField] public AudioClip rangerShootSFX;

    private void Awake() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
        aimRenderer = GetComponent<LineRenderer>();
        rangerStats = GetComponent<DwarfRangerStats>();
    }

    public void AimWeapon()
    {
        aimRenderer.enabled = true;
        aimRenderer.material = aimingMaterial;
        weaponFired = false;
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(rangerAimSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
    }

    public void SetAimVisual()
    {
        Vector3[] positionArray = new Vector3[2];
        RaycastHit hit;
        int layermask1 = 1 << 8;
        int layermask2 = 1 << 6;
        int layermask = layermask1 | layermask2;
        if (!weaponFired)
        {        
            Physics.Raycast(projectileEmitter.position, GetPlayerPosition() - projectileEmitter.position, out hit, 500f, layermask);
            positionArray = new Vector3[2] {projectileEmitter.position, hit.point};
        }
        else
        {
            if (projectile)
            {
                Physics.Raycast(projectile.transform.position, playerDirection, out hit, 500f, layermask);
                positionArray = new Vector3[2] {projectile.transform.position, hit.point};
            }
        }
        //, GetPlayerPosition() + (GetPlayerPosition() - projectileEmitter.position)  <-  extension of ray
        aimRenderer.SetPositions(positionArray);
    }

    public void FireWeapon()
    {
        SetAimVisual();
        playerDirection = (GetPlayerPosition() - projectileEmitter.position);
        projectile = Instantiate(rangerProjectile, projectileEmitter.position, Quaternion.LookRotation(playerDirection, Vector3.up)).GetComponent<RangerWeapon>();
        weaponFired = true;
        projectile.SetCollider(GetComponent<CharacterController>());
        projectile.SetAttack(rangerStats.GetAttack(), 5f);
        projectile.SetSpeed(rangerStats.GetProjectileSpeed());
        playerDirection = (GetPlayerPosition() - projectile.transform.position);
        aimRenderer.material = firingMaterial;
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(rangerShootSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
    }

    private Vector3 GetPlayerPosition()
    {
        Vector3 playerPosition = playerTransform.position;
        playerPosition.y += 0.9f;
        return playerPosition;
    }

    public void StowWeapon()
    {
        aimRenderer.enabled = false;
    }
}
