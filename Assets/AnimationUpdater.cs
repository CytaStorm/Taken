using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationUpdater : MonoBehaviour
{
    Animator animator;
    [SerializeField] NavMeshAgent agent;

    private float speed;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // calculate player speed using positions from current and previous frames
        velocity = agent.velocity;
        speed = velocity.magnitude;

        // pass the player's speed into the animator
        animator.SetFloat("speed", speed);
    }
}
