using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandScript3 : MonoBehaviour
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
        transform.position = new Vector3(-17, 2, 0);
        transform.rotation = Quaternion.identity;
        rb.AddForce(transform.up * 1200);
        rb.AddForce(transform.right * 500);
        rb.AddTorque(dirX, dirY, dirZ);
    }
}
