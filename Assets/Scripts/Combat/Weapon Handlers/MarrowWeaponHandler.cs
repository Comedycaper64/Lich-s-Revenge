using System.Collections;
using System.Collections.Generic;
using Stats;
using Units.Enemy.Marrow;
using UnityEngine;

public class MarrowWeaponHandler : MonoBehaviour
{
    private MarrowStats stats;
    private MarrowStateMachine stateMachine;

    [Header("Fireball")]
    [SerializeField] private GameObject fireballPrefab;
    public Transform fireballEmitter;
    public Transform fireballVisual;
    private FireBallProjectile currentFireball;

    [Header("Flame Pillar")]
    [SerializeField] private GameObject flamePillarPrefab;
    public GameObject flamePillarVisual;
    private List<Vector3> flamePillarLocations = new List<Vector3>();
    private List<GameObject> currentFlamePillarVisuals = new List<GameObject>();
    //private List<FlamePillar> currentFlamePillars = new List<FlamePillar>();

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
                TryGetEnemySpawnLocation(out spawnLocation);
            }   
            GameObject enemySpawn = stateMachine.summonableEnemies[Random.Range(0, stateMachine.summonableEnemies.Length)];

            Instantiate(enemySpawn, spawnLocation, Quaternion.identity);
        }
        stateMachine.Cooldowns.SetSummonCooldown();
    }

    private bool TryGetEnemySpawnLocation(out Vector3 spawnLocation)
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

    public void SetFlamePillarVisuals()
    {
        flamePillarLocations.Clear();
        Vector3 arenaCentreLocation = new Vector3(0, 0, 50);
        for (int i = 0; i < stats.GetFlamePillarNumber(); i++)
        {
            flamePillarLocations.Add(new Vector3(
                arenaCentreLocation.x + Random.Range(-stats.GetFlamePillarSpawnArea(), stats.GetFlamePillarSpawnArea()), 
                arenaCentreLocation.y, 
                arenaCentreLocation.z + Random.Range(-stats.GetFlamePillarSpawnArea(), stats.GetFlamePillarSpawnArea())));
            currentFlamePillarVisuals.Add(Instantiate(flamePillarVisual, flamePillarLocations[i], Quaternion.identity));
            currentFlamePillarVisuals[i].transform.localScale = new Vector3(stats.GetFlamePillarRadius(), 10, stats.GetFlamePillarRadius());
        }
    }

    public void SpawnFlamePillars()
    {
        ClearFlamePillarVisuals();
        foreach(Vector3 pillarPosition in flamePillarLocations)
        {
            FlamePillar newPillar = Instantiate(flamePillarPrefab, pillarPosition, Quaternion.identity).GetComponent<FlamePillar>();
            newPillar.transform.LookAt(stateMachine.Player.transform);
            newPillar.SetCasterCollider(stateMachine.Controller);
            newPillar.SetDamage(stats.GetFlamePillarAttack());
            newPillar.SetMovementSpeed(stats.GetFlamePillarMovement());
            //newPillar.SetPillarRadius(stats.GetFlamePillarRadius());
            newPillar.SetTimeToLive(stats.GetFlamePillarTimeToLive());
        }
        stateMachine.Cooldowns.SetFlamePillarCooldown();
    }

    public void ClearFlamePillarVisuals()
    {
        foreach(GameObject visual in currentFlamePillarVisuals)
        {
            Destroy(visual);
        }
        currentFlamePillarVisuals.Clear();
    }
}
