using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZoneScript2 : MonoBehaviour
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
            case "D2Side1":
                DiceNumberTextScript2.diceNumber = 6;
                break;
            case "D2Side2":
                DiceNumberTextScript2.diceNumber = 5;
                break;
            case "D2Side3":
                DiceNumberTextScript2.diceNumber = 4;
                break;
            case "D2Side4":
                DiceNumberTextScript2.diceNumber = 3;
                break;
            case "D2Side5":
                DiceNumberTextScript2.diceNumber = 2;
                break;
            case "D2Side6":
                DiceNumberTextScript2.diceNumber = 1;
                break;
            }
		}
	}
}
