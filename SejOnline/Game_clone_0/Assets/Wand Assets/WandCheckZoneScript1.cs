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
                networkManagerUI.Wand1Value.Value = "White";
                break;
            case "W1Side2":
                networkManagerUI.Wand1Value.Value = "Black";
                break;
            case "W1Side3":
                networkManagerUI.Wand1Value.Value = "Ship";
                break;
            case "W1Side4":
                networkManagerUI.Wand1Value.Value = "Star";
                break;
            }
		}
	}
}
