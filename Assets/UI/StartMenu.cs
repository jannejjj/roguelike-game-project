using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject lore;
    public GameObject controls;
    public GameObject modifiers;
    public GameObject play;
    public GameObject quit;

    public void LoreOnClick()
    {
        Debug.Log("Lore clicked");
        lore.SetActive(false);
        controls.SetActive(true);
    }

    public void ControlsOnClick()
    {
        controls.SetActive(false);
        modifiers.SetActive(true);
    }

    public void ModifiersOnClick()
    {
        modifiers.SetActive(false);
        play.SetActive(true);
        quit.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene("Summer");
    }

    public void Quit()
    {
        Debug.Log("Quit.");
        Application.Quit();
    }
}