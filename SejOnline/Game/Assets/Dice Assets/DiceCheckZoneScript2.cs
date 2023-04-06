using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZoneScript2 : MonoBehaviour
{
    [SerializeField] private NetworkManagerUI networkManagerUI;
	Vector3 diceVelocity;

	// Update is called once per frame
	void FixedUpdate ()
    {
		diceVelocity = DiceScript2.DiceVelocity;
	}

	void OnTriggerStay(Collider col)
	{
		if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
		{
			switch (col.gameObject.name)
            {
            case "D2Side1":
                networkManagerUI.setDice2Value(6);
                break;
            case "D2Side2":
                networkManagerUI.setDice2Value(5);
                break;
            case "D2Side3":
                networkManagerUI.setDice2Value(4);
                break;
            case "D2Side4":
                networkManagerUI.setDice2Value(3);
                break;
            case "D2Side5":
                networkManagerUI.setDice2Value(2);
                break;
            case "D2Side6":
                networkManagerUI.setDice2Value(1);
                break;
            }
		}
	}
}
