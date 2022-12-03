using System.Collections;
using System.Collections.Generic;
using Stats;
using Units.Player;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    private PlayerStateMachine stateMachine;
    private LichStats lichStats;
    private float defaultCameraFocusPoint = 50f;
    [SerializeField] private LineRenderer fireballAimLine;

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

    public Vector3 GetDirectionToCameraCentre(Transform emitter)
    {
        //Gets rotation that points to middles of screen, cameraFocusPoint determines how far away the centre is from the player
        RaycastHit hit;
        Vector3 relativePos;
        int layermask = 1 << 6;
        if (Physics.Raycast(stateMachine.MainCameraTransform.position, stateMachine.MainCameraTransform.forward, out hit, 100f, layermask))
        {
            relativePos = hit.point - emitter.position;
            return relativePos;
        }
        else
        {
            Vector3 viewportCentre = new Vector3(0.5f, 0.5f, defaultCameraFocusPoint);
            Vector3 cameraCentre = Camera.main.ViewportToWorldPoint(viewportCentre);
            relativePos = cameraCentre - emitter.position;
            return relativePos;
        }
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

    public void UpdateFireballAimLine(Vector3[] newLine)
    {
        if (newLine != null)
        {
            fireballAimLine.enabled = true;
            fireballAimLine.SetPositions(newLine);  
        }
        else
        {
            fireballAimLine.enabled = false;
        }
    }

    public void SpawnFirebolt()
    {
        //Instantiates firebolt at emitter, sets damage of firebolt, ensures it doesn't hit player
        FireBoltProjectile firebolt = Instantiate(fireboltPrefab, fireboltEmitter.position, Quaternion.LookRotation(GetDirectionToCameraCentre(fireboltEmitter), Vector3.up)).GetComponent<FireBoltProjectile>();
        firebolt.SetAttack(Mathf.RoundToInt(lichStats.GetLichAttack()), 10);
        firebolt.SetProjectileSpeed(fireboltStats.GetFireboltSpellProjectileSpeed());
        firebolt.SetPlayerCollider(gameObject.GetComponent<CharacterController>());
    }

    public void SpawnFireball()
    {
        fireballRotation = Quaternion.LookRotation(GetDirectionToCameraCentre(fireballEmitter), Vector3.up);
        currentFireball = Instantiate(fireballPrefab, fireballEmitter.transform.position, fireballRotation).GetComponent<FireBallProjectile>();
        currentFireball.SetAttack(Mathf.RoundToInt(lichStats.GetLichAttack()), 10);
        currentFireball.SetPlayerCollider(gameObject.GetComponent<CharacterController>());
    }

    public void LaunchFireball()
    {
        currentFireball.SetProjectileSpeed(fireballStats.GetFireballSpellProjectileSpeed());
    }
}
