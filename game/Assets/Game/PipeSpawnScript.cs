using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipePrefab;
    public float pipeSpawnTime = 4;
    private float timeSinceLastSpawn = 0;
    public float pipeSpawnHeight = 10;

    // Start is called before the first frame update
    void Start()
    {
        spawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastSpawn < pipeSpawnTime)
        {
            timeSinceLastSpawn += Time.deltaTime;
        }
        else
        {
            spawnPipe();
            timeSinceLastSpawn = 0;
        }
    }

    void spawnPipe()
    {
        float lowerBound = transform.position.y - pipeSpawnHeight;
        float upperBound = transform.position.y + pipeSpawnHeight;
        float randomY = Random.Range(lowerBound, upperBound);
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, 0);
        
        Instantiate(pipePrefab, spawnPosition, Quaternion.identity);
    }
}
