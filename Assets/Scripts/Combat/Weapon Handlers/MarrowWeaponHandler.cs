using System.Collections;
using System.Collections.Generic;
using Stats;
using Units.Enemy.Marrow;
using UnityEngine;

public class MarrowWeaponHandler : MonoBehaviour
{
    private MarrowStats stats;
    private MarrowStateMachine stateMachine;

    [SerializeField] private GameObject fireballPrefab;
    public Transform fireballEmitter;
    public Transform fireballVisual;
    private FireBallProjectile currentFireball;

    [Header("SFX")]
    [SerializeField] private AudioClip fireballCastSFX;
    [SerializeField] private AudioClip fireballLaunchSFX;

    private void Awake() 
    {
        stats = GetComponent<MarrowStats>(); 
        stateMachine = GetComponent<MarrowStateMachine>();   
        float explodeRadius = stats.GetFireballExplodeRadius() * 2.6f; //fireball visual is smaller than it should be due to lich parent, so needs 2.6 buff
        fireballVisual.localScale = new Vector3(explodeRadius, explodeRadius, explodeRadius);
    }

    public void SpawnFireball()
    {
        currentFireball = Instantiate(fireballPrefab, fireballEmitter.transform).GetComponent<FireBallProjectile>();
        currentFireball.SetAttack(stats.GetFireballAttack(), 20f);
        currentFireball.SetTimeToLive(stats.GetFireballDetonationTime());
        currentFireball.SetPlayerCollider(gameObject.GetComponent<CharacterController>());
        currentFireball.SetDamagePlayer(true);
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(fireballCastSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
    }

    public void LaunchFireball()
    {
        currentFireball.transform.SetParent(null);
        currentFireball.transform.LookAt(stateMachine.Player.transform);
        currentFireball.EnableCollider(true);
        currentFireball.SetProjectileSpeed(stats.GetFireballSpellProjectileSpeed());
        currentFireball.SetExplodeRadius(stats.GetFireballExplodeRadius());
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(fireballLaunchSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
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

    public void SpawnFlamePillars()
    {
        //Instantiate a few pillars around the map (not super close to lich) that move around slightly
    }
}
