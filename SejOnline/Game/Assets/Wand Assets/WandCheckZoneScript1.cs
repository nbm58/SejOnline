using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandCheckZoneScript1 : MonoBehaviour
{

	Vector3 WandVelocity;

	// Update is called once per frame
	void FixedUpdate ()
    {
		WandVelocity = WandScript1.WandVelocity;
	}

	void OnTriggerStay(Collider col)
	{
		if (WandVelocity.x == 0f && WandVelocity.y == 0f && WandVelocity.z == 0f)
		{
			switch (col.gameObject.name)
            {
            case "W1Side1":
                WandNumberTextScript1.WandNumber = 4;
                break;
            case "W1Side2":
                WandNumberTextScript1.WandNumber = 3;
                break;
            case "W1Side3":
                WandNumberTextScript1.WandNumber = 2;
                break;
            case "W1Side4":
                WandNumberTextScript1.WandNumber = 1;
                break;
            }
		}
	}
}
