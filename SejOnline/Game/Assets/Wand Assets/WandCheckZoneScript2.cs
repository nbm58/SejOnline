using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandCheckZoneScript2 : MonoBehaviour
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
            case "W2Side1":
                WandNumberTextScript2.WandNumber = 4;
                break;
            case "W2Side2":
                WandNumberTextScript2.WandNumber = 3;
                break;
            case "W2Side3":
                WandNumberTextScript2.WandNumber = 2;
                break;
            case "W2Side4":
                WandNumberTextScript2.WandNumber = 1;
                break;
            }
		}
	}
}
