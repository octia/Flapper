using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public Obstacle[] obstacles;
    public GameObject obstaclePrefab;
    [Range(0.1f,20f)]public float obstacleSpeed = 3.0f;
    public float obstacleFrequency = 1.0f;
    [Space]
    [Range(5f, 40f)] public float obstacleSpawnX = 10f;
    public float obstacleSpawnY = 3f;

    private int avaliableObstacleTypes = 0;
    private int maxObstacles = 15;


    private GameObject[] obstacleInstances;
    private int lastRespawnedObstacle = 0;

    void Start()
    {
        obstacles.OrderBy(x => x.minScore);
        obstacleInstances = new GameObject[15];

        StartCoroutine("ReSpawnObstacles"); //this coroutine respawns last respawned obstacle as a new obstacle
    
    }
    

    void SpawnObstacle(int obstacleNum)
    {
        Vector2 spawnPos = new Vector2(0, 0);
        bool isBottom = false;
        isBottom = (Random.Range(0, 2) == 1 ? true : false);
        if (!isBottom)
        {
            spawnPos = new Vector2(obstacleSpawnX, obstacleSpawnY);
        }
        else
        {
            spawnPos = new Vector2(obstacleSpawnX, obstacleSpawnY * -1);
        }
        obstacleInstances[obstacleNum].transform.position = spawnPos;
    }

    IEnumerator ReSpawnObstacles()
    {
        while (true)
        {
            if(!obstacleInstances[lastRespawnedObstacle])
            {
                Vector2 speed = new Vector2(-obstacleSpeed, 0);

                obstacleInstances[lastRespawnedObstacle] = Instantiate(obstaclePrefab, transform);
                obstacleInstances[lastRespawnedObstacle].GetComponent<Rigidbody2D>().velocity = speed;

            }
            SpawnObstacle(lastRespawnedObstacle);
            if (lastRespawnedObstacle >= maxObstacles-1)
            {
                lastRespawnedObstacle = 0;
            }
            else
            {
                lastRespawnedObstacle++;
            }   
            yield return new WaitForSeconds(obstacleFrequency);
        }
        
    }


    void FixedUpdate()
    {
        
    }
}
