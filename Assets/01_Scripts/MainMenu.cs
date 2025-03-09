using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene(2);
    }

    public void OnTutorialButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
