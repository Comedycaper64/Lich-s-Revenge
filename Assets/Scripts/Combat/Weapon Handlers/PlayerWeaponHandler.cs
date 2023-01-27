using System.Collections;
using System.Collections.Generic;
using Stats;
using Units.Player;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    private LichStats lichStats;
    public bool QTESucceeded = false;
    public bool QTEActive = false;
    public bool fireballLaunched = false;
    private float defaultCameraFocusPoint = 50f;
    private Transform MainCameraTransform;
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
        MainCameraTransform = Camera.main.transform;
        float explodeRadius = fireballStats.GetFireballExplodeRadius() * 2.6f; //fireball visual is smaller than it should be due to lich parent, so needs 2.6 buff
        fireballVisual.localScale = new Vector3(explodeRadius, explodeRadius, explodeRadius);
    }

    public Vector3 GetDirectionToCameraCentre(Transform emitter)
    {
        //Gets rotation that points to middles of screen, cameraFocusPoint determines how far away the centre is from the player
        RaycastHit hit;
        Vector3 relativePos;
        int layermask1 = 1 << 7;
        int layermask2 = 1 << 6;
        int layermask = layermask1 | layermask2;
        if (Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out hit, 100f, layermask))
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
        firebolt.SetAttack(fireboltStats.GetFireboltSpellAttack(), 10f);
        firebolt.SetProjectileSpeed(fireboltStats.GetFireboltSpellProjectileSpeed());
        firebolt.SetPlayerCollider(gameObject.GetComponent<CharacterController>());
    }

    public void SpawnFireball()
    {
        //currentFireball = Instantiate(fireballPrefab, fireballEmitter.transform.position, fireballRotation).GetComponent<FireBallProjectile>();
        currentFireball = Instantiate(fireballPrefab, fireballEmitter.transform).GetComponent<FireBallProjectile>();
        currentFireball.SetAttack(fireballStats.GetFireballSpellAttack(), 20f);
        currentFireball.SetTimeToLive(fireballStats.GetfireballDetonationTime());
        currentFireball.SetPlayerCollider(gameObject.GetComponent<CharacterController>());
        QTESucceeded = false;
        fireballLaunched = false;
    }

    public void StartQTE()
    {
        QTEActive = true;
    }

    public void CompleteQTE()
    {
        QTESucceeded = true;
        QTEActive = false;
        LaunchFireball();
    }

    public void LaunchFireball()
    {
        if (!fireballLaunched)
        {
            fireballRotation = Quaternion.LookRotation(GetDirectionToCameraCentre(fireballEmitter), Vector3.up);
            currentFireball.transform.SetParent(null);
            currentFireball.transform.rotation = fireballRotation;
            currentFireball.SetProjectileSpeed(fireballStats.GetFireballSpellProjectileSpeed());
            currentFireball.SetExplodeRadius(fireballStats.GetFireballExplodeRadius());
            if (!QTESucceeded)
            {   
                QTEActive = false;
                currentFireball.SetDamagePlayer(true);
            }
            else
            {
                currentFireball.SetDamagePlayer(false);
                currentFireball.SetAttack(fireballStats.GetFireballQTEAttack(), 20f);
            }
            fireballLaunched = true;
        }
    }
}
