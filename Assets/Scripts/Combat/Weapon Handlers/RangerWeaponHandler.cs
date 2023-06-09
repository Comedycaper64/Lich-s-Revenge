using System.Collections;
using System.Collections.Generic;
using Stats;
using Units.Enemy.Ranger;
using UnityEngine;

//Script used for the Ranger's attack
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

    //Enables line that is drawn between the ranger and player
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

    //Draws the line between the ranger and player. Uses the line rendered component to set a start and end point for the aim visual.
    //The line can be blocked by objects in certain layers. This is so that the player doesn't think the ranger can shoot through walls
    public void SetAimVisual()
    {
        Vector3[] positionArray = new Vector3[2];
        RaycastHit hit;
        int layermask1 = 1 << 8;
        int layermask2 = 1 << 6;
        int layermask3 = 1 << 0;
        int layermask = layermask1 | layermask2 | layermask3;
        //If aiming, line goes to the player
        if (!weaponFired)
        {        
            Physics.Raycast(projectileEmitter.position, GetPlayerPosition() - projectileEmitter.position, out hit, 500f, layermask);
            positionArray = new Vector3[2] {projectileEmitter.position, hit.point};
        }
        else
        {
            //If weapon has been fired, line goes forward to where the projectile is going
            if (projectile)
            {
                Physics.Raycast(projectile.transform.position, playerDirection, out hit, 500f, layermask);
                positionArray = new Vector3[2] {projectile.transform.position, hit.point};
            }
        }
        //, GetPlayerPosition() + (GetPlayerPosition() - projectileEmitter.position)  <-  extension of ray
        aimRenderer.SetPositions(positionArray);
    }

    //Spawns the crossbow bolt that flies at the player
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
