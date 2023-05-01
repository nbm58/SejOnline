using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript2 : MonoBehaviour
{

    static Rigidbody rb;
    public static Vector3 DiceVelocity;

    // Use this for initialization
    public void Roll()
    {
        rb = GetComponent<Rigidbody>();
        DiceVelocity = rb.velocity;
        float dirX = Random.Range(200, 400);
        float dirY = Random.Range(200, 400);
        float dirZ = Random.Range(200, 400);
        transform.position = new Vector3(-19, 2, 0);
        transform.rotation = Quaternion.identity;
        rb.AddForce(transform.up * 1000);
        rb.AddForce(transform.right * 800);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
