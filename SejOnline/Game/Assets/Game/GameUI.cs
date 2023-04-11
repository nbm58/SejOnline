using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject instructions;
    public Transform coverPosition;

    public void toggleInstructions()
    {
        if (instructions.activeSelf)
        {
            instructions.SetActive(false);
            coverPosition.position = new Vector3(6.0f, -750.0f, 0.0f);

            return;
        }
        
        instructions.SetActive(true);
        coverPosition.position = new Vector3(485.0f, 260.0f, 0.0f);
    }
}
