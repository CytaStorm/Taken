using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Events;

public class NPCScript : InteractableScript
{
    private Transform transform;
	[SerializeField] private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        transform = GetComponent<Transform>();
    }

	protected override void Update()
	{
		// Checks if NPC has arrived at the destination
		if (isMoving && agent.remainingDistance <= agent.stoppingDistance)
		{
			if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
			{
				isMoving = false;  // NPC has finished moving
				Debug.Log("NPC has arrived at the destination.");
			}
		}

		//INTERACTION FILTER
		if (!isFocus || !canInteract)
		{
			return;
		}

		//Actual interaction
		float distance = Vector3.Distance(
			PlayerController.PlayerControl.gameObject.transform.position,
			interactionTransform.position);

		// If its able to be interacted with, Interact
		if (distance <= radius && !hasInteracted && !isMoving)
		{
			//Limited interaction filter
			if (!hasLimitedInteractions) Interact();

			//Do not interact if too many interactions
			if (interactionCount > 0)
			{
				return;
			}

			Interact();
		}
		else if (distance > radius && !isMoving)
		{
			hasInteracted = false;
		}

		_animator.SetBool("Walking", agent.velocity.magnitude > 0);
	}

	protected void LookAtPlayer()
    {
        // If stationary, don't run method
        if (!canMove) { return; }

        // Face player position
        Vector3 destination = PlayerController.PlayerControl.transform.position;
        if (destination != null)
        {
            transform.LookAt(destination);
        }
    }

    protected void TurnToFacePlayer()
    {
        // If stationary, don't run method
        if (!canMove) { return; }

        // Get current angle
        Vector3 oldAngle = transform.rotation.eulerAngles; 
        // Face player position
        Vector3 destination = PlayerController.PlayerControl.transform.position;
        if (destination != null)
        {
            transform.LookAt(destination);
        }
        // Set angle to halfway between
        Vector3 angleDifference = transform.rotation.eulerAngles - oldAngle;
    }

    // Moves the NPC to the destination
    protected void MoveToPosition(Vector3 destination)
    {
        // Only move if the interactable has an agent AND a destination
        if ((agent != null) && (destination != null))
        {
            agent.SetDestination(destination); // Sets the destination for the NavMeshAgent
            isMoving = true; // Sets the NPC to moving state
        }
    }

	public override void Interact()
	{
		_UIManager.ChangeToDialogue();
		LookAtPlayer();
		hasInteracted = true;
		interactionCount++;
		UpdateSceneGraph.Invoke(this);
	}
}
