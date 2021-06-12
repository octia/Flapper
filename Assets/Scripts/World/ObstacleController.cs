using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [Header("Obstacle Parameters")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Obstacle[] obstacles;
    [Range(0.1f, 20f)]
    [SerializeField] private float obsSpeed = 3.0f;
    [Range(0.1f, 4f)]
    [SerializeField] private float obsFrequency = 1.0f;
    [Header("Obstacle Spawning Parameters")]
    [Space]
    [Range(5f, 40f)] 
    [SerializeField] private float obsSpawnX = 10f;
    [Header("In viewport space:")]
    [Range(0f, 0.5f)] 
    [SerializeField] private float obsSpawnYMax = 0.35f;
    [Range(-1f, 1f)] 
    [SerializeField] private float obsSpawnYOffset = 0.2f;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float obsOpeningSizeY = 0.2f;

    [Header("Bomb parameters:")]
    [Range(0f, 15f)]
    [SerializeField] private float bombPushingPower= 3f;


    private int avaliableObstacleTypes = 0;
    private int maxObstacles = 10;

    private float lastObstacleSpeed = 0f;

    private GameObject[] obsInstances;
    private int lastRespawnedObstacle = 0;

    private Coroutine obsRespawner;
    
    public void Bomb()
    {
        GameObject obs;
        for (int i = 0; i < maxObstacles; i++)
        {
            obs = obsInstances[i];
            if (obs)
            {
                if (Camera.main.WorldToViewportPoint(obs.transform.position,Camera.MonoOrStereoscopicEye.Mono).x < 1) // checking if obs is visible
                {
                    GameObject child;
                    for (int childID = 0; childID < obs.transform.childCount; childID++)
                    {
                        child = obs.transform.GetChild(childID).gameObject;
                        
                        if (child.transform.tag == "Reward")
                        {
                            Destroy(child); // destroying to avoid adding points for bombed obstacles
                        }
                        else
                        {
                            // Adding slide-away effect where top part of the obs slides upwards and bottom part slides downwards
                            child.AddComponent<Rigidbody2D>();
                            child.GetComponent<Rigidbody2D>().gravityScale = 0;
                            child.GetComponent<Rigidbody2D>().velocity = Vector3.up * bombPushingPower *(child.transform.position.y > 0 ? 1 : -1);

                            // Disabling the colliders to avoid accidental collisions after bombing
                            child.GetComponent<Collider2D>().enabled = false;
                        }

                    }

                    Destroy(obs, 1.5f);
                }
            }
            
        }
    }

    public void StopObstacles()
    {
        StopCoroutine(obsRespawner);
        foreach (GameObject obs in obsInstances)
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

    public void NewScore(int score)
    {
        while (avaliableObstacleTypes < obstacles.Length)
        {
            if (score >= obstacles[avaliableObstacleTypes].minScore) // avaliableObstacleTypes < obstacles.Length 
            {
                avaliableObstacleTypes++;
            }
            else
            {
                break;
            }
        }
    }

    private void SpawnObstacle(int obsNum)
    {
        GameObject obs = obsInstances[obsNum];
        Transform child;
        AdjustScreenSize adjuster;
        float holePos;
        float offset = obsSpawnYOffset;
        float obsChildSize;

        // assigning sprites and colors to obstacles
        // obs.transform.GetChild(0) returns top part of obs
        // obs.transform.GetChild(1) returns bottom part of obs
        Obstacle obsToUse = obstacles[Random.Range(0, avaliableObstacleTypes)];
        SpriteRenderer sr;
        sr = obs.transform.GetChild(0).GetComponent<SpriteRenderer>();
        sr.sprite = obsToUse.obstacleSpriteTop;
        sr.color = obsToUse.obstacleColorTop;

        sr = obs.transform.GetChild(1).GetComponent<SpriteRenderer>();
        sr.sprite = obsToUse.obstacleSpriteBot;
        sr.color = obsToUse.obstacleColorBot;


        // setting possition 
        holePos = 0.5f + offset + Random.Range(-obsSpawnYMax, obsSpawnYMax);

        for (int i = 0; i < obs.transform.childCount; i++)
        {
            child = obs.transform.GetChild(i);
            adjuster = child.GetComponent<AdjustScreenSize>();
            if (adjuster)
            {
                child.localPosition /= Mathf.Abs(child.position.y); // assumes child has x = 0, z = 0, y != 0, sets y to 1 or -1

                // Sets child localPosition to the distance between edge of hole and edge of screen
                // also appropirately changes child size to fill the entire space
                if (child.localPosition.y > 0)
                {
                    obsChildSize = (1-holePos - (obsOpeningSizeY) / 2f);
                    child.localPosition *= Camera.main.ViewportToWorldPoint(Vector2.up * (holePos + (obsOpeningSizeY+ obsChildSize) / 2)).y;
                }
                else
                {
                    obsChildSize = (holePos - (obsOpeningSizeY) / 2f);
                    child.localPosition *= -Camera.main.ViewportToWorldPoint(Vector2.up * (holePos - (obsOpeningSizeY + obsChildSize) / 2)).y;
                }
                Destroy(child.GetComponent<BoxCollider2D>());
                child.gameObject.AddComponent<BoxCollider2D>();
                // adjusts child size to fill the entire space it is supposed to fill (between hole and enge of screen)
                adjuster.desiredY = obsChildSize;
                adjuster.Adjust();
                
            }
        }


        
    }

    private IEnumerator ReSpawnObstacles()
    {
        Vector2 obsVelocity = Vector2.left * obsSpeed;
        while (true)
        {
            if (lastObstacleSpeed != obsSpeed)
            {
                obsVelocity = Vector2.left * obsSpeed;
                lastObstacleSpeed = obsSpeed;
            }
            GameObject obs = obsInstances[lastRespawnedObstacle];
            if (!obs) // checking if an obs is already instantiated
            {
                // instantinating and assigning velocity to obs
                obs = Instantiate(obstaclePrefab, transform);
                obs.GetComponent<Rigidbody2D>().velocity = obsVelocity;
                obsInstances[lastRespawnedObstacle] = obs;
            }

            obs.transform.position = Vector2.right * obsSpawnX;


            SpawnObstacle(lastRespawnedObstacle); // Set default position to obs, and assign a random scriptable obs template to it

            if (lastObstacleSpeed != obsSpeed)
            {
                obsInstances[lastRespawnedObstacle].GetComponent<Rigidbody2D>().velocity = obsVelocity;
            }


            if (lastRespawnedObstacle >= maxObstacles - 1) //looping through all obstacles in order, then going back to the first one
            {
                lastRespawnedObstacle = 0;
            }
            else
            {
                lastRespawnedObstacle++;
            }

            yield return new WaitForSeconds(1 / obsFrequency); // increasing frequency decreases wait time
        }

    }

    private void Start()
    {
        obstacles.OrderBy(x => x.minScore); // sorting obs types by minimum score required for them to appear.

        foreach (Obstacle obs in obstacles)
        {
            if (obs.minScore == 0 )
            {
                avaliableObstacleTypes++;
            }
        }
        obsInstances = new GameObject[maxObstacles];

        obsRespawner = StartCoroutine(ReSpawnObstacles()); //this coroutine continually recreates oldest obstacles as new obstacles
    
    }



}
