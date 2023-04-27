using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public AudioSource buttonClick;
    public AudioSource rollSound;

    [SerializeField] private GameObject instructionsDisplay;
    [SerializeField] private GameObject aboutDisplay;
     [SerializeField] private GameObject quitConfirmationDisplay;

    public void PlayButton()
    {
        buttonClick.Play();
    }

    public void PlayRoll()
    {
        rollSound.Play();
    }

    public void toggleInstructions()
    {
        if (instructionsDisplay.activeSelf)
        {
            instructionsDisplay.SetActive(false);
        }
        else
        {
            instructionsDisplay.SetActive(true);
            aboutDisplay.SetActive(false);
            quitConfirmationDisplay.SetActive(false);
        }
    }

    public void toggleAbout()
    {
        if (aboutDisplay.activeSelf)
        {
            aboutDisplay.SetActive(false);
        }
        else
        {
            aboutDisplay.SetActive(true);
            instructionsDisplay.SetActive(false);
            quitConfirmationDisplay.SetActive(false);
        }
    }

    public void toggleQuitConfirmation()
    {
        if (quitConfirmationDisplay.activeSelf)
        {
            quitConfirmationDisplay.SetActive(false);
        }
        else
        {
            quitConfirmationDisplay.SetActive(true);
            instructionsDisplay.SetActive(false);
            aboutDisplay.SetActive(false);
        }
    }
}
