using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    public Vector3 velocity = Vector3.back;
    public float maxDistance = 100;

    private float totalDistance;
    private Vector3 startPos;

    void Start()
    {
        totalDistance = 0;
        startPos = transform.position;
    }

    void Update()
    {
        // Move
        transform.position += (velocity * Time.deltaTime);

        // Update total distance moved
        totalDistance = Vector3.Distance(transform.position, startPos);

        // If maxDistance is exceeded, delete object
        if (totalDistance > maxDistance) 
        { 
            Object.Destroy(gameObject);
        }
    }
}
