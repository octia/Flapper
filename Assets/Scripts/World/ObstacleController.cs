using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public Obstacle[] obstacles;
    public GameObject obstaclePrefab;
    [Range(0.1f,20f)]public float obstacleSpeed = 3.0f;
    [Range(0.1f, 4f)] public float obstacleFrequency = 1.0f;
    [Space]
    [Range(5f, 40f)] public float obstacleSpawnX = 10f;
    [Range(1f, 3f)] public float obstacleSpawnYMax = 2f;
    [Range(-1f, 1f)] public float obstacleSpawnYOffset = 0.2f;
    [Range(0.5f, 5f)] public float obstacleOpeningSizeY = 1f;

    private int avaliableObstacleTypes = 1;
    private int maxObstacles = 8;

    private GameObject[] obstacleInstances;
    private int lastRespawnedObstacle = 0;

    void Start()
    {
        obstacles.OrderBy(x => x.minScore);
        obstacleInstances = new GameObject[15];

        StartCoroutine("ReSpawnObstacles"); //this coroutine continually respawns last respawned obstacle as a new obstacle
    
    }
    

    void SpawnObstacle(int obstacleNum)
    {
        Vector2 spawnPos;
        bool isBottom;
        isBottom = (Random.Range(0, 2) == 1 ? true : false);
        float offset = obstacleSpawnYMax;
        spawnPos = new Vector2(obstacleSpawnX, (Random.Range(-obstacleSpawnYMax + offset, obstacleSpawnYMax + offset)));
        obstacleInstances[obstacleNum].transform.position = spawnPos;
    }

    IEnumerator ReSpawnObstacles()
    {
        while (true)
        {
            if(!obstacleInstances[lastRespawnedObstacle])
            {
                Vector2 speed = new Vector2(-obstacleSpeed, 0);
                GameObject obs = Instantiate(obstaclePrefab, transform);
                
                obs.GetComponent<Rigidbody2D>().velocity = speed;
                obs.transform.GetChild(0).position += Vector3.up * obstacleOpeningSizeY / 2;
                obs.transform.GetChild(1).position -= Vector3.up * obstacleOpeningSizeY / 2;

                obstacleInstances[lastRespawnedObstacle] = obs;

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
