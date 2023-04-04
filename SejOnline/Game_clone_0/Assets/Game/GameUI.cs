using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject instructions;

    public void toggleInstructions()
    {
        if (instructions.activeSelf)
        {
            instructions.SetActive(false);
            return;
        }
        instructions.SetActive(true);
    }
}
