using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandScript2 : MonoBehaviour
{

    static Rigidbody rb;
    public static Vector3 WandVelocity;

    // Use this for initialization
    public void Roll()
    {
        rb = GetComponent<Rigidbody>();
        WandVelocity = rb.velocity;
        float dirX = Random.Range(0, 500);
        float dirY = Random.Range(0, 500);
        float dirZ = Random.Range(0, 500);
        transform.position = new Vector3(-19, 2, 0);
        transform.rotation = Quaternion.identity;
        rb.AddForce(transform.up * 1300);
        rb.AddForce(transform.right * 600);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
