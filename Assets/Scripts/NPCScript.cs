using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NPCScript : Interactable
{
    private Transform transform;

    protected override void Awake()
    {
        base.Awake();
        transform = GetComponent<Transform>();
    }

	protected override void Update()
	{
		// Checks if NPC is focused by the player
        if (isFocus && canInteract)
        {
            float distance = Vector3.Distance(
                PlayerController.PlayerControl.gameObject.transform.position,
                interactionTransform.position);
            // If its able to be interacted with, Interact
            if (distance <= radius && !hasInteracted && !isMoving)
            {
                //Debug.Log("INTERACT");
                UIManager.UI.ChangeToDialogue();
                Interact();
                LookAtPlayer();
                hasInteracted = true;
                interactionCount++;
            }
            else if (distance > radius && !isMoving)
            {
                hasInteracted = false;
            }
        }

        // Checks if NPC has arrived at the destination
        if (isMoving && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                isMoving = false;  // NPC has finished moving
                Debug.Log("NPC has arrived at the destination.");
            }
        }
        
    }

    protected void LookAtPlayer()
    {
        // Face player position
        Vector3 destination = PlayerController.PlayerControl.transform.position;
        if (destination != null)
        {
            transform.LookAt(destination);
        }
    }

    protected void TurnToFacePlayer()
    {
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
        // Event to call method in sceneManager that updates currentGraph upon interaction
        UpdateSceneGraph.Invoke(this);
    }
}
