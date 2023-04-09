using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used by the LevelManager to store information about an enemy. This information is used when respawning them
[Serializable]
public class EnemySpawn
{
    [SerializeField] private GameObject enemyUnit;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    public EnemySpawn(GameObject enemyUnit, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        this.enemyUnit = enemyUnit;
        this.spawnPosition = spawnPosition;
        this.spawnRotation = spawnRotation;
    }

    public GameObject GetUnit()
    {
        return enemyUnit;
    }
    public Vector3 GetSpawnPoint()
    {
        return spawnPosition;
    }

    public Quaternion GetSpawnRotation()
    {
        return spawnRotation;
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPosition = newSpawnPoint;
    }

    public void SetSpawnRotation(Quaternion newSpawnRotation)
    {
        spawnRotation = newSpawnRotation;
    }
}
