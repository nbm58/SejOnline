using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public float pipeMoveSpeed = 5;
    public float pipeDestroyX = -45;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.left * pipeMoveSpeed * Time.deltaTime);

        if (transform.position.x < pipeDestroyX)
        {
            Debug.Log("Pipe Destroyed");
            Destroy(gameObject);
        }
    }
}
