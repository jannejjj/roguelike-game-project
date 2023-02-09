using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour
{
    public Transform runScore;
    public Transform highScore;
    TextMeshProUGUI tmpRunScore;
    TextMeshProUGUI tmpHighScore;
    private int highScoreValue;
    private int newScoreValue;

    private void Start()
    {
        tmpRunScore = runScore.GetComponent<TextMeshProUGUI>();
        tmpHighScore = highScore.GetComponent<TextMeshProUGUI>();

        tmpRunScore.text = newScoreValue.ToString();

        if (newScoreValue > highScoreValue)
        {
            highScoreValue = newScoreValue;
            tmpHighScore.text = newScoreValue.ToString();
        }
        else
        {
            tmpHighScore.text = highScoreValue.ToString();
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Summer");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        newScoreValue = PlayerPrefs.GetInt("score");
        Debug.Log("new score:" + newScoreValue);
        highScoreValue = PlayerPrefs.GetInt("highScore");
        Debug.Log("high score:" + highScoreValue);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("highScore", highScoreValue);
    }
}
