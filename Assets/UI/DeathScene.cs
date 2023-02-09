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
    private static int highScoreValue;

    private void Start()
    {
        tmpRunScore = runScore.GetComponent<TextMeshProUGUI>();
        tmpHighScore = highScore.GetComponent<TextMeshProUGUI>();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Summer");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
