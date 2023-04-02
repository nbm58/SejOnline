﻿using System.Collections;
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
                networkManagerUI.Wand2Value.Value = 1;
                break;
            case "W2Side2":
                networkManagerUI.Wand2Value.Value = 2;
                break;
            case "W2Side3":
                networkManagerUI.Wand2Value.Value = 3;
                break;
            case "W2Side4":
                networkManagerUI.Wand2Value.Value = 4;
                break;
            }
		}
	}
}