using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private PlayerStateMachine stateMachine;
    private LichStats lichStats;
    [SerializeField] private GameObject weaponLogic;
    [SerializeField] private float cameraFocusPoint;

    [Header("Firebolt")]
    private FireboltStats fireboltStats;
    [SerializeField] private GameObject fireboltPrefab;
    public Transform fireboltEmitter;
    
    [Header("Fireball")]
    private FireballStats fireballStats;
    [SerializeField] private GameObject fireballPrefab;
    public Transform fireballEmitter;
    public Transform fireballVisual;
    private FireBallProjectile currentFireball;
    private Quaternion fireballRotation;

    private void Start() 
    {
        lichStats = gameObject.GetComponent<LichStats>();
        fireboltStats = gameObject.GetComponent<FireboltStats>();
        fireballStats = gameObject.GetComponent<FireballStats>();
        stateMachine = gameObject.GetComponent<PlayerStateMachine>();
    }

    public void EnableWeapon()
    {
        weaponLogic.SetActive(true);
    }

    public void DisableWeapon()
    {
        weaponLogic.SetActive(false);
    }

    public Vector3 GetDirectionToCameraCentre()
    {
        //Gets rotation that points to middles of screen, cameraFocusPoint determines how far away the centre is from the player
        Vector3 viewportCentre = new Vector3(0.5f, 0.5f, cameraFocusPoint);
        Vector3 cameraCentre = Camera.main.ViewportToWorldPoint(viewportCentre);
        Vector3 relativePos = cameraCentre - transform.position;
        return relativePos;
    }

    public void UpdateFireballVisual(Vector3 newLocation)
    {
        if (newLocation != Vector3.zero)
        {
            fireballVisual.gameObject.SetActive(true);
            fireballVisual.position = newLocation;
        }
        else
        {
            fireballVisual.gameObject.SetActive(false);
        }
    }

    public void SpawnFirebolt()
    {
        //Instantiates firebolt at emitter, sets damage of firebolt, ensures it doesn't hit player
        FireBoltProjectile firebolt = Instantiate(fireboltPrefab, fireboltEmitter.transform.position, Quaternion.LookRotation(GetDirectionToCameraCentre(), Vector3.up)).GetComponent<FireBoltProjectile>();
        firebolt.SetAttack(Mathf.RoundToInt(lichStats.GetLichAttack()), 10);
        firebolt.SetProjectileSpeed(fireboltStats.GetFireboltSpellProjectileSpeed());
        firebolt.SetPlayerCollider(gameObject.GetComponent<CharacterController>());
    }

    public void SpawnFireball()
    {
        fireballRotation = Quaternion.LookRotation(GetDirectionToCameraCentre(), Vector3.up);
        currentFireball = Instantiate(fireballPrefab, fireballEmitter.transform.position, fireballRotation).GetComponent<FireBallProjectile>();
        currentFireball.SetAttack(Mathf.RoundToInt(lichStats.GetLichAttack()), 10);
        currentFireball.SetPlayerCollider(gameObject.GetComponent<CharacterController>());
    }

    public void LaunchFireball()
    {
        currentFireball.SetProjectileSpeed(fireballStats.GetFireballSpellProjectileSpeed());
    }
}
