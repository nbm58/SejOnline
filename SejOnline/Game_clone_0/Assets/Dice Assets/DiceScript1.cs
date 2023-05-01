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
        float dirX = Random.Range(200, 400);
        float dirY = Random.Range(200, 400);
        float dirZ = Random.Range(200, 400);
        transform.position = new Vector3(-18, 2, 0);
        transform.rotation = Quaternion.identity;
        rb.AddForce(transform.up * 1000);
        rb.AddForce(transform.right * 800);
        rb.AddTorque(dirX, dirY, dirZ);
    }

}
