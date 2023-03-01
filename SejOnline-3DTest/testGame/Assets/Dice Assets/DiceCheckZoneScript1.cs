using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZoneScript1 : MonoBehaviour
{

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
                DiceNumberTextScript1.diceNumber = 6;
                break;
            case "D1Side2":
                DiceNumberTextScript1.diceNumber = 5;
                break;
            case "D1Side3":
                DiceNumberTextScript1.diceNumber = 4;
                break;
            case "D1Side4":
                DiceNumberTextScript1.diceNumber = 3;
                break;
            case "D1Side5":
                DiceNumberTextScript1.diceNumber = 2;
                break;
            case "D1Side6":
                DiceNumberTextScript1.diceNumber = 1;
                break;
            }
		}
	}
}
