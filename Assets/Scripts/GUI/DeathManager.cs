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


    private void Awake()
    {
        gameManager = gameStateManagerGO.GetComponent<GamestateManager>();
        score = deathScoreGO.GetComponent<TMP_Text>();
    }

    public void SetCurrentScore(int scoreCount)
    {
        score.text = scoreCount.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }


}
