using System;
using System.Collections;
using System.Collections.Generic;
using Units.Player;
using Stats;
using UnityEngine;
using UnityEngine.SceneManagement;

//Tracks what enemies are in a level and respawns them if the player respawns
public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    private List<EnemySpawn> enemySpawns = new List<EnemySpawn>();

    [Header("Enemy Types")]
    [SerializeField] private GameObject minerEnemy;
    [SerializeField] private GameObject rangerEnemy;
    [SerializeField] private GameObject gunnerEnemy;
    [SerializeField] private GameObject hammererEnemy;
    [SerializeField] private GameObject sentinelEnemy;
    [SerializeField] private GameObject kingAlaric;

    private void Awake() 
    {   
        PlayerStateMachine.OnRespawn += RespawnEnemies;
        //A list of enemyspawns is generated when the scene starts
        foreach(GameObject enemy in enemies)
        {
            GameObject newEnemy;

            if (enemy.GetComponent<DwarfMinerStats>())
            {
                newEnemy = minerEnemy;
            }
            else if (enemy.GetComponent<DwarfRangerStats>())
            {
                newEnemy = rangerEnemy;
            }
            else if (enemy.GetComponent<DwarfGunnerStats>())
            {
                newEnemy = gunnerEnemy;
            }
            else if (enemy.GetComponent<DwarfHammererStats>())
            {
                newEnemy = hammererEnemy;
            }
            else if (enemy.GetComponent<AlaricDeathTrigger>())
            {
                newEnemy = kingAlaric;
            }
            else if (enemy.GetComponent<DwarfSentinelStats>())
            {
                newEnemy = sentinelEnemy;
            }
            else
            {
                newEnemy = null;
            }

            enemySpawns.Add(new EnemySpawn(newEnemy, Vector3.zero, Quaternion.identity));
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            enemySpawns[i].SetSpawnPoint(enemies[i].transform.position);
            enemySpawns[i].SetSpawnRotation(enemies[i].transform.rotation);
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDestroy() 
    {
        PlayerStateMachine.OnRespawn -= RespawnEnemies;
    }

    private void RespawnEnemies()
    {
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();
        foreach (EnemySpawn enemy in enemySpawns)
        {
            enemies.Add(Instantiate(enemy.GetUnit(), enemy.GetSpawnPoint(), enemy.GetSpawnRotation()));
        }
    }
}
