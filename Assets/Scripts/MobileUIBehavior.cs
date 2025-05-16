using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MobileUIBehavior : MonoBehaviour
{
    public float targetRange;
    public Vector3 altTargetPos;
    private Vector3 defaultTargetPos;
    private Vector3 currentTarget;

    private Vector3 startPoint;
    private bool isMoving;
    private float distanceToMove;


    private void Start()
    {
        defaultTargetPos = transform.position;
        currentTarget = altTargetPos;
        isMoving = false;
        startPoint = transform.position;
    }

    private void Update()
    {
        if (isMoving)
        {
            // interpolate the velocity needed to smoothly move between the current
            // position and the target

            transform.position = Vector3.Lerp(transform.position, currentTarget, 0.05f);

            /*
            Vector3 velocity = Vector3.Lerp(transform.position, currentTarget, 0.1f);
            velocity *= Time.deltaTime;
            print(transform.localPosition);
            print(transform.position);

            // move towards the target position
            Vector3 newPos = transform.position + velocity;
            transform.position = newPos;
            */

            // if close enough to the target position, set position to target position,
            // and stop moving
            if (Vector3.Distance(transform.position, startPoint) >= distanceToMove - targetRange)
            {
                transform.position = currentTarget;
                isMoving = false;

                // toggle the target position to prepare for the next move
                if(currentTarget == altTargetPos)
                {
                    currentTarget = defaultTargetPos;
                }
                else
                {
                    currentTarget = altTargetPos;
                }
            }
        }
    }

    public void StartMoving()
    {
        if (!isMoving) 
        {
            isMoving = true;
            distanceToMove = Vector3.Distance(transform.position, currentTarget);
            startPoint = transform.position;
        }
    }
}
