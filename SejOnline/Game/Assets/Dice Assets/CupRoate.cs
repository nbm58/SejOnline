using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupRoate : MonoBehaviour
{
    public bool canRotate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if(canRotate)
        {
            StartCoroutine(Rotate(Vector3.left, 100, 1000.0f));
        }
    }

    public void StartRotation()
    {
        canRotate = true;
    }

    IEnumerator Rotate(Vector3 axis, float angle, float duration = 100.0f)
    {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;

        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;

        while(elapsed < duration && canRotate)
        {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = to;
        canRotate = false;
    }
}


