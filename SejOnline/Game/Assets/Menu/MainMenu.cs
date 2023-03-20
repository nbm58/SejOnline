using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;

    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] public bool fadeIn = false;
    
    public void PlayLocal()
    {
        SceneManager.LoadSceneAsync("LocalGame");
        Debug.Log("LocalGame Scene Loaded");
    }

    public void PlayOnline()
    {
        SceneManager.LoadSceneAsync("OnlineGame");
        Debug.Log("OnlineGame Scene Loaded");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    void Update()
    {
        if (fadeIn)
        {
            mainMenuCanvasGroup.alpha += (Time.deltaTime / 2);
            if (mainMenuCanvasGroup.alpha >= 1)
            {
                fadeIn = false;

                mainMenu.SetActive(true);
            }
        }
    }
}
