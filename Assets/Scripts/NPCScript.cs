using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NPCScript : Interactable
{
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
