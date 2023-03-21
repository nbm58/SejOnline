using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WandNumberTextScript1 : MonoBehaviour
{

    private TMP_Text text1;
    public static int WandNumber;

	// Use this for initialization
	void Start ()
    {
        text1 = GetComponent<TMP_Text>();
    }
	
	// Update is called once per frame
	void Update () {
		text1.text = WandNumber.ToString();
    }
}
