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

    private bool[] usedSpawnLocation;

    private void Awake() 
    {
        stats = GetComponent<MarrowStats>(); 
        stateMachine = GetComponent<MarrowStateMachine>();   
        float explodeRadius = stats.GetFireballExplodeRadius() * 1.6f; //fireball visual is smaller than it should be due to lich parent, so needs 2.6 buff
        fireballVisual.localScale = new Vector3(explodeRadius, explodeRadius, explodeRadius);
        fireballVisual.SetParent(null);
    }

    public void SummonEnemies()
    {
        usedSpawnLocation = new bool[stateMachine.movementWaypoints.Length];
        for (int i = 0; i < stats.GetEnemySpawnNumber(); i++)
        {
            Vector3 spawnLocation = Vector3.zero;
            while(spawnLocation == Vector3.zero)
            {
                TryGetSpawnLocation(out spawnLocation);
            }   
            GameObject enemySpawn = stateMachine.summonableEnemies[Random.Range(0, stateMachine.summonableEnemies.Length)];

            Instantiate(enemySpawn, spawnLocation, Quaternion.identity);
        }
        stateMachine.Cooldowns.SetSummonCooldown();
    }

    private bool TryGetSpawnLocation(out Vector3 spawnLocation)
    {
        int spawnIndex = Random.Range(0, stateMachine.movementWaypoints.Length);
        spawnLocation = Vector3.zero;
        if (usedSpawnLocation[spawnIndex])
        {
            return false;
        }
        else
        {
            spawnLocation = stateMachine.movementWaypoints[spawnIndex].position;
            usedSpawnLocation[spawnIndex] = true;
            return true;
        }
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
        stateMachine.Cooldowns.SetFireballCooldown();
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
