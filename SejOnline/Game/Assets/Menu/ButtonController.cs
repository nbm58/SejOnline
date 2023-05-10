using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public AudioSource buttonClick;
    public AudioSource rollSound;

    public void PlayButton()
    {
        buttonClick.Play();
    }

    public void PlayRoll()
    {
        rollSound.Play();
    }
}
