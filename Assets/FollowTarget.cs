using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public GameObject target;

    private Transform targetTransform;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        targetTransform = target.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = targetTransform.position - offset;
    }
}
