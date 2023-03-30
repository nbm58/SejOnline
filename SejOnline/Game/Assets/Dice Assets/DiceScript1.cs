using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript1 : MonoBehaviour
{

    static Rigidbody rb;
    public static Vector3 diceVelocity;

    // Use this for initialization
    public void Roll()
    {
        rb = GetComponent<Rigidbody>();
        diceVelocity = rb.velocity;
        float dirX = Random.Range(0, 500);
        float dirY = Random.Range(0, 500);
        float dirZ = Random.Range(0, 500);
        transform.position = new Vector3(-16, 4, 0);
        transform.rotation = Quaternion.identity;
        rb.AddForce(transform.up * 600);
        rb.AddForce(transform.right * 700);
        rb.AddTorque(dirX, dirY, dirZ);
    }

}
