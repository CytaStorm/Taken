using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Camera cam;
    public Vector3 offset;

    private Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();  
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cam.transform.position + offset;
    }
}
