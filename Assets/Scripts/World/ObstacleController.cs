using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Obstacle[] obstacles;
    [Range(0.1f,20f)] 
    [SerializeField] private float obstacleSpeed = 3.0f;
    [Range(0.1f, 4f)] 
    [SerializeField] private float obstacleFrequency = 1.0f;
    [Space]
    [Range(5f, 40f)] 
    [SerializeField] private float obstacleSpawnX = 10f;
    [Range(1f, 3f)] 
    [SerializeField] private float obstacleSpawnYMax = 2f;
    [Range(-1f, 1f)] 
    [SerializeField] private float obstacleSpawnYOffset = 0.2f;
    [Range(0.5f, 5f)] 
    [SerializeField] private float obstacleOpeningSizeY = 1f;

    private int avaliableObstacleTypes = 0;
    private int maxObstacles = 5;

    private GameObject[] obstacleInstances;
    private int lastRespawnedObstacle = 0;

    private bool moveObstacles = true;
    private Coroutine obstacleRespawner;
    public void Bomb()
    {
        GameObject obs;
        for (int i = 0; i < maxObstacles; i++)
        {
            obs = obstacleInstances[i];
            if (obs)
            {
                GameObject childTop = obs.transform.GetChild(0).gameObject;
                GameObject childBot = obs.transform.GetChild(1).gameObject;
                GameObject childMid = obs.transform.GetChild(2).gameObject;
                if (Camera.main.WorldToViewportPoint(obs.transform.position,Camera.MonoOrStereoscopicEye.Mono).x < 1)
                {
                    // Adding slide-away effect where top part of the obstacle slides upwards and bottom part slides downwards
                    childTop.AddComponent<Rigidbody2D>();
                    childBot.AddComponent<Rigidbody2D>();
                    childTop.GetComponent<Rigidbody2D>().gravityScale = 0;
                    childBot.GetComponent<Rigidbody2D>().gravityScale = 0;
                    childTop.GetComponent<Rigidbody2D>().velocity = Vector3.up * childTop.transform.position.y;
                    childBot.GetComponent<Rigidbody2D>().velocity = Vector3.up * childBot.transform.position.y;

                    Destroy(childMid); //Destroying the collider awarding points from each set of obstacles

                    // Disabling the colliders to avoid accidental collisions after bombing
                    childTop.GetComponent<Collider2D>().enabled = false;
                    childBot.GetComponent<Collider2D>().enabled = false;

                    // destroying objects after they move away, code for spawning when null already exists
                    Destroy(obs, 1.3f);
                    obstacleInstances[i] = null;
                }
            }
            
        }
    }

    public void StopObstacles()
    {
        StopCoroutine(obstacleRespawner);
        foreach (GameObject obs in obstacleInstances)
        {
            if (obs)
            {
                obs.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            }
        }
    }   
    public void StartObstacles()
    {
        StartCoroutine(ReSpawnObstacles());
    }

    private void Start()
    {
        obstacles.OrderBy(x => x.minScore); // sorting obstacle types by minimum score required for them to appear.

        foreach (Obstacle obs in obstacles)
        {
            if (obs.minScore == 0 )
            {
                avaliableObstacleTypes++;
            }
        }
        obstacleInstances = new GameObject[15];

        obstacleRespawner = StartCoroutine(ReSpawnObstacles()); //this coroutine continually recreates oldest obstacles as new obstacles
    
    }

    private void SpawnObstacle(int obstacleNum)
    {
        GameObject obs = obstacleInstances[obstacleNum];
        Vector2 spawnPos;
        float offset = obstacleSpawnYOffset;

        
        // setting possition 
        spawnPos = new Vector2(obstacleSpawnX, (Random.Range(-obstacleSpawnYMax + offset, obstacleSpawnYMax + offset)));
        obstacleInstances[obstacleNum].transform.position = spawnPos;


        

        // assigning sprites and colors to obstacles
        // obs.transform.GetChild(0) returns top part of obstacle
        // obs.transform.GetChild(1) returns bottom part of obstacle
        Obstacle obstacleToUse = obstacles[Random.Range(0, avaliableObstacleTypes)];
        SpriteRenderer sr;
        sr = obs.transform.GetChild(0).GetComponent<SpriteRenderer>();
        sr.sprite = obstacleToUse.obstacleSpriteTop;
        sr.color = obstacleToUse.obstacleColorTop;
        
        sr = obs.transform.GetChild(1).GetComponent<SpriteRenderer>();
        sr.sprite = obstacleToUse.obstacleSpriteBot;
        sr.color = obstacleToUse.obstacleColorBot;

    }

    private IEnumerator ReSpawnObstacles()
    {
        while (moveObstacles)
        {
            if(!obstacleInstances[lastRespawnedObstacle]) // checking if an obstacle is already instantiated
            {
                GameObject obs = Instantiate(obstaclePrefab, transform);

                // assigning velocity to obstacle
                Vector2 speed = Vector2.left * obstacleSpeed;
                obs.GetComponent<Rigidbody2D>().velocity = speed;
                obs.transform.GetChild(0).position += Vector3.up * obstacleOpeningSizeY / 2;
                obs.transform.GetChild(1).position -= Vector3.up * obstacleOpeningSizeY / 2;

                obstacleInstances[lastRespawnedObstacle] = obs;

            }

            SpawnObstacle(lastRespawnedObstacle); // Set default position to obstacle, and assign a random scriptable obstacle to it
            
            if (lastRespawnedObstacle >= maxObstacles-1) //looping through all obstacles in order, then going back to the first one
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


}
