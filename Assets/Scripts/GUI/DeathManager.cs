using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class DeathManager : MonoBehaviour
{
    [Header("GameObjects: ")]
    [SerializeField] private GameObject gameStateManagerGO;
    [SerializeField] private GameObject deathScoreGO;
    [SerializeField] private GameObject topScoreGO;
    
    private GamestateManager gameManager;
    private TMP_Text score;
    private TMP_Text[] topScores;

    private int topScoreCount = 5;

    public void SetCurrentScore(int scoreCount)
    {
        score.text = scoreCount.ToString();
        UpdateMaxScores(scoreCount);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reloads current scene, effectively restarting the game
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateMaxScores(int newScore)
    {
        int topScore;
        for (int i = topScoreCount-1; i >= 0; i--)
        {
            topScore = PlayerPrefs.GetInt("Top"+i.ToString(), -1);
            if (topScore != -1)
            {
                topScores[i].text = topScore.ToString();
            }
            else
            {
                topScores[i].text = "--";
            }
        }
    }


    private void Awake()
    {
        gameManager = gameStateManagerGO.GetComponent<GamestateManager>();
        score = deathScoreGO.GetComponent<TMP_Text>();
        topScores = new TMP_Text[5];
        for (int i = 0; i < topScoreCount; i++)
        {
            topScores[i] = GetComponent<TMP_Text>();
        }
    }



}
