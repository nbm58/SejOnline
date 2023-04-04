using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    
    private OptionsMenu optionsMenuScript;
    private MainMenu mainMenuScript;

    [SerializeField] private CanvasGroup startMenuCanvasGroup;

    [SerializeField] private bool fadeOut = false;
    
    void Awake()
    {
        optionsMenuScript = optionsMenu.GetComponent<OptionsMenu>();
        mainMenuScript = mainMenu.GetComponent<MainMenu>();

        optionsMenuScript.loadPlayerPrefs();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            fadeOut = true;
        }

        if (fadeOut)
        {
            startMenuCanvasGroup.alpha -= Time.deltaTime;
            if (startMenuCanvasGroup.alpha <= 0)
            {
                fadeOut = false;
                startMenu.SetActive(false);
                
                mainMenu.SetActive(true);
                
                mainMenuScript.fadeIn = true;
            }
        }
    }
}
