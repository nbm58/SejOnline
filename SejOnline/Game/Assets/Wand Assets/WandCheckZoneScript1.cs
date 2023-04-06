using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandCheckZoneScript1 : MonoBehaviour
{
    [SerializeField] private NetworkManagerUI networkManagerUI;
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
                networkManagerUI.setWand1Value("White");
                break;
            case "W1Side2":
                networkManagerUI.setWand1Value("Black");
                break;
            case "W1Side3":
                networkManagerUI.setWand1Value("Ship");
                break;
            case "W1Side4":
                networkManagerUI.setWand1Value("Star");
                break;
            }
		}
	}
}
