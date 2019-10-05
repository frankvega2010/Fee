﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject enemyTemplate;
    public GameObject entryWaypoint;
    public KeyCode spawnKey;
    public bool automaticSpawn;

    [Header("Speed Settings")]
    public float minSpeed;
    public float maxSpeed;

    [Header("Time Settings")]
    public float minTime;
    public float maxTime;

    private float spawnTimer;
    private float finalSpawnTime;
    private NewEnemy enemyProperties;

    private void Start()
    {
        enemyProperties = enemyTemplate.GetComponent<NewEnemy>();
        SetRandomSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(automaticSpawn)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= finalSpawnTime)
            {
                SpawnEnemy();
                SetRandomSpawnTime();
                spawnTimer = 0;
            }
        }

        if(Input.GetKeyDown(spawnKey))
        {
                SpawnEnemy();
        }
    }

    private void SetRandomSpawnTime()
    {
        finalSpawnTime = Random.Range(minTime, maxTime);
    }

    public void SpawnEnemy()
    {
        if (CanSpawn())
        {
            enemyProperties.speed = Random.Range(minSpeed, maxSpeed);
            enemyProperties.initialWaypoint = entryWaypoint;
            GameObject newEnemy = Instantiate(enemyTemplate);
            newEnemy.transform.position = entryWaypoint.transform.position;
            newEnemy.SetActive(true);
            //GameManager.Get().enemies.Add(newEnemy);
        }
            
    }

    public bool CanSpawn()
    {
        if(GameManager.Get().enemies.Count < GameManager.Get().maxEnemies)
        {
            return true;
        }

        return false;
    }
}