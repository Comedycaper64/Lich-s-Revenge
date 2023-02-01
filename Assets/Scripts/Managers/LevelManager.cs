using System;
using System.Collections;
using System.Collections.Generic;
using Units.Player;
using Stats;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //[SerializeField] private GameObject levelButton;
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    private List<EnemySpawn> enemySpawns = new List<EnemySpawn>();
    //private PlayerStateMachine Player;

    [Header("Enemy Types")]
    [SerializeField] private GameObject minerEnemy;
    [SerializeField] private GameObject rangerEnemy;
    [SerializeField] private GameObject gunnerEnemy;
    [SerializeField] private GameObject hammererEnemy;

    private void Awake() 
    {   
        //Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
        //StateMachine.OnEnemyUnitDead += EnemyDied;
        PlayerStateMachine.OnRespawn += RespawnEnemies;

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
            else
            {
                newEnemy = null;
            }

            enemySpawns.Add(new EnemySpawn(newEnemy, Vector3.zero));
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            enemySpawns[i].SetSpawnPoint(enemies[i].transform.position);
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

    // private void EnemyDied(object sender, GameObject enemy)
    // {
    //     if (enemies.Contains(enemy))
    //     {
    //         enemies.Remove(enemy);
    //     }
    // }

    private void RespawnEnemies()
    {
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();
        foreach (EnemySpawn enemy in enemySpawns)
        {
            enemies.Add(Instantiate(enemy.GetUnit(), enemy.GetSpawnPoint(), Quaternion.identity));
        }
    }
}
