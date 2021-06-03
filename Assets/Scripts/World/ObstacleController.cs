using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public Obstacle[] obstacles;
    public GameObject obstaclePrefab;
    public float obstacleSpeed = 1.0f;
    public float obstacleFrequency = 1.0f;

    private int avaliableObstacleTypes = 0;
    private int maxObstacles;


    private GameObject[] obstacleInstances;


    void Start()
    {
        obstacles.OrderBy(x => x.minScore);
        
    
    }

    IEnumerator SpawnObstacles()
    {
        yield return null;
    }


    void Update()
    {
        
    }
}
