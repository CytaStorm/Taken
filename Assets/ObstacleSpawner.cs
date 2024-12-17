using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    public List<GameObject> obstacles;
    public float maxDistance;
    public float spawnRate;

    public Vector3 velocity;
    public float xRange;
    public float yRange;
    public float zRange;

    private Vector3 position;

    void Awake()
    {
        position = transform.position;
        AssignParameters();
    }

    void Update()
    {
        // Spawn in obstacles from prefabs list
        for (int i = 0; i < spawnRate; i++)
        {
            int n = Random.Range(0, obstacles.Count - 1);
            Vector3 offset = new Vector3
                (Random.Range(-xRange, xRange), // x offset
                 Random.Range(-yRange, yRange), // y offset
                 Random.Range(-zRange, zRange)  // z offset
                );

            Object.Instantiate(obstacles[n], position + offset, Quaternion.identity);
        }
    }

    void AssignParameters()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            ObstacleMove obstacleMove = obstacles[i].GetComponent<ObstacleMove>();
            obstacleMove.velocity = velocity;
            obstacleMove.maxDistance = maxDistance;
        }
    }
}
