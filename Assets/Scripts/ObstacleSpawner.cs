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

    public Vector2 initialSpawnRange;
    public int numInitialSpawns;

    private Vector3 position;
    private float timer;

    void Awake()
    {
        position = transform.position;
        timer = 0f;

        AssignParameters();

        // Create bed of trees within spawn range
        for (int i = 0; i < numInitialSpawns; i++)
        {
            int n = Random.Range(0, obstacles.Count - 1);

            Vector3 offset = new Vector3
                (Random.Range(-initialSpawnRange.x, 0), // x offset
                 Random.Range(-initialSpawnRange.y, 0), // y offset
                 Random.Range(-zRange, zRange)  // z offset
                );

            Object.Instantiate(obstacles[n], position + offset, Quaternion.identity);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Spawn in obstacles from prefabs list
        if (timer >= 1/spawnRate)
        {
            timer = 0f;

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
