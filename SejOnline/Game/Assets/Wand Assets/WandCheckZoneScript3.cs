using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandCheckZoneScript3 : MonoBehaviour
{
    [SerializeField] private NetworkManagerUI networkManagerUI;
	Vector3 WandVelocity;

	// Update is called once per frame
	void FixedUpdate ()
    {
        WandVelocity = WandScript3.WandVelocity;
	}

	void OnTriggerStay(Collider col)
	{
		if (WandVelocity.x == 0f && WandVelocity.y == 0f && WandVelocity.z == 0f)
		{
			switch (col.gameObject.name)
            {
            case "W3Side1":
                networkManagerUI.Wand3Value.Value = 1;
                break;
            case "W3Side2":
                networkManagerUI.Wand3Value.Value = 2;
                break;
            case "W3Side3":
                networkManagerUI.Wand3Value.Value = 3;
                break;
            case "W3Side4":
                networkManagerUI.Wand3Value.Value = 4;
                break;
            }
        }
    }
}
