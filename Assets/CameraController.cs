using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, -1f, -1.2f);
    public float zoom = 10f;
    public float pitch = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        // follow target after each frame update
        transform.position = target.position - (offset * zoom);
        // look at target
        transform.LookAt(target.position + (Vector3.up * pitch));
    }
}
