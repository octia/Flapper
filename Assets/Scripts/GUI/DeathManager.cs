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
    [SerializeField] private GameObject newTopGO;


    private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
    private const string TWEET_LANGUAGE = "en";

    private GamestateManager gameManager;
    private TMP_Text score;
    private TMP_Text[] topScores;

    private int topScoreCount = 5;

    private int currScore = 0;
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
    
    public void TweetScore()
    {
        string tweet = "I've reached " + currScore + " points in Flappy Witch, and you can too! Download it, and try to beat me! [LINK HERE]";
        
        Application.OpenURL(TWITTER_ADDRESS +
                    "?text=" + WWW.EscapeURL(tweet) +
                    "&amp;lang=" + WWW.EscapeURL(TWEET_LANGUAGE));
    }

    private void UpdateMaxScores(int newScore)
    {
        currScore = newScore;
        int topScore;
        bool newTopAdded = false ;
        for (int i = 0; i < 5; i++)
        {
            string key = "Top" + i.ToString();
            topScore = PlayerPrefs.GetInt(key, -1);
            if (newScore > topScore && !newTopAdded)
            {
                topScore = newScore;
                PlayerPrefs.SetInt(key, newScore);
                topScores[i].color = Color.red;
                topScores[i].fontStyle = TMPro.FontStyles.Bold;
                newTopGO.SetActive(true);
                newTopAdded = true;
            }
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
            topScores[i] = topScoreGO.transform.GetChild(i).GetComponent<TMP_Text>();
        }
    }



}
