using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private GameObject gameStateManagerGO;
    [SerializeField] private GameObject scoreGO;
    [SerializeField] private GameObject topScoreGO;

    private GamestateManager gameManager;
    private TMP_Text score;
    private TMP_Text[] topScores;

    // Start is called before the first frame update
    private void Awake()
    {
        gameManager = gameStateManagerGO.GetComponent<GamestateManager>();
        score = scoreGO.GetComponent<TMP_Text>();
    }

    public void SetCurrentScore(int scoreCount)
    {
        score.text = scoreCount.ToString();
    }

    public void Resurrect()
    {

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToMenu()
    {

    }


}
