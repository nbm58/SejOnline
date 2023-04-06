using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandCheckZoneScript2 : MonoBehaviour
{
    [SerializeField] private NetworkManagerUI networkManagerUI;
	Vector3 WandVelocity;

	// Update is called once per frame
	void FixedUpdate ()
    {
		WandVelocity = WandScript2.WandVelocity;
	}

	void OnTriggerStay(Collider col)
	{
		if (WandVelocity.x == 0f && WandVelocity.y == 0f && WandVelocity.z == 0f)
		{
			switch (col.gameObject.name)
            {
            case "W2Side1":
                networkManagerUI.setWand2Value("White");
                break;
            case "W2Side2":
                networkManagerUI.setWand2Value("Black");
                break;
            case "W2Side3":
                networkManagerUI.setWand2Value("Ship");
                break;
            case "W2Side4":
                networkManagerUI.setWand2Value("Star");
                break;
            }
		}
	}
}
