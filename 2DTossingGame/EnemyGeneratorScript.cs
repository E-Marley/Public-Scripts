using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyGeneratorScript : MonoBehaviour
{
    public List<GameObject> pooledEnemies;
    [SerializeField] GameObject enemy;
    [SerializeField] int enemyPoolSize;
    [SerializeField] float spawnRange;
    [SerializeField] int minSpawnInRange;
    [SerializeField] int maxSpawnInRange;
    float spawnRate = 1f;
   
    private void OnEnable()
    {
        GameEvents.OnPlayerStart += StartSpawning;
        GameEvents.OnPlayerStop += StopSpawning;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerStart -= StartSpawning;
        GameEvents.OnPlayerStop -= StopSpawning;
    }

    void Start()
    {
        //Create deactivated object pool on start
        pooledEnemies = new List<GameObject>();
        for (int i = 0; i < enemyPoolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(enemy);
            obj.SetActive(false);
            pooledEnemies.Add(obj);
        }
    }


    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledEnemies.Count; i++)
        {
            if (!pooledEnemies[i].activeInHierarchy)
            {
                return pooledEnemies[i];
            }
        }  
        return null;
    }


    private void StartSpawning()
    {
        InvokeRepeating("CreateEnemiesOffScreen", 0, spawnRate);
    }

    private void StopSpawning()
    {
        CancelInvoke("CreateEnemiesOffScreen");
    }

    private void CreateEnemiesOffScreen()
    {
        int randomNumber = Random.Range(minSpawnInRange, maxSpawnInRange);
        for (int i = 0; i < randomNumber; i++)
        {
            GenerateEnemy();
        }
    }

    private void GenerateEnemy()
    {
        GameObject obstacle = GetPooledObject();
        if (obstacle != null)
        {
            obstacle.transform.position = new Vector2(transform.position.x + Random.Range(-spawnRange, spawnRange), transform.position.y);
            obstacle.SetActive(true);
        }
    }
}
