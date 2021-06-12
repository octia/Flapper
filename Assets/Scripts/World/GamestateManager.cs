using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamestateManager : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject scoreGO;
    [SerializeField] private GameObject bombGO;
    [SerializeField] private GameObject playerGO;
    [SerializeField] private GameObject obstacleGO;
    [SerializeField] private GameObject deathGO;
    
    [Header("Counters")]
    [SerializeField] private int scoreCount = 0;
    [SerializeField] private int bombCount = 0;

    [Header("Bomb Parameters")]
    [Range(1, 10)] 
    [SerializeField] private int bombAwardFrequency = 10;
    [Range(1, 10)] 
    [SerializeField] private int bombCountMax = 3;

    private string scoreText = "";
    private string bombText = "Bombs: ";

    private TMP_Text scoreHolder;
    private TMP_Text bombHolder;
    private ObstacleController obstacles;
    private PlayerController player;
    private DeathManager deathManager;
    
    // Start is called before the first frame update
    private void Start()
    {
        Application.targetFrameRate = -1;
        scoreHolder = scoreGO.GetComponent<TMP_Text>();
        bombHolder = bombGO.GetComponent<TMP_Text>();
        obstacles = obstacleGO.GetComponent<ObstacleController>();
        player = playerGO.GetComponent<PlayerController>();
        deathManager = deathGO.GetComponent<DeathManager>();
        
        UpdateCounters();
    }

    public void UseBomb()
    {
        if (bombCount > 0)
        {
            obstacles.Bomb();
            bombCount--;
            UpdateCounters();
        }
    }

    public void Die()
    {
        obstacles.StopObstacles();
        deathGO.SetActive(true);
        deathManager.SetCurrentScore(scoreCount);
        gameObject.SetActive(false);
    }

    public void Resurrect()
    {
        obstacles.StartObstacles();
        obstacles.Bomb();
    }
    
    public void AddPoint(int amount = 1)
    {
        if (bombCount <= bombCountMax)
        {

            /// Explanation:
            /// the IF below this comment works exactly as:
            /// for (int i = 0; i < amount; i++)
            /// {
            ///     scoreCount++;
            ///     if (scoreCount % bombAwardFrequency == 0 && scoreCount != 0)
            ///     {
            ///         if (bombCount < bombCountMax)
            ///             bombCount++;
            ///     }
            /// }
            /// It has been modified for efficiency, as modulo operations in for loops tend to be resource expensive.
            /// It shouldn't matter for current scoring system.
            /// But if, for exaple, 10 points were added per FixedUpdate, it could affect preformance.
            /// The code below relies on the fact that casting from floats to ints always returns a number rounded down.
            if (((int)(((float)(scoreCount + amount)) / bombAwardFrequency)) != ((int)((float)scoreCount /bombAwardFrequency)))
            {
                bombCount += (int)((float)(scoreCount + amount) / bombAwardFrequency) - (int)((float)scoreCount / bombAwardFrequency);
                if (bombCount > bombCountMax)
                {
                    bombCount = bombCountMax;
                }
            }
        }

        scoreCount += amount;
        UpdateCounters();
        obstacles.NewScore(scoreCount); // todo: change to event?
    }

    private void UpdateCounters()
    {
        if (scoreGO)
        {
            scoreHolder.text = scoreText + scoreCount.ToString();

        }
        if (bombGO)
        {
            bombHolder.text = bombText + bombCount.ToString();
        }
    }

}
