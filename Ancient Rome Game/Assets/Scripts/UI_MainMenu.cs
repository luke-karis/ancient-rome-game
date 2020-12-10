using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject creditsMenu;
    public GameObject sourcesMenu;

    public void loadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void loadMainMenu()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        sourcesMenu.SetActive(false);
    }

    public void loadCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void loadSources()
    {
        mainMenu.SetActive(false);
        sourcesMenu.SetActive(true);
    }
}
