using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpawn
{
    [SerializeField] private GameObject enemyUnit;
    [SerializeField] private Vector3 spawnPosition;

    public EnemySpawn(GameObject enemyUnit, Vector3 spawnPosition)
    {
        this.enemyUnit = enemyUnit;
        this.spawnPosition = spawnPosition;
    }

    public GameObject GetUnit()
    {
        return enemyUnit;
    }
    public Vector3 GetSpawnPoint()
    {
        return spawnPosition;
    }
}
