using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZoneScript1 : MonoBehaviour
{
    [SerializeField] private NetworkManagerUI networkManagerUI;
	Vector3 diceVelocity;

	// Update is called once per frame
	void FixedUpdate ()
    {
		diceVelocity = DiceScript1.diceVelocity;
	}

	void OnTriggerStay(Collider col)
	{
		if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
		{
			switch (col.gameObject.name)
            {
            case "D1Side1":
                networkManagerUI.Dice1Value.Value = 6;
                break;
            case "D1Side2":
                networkManagerUI.Dice1Value.Value = 5;
                break;
            case "D1Side3":
                networkManagerUI.Dice1Value.Value = 4;
                break;
            case "D1Side4":
                networkManagerUI.Dice1Value.Value = 3;
                break;
            case "D1Side5":
                networkManagerUI.Dice1Value.Value = 2;
                break;
            case "D1Side6":
                networkManagerUI.Dice1Value.Value = 1;
                break;
            }
		}
	}
}
