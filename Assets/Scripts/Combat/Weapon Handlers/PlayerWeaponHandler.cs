using System.Collections;
using System.Collections.Generic;
using Stats;
using Units.Player;
using UnityEngine;

//Script used by the player's offensive abilities
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
    [SerializeField] private AudioClip fireboltCastSFX;
    
    [Header("Fireball")]
    private FireballStats fireballStats;
    [SerializeField] private GameObject fireballPrefab;
    public Transform fireballEmitter;
    public Transform fireballVisual;
    private FireBallProjectile currentFireball;
    private Quaternion fireballRotation;
    [SerializeField] private GameObject fireballQTEVFX;

    [Header("Fireball SFX")]
    [SerializeField] private AudioClip fireballCastSFX;
    [SerializeField] private AudioClip fireballLaunchSFX;
    [SerializeField] private AudioClip fireballLaunchQTESFX;

    private void Start() 
    {
        lichStats = gameObject.GetComponent<LichStats>();
        fireboltStats = gameObject.GetComponent<FireboltStats>();
        fireballStats = gameObject.GetComponent<FireballStats>();
        MainCameraTransform = Camera.main.transform;
        float explodeRadius = fireballStats.GetFireballExplodeRadius() * 2.6f; //fireball visual is smaller than it should be due to lich parent, so needs 2.6 increase
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
        //If the centre of the screen has a target (enemy or environment), causes the projectile to go towards it.
        //Otherwise just launches projectile into the middle of the screen.
        //Basically a slight aim adjustment due to the camera perspective
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

    //Updates location of the explosion visual present when casting the fireball
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

    //Same as above, but for the line showing where the fireball will go
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

    //Instantiates firebolt at emitter, sets damage of firebolt, ensures it doesn't hit player
    public void SpawnFirebolt()
    {
        FireBoltProjectile firebolt = Instantiate(fireboltPrefab, fireboltEmitter.position, Quaternion.LookRotation(GetDirectionToCameraCentre(fireboltEmitter), Vector3.up)).GetComponent<FireBoltProjectile>();
        firebolt.SetAttack(fireboltStats.GetFireboltSpellAttack(), 10f);
        firebolt.SetProjectileSpeed(fireboltStats.GetFireboltSpellProjectileSpeed());
        firebolt.SetPlayerCollider(gameObject.GetComponent<CharacterController>());
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(fireboltCastSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
        if (lichStats.debugBadAttackSpread)
        {
            firebolt.transform.eulerAngles += (new Vector3(Random.Range(-40, 40),Random.Range(-40, 40),Random.Range(-40, 40)));
        }
    }

    //Same as above, but for the fireball
    public void SpawnFireball()
    {
        currentFireball = Instantiate(fireballPrefab, fireballEmitter.transform).GetComponent<FireBallProjectile>();
        currentFireball.SetAttack(fireballStats.GetFireballSpellAttack(), 20f);
        currentFireball.SetTimeToLive(fireballStats.GetfireballDetonationTime());
        currentFireball.SetPlayerCollider(gameObject.GetComponent<CharacterController>());
        QTESucceeded = false;
        fireballLaunched = false;
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(fireballCastSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
    }

    //The fireball features a mechanic that rewards player timing. If the player releases the fireball as soon as it appears
    //above the player's head, it deals more damage and doesn't harm the player if he's within the explosion radius.
    //If the player times the release correctly, CompleteQTE is executed
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

    //Launches the fireball towards where the player is aiming it
    public void LaunchFireball()
    {
        if (!fireballLaunched)
        {
            fireballRotation = Quaternion.LookRotation(GetDirectionToCameraCentre(fireballEmitter), Vector3.up);
            currentFireball.transform.SetParent(null);
            currentFireball.transform.rotation = fireballRotation;
            currentFireball.EnableCollider(true);
            currentFireball.SetProjectileSpeed(fireballStats.GetFireballSpellProjectileSpeed());
            currentFireball.SetExplodeRadius(fireballStats.GetFireballExplodeRadius());
            if (!QTESucceeded)
            {   
                QTEActive = false;
                currentFireball.SetDamagePlayer(true);
                if (SoundManager.Instance)
                {
                    AudioSource.PlayClipAtPoint(fireballLaunchSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
                }
            }
            else
            {
                currentFireball.SetDamagePlayer(false);
                currentFireball.SetAttack(fireballStats.GetFireballQTEAttack(), 20f);
                GameObject qteVFX = Instantiate(fireballQTEVFX, fireballEmitter);
                Destroy(qteVFX, 3f);
                if (SoundManager.Instance)
                {
                    AudioSource.PlayClipAtPoint(fireballLaunchQTESFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
                }
            }
            fireballLaunched = true;
        }
    }
}
