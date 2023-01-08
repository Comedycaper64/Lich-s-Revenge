using System;
using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //[SerializeField] private GameObject levelButton;
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private List<EnemySpawn> enemySpawns = new List<EnemySpawn>();
    private PlayerStateMachine Player;

    private void Awake() 
    {   
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
        StateMachine.OnEnemyUnitDead += EnemyDied;
        Player.OnRespawn += RespawnEnemies;
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
        Player.OnRespawn -= RespawnEnemies;
    }

    private void EnemyDied(object sender, GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
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
            enemies.Add(Instantiate(enemy.GetUnit(), enemy.GetSpawnPoint(), Quaternion.identity));
        }
    }
}
