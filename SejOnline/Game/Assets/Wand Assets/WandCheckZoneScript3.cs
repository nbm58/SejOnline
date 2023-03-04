using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandCheckZoneScript3 : MonoBehaviour
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
            case "W3Side1":
                WandNumberTextScript3.WandNumber = 4;
                break;
            case "W3Side2":
                WandNumberTextScript3.WandNumber = 3;
                break;
            case "W3Side3":
                WandNumberTextScript3.WandNumber = 2;
                break;
            case "W3Side4":
                WandNumberTextScript3.WandNumber = 1;
                break;
		    }
        }
    }
}
