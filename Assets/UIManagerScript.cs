using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManagerScript : MonoBehaviour {
    public static UIManagerScript instance;
    public GameObject startButton;
    public GameObject beforeStartPanel;
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    [Space(10)]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverHighScoreText;

    private void Awake()
    {
        instance = this;

    }
    public void _Tap()
    {
        if(!GameManagerScript.instance.isGameStart  && !GameManagerScript.instance.isGameOver)
        {
            GameManagerScript.instance.StartGame();
            GameManagerScript.instance.NextPlatform();
            startButton.SetActive(false);
        }
        beforeStartPanel.SetActive(false);
        gamePanel.SetActive(true);

    }
    public void UpdateScore(float f)
    {
        scoreText.text = f.ToString("F2") + "m";
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverScoreText.text ="Score : "+ GameManagerScript.instance.score.ToString("F2") + "m";
        gameOverHighScoreText.text = "Best : " + PlayerPrefs.GetFloat("SCORE", 0).ToString("F2") + "m";
    }

    public void _RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
