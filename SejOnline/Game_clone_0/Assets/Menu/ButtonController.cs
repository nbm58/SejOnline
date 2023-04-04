using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public AudioSource buttonClick;

    public void PlayButton()
    {
        buttonClick.Play();
    }
}
