using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WandNumberTextScript2 : MonoBehaviour
{

    private TMP_Text text2;
    public static int WandNumber;

	// Use this for initialization
	void Start ()
    {
        text2 = GetComponent<TMP_Text>();
    }
	
	// Update is called once per frame
	void Update () {
        text2.text = WandNumber.ToString();
    }
}
