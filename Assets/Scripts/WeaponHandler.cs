using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private LichStats stats;
    private PlayerStateMachine stateMachine;
    [SerializeField] private GameObject weaponLogic;
    [SerializeField] private GameObject fireboltPrefab;
    [SerializeField] private GameObject fireballPrefab;
    public Transform fireboltEmitter;
    public Transform fireballEmitter;
    public Transform fireballVisual;
    [SerializeField] private float cameraFocusPoint;

    private void Start() 
    {
        stats = gameObject.GetComponent<LichStats>();
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
        firebolt.SetAttack(Mathf.RoundToInt(stats.GetLichAttack()), 10);
        firebolt.SetPlayerCollider(gameObject.GetComponent<CharacterController>());
    }

    public void SpawnFireball()
    {
        FireBallProjectile fireBall = Instantiate(fireballPrefab, fireballEmitter.transform.position, Quaternion.LookRotation(GetDirectionToCameraCentre(), Vector3.up)).GetComponent<FireBallProjectile>();
        fireBall.SetAttack(Mathf.RoundToInt(stats.GetLichAttack()), 10);
        fireBall.SetPlayerCollider(gameObject.GetComponent<CharacterController>());
    }
}
